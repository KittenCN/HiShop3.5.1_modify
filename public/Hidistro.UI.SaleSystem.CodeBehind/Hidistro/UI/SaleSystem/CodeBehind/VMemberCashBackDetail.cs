namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.CashBack;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Entities.CashBack;
    using Hidistro.Entities.Members;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberCashBackDetail : VMemberTemplatedWebControl
    {
        private Literal litAmount;
        private Literal litCashBackAmount;
        private Literal litFinished;
        private Literal litIsValid;
        private Literal litPercentage;
        private Literal litPoints;
        private Literal litRechargeAmount;
        private Literal litType;
        private Literal litUserName;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("优惠金额");
            this.litUserName = this.FindControl("litUserName") as Literal;
            this.litAmount = this.FindControl("litAmount") as Literal;
            this.litPoints = this.FindControl("litPoints") as Literal;
            this.litType = this.FindControl("litType") as Literal;
            this.litIsValid = this.FindControl("litIsValid") as Literal;
            this.litRechargeAmount = this.FindControl("litRechargeAmount") as Literal;
            this.litCashBackAmount = this.FindControl("litCashBackAmount") as Literal;
            this.litPercentage = this.FindControl("litPercentage") as Literal;
            this.litFinished = this.FindControl("litFinished") as Literal;
            this.BindCashBack();
        }

        private void BindCashBack()
        {
            CashBackInfo cashBackInfo = CashBackHelper.GetCashBackInfo(int.Parse(this.Page.Request.QueryString["Id"]));
            MemberInfo member = MemberHelper.GetMember(cashBackInfo.UserId);
            this.litUserName.Text = member.UserName;
            this.litAmount.Text = member.AvailableAmount.ToString("F2");
            this.litPoints.Text = member.Points.ToString();
            this.litRechargeAmount.Text = cashBackInfo.RechargeAmount.ToString("f2");
            this.litIsValid.Text = cashBackInfo.IsValid ? "有效" : "失效";
            this.litCashBackAmount.Text = cashBackInfo.CashBackAmount.ToString("F2");
            this.litPercentage.Text = cashBackInfo.Percentage.ToString("F2") + "%";
            this.litFinished.Text = cashBackInfo.IsFinished ? "已完成" : "返现中";
            this.litType.Text = cashBackInfo.CashBackType.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMemberCashBackDetail.html";
            }
            base.OnInit(e);
        }
    }
}

