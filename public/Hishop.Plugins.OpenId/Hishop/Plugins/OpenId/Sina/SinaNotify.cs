namespace Hishop.Plugins.OpenId.Sina
{
    using Hishop.Plugins;
    using NetDimension.Weibo;
    using NetDimension.Weibo.Entities.user;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web;
    using System.Xml;

    public class SinaNotify : OpenIdNotify
    {
        private readonly SortedDictionary<string, string> parameters;
        private NameValueCollection paramets;
        private const string ReUrl = "ReturnUrl";

        public SinaNotify(NameValueCollection _parameters)
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
            string message = "验证失败";
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            string openId = string.Empty;
            if (this.parameters["code"] != null)
            {
                Client client = null;
                string innerText = document.FirstChild.SelectSingleNode("AppKey").InnerText;
                string appSecret = document.FirstChild.SelectSingleNode("AppSecret").InnerText;
                HttpCookie cookie = HttpContext.Current.Request.Cookies["ReturnUrl"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("ReturnUrl");
                }
                string callbackUrl = cookie.Value;
                AccessToken accessTokenByAuthorizationCode = new OAuth(innerText, appSecret, callbackUrl).GetAccessTokenByAuthorizationCode(this.parameters["code"]);
                if (!string.IsNullOrEmpty(accessTokenByAuthorizationCode.Token))
                {
                    client = new Client(new OAuth(innerText, appSecret, accessTokenByAuthorizationCode.Token, null));
                    string uID = client.API.Account.GetUID();
                    Entity entity = client.API.Users.Show(uID, null);
                    HttpContext.Current.Request.Cookies.Remove("ReturnUrl");
                    openId = entity.ScreenName;
                    flag = true;
                }
                else
                {
                    message = "AccessToken参数值不存在！";
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

