namespace Hishop.MeiQia.Api.Api
{
    using Hishop.MeiQia.Api.Util;
    using System;
    using System.Collections.Generic;

    public class CustomerApi
    {
        public static string CreateCustomer(string accessToken, IDictionary<string, string> parameters)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/create/userver?access_token={0}", accessToken);
            return new WebUtils().DoPost(url, parameters);
        }

        public static string DeleteCustomer(string accessToken, string unit, string userver)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/delete/userver?access_token={0}", accessToken);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("unit", unit);
            parameters.Add("userver", userver);
            return new WebUtils().DoPost(url, parameters);
        }

        public static string GetUserverId(string accessToken, string userver)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/get/userverid?access_token={0}&userver={1}", accessToken, userver);
            string msg = new WebUtils().DoGet(url, null);
            if (msg.Contains("id"))
            {
                return Common.GetJsonValue(msg, "id");
            }
            return string.Empty;
        }

        public static string UpdateCustomer(string accessToken, IDictionary<string, string> parameters)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/update/userver?access_token={0}", accessToken);
            return new WebUtils().DoPost(url, parameters);
        }
    }
}

