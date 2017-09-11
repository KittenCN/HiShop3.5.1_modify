namespace Hidistro.UI.Web.Vshop
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    public class GetWinXinInfo : Page
    {
        private string GetResponseResult(string url)
        {
            using (HttpWebResponse response = (HttpWebResponse) WebRequest.Create(url).GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public void GetWeiXinInfoByAuth(SiteSettings site, string return_url)
        {
            string str = this.Page.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(str))
            {
                string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + site.WeixinAppId + "&secret=" + site.WeixinAppSecret + "&code=" + str + "&grant_type=authorization_code");
                if (responseResult.Contains("access_token"))
                {
                    JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                    obj2["openid"].ToString();
                    string str3 = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");
                    if (str3.Contains("nickname"))
                    {
                        JObject obj3 = JsonConvert.DeserializeObject(str3) as JObject;
                        if (MemberHelper.SetUserHeadAndUserName(obj3["openid"].ToString(), obj3["headimgurl"].ToString(), Globals.UrlDecode(obj3["nickname"].ToString()), 1))
                        {
                            Globals.Debuglog("重写成功！" + str3, "_Debuglog.txt");
                        }
                        base.Response.Redirect(return_url);
                    }
                    else
                    {
                        base.Response.Redirect(return_url);
                    }
                }
                else
                {
                    base.Response.Redirect(return_url);
                }
            }
            else if (!string.IsNullOrEmpty(this.Page.Request.QueryString["state"]))
            {
                base.Response.Redirect("用户拒绝授权");
            }
            else
            {
                string str4 = "snsapi_userinfo";
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "")) + "&response_type=code&scope=" + str4 + "&state=STATE#wechat_redirect";
                this.Page.Response.Redirect(url);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = Globals.RequestQueryStr("return_url");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            this.GetWeiXinInfoByAuth(masterSettings, str);
            base.Response.Write(HttpContext.Current.Request.Url.ToString());
        }
    }
}

