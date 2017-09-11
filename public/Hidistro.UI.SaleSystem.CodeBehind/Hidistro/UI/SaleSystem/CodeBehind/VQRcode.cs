namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VQRcode : VshopTemplatedWebControl
    {
        private Literal litgotourl;
        private Literal litimage;
        private Literal litItemParams;
        private Literal litstorename;
        private Literal liturl;

        protected override void AttachChildControls()
        {
            this.litimage = (Literal) this.FindControl("litimage");
            this.litgotourl = (Literal) this.FindControl("litgotourl");
            this.liturl = (Literal) this.FindControl("liturl");
            this.litstorename = (Literal) this.FindControl("litstorename");
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReferralId"]))
            {
                this.liturl.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/Default.aspx?ReferralId=" + this.Page.Request.QueryString["ReferralId"];
                this.litgotourl.Text = this.liturl.Text;
                this.litstorename.Text = DistributorsBrower.GetCurrentDistributors(int.Parse(this.Page.Request.QueryString["ReferralId"]), true).StoreName;
            }
            this.litimage.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/Storage/master/QRcord.jpg";
            PageTitle.AddSiteNameTitle(this.litstorename.Text + "店铺二维码");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str = "";
            if (!string.IsNullOrEmpty(masterSettings.ShopSpreadingCodePic))
            {
                str = Globals.HostPath(HttpContext.Current.Request.Url) + masterSettings.ShopSpreadingCodePic;
            }
            this.litItemParams.Text = str + "|" + masterSettings.ShopSpreadingCodeName + "|" + masterSettings.ShopSpreadingCodeDescription.Replace("|", "｜");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VQRcode.html";
            }
            base.OnInit(e);
        }
    }
}

