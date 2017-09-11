namespace Hidistro.UI.Web.Admin.Sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Drawing;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class OrderStatisticsDetail : AdminPage
    {
        private DateTime? BeginDate;
        protected Button btnMonthView;
        protected Button btnSearch;
        protected Button btnWeekView;
        public int BuyerNumber;
        private DateTime? EndDate;
        protected HtmlForm form1;
        public string FXBuyAvgPrice;
        public string FXCommissionFee;
        public int FXOrderNumber;
        public string FXSaleAmountFee;
        private int lastDay;
        protected Pager pager;
        protected Repeater rptList;
        private int Top;
        protected ucDateTimePicker txtBeginDate;
        protected ucDateTimePicker txtEndDate;
        public int UserId;

        protected OrderStatisticsDetail() : base("m10", "tjp02")
        {
            this.FXSaleAmountFee = "0";
            this.FXBuyAvgPrice = "0";
            this.FXCommissionFee = "0";
            this.Top = 10;
        }

        private void BindData()
        {
            OrderStatisticsQuery_UnderShop entity = new OrderStatisticsQuery_UnderShop {
                BeginDate = this.BeginDate,
                EndDate = this.EndDate,
                Top = new int?(this.Top),
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "SaleAmountFee",
                ShopLevel = 1,
                AgentId = new int?(base.GetUrlIntParam("UserId"))
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = ShopStatisticHelper.GetOrderStatisticReport_UnderShop(entity);
            this.pager.TotalRecords = result.TotalRecords;
            this.rptList.DataSource = result.Data;
            this.rptList.DataBind();
            DataRow orderStatisticReportGlobalByAgentID = ShopStatisticHelper.GetOrderStatisticReportGlobalByAgentID(entity);
            this.FXOrderNumber = base.GetFieldIntValue(orderStatisticReportGlobalByAgentID, "OrderNumber");
            this.BuyerNumber = base.GetFieldIntValue(orderStatisticReportGlobalByAgentID, "BuyerNumber");
            this.FXSaleAmountFee = base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "SaleAmountFee").ToString("N2");
            this.FXBuyAvgPrice = "0";
            if (base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "BuyerNumber") > 0M)
            {
                this.FXBuyAvgPrice = Math.Round((decimal) (base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "SaleAmountFee") / base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "BuyerNumber")), 2).ToString("N2");
            }
            this.FXCommissionFee = base.GetFieldDecimalValue(orderStatisticReportGlobalByAgentID, "CommissionAmountFee").ToString("N2");
        }

        protected void btnMonthView_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.BeginDate = new DateTime?(now.AddDays(-29.0));
            this.EndDate = new DateTime?(now);
            this.lastDay = 30;
            this.ReBind_Url(true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.txtBeginDate.TextToDate.HasValue)
            {
                this.BeginDate = new DateTime?(this.txtBeginDate.TextToDate.Value);
            }
            if (this.txtEndDate.TextToDate.HasValue)
            {
                this.EndDate = new DateTime?(this.txtEndDate.TextToDate.Value);
            }
            this.lastDay = 0;
            this.ReBind_Url(true);
        }

        protected void btnWeekView_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.BeginDate = new DateTime?(now.AddDays(-6.0));
            this.EndDate = new DateTime?(now);
            this.lastDay = 7;
            this.ReBind_Url(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                this.BeginDate = new DateTime?(DateTime.Today.AddDays(-7.0));
                this.EndDate = new DateTime?(DateTime.Today);
                this.Top = 10;
                if (base.GetUrlParam("BeginDate") != "")
                {
                    this.BeginDate = new DateTime?(DateTime.Parse(base.GetUrlParam("BeginDate")));
                }
                if (base.GetUrlParam("EndDate") != "")
                {
                    this.EndDate = new DateTime?(DateTime.Parse(base.GetUrlParam("EndDate")));
                }
                if (base.GetUrlParam("Top") != "")
                {
                    this.Top = int.Parse(base.GetUrlParam("Top"));
                }
                this.txtBeginDate.TextToDate = this.BeginDate;
                this.txtEndDate.TextToDate = this.EndDate;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
                {
                    int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
                    if (this.lastDay >= 7)
                    {
                        this.btnWeekView.BorderColor = (this.lastDay == 7) ? ColorTranslator.FromHtml("#1CA47D") : ColorTranslator.FromHtml("");
                        this.btnMonthView.BorderColor = (this.lastDay == 30) ? ColorTranslator.FromHtml("#1CA47D") : ColorTranslator.FromHtml("");
                    }
                    else
                    {
                        this.btnWeekView.BorderColor = ColorTranslator.FromHtml("");
                        this.btnMonthView.BorderColor = ColorTranslator.FromHtml("");
                    }
                }
            }
            else
            {
                if (this.txtBeginDate.TextToDate.HasValue)
                {
                    this.BeginDate = new DateTime?(this.txtBeginDate.TextToDate.Value);
                }
                if (this.txtEndDate.TextToDate.HasValue)
                {
                    this.EndDate = new DateTime?(this.txtEndDate.TextToDate.Value);
                }
                int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
            }
            this.UserId = base.GetUrlIntParam("UserId");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind_Url(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("BeginDate", this.BeginDate.ToString());
            queryStrings.Add("EndDate", this.EndDate.ToString());
            queryStrings.Add("Top", this.Top.ToString());
            queryStrings.Add("UserId", this.UserId.ToString());
            queryStrings.Add("lastDay", this.lastDay.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

