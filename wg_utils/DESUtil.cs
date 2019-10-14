using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace wg_utils
{
    //internal 同一命名空间下可以访问
    /// <summary>
    ///
    /// </summary>
    internal class DESUtil
    {
        //private string _DESKey = "MAIBAKEY";
        //public string DESKey
        //{
        //    set
        //    {
        //        _DESKey = value;
        //    }
        //}

        /// <summary>
        /// 加密（出来的是Hex）
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public string DESEncrypt(string toEncrypt, string _DESKey)
        {
            //定义DES加密服务提供类
            var des = new DESCryptoServiceProvider();
            //加密字符串转换为byte数组
            var inputByte = System.Text.ASCIIEncoding.UTF8.GetBytes(toEncrypt);
            //加密密匙转化为byte数组
            var key = Encoding.ASCII.GetBytes(_DESKey); //DES密钥(必须8字节)
            des.Key = key;
            des.IV = key;
            //创建其支持存储区为内存的流
            var ms = new MemoryStream();
            //定义将数据流链接到加密转换的流
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByte, 0, inputByte.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (var b in ms.ToArray())
            {
                //向可变字符串追加转换成十六进制数字符串的加密后byte数组。
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();

        }

        /// <summary>
        /// DES加密，出来的是正常字符串，为了满足某些伙伴
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DESEncryptNoHhex(string sourceString, string key)
        {
            var btKey = Encoding.UTF8.GetBytes(key);

            var btIV = Encoding.UTF8.GetBytes(key);

            var des = new DESCryptoServiceProvider();

            using (var ms = new MemoryStream())
            {
                var inData = Encoding.UTF8.GetBytes(sourceString);
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(inData, 0, inData.Length);
                    cs.FlushFinalBlock();
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public string DESDecrypt(string toDecrypt, string _DESKey)
        {
            //定义DES加密解密服务提供类
            var des = new DESCryptoServiceProvider();
            //加密密匙转化为byte数组
            var key = Encoding.ASCII.GetBytes(_DESKey);
            des.Key = key;
            des.IV = key;
            //将被解密的字符串每两个字符以十六进制解析为byte类型，组成byte数组
            var length = (toDecrypt.Length / 2);
            var inputByte = new byte[length];
            for (var index = 0; index < length; index++)
            {
                var substring = toDecrypt.Substring(index * 2, 2);
                inputByte[index] = Convert.ToByte(substring, 16);
            }
            //创建其支持存储区为内存的流
            var ms = new MemoryStream();
            //定义将数据流链接到加密转换的流
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByte, 0, inputByte.Length);
            cs.FlushFinalBlock();

            return Encoding.UTF8.GetString((ms.ToArray()));
        }

        /// <summary>
        /// DES解密，进去的密文不能是Hex的
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string DESDecryptNoHex(string encryptedString, string key)
        {
            var btKey = Encoding.UTF8.GetBytes(key);

            var btIV = Encoding.UTF8.GetBytes(key);

            var des = new DESCryptoServiceProvider();

            using (var ms = new MemoryStream())
            {
                var inData = Convert.FromBase64String(encryptedString);
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                {
                    cs.Write(inData, 0, inData.Length);

                    cs.FlushFinalBlock();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
