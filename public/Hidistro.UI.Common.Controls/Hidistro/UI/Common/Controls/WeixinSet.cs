namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hishop.Weixin.MP.Api;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class WeixinSet : Literal
    {
        public string _GetTokenError = "";
        public string htmlAppID = string.Empty;
        public string htmlNonceStr = "QoN4FvGbxdTi7mnffL";
        public string htmlSignature = string.Empty;
        public string htmlstring1 = string.Empty;
        public string htmlTicket = string.Empty;
        public string htmlTimeStamp = string.Empty;
        public string htmlToken = string.Empty;

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime time2 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            TimeSpan span = (TimeSpan) (time - time2);
            return (int) span.TotalSeconds;
        }

        public string DoGet(string url)
        {
            try
            {
                HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
                webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
                Encoding encoding = Encoding.UTF8;
                return this.GetResponseAsString(rsp, encoding);
            }
            catch
            {
                return "";
            }
        }

        private string GetCacheToken(string appid, string secret)
        {
            string str = TokenApi.GetToken_Message(appid, secret);
            if ((!string.IsNullOrEmpty(str) && str.Contains("errmsg")) && str.Contains("errcode"))
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
                if (((dictionary != null) && dictionary.ContainsKey("errcode")) && dictionary.ContainsKey("errmsg"))
                {
                    this._GetTokenError = dictionary["errcode"] + "|" + dictionary["errmsg"];
                    return str;
                }
                this._GetTokenError = "";
                return str;
            }
            if (string.IsNullOrEmpty(str))
            {
                str = "";
                this._GetTokenError = "获取令牌失败";
            }
            return str;
        }

        public string GetJsApi_ticket(string token)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token);
            string str2 = string.Empty;
            try
            {
                str2 = this.DoGet(url);
            }
            catch
            {
            }
            if (!string.IsNullOrEmpty(str2))
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(str2);
                if ((dictionary != null) && dictionary.ContainsKey("ticket"))
                {
                    return dictionary["ticket"];
                }
            }
            return string.Empty;
        }

        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream responseStream = null;
            StreamReader reader = null;
            string str;
            try
            {
                responseStream = rsp.GetResponseStream();
                reader = new StreamReader(responseStream, encoding);
                str = reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return str;
        }

        public string GetSignature(string token, string timestamp, string nonce, out string str)
        {
            string str2 = this.Page.Request.Url.ToString().Replace(":" + this.Page.Request.Url.Port.ToString(), "");
            string str3 = this.GetJsApi_ticket(token);
            this.htmlTicket = str3;
            string str4 = "jsapi_ticket=" + str3;
            string str5 = "noncestr=" + nonce;
            string str6 = "timestamp=" + timestamp;
            string str7 = "url=" + str2;
            string[] strArray = new string[] { str4, str5, str6, str7 };
            str = string.Join("&", strArray);
            string str8 = str;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            int num = 0x186a0;
            HttpWebRequest request = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                request = (HttpWebRequest) WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                request = (HttpWebRequest) WebRequest.Create(url);
            }
            request.ServicePoint.Expect100Continue = false;
            request.Method = method;
            request.KeepAlive = true;
            request.UserAgent = "Hishop";
            request.Timeout = num;
            return request;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.htmlAppID = masterSettings.WeixinAppId;
            string weixinAppSecret = masterSettings.WeixinAppSecret;
            string cacheToken = this.GetCacheToken(this.htmlAppID, weixinAppSecret);
            this.htmlTimeStamp = ConvertDateTimeInt(DateTime.Now).ToString();
            this.htmlSignature = this.GetSignature(cacheToken, this.htmlTimeStamp, this.htmlNonceStr, out this.htmlstring1);
            base.Text = "<script>wx.config({ debug: false,appId: '" + this.htmlAppID + "',timestamp: '" + this.htmlTimeStamp + "', nonceStr: '" + this.htmlNonceStr + "',signature: '" + this.htmlSignature + "',jsApiList: ['checkJsApi','onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','chooseWXPay']});var _GetTokenError='" + this._GetTokenError + "'; </script>";
            base.Render(writer);
        }
    }
}

