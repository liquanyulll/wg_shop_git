using Aliyun.Api.LogService;
using Aliyun.Api.LogService.Domain.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wg_utils
{
    public class AliyunLogHelper
    {
        public  async Task DoAsync()
        {
            var client = LogServiceClientBuilders.HttpBuilder
        .Endpoint("cn-shanghai.log.aliyuncs.com", "wgshop")
        .Credential("LTAI4Fdcw8xRXhJieZQ1VyYY", "C0ICoLskAsoaRBzThQeHBGI3LMRbs0")
        .Build();

            // 原始日志
            var rawLogs = new[]
            {
                "2019-11-12 12:34:56 INFO id=1 status=foo",
                "2019-11-12 12:34:57 INFO id=2 status=bar",
                "2019-11-12 12:34:58 INFO id=1 status=foo",
                "2019-11-12 12:34:59 WARN id=1 status=foo",
            };

            // 解释 LogInfo
            var parsedLogs = rawLogs
                .Select(x =>
                {
                    var components = x.Split(' ');

                    var date = components[0];
                    var time = components[1];
                    var level = components[2];
                    var id = components[3].Split('=');
                    var status = components[4].Split('=');

                    var logInfo = new LogInfo
                    {
                        Contents =
                        {
                            {"level", level},
                            {id[0], id[1]},
                            {status[0], status[1]},
                        },
                        Time = DateTimeOffset.ParseExact($"{date} {time}", "yyyy-MM-dd HH:mm:ss", null)
                    };

                    return logInfo;
                })
                .ToList();

            var logGroupInfo = new LogGroupInfo
            {
                Topic = "example",
                LogTags =
                {
                    {"example", "true"},
                },
                Source="test",
                Logs = parsedLogs
            };

            var response = await client.PostLogStoreLogsAsync("wgshop1", logGroupInfo);
            //var response = await client.PostLogStoreLogsAsync(new PostLogsRequest(),)

            // 此接口没有返回结果，确保返回结果成功即可。
            var result= response.EnsureSuccess();
            var abc = 111;
        }
    }
}
