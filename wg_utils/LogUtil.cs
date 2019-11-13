using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using wg_model.Systems;

namespace wg_utils
{
    public static class LogUtil
    {
        public static string LogQueueKey = "Logs";

        public static void DoLog(string logType, string msg, string transGuid = "")
        {
            var log = new DoLogModel()
            {
                LogType = logType,
                Msg = msg,
                TransGuid = transGuid,
                Env = SystemUtil.Env
            };
            var json = JsonConvert.SerializeObject(log);
            RedisMQHelper.Subscriber.Publish(LogQueueKey, json);
        }

        public static void LogExceptionInfo(string LogType, Exception ex, string customContent = "")
        {
            try
            {

                var errorMsg = string.Empty;
                var particular = string.Empty;
                if (ex != null)
                {
                    errorMsg = GetAllExceptionMsg(ex as Exception);
                    particular = (ex as Exception).StackTrace;
                }

                DoLog(LogType, "_ErrorMsg:" + errorMsg + "_ErrorStack:" + particular + "_CustomConent:" + customContent);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 以递归的形式，把所有的InnerMsg全部弄出来
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="outExMsg"></param>
        /// <returns></returns>
        public static string GetAllExceptionMsg(Exception ex)
        {
            var str = ex.Message;
            var cnt = 0;
            while (ex.InnerException != null)
            {
                cnt++;
                ex = ex.InnerException;
                str += ex.Message + "||InnerExp" + cnt + ":";   // ex.Message;
            }

            //var exception = ex as ;
            //if (exception?.EntityValidationErrors != null && exception.EntityValidationErrors.Any())
            //{
            //    str = exception.EntityValidationErrors.SelectMany(dbEntityValidationResult => dbEntityValidationResult.ValidationErrors).Aggregate(str, (current, dbValidationError) => current + ("||EntityValidationErrors:" + dbValidationError.ErrorMessage));
            //}

            return str;
        }
    }
}
