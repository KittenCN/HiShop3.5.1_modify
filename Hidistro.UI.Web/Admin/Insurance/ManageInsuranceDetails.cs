namespace Hidistro.UI.Web.Admin.Insurance
{
    using ASPNET.WebControls;
    using Common.Controls;
    using Hidistro.ControlPanel.CashBack;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.CashBack;
    using Hidistro.Entities.Members;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageInsuranceDetails : Page
    {
        protected Panel divEmpty;
        protected HtmlForm form1;
        protected Literal litAmount;
        protected Literal litCashBackAmount;
        protected Literal litCashBackType;
        protected Literal litFinished;
        protected Literal litIsValid;
        protected Literal litPercentage;
        protected Literal litPoints;
        protected Literal litRechargeAmount;
        protected Literal litUserName;
        protected Pager pager;
      
        protected Repeater rptList;

        private void BindCashBack()
        {
            CashBackInfo cashBackInfo = CashBackHelper.GetCashBackInfo(int.Parse(base.Request.QueryString["CashBackId"]));
            MemberInfo member = MemberHelper.GetMember(cashBackInfo.UserId);
            this.litUserName.Text = member.UserName;
            this.litAmount.Text = member.AvailableAmount.ToString("F2");
            this.litPoints.Text = member.Points.ToString();
            this.litRechargeAmount.Text = cashBackInfo.RechargeAmount.ToString("f2");
            this.litIsValid.Text = cashBackInfo.IsValid ? "有效" : "失效";
            this.litCashBackAmount.Text = cashBackInfo.CashBackAmount.ToString("F2");
            this.litPercentage.Text = cashBackInfo.Percentage.ToString("F2") + "%";
            this.litFinished.Text = cashBackInfo.IsFinished ? "已完成" : "返现中";
            this.litCashBackType.Text = cashBackInfo.CashBackType.ToString();
        }

        private void BindCashBackDetails()
        {
            CashBackDetailsQuery query = new CashBackDetailsQuery {
                CashBackId = int.Parse(base.Request.QueryString["CashBackId"]),
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortBy = "CashBackDetailsId",
                SortOrder = SortAction.Desc
            };
            DbQueryResult cashBackDetailsByPager = CashBackHelper.GetCashBackDetailsByPager(query);
            this.rptList.DataSource = cashBackDetailsByPager.Data;
            this.rptList.DataBind();
            this.pager.TotalRecords = cashBackDetailsByPager.TotalRecords;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.BindCashBack();
                this.BindCashBackDetails();
            }
        }
    }
}

