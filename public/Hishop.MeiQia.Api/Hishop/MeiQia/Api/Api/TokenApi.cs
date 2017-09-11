namespace Hishop.MeiQia.Api.Api
{
    using Domain;
    using Hishop.MeiQia.Api.Util;
    using System;
    using System.Web.Script.Serialization;

    public class TokenApi
    {
        public static string GetToken(string appid, string secret)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/token?grant_type=client_credential&&appid={0}&secret={1}", appid, secret);
            return new WebUtils().DoGet(url, null);
        }

        public static string GetTokenValue(string appid, string secret)
        {
            string url = string.Format("http://open.meiqia.com/cgi-bin/token?grant_type=client_credential&&appid={0}&secret={1}", appid, secret);
            string input = new WebUtils().DoGet(url, null);
            if (input.Contains("access_token"))
            {
                return new JavaScriptSerializer().Deserialize<Token>(input).access_token;
            }
            return string.Empty;
        }
    }
}

