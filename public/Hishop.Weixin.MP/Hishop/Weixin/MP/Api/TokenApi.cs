namespace Hishop.Weixin.MP.Api
{
    using Domain;
    using Hishop.Weixin.MP.Util;
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Script.Serialization;

    public class TokenApi
    {
        private static object lockobj = new object();

        public static bool CheckIsRightToken(string Token)
        {
            bool flag = true;
            return ((!Token.Contains("errcode") && !Token.Contains("errmsg")) && flag);
        }

        public static string GetToken(string appid, string secret)
        {
            string str = string.Empty;
            int num = 600;
            str = HttpRuntime.Cache.Get("weixinToken") as string;
            if (string.IsNullOrEmpty(str))
            {
                lock (lockobj)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
                        str = new WebUtils().DoGet(url, null);
                        HttpRuntime.Cache.Insert("weixinToken", str, null, DateTime.Now.AddSeconds((double) num), Cache.NoSlidingExpiration);
                    }
                }
            }
            return str;
        }

        public static string GetToken_Message(string appid, string secret)
        {
            string token = GetToken(appid, secret);
            if (token.Contains("access_token"))
            {
                token = new JavaScriptSerializer().Deserialize<Token>(token).access_token;
            }
            return token;
        }

        public string AppId
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppId");
            }
        }

        public string AppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppSecret");
            }
        }
    }
}

