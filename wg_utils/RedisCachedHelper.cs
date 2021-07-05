using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace wg_utils
{
    /// <summary>
    /// defaultDatabase 尽量用0，很多方法需要这个参数，但是他们默认都是0
    /// key的长度20万是ok的，50万报错了（key太长会很卡，不过也不会用到这么长的key）
    ///  keyvalue长度100万没出错，200万出的错 估算一次能方向201904月前原系统中原最大的keyvalue*100倍
    /// </summary>
    public static class RedisCachedHelper
    {
        private static readonly string Coonstr = "49.234.42.81:6379,password=a1b2c3d4e5f6,allowAdmin=true,defaultDatabase=0";
        private static object _locker = new Object();
        private static ConnectionMultiplexer _instance = null;
        private static readonly string EventLogName = "Redis_EventLog";
        private static readonly string ErrorLogName = "Redis_ErrorLog";

        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = ConnectionMultiplexer.Connect(Coonstr);
                        }
                    }
                }
                //注册如下事件
                _instance.ConnectionFailed += MuxerConnectionFailed;
                _instance.ConnectionRestored += MuxerConnectionRestored;
                _instance.ErrorMessage += MuxerErrorMessage;
                _instance.ConfigurationChanged += MuxerConfigurationChanged;
                _instance.HashSlotMoved += MuxerHashSlotMoved;
                _instance.InternalError += MuxerInternalError;
                return _instance;
            }
        }

        static RedisCachedHelper() { }

        public static IDatabase GetDatabase()
        {
            return Instance.GetDatabase();
        }

        #region 注册事件
        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            //LogUtil.DoLog(EventLogName, "重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }
        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            //LogUtil.DoLog(EventLogName, "重新建立连接之前的错误_ConnectionRestored: " + e.EndPoint);
        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //LogUtil.DoLog(EventLogName, "发生错误时_ErrorMessage: " + e.Message);
        }
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //LogUtil.DoLog(EventLogName, "配置更改时_Configuration changed: " + e.EndPoint);
        }
        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            //LogUtil.DoLog(EventLogName, "更改集群_HashSlotMoved:NewEndPoint: " + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }
        /// <summary>
        /// Redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            //LogUtil.DoLog(EventLogName, "Redis类库错误_InternalError:Message" + e.Exception.Message);
        }
        #endregion

        #region 锁
        /// <summary>
        /// 分布式等待锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cb">取到锁后所做的事情(回调函数)</param>
        /// <param name="lockExpiry">锁的过期时间（秒），没填默认为永不过期</param>
        /// <param name="retry">重试次数，没填默认为代表一定要拿到锁，一直等</param>
        /// <param name="sleepMillisecond">没取到时，下次再取延迟多少毫秒，默认100毫秒</param>
        /// <returns></returns>
        public static bool WaitLock(string key, string value, Action cb = null, bool release = false, int? lockExpiry = null, int? retry = null, int sleepMillisecond = 100)
        {
            try
            {
                //锁的过期时间，没填默认为永不过期
                var expiry = lockExpiry.HasValue ? TimeSpan.FromSeconds(lockExpiry.Value) : TimeSpan.MaxValue;

                while (!retry.HasValue || retry.Value > 1)
                {
                    //尝试获取锁
                    var token = Serialize(value);
                    if (GetDatabase().LockTake(key, token, expiry))
                    {
                        try
                        {
                            cb?.Invoke();
                            return true;
                        }
                        finally
                        {
                            //解锁
                            if (release)
                            {
                                GetDatabase().LockRelease(key, token);
                            }
                        }
                    }
                    else
                    {
                        //重试延迟时间
                        Thread.Sleep(sleepMillisecond);
                        if (retry.HasValue)
                        {
                            retry--;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                //LogUtil.LogExceptionInfo(ErrorLogName, ex);
                return false;
            }

        }

        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key">锁key</param>
        /// <param name="lockExpiry">锁的过期时间（秒），没填默认为永不过期</param>
        /// <param name="retry">重试次数，没填默认为代表一定要拿到锁，一直等</param>
        /// <param name="sleepMillisecond">没取到时，下次再取延迟多少毫秒，默认100毫秒</param>
        /// 注意：retry*sleepMillisecond即为总计愿意等待锁多少时间
        /// <returns></returns>
        public static bool GetLock(string key, int? lockExpiry = null, int? retry = null, int sleepMillisecond = 100)
        {
            return WaitLock(key, "Y", release: false, lockExpiry: lockExpiry, retry: retry, sleepMillisecond: sleepMillisecond);
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁key</param>
        /// <returns></returns>
        public static bool ReleaseLock(string key)
        {
            try
            {
                var token = Serialize("Y");
                return GetDatabase().LockRelease(key, token);
            }
            catch (Exception ex)
            {
                //LogUtil.LogExceptionInfo(ErrorLogName, ex);
                return false;
            }
        }
        #endregion

        private static byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }
        private static T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 非永久对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool Set(string key, object value, int cacheTime = 20)
        {
            try
            {
                return Set(key, value, false, cacheTime);
            }
            catch (Exception ex)
            {
                //LogUtil.LogExceptionInfo(ErrorLogName, ex);
                return false;
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">data</param>
        /// <param name="isPermanent">是否为永久缓存对象</param>
        /// <param name="cacheTime">cacheTime(分)默认为20分钟</param>
        public static bool Set(string key, object data, bool isPermanent, int cacheTime = 20)
        {
            try
            {
                if (data == null)
                    return false;

                var entryBytes = Serialize(data);

                if (isPermanent)
                {
                    return GetDatabase().StringSet(key, entryBytes, TimeSpan.MaxValue);
                }
                else
                {
                    var expiresIn = TimeSpan.FromMinutes(cacheTime);
                    return GetDatabase().StringSet(key, entryBytes, expiresIn);
                }
            }
            catch (Exception ex)
            {
                //LogUtil.LogExceptionInfo(ErrorLogName, ex);
                return false;
            }
        }

        public static T Get<T>(string key)
        {
            try
            {
                //待优化
                //1.对于频繁请求相同的数据，我们可以直接放到HttpContextBase.item 或Cache中去这样省去连接redis的时间

                var rValue = GetDatabase().StringGet(key);
                if (!rValue.HasValue)
                    return default(T);
                var result = Deserialize<T>(rValue);
                return result;
            }
            catch (Exception ex)
            {
                //LogUtil.LogExceptionInfo(ErrorLogName, ex);
                return default(T);
            }
        }

        public static bool RemoveKey(string key)
        {
            try
            {
                return GetDatabase().KeyDelete(key);
            }
            catch (Exception ex)
            {
                //LogUtil.LogExceptionInfo(ErrorLogName, ex);
                return false;
            }
        }

        /// <summary>
        /// 模糊清理
        /// </summary>
        /// <param name="pattern">key</param>
        public static bool RemoveByPattern(string pattern)
        {
            try
            {
                foreach (var ep in Instance.GetEndPoints())
                {
                    var server = Instance.GetServer(ep);
                    var _db = GetDatabase();
                    var keys = server.Keys(pattern: "*" + pattern + "*");
                    foreach (var key in keys)
                        _db.KeyDelete(key);
                }
                return true;
            }
            catch (Exception ex)
            {
                //LogUtil.LogExceptionInfo(ErrorLogName, ex);
                return false;
            }
        }

        /// <summary>
        /// 清理所有
        /// </summary>
        public static bool KillAll()
        {
            try
            {
                //它需要管理权限allowadmin = true
                foreach (var ep in Instance.GetEndPoints())
                {
                    var server = Instance.GetServer(ep);


                    //server.FlushDatabase(0); //外面defaultDatabase配置的多少就是填多少
                    //server.FlushAllDatabases(); // to wipe all databases 清除所有库里面的

                    var _db = GetDatabase();
                    //清除当前库里面的所有key
                    var keys = server.Keys();
                    foreach (var key in keys)
                        _db.KeyDelete(key);
                }
                return true;
            }
            catch (Exception ex)
            {
                //LogUtil.LogExceptionInfo(ErrorLogName, ex);
                return false;
            }

        }

        #region hashi
        public static bool HashSet(string key, string key2, string value)
        {
            return GetDatabase().HashSet(key, key2, Serialize(value));
        }

        public static T HashGet<T>(string key, string key2)
        {
            var value = GetDatabase().HashGet(key, key2);
            var result = Deserialize<T>(value);
            return result;
        }

        public static RedisValue HashGetAll(string key, string key2)
        {
            return GetDatabase().HashGet(key, key2);
        }
        #endregion
    }
}
