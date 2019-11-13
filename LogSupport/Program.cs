using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.IO;
using System.Threading.Tasks;
using wg_model.Systems;
using wg_utils;

namespace LogSupport
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Logs\wgshop\";
            Action<RedisChannel, RedisValue> action = (channel, message) =>
            {
                try
                {
                    var log = JsonConvert.DeserializeObject<DoLogModel>(message);
                    Console.WriteLine($"环境：{log.Env},文件：{log.LogType}");
                    Task.Run(() =>
                    {
                        var logPath = path + log.Env;
                        var fileName = DateTime.Now.ToString("yyyyMMddHH") + "-" + log.LogType.Replace("_", "-") + ".txt";

                        if (!Directory.Exists(logPath))
                        {
                            Directory.CreateDirectory(logPath);
                        }
                        var vFileName = logPath + @"\" + fileName;
                        var fs = new FileStream(vFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                        var sw = new StreamWriter(fs);
                        sw.WriteLine(DateTime.Now.ToLongTimeString() + ":" + log.Msg);//记录生成log的时间
                        sw.Close();
                        fs.Close();
                    });
                }
                catch (Exception ex)
                {
                    // ignored
                    Console.WriteLine("系统错误:" + ex.Message);
                }
            };

            RedisMQHelper.Subscriber.Subscribe(LogUtil.LogQueueKey, action);

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
