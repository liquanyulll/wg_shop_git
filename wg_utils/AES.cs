using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace wg_utils
{
    public class AES
    {
        /// <summary>
        /// AESKey的长度，分别为64位，128位，256位
        /// </summary>
        public enum AesKeyLength : int
        {
            Short64 = 8, Middle128 = 16, Long256 = 32
        }

        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <param name="Key">密钥</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public string AESEncrypt(string plainText, string Key, AesKeyLength keyLengh)
        {
            int keyL = 32;
            switch (keyLengh)
            {
                case AesKeyLength.Long256:
                    keyL = 32;
                    break;
                case AesKeyLength.Middle128:
                    keyL = 16;
                    break;
                case AesKeyLength.Short64:
                    keyL = 8;
                    break;
            }

            MemoryStream mStream = new MemoryStream();
            RijndaelManaged aes = new RijndaelManaged();

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            Byte[] bKey = new Byte[keyL];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);

            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            //aes.Key = _key;
            aes.Key = bKey;
            //aes.IV = _iV;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <param name="Key">密钥</param>
        /// <returns>返回解密后的明文字符串</returns>
        public string AESDecrypt(string showText, string Key, AesKeyLength keyLengh)
        {
            int keyL = 32;
            switch (keyLengh)
            {
                case AesKeyLength.Long256:
                    keyL = 32;
                    break;
                case AesKeyLength.Middle128:
                    keyL = 16;
                    break;
                case AesKeyLength.Short64:
                    keyL = 8;
                    break;
            }

            Byte[] encryptedBytes = Convert.FromBase64String(showText);
            Byte[] bKey = new Byte[keyL];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);

            MemoryStream mStream = new MemoryStream(encryptedBytes);
            //mStream.Write( encryptedBytes, 0, encryptedBytes.Length );
            //mStream.Seek( 0, SeekOrigin.Begin );
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            //aes.IV = _iV;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }


    }
}
