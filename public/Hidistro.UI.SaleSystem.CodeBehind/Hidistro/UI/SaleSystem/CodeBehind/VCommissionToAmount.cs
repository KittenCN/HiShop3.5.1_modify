namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VCommissionToAmount : VMemberTemplatedWebControl
    {
        protected decimal surpluscommission = 0.00M;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("佣金转余额");
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false));
            if ((userIdDistributors != null) && (userIdDistributors.UserId > 0))
            {
                this.surpluscommission = userIdDistributors.ReferralBlance;
                decimal num = DistributorsBrower.CommionsRequestSumMoney(userIdDistributors.UserId);
                this.surpluscommission -= num;
            }
            HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("MaxCommission");
            hidden.Value = Math.Round((decimal) (this.surpluscommission - 0.005M), 2).ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-CommissionToAmount.html";
            }
            base.OnInit(e);
        }
    }
}

