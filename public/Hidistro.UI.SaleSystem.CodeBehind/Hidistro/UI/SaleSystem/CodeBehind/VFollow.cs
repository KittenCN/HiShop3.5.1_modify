namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VFollow : VshopTemplatedWebControl
    {
        private Literal litJs;

        protected override void AttachChildControls()
        {
            string str = Globals.RequestQueryNum("ReferralId").ToString();
            this.litJs = (Literal) this.FindControl("litJs");
            this.litJs.Text = "<script type=\"text/javascript\">window.location.href='/default.aspx?ReferralId=" + str + "&go=1'</script>";
            PageTitle.AddSiteNameTitle("关注引导页");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vFollow.html";
            }
            base.OnInit(e);
        }
    }
}

