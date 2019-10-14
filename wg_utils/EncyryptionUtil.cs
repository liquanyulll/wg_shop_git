using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace wg_utils
{
    /// <summary>
    /// 该Class用来存放各种加密方法
    /// </summary>
    public static class EncyryptionUtil
    {
        public static string DES_KEY = "SHOPSYST";
        public static string AES_KEY = "3A3R33D5EW59-TLI";

        /// <summary>
        /// 该方法需要自动在密码字符串后加上WebConfig内约定的字符串，然后再进行二次MD5
        /// </summary>
        /// <param name="pi_password"></param>
        /// <returns></returns>
        //public static string genUserPassMD5(string pi_password)
        //{
        //    var passAtt = ConfigurationManager.AppSettings["EncryptSalt"];
        //    var readyPass = pi_password + passAtt;
        //    var afterMD5 = getMD5(readyPass);
        //    afterMD5 = getMD5(afterMD5);//进行两次MD5k
        //    return afterMD5;
        //}


        /// <summary>
        /// 输入字符串获取MD5（32位）的方法，该方法适用于我们自己的系统
        /// </summary>
        /// <param name="pi_string"></param>
        /// <returns></returns>
        public static string getMD5(string pi_string)
        {
            // Use input string to calculate MD5 hash
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(pi_string);//注意使用Encoding.ASCII
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }

        private static string BaseMd5(byte[] inputBytes)
        {
            var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        //Encoding.GetEncoding("GBK")
        public static string GetMd5GBK(string pi_string)
        {
            return BaseMd5(Encoding.GetEncoding("GBK").GetBytes(pi_string));
        }
        /// <summary>
        /// UTF8的MD5获取方法，这个一般通用到所有合作伙伴
        /// </summary>
        /// <param name="pi_string"></param>
        /// <returns></returns>
        public static string GetMd5Utf8(string pi_string)
        {
            return BaseMd5(Encoding.UTF8.GetBytes(pi_string));
        }


        /// <summary>
        /// 用来生成随机密码
        /// </summary>
        /// <param name="passwordLen"></param>
        /// <returns></returns>
        public static string GetRandomPassword(int passwordLen)
        {
            var randomChars = "BCDFGHJKMPQRTVWXY2346789";
            var password = string.Empty;
            int randomNum;
            var random = new Random();
            for (var i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
        }

        /// <summary>
        /// AES加密，默认为256位的Key，没有向量
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key">如果没有默认值，那么就取WebConfig中的（方便前后台传递信息加密）</param>
        /// <returns></returns>
        public static string AESEncrypt(string plainText, string key = "")
        {
            string AESKey;
            if (string.IsNullOrWhiteSpace(key))
            {
                AESKey = AES_KEY;
            }
            else
            {
                AESKey = key;
            }
            return new AES().AESEncrypt(plainText, AESKey, AES.AesKeyLength.Long256);
        }

        /// <summary>
        /// AES解密，默认为256位的Key，没有向量
        /// </summary>
        /// <param name="showText"></param>
        /// <param name="key">如果没有默认值，那么就取WebConfig中的（方便前后台传递信息加密）</param>
        /// <returns></returns>
        public static string AESDecrypt(string showText, string key = "")
        {
            string AESKey;
            if (string.IsNullOrWhiteSpace(key))
            {
                AESKey = AES_KEY;
            }
            else
            {
                AESKey = key;
            }
            return new AES().AESDecrypt(showText, AESKey, AES.AesKeyLength.Long256);
        }

        public const string DESTypeHex = "HEX";
        public const string DESTypeNoHex = "NoHEX";

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string DESEncrypt(string plainText, string key = "", string type = "HEX")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                key = DES_KEY;
            }

            if (type == "HEX")
            {
                return new DESUtil().DESEncrypt(plainText, key);
            }
            return new DESUtil().DESEncryptNoHhex(plainText, key);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="showText"></param>
        /// <returns></returns>
        public static string DESDecrypt(string showText, string key = "", string type = "HEX")
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                key = DES_KEY;
            }

            if (type == "HEX")
            {
                return new DESUtil().DESDecrypt(showText, key);
            }
            return new DESUtil().DESDecryptNoHex(showText, key);
        }

    }
}
