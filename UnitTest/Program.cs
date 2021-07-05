using System;
using System.Threading;
using wg_utils;

namespace UnitTest
{
    class Program
    {



        static void Main(string[] args)
        {

            //LogUtil.DoLog("CFF", "123123123", "adasdasdasda");
            //#region 阿里云日志
            //AliyunLogHelper aliyunLog = new AliyunLogHelper();
            //aliyunLog.DoAsync().Wait();
            //#endregion

            #region 密钥生产
            var sup = new MoneyKeySupport();
            var cdf1 = sup.AddMoneyKey("Kfaka", 1, 100);
            var cdf2 = sup.AddMoneyKey("Kfaka", 5, 100);
            var cdf3 = sup.AddMoneyKey("Kfaka", 10, 100);
            var cdf4 = sup.AddMoneyKey("Kfaka", 100, 100);
            #endregion

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
