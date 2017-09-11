namespace Hidistro.UI.Common.Controls
{
    using Aop.Api.Response;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.AlipayFuwu.Api.Model;
    using Hishop.AlipayFuwu.Api.Util;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.UI;

    [PersistChildren(false), ParseChildren(true)]
    public abstract class VshopTemplatedWebControl : TemplatedWebControl
    {
        protected int referralId;
        private string skinName;

        protected VshopTemplatedWebControl()
        {
            if (HttpContext.Current.Request.Form.Keys.Count == 0)
            {
                HiAffiliation.LoadPage();
            }
            this.WeiXinLoginTry();
        }

        public void AlipayLoginAction(SiteSettings site)
        {
            if (string.IsNullOrEmpty(AlipayFuwuConfig.appId) && !AlipayFuwuConfig.CommSetConfig(site.AlipayAppid, this.Page.Server.MapPath("~/"), "GBK"))
            {
                this.WriteFuwuError(this.Page.Request.QueryString.ToString(), "服务窗口参数配置不准确！");
                this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx");
            }
            string str = this.Page.Request.QueryString["auth_code"];
            string str2 = this.Page.Request.QueryString["scope"];
            if (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str))
            {
                this.WriteFuwuError(this.Page.Request.QueryString.ToString(), "已授权");
                AlipaySystemOauthTokenResponse oauthTokenResponse = AliOHHelper.GetOauthTokenResponse(str);
                this.WriteFuwuError(AliOHHelper.SerializeObject(oauthTokenResponse, true), "获取AccessToken");
                if (((oauthTokenResponse != null) && !oauthTokenResponse.IsError) && (oauthTokenResponse.AccessToken != null))
                {
                    string alipayUserId = oauthTokenResponse.AlipayUserId;
                    string openId = "";
                    JObject obj2 = JsonConvert.DeserializeObject(oauthTokenResponse.Body) as JObject;
                    if (obj2["alipay_system_oauth_token_response"]["user_id"] != null)
                    {
                        openId = obj2["alipay_system_oauth_token_response"]["user_id"].ToString();
                    }
                    if (this.HasReg(openId, "fuwu") || this.HasReg(alipayUserId, "fuwu"))
                    {
                        MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(openId, "fuwu");
                        if ((openIdMember == null) || (openIdMember.Status == Convert.ToInt32(UserStatus.DEL)))
                        {
                            this.Page.Response.Redirect(Globals.ApplicationPath + "/logout.aspx");
                        }
                        string alipayOpenid = openIdMember.AlipayOpenid;
                        if (((alipayUserId != "") && (alipayUserId != alipayOpenid)) || string.IsNullOrEmpty(alipayOpenid))
                        {
                            openIdMember.AlipayOpenid = alipayUserId;
                            MemberProcessor.SetAlipayInfos(openIdMember);
                        }
                        this.setLogin(openIdMember.UserId);
                        this.WriteFuwuError("已存在用户登入！", openId);
                    }
                    else
                    {
                        AlipayUserUserinfoShareResponse alipayUserUserinfo = AliOHHelper.GetAlipayUserUserinfo(oauthTokenResponse.AccessToken);
                        this.WriteFuwuError(AliOHHelper.SerializeObject(alipayUserUserinfo, true), "获取用户信息");
                        string str7 = "";
                        string realName = "";
                        string avatar = "";
                        if ((alipayUserUserinfo != null) && !alipayUserUserinfo.IsError)
                        {
                            avatar = alipayUserUserinfo.Avatar;
                            if (alipayUserUserinfo.RealName != null)
                            {
                                realName = alipayUserUserinfo.RealName;
                            }
                            if (string.IsNullOrEmpty(alipayUserId))
                            {
                                alipayUserId = alipayUserUserinfo.UserId;
                            }
                            if (string.IsNullOrEmpty(openId))
                            {
                                JObject obj3 = JsonConvert.DeserializeObject(alipayUserUserinfo.Body) as JObject;
                                if (obj3["alipay_user_id"] != null)
                                {
                                    openId = obj3["alipay_user_id"].ToString();
                                }
                            }
                        }
                        str7 = "FW*" + openId.Substring(10);
                        string generateId = Globals.GetGenerateId();
                        MemberInfo member = new MemberInfo {
                            GradeId = MemberProcessor.GetDefaultMemberGrade(),
                            UserName = str7,
                            CreateDate = DateTime.Now,
                            SessionId = generateId,
                            SessionEndTime = DateTime.Now.AddYears(10),
                            UserHead = avatar,
                            AlipayAvatar = avatar,
                            AlipayLoginId = str7,
                            AlipayOpenid = alipayUserId,
                            AlipayUserId = openId,
                            AlipayUsername = realName
                        };
                        HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
                        if (cookie != null)
                        {
                            member.ReferralUserId = Convert.ToInt32(cookie.Value);
                        }
                        else
                        {
                            member.ReferralUserId = 0;
                        }
                        member.Password = HiCryptographer.Md5Encrypt("888888");
                        MemberProcessor.CreateMember(member);
                        MemberInfo info3 = MemberProcessor.GetMember(generateId);
                        this.setLogin(info3.UserId);
                    }
                }
                else
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri.ToString()));
                }
            }
            else if (!string.IsNullOrEmpty(str2))
            {
                this.WriteFuwuError(this.Page.Request.QueryString.ToString(), "拒绝授权");
                this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx");
            }
            else
            {
                string msg = AliOHHelper.AlipayAuthUrl(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, ""), site.AlipayAppid, "auth_userinfo");
                this.WriteFuwuError(msg, "用户登入授权的路径");
                this.Page.Response.Redirect(msg);
            }
        }

        private string ControlText()
        {
            if (!this.SkinFileExists)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(System.IO.File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
            if (builder.Length == 0)
            {
                return null;
            }
            builder.Replace("<%", "").Replace("%>", "");
            string vshopSkinPath = Globals.GetVshopSkinPath(null);
            builder.Replace("/images/", vshopSkinPath + "/images/");
            builder.Replace("/script/", vshopSkinPath + "/script/");
            builder.Replace("/style/", vshopSkinPath + "/style/");
            builder.Replace("/utility/", Globals.ApplicationPath + "/utility/");
            builder.Insert(0, "<%@ Register TagPrefix=\"UI\" Namespace=\"ASPNET.WebControls\" Assembly=\"ASPNET.WebControls\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
            builder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
            MatchCollection matchs = Regex.Matches(builder.ToString(), "href(\\s+)?=(\\s+)?\"url:(?<UrlName>.*?)(\\((?<Param>.*?)\\))?\"", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            for (int i = matchs.Count - 1; i >= 0; i--)
            {
                int startIndex = matchs[i].Groups["UrlName"].Index - 4;
                int length = matchs[i].Groups["UrlName"].Length + 4;
                if (matchs[i].Groups["Param"].Length > 0)
                {
                    length += matchs[i].Groups["Param"].Length + 2;
                }
                builder.Remove(startIndex, length);
                builder.Insert(startIndex, Globals.GetSiteUrls().UrlData.FormatUrl(matchs[i].Groups["UrlName"].Value.Trim(), new object[] { matchs[i].Groups["Param"].Value }));
            }
            return builder.ToString();
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!this.LoadHtmlThemedControl())
            {
                throw new SkinNotFoundException(this.SkinPath);
            }
            this.AttachChildControls();
        }

        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return this.Page.Request.Url.AbsolutePath;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Page.Request.Url.AbsolutePath).Append("?");
            foreach (string str2 in queryStrings.Keys)
            {
                if (queryStrings[str2] != null)
                {
                    string str = queryStrings[str2].Trim();
                    if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                    {
                        builder.Append(str2).Append("=").Append(this.Page.Server.UrlEncode(str)).Append("&");
                    }
                }
            }
            queryStrings.Clear();
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public string GetOpenID(SiteSettings site, int isMustLogin)
        {
            string str = this.Page.Request.QueryString["code"];
            int num = Globals.RequestQueryNum("ReferralId");
            try
            {
                if (string.IsNullOrEmpty(Globals.GetCurrentWXOpenId) && (site != null))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + site.WeixinAppId + "&secret=" + site.WeixinAppSecret + "&code=" + str + "&grant_type=authorization_code");
                        if (!string.IsNullOrEmpty(responseResult) && responseResult.Contains("access_token"))
                        {
                            JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                            string openId = obj2["openid"].ToString();
                            Globals.GetCurrentWXOpenId = openId;
                            try
                            {
                                if (!this.HasReg(openId, "wx"))
                                {
                                    string generateId = Globals.GetGenerateId();
                                    MemberInfo member = new MemberInfo {
                                        GradeId = MemberProcessor.GetDefaultMemberGrade(),
                                        UserName = Globals.UrlDecode("新用户"),
                                        OpenId = openId,
                                        CreateDate = DateTime.Now,
                                        SessionId = generateId,
                                        SessionEndTime = DateTime.Now.AddYears(10),
                                        UserHead = Globals.GetWebUrlStart() + "/templates/common/images/user.png",
                                        ReferralUserId = Globals.GetCurrentDistributorId(),
                                        Password = HiCryptographer.Md5Encrypt("888888")
                                    };
                                    string str6 = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");
                                    JObject obj3 = JsonConvert.DeserializeObject(str6) as JObject;
                                    if (!str6.Contains("nickname"))
                                    {
                                        if (isMustLogin != 0)
                                        {
                                            string urlToEncode = Regex.Replace(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, ""), "&code=(.*)&state=STATE", "");
                                            string str8 = "snsapi_userinfo";
                                            return ("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(urlToEncode) + "&response_type=code&scope=" + str8 + "&state=STATE#wechat_redirect");
                                        }
                                        member.Status = Convert.ToInt32(UserStatus.Visitor);
                                        MemberProcessor.CreateMember(member);
                                    }
                                    else
                                    {
                                        member.UserName = Globals.UrlDecode(obj3["nickname"].ToString());
                                        member.UserHead = obj3["headimgurl"].ToString();
                                        MemberProcessor.CreateMember(member);
                                        MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(openId, "wx");
                                        if (openIdMember == null)
                                        {
                                            Globals.ClearUserCookie();
                                            return ("/Default.aspx?ReferralId=" + num);
                                        }
                                        MemberHelper.SetUserHeadAndUserName(openId, "", "", 2);
                                        this.setLogin(openIdMember.UserId);
                                    }
                                }
                                else
                                {
                                    string str10 = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");
                                    JObject obj4 = JsonConvert.DeserializeObject(str10) as JObject;
                                    if (str10.Contains("nickname"))
                                    {
                                        MemberInfo info3 = MemberProcessor.GetOpenIdMember(openId, "wx");
                                        if (info3 == null)
                                        {
                                            Globals.ClearUserCookie();
                                            return ("/Default.aspx?ReferralId=" + num);
                                        }
                                        if (((info3.UserName == "新用户") || string.IsNullOrEmpty(info3.UserHead)) || info3.UserHead.ToLower().Contains("/templates/common/images/user.png"))
                                        {
                                            MemberHelper.SetUserHeadAndUserName(openId, obj4["headimgurl"].ToString(), Globals.UrlDecode(obj4["nickname"].ToString()), 1);
                                        }
                                        else if (info3.IsAuthorizeWeiXin == 0)
                                        {
                                            MemberHelper.SetUserHeadAndUserName(openId, "", "", 2);
                                        }
                                        this.setLogin(info3.UserId);
                                    }
                                }
                            }
                            catch (ThreadAbortException exception)
                            {
                                Globals.Debuglog(exception.ToString(), "_DebuglogWX_021.txt");
                            }
                            catch (Exception exception2)
                            {
                                Globals.Debuglog(exception2.ToString() + "||" + responseResult, "_DebuglogWX_01.txt");
                            }
                        }
                    }
                    else
                    {
                        return ("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "")) + "&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect");
                    }
                }
            }
            catch (ThreadAbortException exception3)
            {
                Globals.Debuglog(exception3.ToString(), "_DebuglogWX_02.txt");
            }
            catch (Exception exception4)
            {
                Globals.Debuglog(exception4.ToString(), "_DebuglogWX_03.txt");
            }
            return "";
        }


        public string GetOpenID(SiteSettings site)
        {
            string str = this.Page.Request.QueryString["code"];
            try
            {
                if (string.IsNullOrEmpty(Globals.GetCurrentWXOpenId) && (site != null))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + site.WeixinAppId + "&secret=" + site.WeixinAppSecret + "&code=" + str + "&grant_type=authorization_code");
                        if (!string.IsNullOrEmpty(responseResult) && responseResult.Contains("access_token"))
                        {
                            JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                            string openId = obj2["openid"].ToString();
                            Globals.GetCurrentWXOpenId = openId;

                        }
                    }
                    else
                    {
                        return ("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "")) + "&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect");
                    }
                }
            }
            catch (ThreadAbortException exception3)
            {
                Globals.Debuglog(exception3.ToString(), "_DebuglogWX_02.txt");
            }
            catch (Exception exception4)
            {
                Globals.Debuglog(exception4.ToString(), "_DebuglogWX_03.txt");
            }
            return "";
        }

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

        protected void GotoResourceNotFound(string errorMsg = "")
        {
            this.GotoResourceNotFound(ErrorType.前台其它错误, errorMsg);
        }

        protected void GotoResourceNotFound(ErrorType type, string errorMsg = "")
        {
            this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/ResourceNotFound.aspx?type={0}&errorMsg={1}", (int) type, errorMsg));
        }

        public bool HasReg(string OpenId, string fromWay = "wx")
        {
            MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(OpenId, fromWay);
            if (openIdMember != null)
            {
                this.setLogin(openIdMember.UserId);
                return true;
            }
            return false;
        }

        protected bool LoadHtmlThemedControl()
        {
            string str = this.ControlText();
            if (!string.IsNullOrEmpty(str))
            {
                Control child = this.Page.ParseControl(str);
                child.ID = "_";
                this.Controls.Add(child);
                return true;
            }
            return false;
        }

        public void ReloadPage(NameValueCollection queryStrings)
        {
            this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings));
        }

        public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
        }

        public void setLogin(int UserId)
        {
            HttpCookie cookie = new HttpCookie("Vshop-Member") {
                Value = UserId.ToString(),
                Expires = DateTime.Now.AddYears(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpCookie cookie2 = new HttpCookie("Vshop-Member-Verify") {
                Value = Globals.EncryptStr(UserId.ToString()),
                Expires = DateTime.Now.AddYears(1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie2);
            this.Page.Session["userid"] = UserId.ToString();
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(UserId);
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                Globals.SetDistributorCookie(userIdDistributors.UserId);
            }
        }

        public void WeixinLoginAction(SiteSettings site, int isMustLogin)
        {
            try
            {
                string openID = this.GetOpenID(site, isMustLogin);
                if (!string.IsNullOrEmpty(openID))
                {
                    this.Page.Response.Redirect(openID);
                }
                else
                {
                    string getCurrentWXOpenId = Globals.GetCurrentWXOpenId;
                    string str3 = Globals.RequestQueryStr("code");
                    if (!string.IsNullOrEmpty(str3) && str3.Contains(","))
                    {
                        Globals.Debuglog("code问题获取值为：" + str3, "_DebugCodeErr.txt");
                        int num = Globals.RequestQueryNum("ReferralId");
                        this.Page.Response.Redirect("/default.aspx?ReferralId=" + num);
                    }
                    else if ((isMustLogin == 1) || (isMustLogin == 2))
                    {
                        if (string.IsNullOrEmpty(getCurrentWXOpenId))
                        {
                            Globals.Debuglog("WXOpenId问题获取值空：", "_DebugCodeErr.txt");
                            string urlToEncode = Regex.Replace(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, ""), "&code=(.*)&state=STATE", "");
                            string str5 = "snsapi_base";
                            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(urlToEncode) + "&response_type=code&scope=" + str5 + "&state=STATE#wechat_redirect";
                            this.Page.Response.Redirect(url);
                        }
                        else if (string.IsNullOrEmpty(str3))
                        {
                            Globals.Debuglog("进入1，必须登录的页面，并且无code请求" + this.Page.Request.Url.ToString(), "_debugtest.txt");
                            MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(getCurrentWXOpenId, "wx");
                            if ((openIdMember != null) && (openIdMember.Status == Convert.ToInt32(UserStatus.DEL)))
                            {
                                this.Page.Response.Redirect(Globals.ApplicationPath + "/logout.aspx");
                            }
                            else if (openIdMember == null)
                            {
                                string str7 = Regex.Replace(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, ""), "&code=(.*)&state=STATE", "");
                                string str8 = "snsapi_userinfo";
                                string str9 = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(str7) + "&response_type=code&scope=" + str8 + "&state=STATE#wechat_redirect";
                                this.Page.Response.Redirect(str9);
                            }
                            else if ((openIdMember.IsAuthorizeWeiXin == 0) || (!string.IsNullOrEmpty(openIdMember.UserHead) && openIdMember.UserHead.Contains("/templates/common/images/user.png")))
                            {
                                string str10 = Regex.Replace(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, ""), "&code=(.*)&state=STATE", "");
                                Globals.Debuglog("进入2，检测为匿名用户，并请求授权登录，去获得code参数" + this.Page.Request.Url.ToString(), "_debugtest.txt");
                                string str11 = "snsapi_userinfo";
                                string str12 = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(str10) + "&response_type=code&scope=" + str11 + "&state=STATE#wechat_redirect";
                                this.Page.Response.Redirect(str12);
                            }
                            else
                            {
                                this.setLogin(openIdMember.UserId);
                            }
                        }
                        else if ((Globals.GetCurrentMemberUserId(false) == 0) || (Globals.ToNum(this.Page.Session["userid"]) == 0))
                        {
                            Globals.Debuglog("进入4，code请求地址，获取用户基本信息" + this.Page.Request.Url.ToString(), "_debugtest.txt");
                            string responseResult = this.GetResponseResult("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + site.WeixinAppId + "&secret=" + site.WeixinAppSecret + "&code=" + str3 + "&grant_type=authorization_code");
                            if (responseResult.Contains("access_token"))
                            {
                                MemberInfo info2 = MemberProcessor.GetOpenIdMember(getCurrentWXOpenId, "wx");
                                if (info2 == null)
                                {
                                    Globals.Debuglog("进入16，检测用户信息为空，清空Cookie并跳转到首页" + this.Page.Request.Url.ToString(), "_debugtest.txt");
                                    Globals.ClearUserCookie();
                                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Default.aspx");
                                }
                                else
                                {
                                    Globals.Debuglog("进入5，准备获取到基本信息" + this.Page.Request.Url.ToString(), "_debugtest.txt");
                                    JObject obj2 = JsonConvert.DeserializeObject(responseResult) as JObject;
                                    string str14 = this.GetResponseResult("https://api.weixin.qq.com/sns/userinfo?access_token=" + obj2["access_token"].ToString() + "&openid=" + obj2["openid"].ToString() + "&lang=zh_CN");
                                    JObject obj3 = JsonConvert.DeserializeObject(str14) as JObject;
                                    if (str14.Contains("nickname"))
                                    {
                                        Globals.Debuglog("进入16，获取到基本信息" + this.Page.Request.Url.ToString(), "_debugtest.txt");
                                        if (string.IsNullOrEmpty(info2.UserHead) || info2.UserHead.ToString().Contains("/templates/common/images/user.png"))
                                        {
                                            MemberHelper.SetUserHeadAndUserName(getCurrentWXOpenId, obj3["headimgurl"].ToString(), Globals.UrlDecode(obj3["nickname"].ToString()), 1);
                                        }
                                        else if (info2.IsAuthorizeWeiXin != 1)
                                        {
                                            MemberHelper.SetUserHeadAndUserName(getCurrentWXOpenId, "", "", 2);
                                        }
                                        this.setLogin(info2.UserId);
                                    }
                                    else if (!string.IsNullOrEmpty(info2.UserHead) && !info2.UserHead.ToString().Contains("/templates/common/images/user.png"))
                                    {
                                        MemberHelper.SetUserHeadAndUserName(getCurrentWXOpenId, "", "", 2);
                                        this.setLogin(info2.UserId);
                                    }
                                }
                            }
                            else
                            {
                                string str15 = Regex.Replace(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, ""), "&code=(.*)&state=STATE", "");
                                Globals.Debuglog("获取不到access_token,说明需要授权登录，去获得code参数" + this.Page.Request.Url.ToString(), "_debugtest.txt");
                                string str16 = "snsapi_userinfo";
                                string str17 = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + site.WeixinAppId + "&redirect_uri=" + Globals.UrlEncode(str15) + "&response_type=code&scope=" + str16 + "&state=STATE#wechat_redirect";
                                this.Page.Response.Redirect(str17);
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                Globals.Debuglog(exception.ToString() + this.Page.Request.Url.ToString() + "openid:" + Globals.GetCurrentWXOpenId, "_DebugWXLogin.txt");
            }
        }

        protected virtual void WeiXinLoginTry()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string userAgent = this.Page.Request.UserAgent;
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            string str2 = HttpContext.Current.Request.Url.ToString().ToLower();
            if (((!str2.Contains("userlogin.aspx") && !str2.Contains("userlogining.aspx")) && !str2.Contains("register.aspx")) && ((((currentMember == null) || (this.Page.Session["userid"] == null)) || (this.Page.Session["userid"].ToString() != currentMember.UserId.ToString())) && (userAgent.ToLower().Contains("micromessenger") && masterSettings.IsValidationService)))
            {
                this.WeixinLoginAction(masterSettings, masterSettings.IsAutoToLogin ? 2 : 0);
            }
        }

        public void WriteError(string msg, string OpenId)
        {
        }

        public void WriteFuwuError(string msg, string OpenId)
        {
            Globals.Debuglog(msg + "||OpenId:" + OpenId, "_Debuglogfuwulogin.txt");
        }

        private bool SkinFileExists
        {
            get
            {
                return !string.IsNullOrEmpty(this.SkinName);
            }
        }

        public virtual string SkinName
        {
            get
            {
                return this.skinName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.ToLower(CultureInfo.InvariantCulture);
                    if (value.EndsWith(".html"))
                    {
                        this.skinName = value;
                    }
                }
            }
        }

        protected virtual string SkinPath
        {
            get
            {
                return (Globals.ApplicationPath + "/Templates/common/" + this.skinName);
            }
        }
    }
}

