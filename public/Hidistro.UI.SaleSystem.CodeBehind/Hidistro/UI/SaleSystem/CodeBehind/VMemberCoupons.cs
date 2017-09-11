namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberCoupons : VMemberTemplatedWebControl
    {
        private Repeater rptCoupons;

        protected override void AttachChildControls()
        {
            int result = 2;
            int.TryParse(this.Page.Request.QueryString["usedType"], out result);
            DataTable userCoupons = MemberProcessor.GetUserCoupons(Globals.GetCurrentMemberUserId(false), result);
            if (userCoupons != null)
            {
                this.rptCoupons = (Repeater) this.FindControl("rptCoupons");
                this.rptCoupons.DataSource = userCoupons;
                this.rptCoupons.DataBind();
            }
            PageTitle.AddSiteNameTitle("优惠券");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vmemberCoupons.html";
            }
            base.OnInit(e);
        }
    }
}

