namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VLoginOut : SimpleTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            string str = Globals.GetCurrentDistributorId().ToString();
            Globals.ClearUserCookie();
            Literal literal = (Literal) this.FindControl("litJs");
            literal.Text = "<script type=\"text/javascript\">window.location.href='/default.aspx?ReferralId=" + str + "'</script>";
            PageTitle.AddSiteNameTitle("退出登录");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VLogout.html";
            }
            base.OnInit(e);
        }
    }
}

