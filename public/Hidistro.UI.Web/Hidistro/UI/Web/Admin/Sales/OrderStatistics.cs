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

    public class OrderStatistics : AdminPage
    {
        private DateTime? BeginDate;
        protected Button btnMonthView;
        protected Button btnSearch;
        protected Button btnWeekView;
        protected DropDownList ddlTop;
        private DateTime? EndDate;
        protected HtmlForm form1;
        public string FXCommissionFee;
        public int FXOrderNumber;
        public string FXResultPercent;
        public string FXSaleAmountFee;
        private int lastDay;
        public int OrderNumber;
        protected Pager pager;
        protected Repeater rptList;
        public string SaleAmountFee;
        private int Top;
        protected ucDateTimePicker txtBeginDate;
        protected ucDateTimePicker txtEndDate;

        protected OrderStatistics() : base("m10", "tjp02")
        {
            this.SaleAmountFee = "0";
            this.FXSaleAmountFee = "0";
            this.FXResultPercent = "0";
            this.FXCommissionFee = "0";
            this.Top = 10;
        }

        private void BindData()
        {
            OrderStatisticsQuery entity = new OrderStatisticsQuery {
                BeginDate = this.BeginDate,
                EndDate = this.EndDate,
                Top = new int?(this.Top),
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "SaleAmountFee"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult orderStatisticReport = ShopStatisticHelper.GetOrderStatisticReport(entity);
            DateTime beginDate = DateTime.Today.AddDays(-6.0);
            if (this.txtBeginDate.TextToDate.HasValue)
            {
                beginDate = this.txtBeginDate.TextToDate.Value;
            }
            DateTime now = DateTime.Now;
            if (this.txtEndDate.TextToDate.HasValue)
            {
                now = this.txtEndDate.TextToDate.Value;
            }
            DataRow drOne = ShopStatisticHelper.GetOrder_Member_CountInfo(beginDate, now);
            this.OrderNumber = base.GetFieldIntValue(drOne, "OrderNumber");
            this.SaleAmountFee = base.GetFieldDecimalValue(drOne, "SaleAmountFee").ToString("N2");
            this.FXOrderNumber = base.GetFieldIntValue(drOne, "FXOrderNumber");
            this.FXSaleAmountFee = base.GetFieldDecimalValue(drOne, "FXSaleAmountFee").ToString("N2");
            this.FXResultPercent = "0";
            if (base.GetFieldDecimalValue(drOne, "SaleAmountFee") > 0M)
            {
                this.FXResultPercent = Math.Round((decimal) ((base.GetFieldDecimalValue(drOne, "FXSaleAmountFee") / base.GetFieldDecimalValue(drOne, "SaleAmountFee")) * 100M), 2).ToString("N2");
            }
            this.FXCommissionFee = base.GetFieldDecimalValue(drOne, "FXCommissionFee").ToString("N2");
            this.rptList.DataSource = orderStatisticReport.Data;
            this.rptList.DataBind();
            this.pager.TotalRecords = orderStatisticReport.TotalRecords;
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

        protected void ddlTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.ReBind_Url(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                this.BeginDate = new DateTime?(DateTime.Today.AddDays(-6.0));
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
                this.ddlTop.SelectedValue = this.Top.ToString();
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
                this.Top = int.Parse(this.ddlTop.SelectedValue);
                int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (base.GetUrlParam("BeginDate") == "")
            {
                string retInfo = "";
                ShopStatisticHelper.StatisticsOrdersByRecDate(DateTime.Today, UpdateAction.AllUpdate, 0, out retInfo);
            }
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
            queryStrings.Add("lastDay", this.lastDay.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

