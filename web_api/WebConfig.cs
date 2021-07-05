using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_api
{
    public static class WebConfig
    {
        public static string SystemWebDomain = "";
        public static string SystemStcWebUrl = "http://47.101.208.172/wgstc/";
        public static string GetProductImageUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            if (url.Contains("http", StringComparison.OrdinalIgnoreCase) || url.Contains("https", StringComparison.OrdinalIgnoreCase))
            {
                return url;
            }
            return SystemStcWebUrl + "images/products/" + url;
        }
    }
}
