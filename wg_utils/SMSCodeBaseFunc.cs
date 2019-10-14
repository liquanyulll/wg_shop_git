using System;
using System.Collections.Generic;
using System.Text;

namespace wg_utils
{
    /// <summary>
    /// 1.手机验证码需要和手机关联
    /// 2.手机验证码需要是一次性的，用完后不能再次使用
    /// 3.登陆的验证码不能用来做修改密码
    /// </summary>
    public class SMSCodeBaseFunc
    {
        public static string SendSmsCode(string mobile, string smsType = "all")
        {
            var code = new Random(StaticSeed.Seed).Next(1000, 9999).ToString();
            var result= SMSUtil.SendSms(mobile, code);
            var key = $"{mobile}-{smsType}-{code}";
            RedisCachedHelper.Set(key, code, false, 10);
            return code;
        }
        public static bool CheckSmsCode(string mobile, string code, string smsType = "all", bool removeKey = true)
        {
            var key = $"{mobile}-{smsType}-{code}";
            var value = RedisCachedHelper.Get<string>(key);
            if (!string.IsNullOrEmpty(value) && removeKey)
            {
                RedisCachedHelper.RemoveKey(key);
            }
            return !string.IsNullOrEmpty(value);
        }

        public static bool RemoveSmsCodeCache(string mobile, string code, string smsType = "all")
        {
            var key = $"{mobile}-{smsType}-{code}";
            return RedisCachedHelper.RemoveKey(key);
        }
    }
}
