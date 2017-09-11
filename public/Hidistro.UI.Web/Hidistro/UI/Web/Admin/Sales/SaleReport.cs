namespace Hidistro.UI.Web.Admin.Sales
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Drawing;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SaleReport : AdminPage
    {
        private DateTime? BeginDate;
        protected Button btnMonthView;
        protected Button btnSearch;
        protected Button btnWeekView;
        public decimal BuyerAvgPrice;
        public decimal BuyerNumber;
        public string DateListA;
        public string DateListB;
        private DateTime? EndDate;
        protected HtmlForm form1;
        private int lastDay;
        public int NewAgentNumber;
        public int NewMemberNumber;
        public decimal OrderNumber;
        public string QtyListA1;
        public string QtyListA2;
        public string QtyListB1;
        public string QtyListB2;
        public decimal SaleAmountFee;
        protected ucDateTimePicker txtBeginDate;
        protected ucDateTimePicker txtEndDate;

        protected SaleReport() : base("m10", "tjp01")
        {
            this.DateListA = "";
            this.DateListB = "";
            this.QtyListA1 = "";
            this.QtyListA2 = "";
            this.QtyListB1 = "";
            this.QtyListB2 = "";
        }

        private void BindData()
        {
            DateTime beginDate = DateTime.Today.AddDays(-6.0);
            if (this.txtBeginDate.TextToDate.HasValue)
            {
                beginDate = DateTime.Parse(this.txtBeginDate.TextToDate.Value.ToString());
            }
            DateTime now = DateTime.Now;
            if (this.txtEndDate.TextToDate.HasValue)
            {
                now = DateTime.Parse(this.txtEndDate.TextToDate.Value.ToString());
            }
            DataRow drOne = ShopStatisticHelper.GetOrder_Member_CountInfo(beginDate, now);
            DataTable saleReport = ShopStatisticHelper.GetSaleReport(beginDate, now);
            TimeSpan span = (TimeSpan) (now - beginDate);
            int days = span.Days;
            this.OrderNumber = base.GetFieldDecimalValue(drOne, "OrderNumber");
            this.BuyerNumber = base.GetFieldDecimalValue(drOne, "BuyerNumber");
            this.SaleAmountFee = base.GetFieldDecimalValue(drOne, "SaleAmountFee");
            this.BuyerAvgPrice = 0M;
            if (this.BuyerNumber > 0M)
            {
                this.BuyerAvgPrice = Math.Round((decimal) (this.SaleAmountFee / this.BuyerNumber), 2);
            }
            this.NewMemberNumber = base.GetFieldIntValue(drOne, "NewMemberNumber");
            this.NewAgentNumber = base.GetFieldIntValue(drOne, "NewAgentNumber");
            this.DateListA = "";
            this.DateListB = "";
            int num2 = 0;
            days = saleReport.Rows.Count;
            foreach (DataRow row2 in saleReport.Rows)
            {
                this.DateListA = this.DateListA + "'" + Convert.ToDateTime(row2["RecDate"].ToString()).ToString("yyyy-MM-dd") + "'";
                this.QtyListA1 = this.QtyListA1 + base.GetFieldIntValue(row2, "OrderNumber").ToString();
                this.QtyListA2 = this.QtyListA2 + base.GetFieldDecimalValue(row2, "SaleAmountFee").ToString();
                this.QtyListB1 = this.QtyListB1 + base.GetFieldIntValue(row2, "NewMemberNumber").ToString();
                this.QtyListB2 = this.QtyListB2 + base.GetFieldIntValue(row2, "NewAgentNumber").ToString();
                if (num2 < (days - 1))
                {
                    this.DateListA = this.DateListA + ",";
                    this.QtyListA1 = this.QtyListA1 + ",";
                    this.QtyListA2 = this.QtyListA2 + ",";
                    this.DateListB = this.DateListB + ",";
                    this.QtyListB1 = this.QtyListB1 + ",";
                    this.QtyListB2 = this.QtyListB2 + ",";
                }
                num2++;
            }
            this.DateListB = this.DateListA;
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            bool hasValue = this.txtBeginDate.TextToDate.HasValue;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            this.txtBeginDate.TextToDate = new DateTime?(DateTime.Today.AddDays(-3.0));
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                this.BeginDate = new DateTime?(DateTime.Today.AddDays(-6.0));
                this.EndDate = new DateTime?(DateTime.Today);
                if (base.GetUrlParam("BeginDate") != "")
                {
                    this.BeginDate = new DateTime?(DateTime.Parse(base.GetUrlParam("BeginDate")));
                }
                if (base.GetUrlParam("EndDate") != "")
                {
                    this.EndDate = new DateTime?(DateTime.Parse(base.GetUrlParam("EndDate")));
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
            if (!base.IsPostBack)
            {
                this.LoadParameters();
                this.BindData();
            }
        }

        private void ReBind_Url(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("BeginDate", this.BeginDate.ToString());
            queryStrings.Add("EndDate", this.EndDate.ToString());
            queryStrings.Add("lastDay", this.lastDay.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

