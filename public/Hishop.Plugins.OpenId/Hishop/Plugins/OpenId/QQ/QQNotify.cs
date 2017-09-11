namespace Hishop.Plugins.OpenId.QQ
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;

    public class QQNotify : OpenIdNotify
    {
        private readonly SortedDictionary<string, string> parameters;
        private NameValueCollection paramets;
        private const string ReUrl = "ReturnUrl";

        public QQNotify(NameValueCollection _parameters)
        {
            this.paramets = _parameters;
            this.parameters = new SortedDictionary<string, string>();
            string[] allKeys = _parameters.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                this.parameters.Add(allKeys[i], _parameters[allKeys[i]]);
            }
            this.parameters.Remove("HIGW");
            this.parameters.Remove("HITO");
        }

        public override void Verify(int timeout, string configXml)
        {
            bool flag = false;
            string str = string.Empty;
            string message = "验证失败";
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            string str3 = string.Empty;
            if (!string.IsNullOrEmpty(this.parameters["code"]))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["ReturnUrl"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("ReturnUrl");
                }
                string str4 = cookie.Value;
                string str5 = string.Empty;
                string str6 = this.parameters["code"];
                string innerText = document.FirstChild.SelectSingleNode("Partner").InnerText;
                string str8 = document.FirstChild.SelectSingleNode("Key").InnerText;
                WebUtils utils = new WebUtils();
                try
                {
                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("grant_type", "authorization_code");
                    parameters.Add("client_id", innerText);
                    parameters.Add("client_secret", str8);
                    parameters.Add("code", str6);
                    parameters.Add("redirect_uri", str4);
                    parameters.Add("state", "hishop");
                    string str9 = utils.DoPost("https://graph.qq.com/oauth2.0/token", parameters);
                    str5 = utils.GetParameters(str9, "access_token");
                    if (!string.IsNullOrEmpty(str5))
                    {
                        IDictionary<string, string> dictionary2 = new Dictionary<string, string>();
                        dictionary2.Add("access_token", str5);
                        string input = utils.DoPost("https://graph.qq.com/oauth2.0/me", dictionary2);
                        string pattern = "\"openid\":\"(?<openid>[\\w]+)\"";
                        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                        str3 = regex.Matches(input)[0].Groups["openid"].Value;
                        IDictionary<string, string> dictionary3 = new Dictionary<string, string>();
                        dictionary3.Add("access_token", str5);
                        dictionary3.Add("oauth_consumer_key", innerText);
                        dictionary3.Add("openid", str3);
                        string str12 = utils.DoPost("https://graph.qq.com/user/get_user_info", dictionary3);
                        message = message + str12;
                        string str13 = "\"nickname\":\\s*\"(?<nickname>.+)\"\\s*,\\s*\"gender\"";
                        Regex regex2 = new Regex(str13, RegexOptions.IgnoreCase);
                        str = regex2.Matches(str12)[0].Groups["nickname"].Value;
                        message = message + str;
                        if (!string.IsNullOrEmpty(str))
                        {
                            HttpCookie cookie2 = HttpContext.Current.Request.Cookies["NickName"];
                            if (cookie2 == null)
                            {
                                cookie2 = new HttpCookie("NickName");
                            }
                            cookie2.Value = HttpContext.Current.Server.UrlEncode(str);
                            cookie2.Expires = DateTime.Now.AddHours(1.0);
                            HttpContext.Current.Response.Cookies.Add(cookie2);
                            flag = true;
                        }
                    }
                }
                catch (Exception exception)
                {
                    flag = false;
                    message = message + "获取授权失败！" + exception.Message;
                }
            }
            if (flag)
            {
                this.OnAuthenticated(str3);
            }
            else
            {
                this.OnFailed(message);
            }
        }
    }
}

