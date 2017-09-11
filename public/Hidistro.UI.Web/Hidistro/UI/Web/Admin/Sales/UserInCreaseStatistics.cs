namespace Hidistro.UI.Web.Admin.Sales
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
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

    public class UserInCreaseStatistics : AdminPage
    {
        private DateTime? BeginDate;
        protected Button btnMonthView;
        protected Button btnSearch;
        protected Button btnWeekView;
        public string DateList;
        private DateTime? EndDate;
        protected HtmlForm form1;
        private int lastDay;
        public string QtyList1;
        protected ucDateTimePicker txtBeginDate;
        protected ucDateTimePicker txtEndDate;

        protected UserInCreaseStatistics() : base("m10", "tjp08")
        {
            this.QtyList1 = "";
            this.DateList = "";
        }

        private void BindData()
        {
            OrderStatisticsQuery entity = new OrderStatisticsQuery();
            DateTime local1 = this.BeginDate.Value;
            entity.BeginDate = this.txtBeginDate.TextToDate;
            DateTime local2 = this.EndDate.Value;
            entity.EndDate = this.txtEndDate.TextToDate;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "RecDate";
            Globals.EntityCoding(entity, true);
            DataTable dtDist = ShopStatisticHelper.Member_GetInCreateReport(entity);
            TimeSpan span = entity.EndDate.Value - entity.BeginDate.Value;
            this.lastDay = span.Days + 1;
            this.LoadTradeDataList(dtDist, this.BeginDate.Value, this.lastDay);
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

        private void LoadTradeDataList(DataTable dtDist, DateTime BeginDate, int Days)
        {
            this.DateList = "";
            int num = 0;
            for (int i = 0; i < Days; i++)
            {
                DataRow[] rowArray = dtDist.Select("RecDate='" + BeginDate.AddDays((double) i).ToString("yyyy-MM-dd") + "' ");
                if (rowArray.Length > 0)
                {
                    this.QtyList1 = this.QtyList1 + base.GetFieldValue(rowArray[0], "NewMemberNumber");
                }
                else
                {
                    this.QtyList1 = this.QtyList1 + "0";
                }
                this.DateList = this.DateList + "'" + BeginDate.AddDays((double) i).ToString("yyyy-MM-dd") + "'";
                if (num < (Days - 1))
                {
                    this.DateList = this.DateList + ",";
                    this.QtyList1 = this.QtyList1 + ",";
                }
                num++;
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
            queryStrings.Add("lastDay", this.lastDay.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

