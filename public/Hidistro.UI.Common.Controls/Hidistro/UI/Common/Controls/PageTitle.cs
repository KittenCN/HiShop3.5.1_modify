namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.MP.Api;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;

    [ParseChildren(false), PersistChildren(true)]
    public class PageTitle : Control
    {
        private string _GetTokenError = "";
        private const string descKey = "Hishop.Desc.Value";
        private const string titleKey = "Hishop.Title.Value";

        public static void AddDescrption(string desc)
        {
            if (HttpContext.Current == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpContext.Current.Items["Hishop.Desc.Value"] = desc;
        }

        public static void AddSiteDescription(string desc)
        {
            AddDescrption(string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { desc }));
        }

        public static void AddSiteNameTitle(string title)
        {
            AddTitle(string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { title }));
        }

        public static void AddTitle(string title)
        {
            if (HttpContext.Current == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpContext.Current.Items["Hishop.Title.Value"] = title;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        private int ConvertDateTimeInt(DateTime time)
        {
            DateTime time2 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            TimeSpan span = (TimeSpan) (time - time2);
            return (int) span.TotalSeconds;
        }

        public string DoGet(string url)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            try
            {
                HttpWebResponse rsp = (HttpWebResponse) webRequest.GetResponse();
                Encoding encoding = Encoding.UTF8;
                return this.GetResponseAsString(rsp, encoding);
            }
            catch (Exception exception)
            {
                Globals.Debuglog("获取信息ticket错误：" + exception.Message, "_Debuglog.txt");
                return string.Empty;
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
            string str2 = this.DoGet(url);
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

        private string GetSignature(string token, string timestamp, string nonce, out string str)
        {
            string str2 = this.Page.Request.Url.ToString().Replace(":" + this.Page.Request.Url.Port.ToString(), "");
            string str3 = this.GetJsApi_ticket(token);
            string str4 = "jsapi_ticket=" + str3;
            string str5 = "noncestr=" + nonce;
            string str6 = "timestamp=" + timestamp;
            string str7 = "url=" + str2;
            string[] strArray = new string[] { str4, str5, str6, str7 };
            str = string.Join("&", strArray);
            string str8 = str;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
        }

        private HttpWebRequest GetWebRequest(string url, string method)
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
            string storeName = string.Empty;
            string storeDescription = string.Empty;
            string logo = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (((cookie != null) && !string.IsNullOrEmpty(cookie.Value)) && masterSettings.IsShowDistributorSelfStoreName)
            {
                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(Globals.ToNum(cookie.Value));
                if ((distributorInfo != null) && (distributorInfo.ReferralStatus != 9))
                {
                    storeName = distributorInfo.StoreName;
                    storeDescription = distributorInfo.StoreDescription;
                    logo = distributorInfo.Logo;
                }
            }
            if (string.IsNullOrEmpty(storeName))
            {
                storeName = masterSettings.SiteName;
                storeDescription = masterSettings.ShopIntroduction;
                logo = masterSettings.DistributorLogoPic;
            }
            string str5 = this.Context.Items["Hishop.Title.Value"] as string;
            string str6 = this.Context.Items["Hishop.Desc.Value"] as string;
            if (string.IsNullOrEmpty(str6))
            {
                str6 = storeDescription;
            }
            if (string.IsNullOrEmpty(str5))
            {
                str5 = storeName;
                writer.WriteLine("<title>{0}</title>", storeName);
            }
            else
            {
                writer.WriteLine("<title>{0}</title>", str5 + " - " + storeName);
                writer.WriteLine("<meta name=\"keywords\" content=\"{0}\" />", str5);
            }
            writer.WriteLine("<meta name=\"description\" content=\"{0}\" />", str6);
            string telReg = masterSettings.TelReg;
            string str8 = " var followWtSiteName='" + storeName + "'; var followWtImgUrl='" + logo + "';";
            if (this.Page.Request.UserAgent.ToLower().Contains("micromessenger") || (Globals.RequestQueryNum("istest") == 1))
            {
                str8 = str8 + " var isfollowWt='true';";
                Uri url = HttpContext.Current.Request.Url;
                string str10 = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
                if (!logo.StartsWith("http"))
                {
                    logo = str10 + logo;
                }
                string timestamp = this.ConvertDateTimeInt(DateTime.Now).ToString();
                string str = string.Empty;
                string nonce = "QoN4FvGbxdTi7mnffL";
                string cacheToken = this.GetCacheToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
                string str15 = this.GetSignature(cacheToken, timestamp, nonce, out str);
                writer.WriteLine(string.Concat(new object[] { 
                    "<script src=\"http://res.wx.qq.com/open/js/jweixin-1.0.0.js\"></script><script>", str8, "var telReg=", telReg, ";wx.config({ debug: false,appId: '", masterSettings.WeixinAppId, "',timestamp: '", timestamp, "', nonceStr: '", nonce, "',signature: '", str15, "',jsApiList: ['checkJsApi','onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','chooseWXPay']});var _GetTokenError='", this._GetTokenError, "'; var wxinshare_title='", HttpContext.Current.Server.HtmlEncode(storeName.Replace("\n", " ").Replace("\r", "")), 
                    "';var wxinshare_desc='", HttpContext.Current.Server.HtmlEncode(storeDescription.Replace("\n", " ").Replace("\r", "")), "';var wxinshare_link='", str10, "/default.aspx?ReferralId=", Globals.GetCurrentDistributorId(), "';var fxShopName='", HttpContext.Current.Server.HtmlEncode(storeName.Replace("\n", " ").Replace("\r", "")), "';var wxinshare_imgurl='", logo, "'</script><script src=\"/templates/common/script/WeiXinShare.js?201603\"></script>"
                 }));
            }
            else
            {
                str8 = str8 + " var isfollowWt='false';";
                writer.WriteLine("<script>" + str8 + "var telReg=" + telReg + ";var _GetTokenError=''; var wxinshare_title='';var wxinshare_desc='';var wxinshare_link='';var wxinshare_imgurl='';;var fxShopName=''</script>");
            }
        }
    }
}

