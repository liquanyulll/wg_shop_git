﻿using System;
using System.Threading;
using wg_utils;

namespace UnitTest
{
    class Program
    {



        static void Main(string[] args)
        {

            LogUtil.DoLog("CFF", "123123123", "adasdasdasda");
            #region 阿里云日志
            AliyunLogHelper aliyunLog = new AliyunLogHelper();
            aliyunLog.DoAsync().Wait();
            #endregion

            #region 密钥生产
            //var sup = new MoneyKeySupport();
            //var cdf = sup.AddMoneyKey("0faka", 5, 50);
            #endregion

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
