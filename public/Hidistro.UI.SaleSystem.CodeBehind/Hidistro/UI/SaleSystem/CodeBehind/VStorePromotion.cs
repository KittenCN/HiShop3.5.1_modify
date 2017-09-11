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
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VStorePromotion : VshopTemplatedWebControl
    {
        private Literal litLinkurl;
        private Literal litStoreurl;
        private Literal litStroeDesc;
        private Literal litStroeName;
        private Image Logoimage;
        private HtmlImage storeCode;
        private HtmlImage storeFollowCode;
        private int userId;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("店铺推广");
            int userid = Globals.RequestQueryNum("ReferralId");
            if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
            {
                this.Page.Response.Redirect("/default.aspx?ReferralId=" + userid);
                this.Page.Response.End();
            }
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(this.userId);
            if (userIdDistributors == null)
            {
                this.Page.Response.Redirect("/default.aspx?ReferralId=" + this.userId.ToString());
                this.Page.Response.End();
            }
            else
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                bool isShowDistributorSelfStoreName = masterSettings.IsShowDistributorSelfStoreName;
                userid = userIdDistributors.UserId;
                this.litStroeDesc = (Literal) this.FindControl("litStroeDesc");
                this.litLinkurl = (Literal) this.FindControl("litLinkurl");
                this.litStoreurl = (Literal) this.FindControl("litStoreurl");
                string urlToEncode = Globals.FullPath("/Default.aspx?ReferralId=" + userid);
                this.litLinkurl.Text = urlToEncode;
                this.litStoreurl.Text = urlToEncode;
                this.Logoimage = (Image) this.FindControl("Logoimage");
                this.storeCode = (HtmlImage) this.FindControl("storeCode");
                this.storeFollowCode = (HtmlImage) this.FindControl("storeFollowCode");
                if (!string.IsNullOrEmpty(userIdDistributors.Logo))
                {
                    this.Logoimage.ImageUrl = Globals.HostPath(this.Page.Request.Url) + userIdDistributors.Logo;
                }
                else
                {
                    userIdDistributors.Logo = "/Utility/pics/headLogo.jpg";
                }
                this.storeCode.Src = "/Api/CreatQRCode.ashx?code=" + Globals.UrlEncode(urlToEncode) + "&Logo=" + userIdDistributors.Logo;
                if (masterSettings.IsValidationService)
                {
                    this.storeFollowCode.Src = "";
                    ScanInfos info = ScanHelp.GetScanInfosByUserId(userid, 0, "WX");
                    if (info == null)
                    {
                        ScanHelp.CreatNewScan(userid, "WX", 0);
                        info = ScanHelp.GetScanInfosByUserId(userid, 0, "WX");
                    }
                    string qRImageUrlByTicket = "";
                    if ((info != null) && !string.IsNullOrEmpty(info.CodeUrl))
                    {
                        qRImageUrlByTicket = BarCodeApi.GetQRImageUrlByTicket(info.CodeUrl);
                    }
                    else
                    {
                        string token = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
                        if (TokenApi.CheckIsRightToken(token))
                        {
                            string str4 = BarCodeApi.CreateTicket(token, info.Sceneid, "QR_LIMIT_SCENE", "2592000");
                            if (!string.IsNullOrEmpty(str4))
                            {
                                qRImageUrlByTicket = BarCodeApi.GetQRImageUrlByTicket(str4);
                                info.CodeUrl = str4;
                                info.CreateTime = DateTime.Now;
                                info.LastActiveTime = DateTime.Now;
                                ScanHelp.updateScanInfosCodeUrl(info);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(qRImageUrlByTicket))
                    {
                        this.storeFollowCode.Src = "/Api/CreatQRCode.ashx?Combin=" + Globals.UrlEncode(qRImageUrlByTicket) + "&Logo=" + userIdDistributors.Logo;
                    }
                    else
                    {
                        this.storeFollowCode.Src = "";
                    }
                }
                this.litStroeName = (Literal) this.FindControl("litStroeName");
                this.litStroeName.Text = userIdDistributors.StoreName;
                this.litStroeDesc.Text = userIdDistributors.StoreDescription;
                if (!isShowDistributorSelfStoreName)
                {
                    this.Logoimage.ImageUrl = masterSettings.DistributorLogoPic;
                    this.litStroeName.Text = masterSettings.SiteName;
                    this.litStroeDesc.Text = masterSettings.ShopIntroduction;
                    this.storeCode.Src = "/Api/CreatQRCode.ashx?code=" + Globals.UrlEncode(urlToEncode) + "&Logo=" + masterSettings.DistributorLogoPic;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VStorePromotion.html";
            }
            base.OnInit(e);
        }
    }
}

