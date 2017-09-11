namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VLoginGuide : VshopTemplatedWebControl
    {
        private HtmlInputHidden hidWeixinLoginUrl;
        private HtmlInputHidden hidWeixinNumber;
        private HtmlImage imgWeixin;

        protected override void AttachChildControls()
        {
            this.imgWeixin = (HtmlImage) this.FindControl("imgWeixin");
            this.hidWeixinNumber = (HtmlInputHidden) this.FindControl("hidWeixinNumber");
            this.hidWeixinLoginUrl = (HtmlInputHidden) this.FindControl("hidWeixinLoginUrl");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            this.hidWeixinNumber.Value = masterSettings.WeixinNumber;
            this.imgWeixin.Src = masterSettings.WeiXinCodeImageUrl;
            PageTitle.AddSiteNameTitle("登录向导");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vLoginGuide.html";
            }
            base.OnInit(e);
        }
    }
}

