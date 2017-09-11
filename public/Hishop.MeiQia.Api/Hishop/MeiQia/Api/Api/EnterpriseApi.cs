namespace Hishop.MeiQia.Api.Api
{
    using Hishop.MeiQia.Api.Util;
    using System;
    using System.Collections.Generic;

    public class EnterpriseApi
    {
        public static string ActivateEnterprise(string accessToken, string unit)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/activate/userver?access_token={0}", accessToken);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("unit", unit);
            return new WebUtils().DoPost(url, parameters);
        }

        public static string CreateEnterprise(string accessToken, IDictionary<string, string> parameters)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/create/unit?access_token={0}", accessToken);
            return new WebUtils().DoPost(url, parameters);
        }

        public static string GetUnitId(string accessToken, string unit)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/get/unitid?access_token={0}&unit={1}", accessToken, unit);
            string msg = new WebUtils().DoGet(url, null);
            if (msg.Contains("id"))
            {
                return Common.GetJsonValue(msg, "id");
            }
            return string.Empty;
        }

        public static string UpdateEnterprise(string accessToken, IDictionary<string, string> parameters)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/update/unit?access_token={0}", accessToken);
            return new WebUtils().DoPost(url, parameters);
        }
    }
}

