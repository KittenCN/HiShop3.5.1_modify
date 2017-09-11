namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.Web.UI.WebControls;

    public class ShowQRCode : AdminPage
    {
        protected Image imgQRCode;
        protected Script Script5;
        protected Script Script6;

        protected ShowQRCode() : base("m06", "wxp12")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack && (base.Request.QueryString["action"] == "show"))
            {
                this.ShowQRCodeImage();
            }
        }

        protected void ShowQRCodeImage()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str = base.Request.QueryString["id"];
            if (string.IsNullOrEmpty(str))
            {
                base.Response.Redirect("WifiSetList.aspx");
            }
            string wifiInfo = "wifi_" + str;
            wifiInfo = str;
            string qRImageUrlByTicket = BarCodeApi.GetQRImageUrlByTicket(BarCodeApi.CreateTicketWifi(TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret), wifiInfo));
            this.imgQRCode.ImageUrl = qRImageUrlByTicket;
        }
    }
}

