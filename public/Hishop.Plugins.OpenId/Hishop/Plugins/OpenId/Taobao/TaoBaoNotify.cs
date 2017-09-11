namespace Hishop.Plugins.OpenId.Taobao
{
    using Hishop.Plugins;
    using LitJson;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class TaoBaoNotify : OpenIdNotify
    {
        private readonly string NickName = "NickName";
        private readonly SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
        private const string ReUrl = "ReturnUrl";
        private readonly string TokenKey = "TokenKey";

        public TaoBaoNotify(NameValueCollection _parameters)
        {
            string[] allKeys = _parameters.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                this.parameters.Add(allKeys[i], _parameters[allKeys[i]]);
            }
            this.parameters.Remove("HIGW");
            this.parameters.Remove("HITO");
        }

        public string Base64ToString(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }

        public string GetParameters(string parameters, string key)
        {
            string str = string.Empty;
            try
            {
                string[] strArray = this.Base64ToString(parameters).Split(new char[] { '&' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    string[] strArray2 = strArray[i].Split(new char[] { '=' });
                    if (strArray2[0].ToLower() == key.ToLower())
                    {
                        return strArray2[1];
                    }
                }
            }
            catch
            {
            }
            return str;
        }

        public override void Verify(int timeout, string configXml)
        {
            bool flag = false;
            string message = "验证失败";
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            string openId = string.Empty;
            if (this.parameters["code"] != null)
            {
                string str3 = this.parameters["code"];
                string innerText = document.FirstChild.SelectSingleNode("AppKey").InnerText;
                string str5 = document.FirstChild.SelectSingleNode("AppSecret").InnerText;
                try
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["ReturnUrl"];
                    if (cookie == null)
                    {
                        cookie = new HttpCookie("ReturnUrl");
                    }
                    string str6 = cookie.Value;
                    IDictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("grant_type", "authorization_code");
                    parameters.Add("code", str3);
                    parameters.Add("redirect_uri", str6);
                    parameters.Add("client_id", innerText);
                    parameters.Add("client_secret", str5);
                    string str7 = new WebUtils().DoPost("https://oauth.taobao.com/token", parameters);
                    if (!string.IsNullOrEmpty(str7))
                    {
                        JsonData data = JsonMapper.ToObject(str7);
                        if (data["access_token"] != null)
                        {
                            HttpCookie cookie2 = HttpContext.Current.Request.Cookies[this.TokenKey];
                            if (cookie2 == null)
                            {
                                cookie2 = new HttpCookie(this.TokenKey);
                            }
                            cookie2.Value = (string) data["access_token"];
                            cookie2.Expires = DateTime.Now.AddHours(2.0);
                            HttpContext.Current.Response.Cookies.Add(cookie2);
                            if (data["taobao_user_nick"] != null)
                            {
                                HttpCookie cookie3 = HttpContext.Current.Request.Cookies[this.NickName];
                                if (cookie3 == null)
                                {
                                    cookie3 = new HttpCookie(this.NickName);
                                }
                                cookie3.Value = HttpContext.Current.Server.UrlEncode((string) data["taobao_user_nick"]);
                                cookie3.Expires = DateTime.Now.AddHours(2.0);
                                HttpContext.Current.Response.Cookies.Add(cookie3);
                                openId = (string) data["taobao_user_nick"];
                            }
                            flag = true;
                        }
                        else
                        {
                            HttpContext.Current.Response.Write(str7);
                            flag = false;
                        }
                    }
                }
                catch (Exception exception)
                {
                    HttpContext.Current.Response.Write(exception.Message);
                    flag = false;
                }
            }
            if (flag)
            {
                this.OnAuthenticated(openId);
            }
            else
            {
                this.OnFailed(message);
            }
        }
    }
}

