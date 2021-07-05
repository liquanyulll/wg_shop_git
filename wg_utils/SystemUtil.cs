using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace wg_utils
{
    public static class StaticSeed
    {
        public static int Seed => GetRandomSeed();

        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

    }

    public class SystemUtil
    {
        /// <summary>
        /// 当前环境
        /// </summary>
        public static string Env { get; set; }

        /// <summary>
        /// 图形验证码随机产生器，先调用这个方法得到随机码后，再调用CreateValidateGraphic
        /// </summary>
        /// <param name="CodeLength"></param>
        /// <returns></returns>
        public static string GetRandomVGCharac(int CodeLength)
        {
            var sCode = String.Empty;
            char[] oCharacter = {
               '2','3','4','5','6','8','9',
               'A','B','C','D','E','F','G','H','J','K', 'L','M','N','P','R','S','T','W','X','Y'
              };
            var oRnd = new Random();
            //生成驗證碼字串
            for (var N1 = 0; N1 <= CodeLength - 1; N1++)
            {
                sCode += oCharacter[oRnd.Next(oCharacter.Length)];
            }

            return sCode;

        }

        public static string GenerateStringId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 產生圖形驗證碼。
        /// </summary>
        /// <param name="Code">传入验证码</param>
        /// <param name="CodeLength">驗證碼字元數。</param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="FontSize"></param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic(string Code, int Width = 100, int Height = 34, int FontSize = 24)
        {
            int randAngle = 45; //随机转动角度
            var sCode = Code;
            //顏色列表，用於驗證碼、噪線、噪點
            Color[] oColors ={
             System.Drawing.Color.Black,
             System.Drawing.Color.Red,
             System.Drawing.Color.Blue,
             System.Drawing.Color.Green,
             System.Drawing.Color.Orange,
             System.Drawing.Color.Brown,
             System.Drawing.Color.Brown,
             System.Drawing.Color.DarkBlue
            };
            //字體列表，用於驗證碼
            string[] oFontNames = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            //驗證碼的字元集，去掉了一些容易混淆的字元
            char[] oCharacter = {
       '2','3','4','5','6','8','9',
       'A','B','C','D','E','F','G','H','J','K', 'L','M','N','P','R','S','T','W','X','Y'
      };
            var oRnd = new Random();
            Bitmap oBmp = null;
            Graphics oGraphics = null;
            var N1 = 0;
            var oPoint1 = default(System.Drawing.Point);
            var oPoint2 = default(System.Drawing.Point);
            string sFontName = null;
            Font oFont = null;
            var oColor = default(Color);

            oBmp = new Bitmap(Width, Height);
            oGraphics = Graphics.FromImage(oBmp);
            oGraphics.Clear(System.Drawing.Color.White);
            try
            {
                for (N1 = 0; N1 <= 4; N1++)
                {
                    //畫噪線
                    oPoint1.X = oRnd.Next(Width);
                    oPoint1.Y = oRnd.Next(Height);
                    oPoint2.X = oRnd.Next(Width);
                    oPoint2.Y = oRnd.Next(Height);
                    oColor = oColors[oRnd.Next(oColors.Length)];
                    oGraphics.DrawLine(new Pen(oColor), oPoint1, oPoint2);
                }

                float spaceWith = 0, dotX = 0, dotY = 0;
                if (sCode.Length != 0)
                {
                    spaceWith = (Width - FontSize * sCode.Length - 10) / sCode.Length;
                }
                //文字距中
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                for (N1 = 0; N1 <= sCode.Length - 1; N1++)
                {
                    //畫驗證碼字串
                    sFontName = oFontNames[oRnd.Next(oFontNames.Length)];
                    oFont = new Font(sFontName, FontSize, FontStyle.Italic);
                    oColor = oColors[oRnd.Next(oColors.Length)];

                    dotY = (Height - oFont.Height) / 2 + 2;//中心下移2像素
                    dotX = Convert.ToSingle(N1) * FontSize + (N1 + 1) * spaceWith;

                    //Point dot = new Point(0, 0);
                    //float angle = oRnd.Next(-randAngle, randAngle);//转动的度数
                    //oGraphics.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置
                    //oGraphics.RotateTransform(angle);

                    oGraphics.DrawString(sCode[N1].ToString(), oFont, new SolidBrush(oColor), dotX, dotY);

                    //oGraphics.RotateTransform(-angle);//转回去
                    //oGraphics.TranslateTransform(-2, -dot.Y);//移动光标到指定位置，每个字符紧凑显示，避免被软件识别
                }

                for (var i = 0; i <= 80; i++)
                {
                    //畫噪點
                    var x = oRnd.Next(oBmp.Width);
                    var y = oRnd.Next(oBmp.Height);
                    var clr = oColors[oRnd.Next(oColors.Length)];
                    oBmp.SetPixel(x, y, clr);
                }

                Code = sCode;
                //保存图片数据
                var stream = new MemoryStream();
                oBmp.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                oGraphics.Dispose();
            }
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
