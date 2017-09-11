namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hishop.Weixin.MP.Api;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VStoreCard : VshopTemplatedWebControl
    {
        private HtmlControl editPanel;
        private HtmlImage imglogo;
        private HtmlInputHidden ShareInfo;
        private SiteSettings siteSettings = SettingsManager.GetMasterSettings(true);
        private int userId;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["ReferralId"], out this.userId))
            {
                this.Context.Response.Redirect("/");
            }
            else
            {
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(this.userId);
                if (userIdDistributors == null)
                {
                    this.Context.Response.Redirect("/");
                }
                else
                {
                    this.imglogo = (HtmlImage) this.FindControl("QrcodeImg");
                    int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
                    this.editPanel = (HtmlControl) this.FindControl("editPanel");
                    this.editPanel.Visible = false;
                    if (currentMemberUserId == this.userId)
                    {
                        this.imglogo.Attributes.Add("Admin", "true");
                        MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                        DateTime cardCreatTime = userIdDistributors.CardCreatTime;
                        string str = File.ReadAllText(HttpRuntime.AppDomainAppPath.ToString() + "Storage/Utility/StoreCardSet.js");
                        JObject obj2 = JsonConvert.DeserializeObject(str) as JObject;
                        DateTime time2 = new DateTime();
                        if ((obj2 != null) && (obj2["writeDate"] != null))
                        {
                            time2 = DateTime.Parse(obj2["writeDate"].ToString());
                        }
                        ScanInfos info = ScanHelp.GetScanInfosByUserId(currentMember.UserId, 0, "WX");
                        if (info == null)
                        {
                            ScanHelp.CreatNewScan(currentMember.UserId, "WX", 0);
                            info = ScanHelp.GetScanInfosByUserId(currentMember.UserId, 0, "WX");
                        }
                        string codeUrl = "";
                        if (info == null)
                        {
                            codeUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Follow.aspx?ReferralId=" + currentMember.UserId.ToString();
                        }
                        else
                        {
                            codeUrl = info.CodeUrl;
                            if (string.IsNullOrEmpty(codeUrl))
                            {
                                string token = TokenApi.GetToken_Message(this.siteSettings.WeixinAppId, this.siteSettings.WeixinAppSecret);
                                if (TokenApi.CheckIsRightToken(token))
                                {
                                    string str4 = BarCodeApi.CreateTicket(token, info.Sceneid, "QR_LIMIT_SCENE", "2592000");
                                    if (!string.IsNullOrEmpty(str4))
                                    {
                                        codeUrl = str4;
                                        info.CodeUrl = str4;
                                        info.CreateTime = DateTime.Now;
                                        info.LastActiveTime = DateTime.Now;
                                        ScanHelp.updateScanInfosCodeUrl(info);
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(codeUrl))
                            {
                                codeUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/Follow.aspx?ReferralId=" + currentMember.UserId.ToString();
                            }
                            else
                            {
                                codeUrl = BarCodeApi.GetQRImageUrlByTicket(codeUrl);
                            }
                        }
                        if (string.IsNullOrEmpty(userIdDistributors.StoreCard) || (cardCreatTime < time2))
                        {
                            string storeName = userIdDistributors.StoreName;
                            if (!this.siteSettings.IsShowDistributorSelfStoreName)
                            {
                                storeName = this.siteSettings.SiteName;
                            }
                            StoreCardCreater creater = new StoreCardCreater(str, currentMember.UserHead, userIdDistributors.Logo, codeUrl, currentMember.UserName, storeName, this.userId, this.userId);
                            string imgUrl = "";
                            if (creater.ReadJson() && creater.CreadCard(out imgUrl))
                            {
                                DistributorsBrower.UpdateStoreCard(this.userId, imgUrl);
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(userIdDistributors.StoreCard))
                    {
                        userIdDistributors.StoreCard = "/Storage/master/DistributorCards/StoreCard" + this.userId.ToString() + ".jpg";
                    }
                    this.ShareInfo = (HtmlInputHidden) this.FindControl("ShareInfo");
                    this.imglogo.Src = userIdDistributors.StoreCard;
                    PageTitle.AddSiteNameTitle("掌柜名片");
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            string str = HttpContext.Current.Request["action"];
            if (str == "ReCreadt")
            {
                HttpContext.Current.Response.ContentType = "application/json";
                string str2 = HttpContext.Current.Request["imageUrl"];
                string s = "";
                if (string.IsNullOrEmpty(str2))
                {
                    s = "{\"success\":\"false\",\"message\":\"图片地址为空\"}";
                }
                try
                {
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
                    string str4 = str2;
                    string str5 = str2;
                    ScanInfos infos = ScanHelp.GetScanInfosByUserId(currentMember.UserId, 0, "WX");
                    if (infos == null)
                    {
                        ScanHelp.CreatNewScan(currentMember.UserId, "WX", 0);
                        infos = ScanHelp.GetScanInfosByUserId(currentMember.UserId, 0, "WX");
                    }
                    string qRImageUrlByTicket = "";
                    if (infos == null)
                    {
                        qRImageUrlByTicket = Globals.HostPath(HttpContext.Current.Request.Url) + "/Follow.aspx?ReferralId=" + currentMember.UserId.ToString();
                    }
                    else
                    {
                        qRImageUrlByTicket = BarCodeApi.GetQRImageUrlByTicket(infos.CodeUrl);
                    }
                    string str7 = File.ReadAllText(HttpRuntime.AppDomainAppPath.ToString() + "Storage/Utility/StoreCardSet.js");
                    string storeName = userIdDistributors.StoreName;
                    if (!this.siteSettings.IsShowDistributorSelfStoreName)
                    {
                        storeName = this.siteSettings.SiteName;
                    }
                    StoreCardCreater creater = new StoreCardCreater(str7, str4, str5, qRImageUrlByTicket, currentMember.UserName, storeName, currentMember.UserId, currentMember.UserId);
                    string imgUrl = "";
                    if (creater.ReadJson() && creater.CreadCard(out imgUrl))
                    {
                        s = "{\"success\":\"true\",\"message\":\"生成成功\"}";
                        DistributorsBrower.UpdateStoreCard(currentMember.UserId, imgUrl);
                    }
                    else
                    {
                        s = "{\"success\":\"false\",\"message\":\"" + imgUrl + "\"}";
                    }
                }
                catch (Exception exception)
                {
                    s = "{\"success\":\"false\",\"message\":\"" + exception.Message + "\"}";
                }
                HttpContext.Current.Response.Write(s);
                HttpContext.Current.Response.End();
            }
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VStoreCard.html";
            }
            base.OnInit(e);
        }
    }
}

