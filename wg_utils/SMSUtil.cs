using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace wg_utils
{
    public class SMSUtil
    {
        //        AccessKeyID:
        //LTAI4Fey6Xu5yAbT2F74hwvF
        //AccessKeySecret:
        //px3YX3VKzwzu0xcZe9syHsxSfNjtI0
        public static string RegionId = "cn-hangzhou";
        public static string AccessKeyId = "LTAI4Fey6Xu5yAbT2F74hwvF";
        public static string Secret = "px3YX3VKzwzu0xcZe9syHsxSfNjtI0";

        public static string SignName = "Supper快";
        public static string TemplateCode = "SMS_130790141";

        //public static IClientProfile profile;
        public static DefaultAcsClient client;

        public static DefaultAcsClient _client
        {
            get
            {
                if (client == null)
                {
                    IClientProfile profile = DefaultProfile.GetProfile(RegionId, AccessKeyId, Secret);
                    client = new DefaultAcsClient(profile);
                }
                return client;
            }
        }

        public static bool SendSms(string mobile, string code)
        {
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", mobile);
            request.AddQueryParameters("SignName", SignName);
            request.AddQueryParameters("TemplateCode", TemplateCode);
            request.AddQueryParameters("TemplateParam", "{\"code\":\"" + code + "\"}");
            try
            {
                CommonResponse response = _client.GetCommonResponse(request);
                var result = System.Text.Encoding.Default.GetString(response.HttpResponse.Content);
                return result.Contains("OK");
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            //catch (ServerException e)
            //{
            //}
            //catch (ClientException e)
            //{
            //    Console.WriteLine(e);
            //}
        }
    }
}
