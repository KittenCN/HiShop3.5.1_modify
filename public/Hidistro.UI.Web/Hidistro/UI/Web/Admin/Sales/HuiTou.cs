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

    public class HuiTou : AdminPage
    {
        private DateTime? BeginDate;
        protected Button btnMonthView;
        protected Button btnSearch;
        protected Button btnWeekView;
        public int BuyerNumber;
        public string DateList;
        private DateTime? EndDate;
        protected HtmlForm form1;
        private int lastDay;
        public int OldMember;
        public string OldMemberPercent;
        public string QtyListAll;
        public string QtyListNew;
        public string QtyListOld;
        private int Top;
        protected ucDateTimePicker txtBeginDate;
        protected ucDateTimePicker txtEndDate;

        protected HuiTou() : base("m10", "tjp03")
        {
            this.OldMemberPercent = "0";
            this.Top = 10;
            this.QtyListOld = "";
            this.QtyListNew = "";
            this.QtyListAll = "";
            this.DateList = "";
        }

        private void BindData()
        {
            DataSet set = ShopStatisticHelper.GetOrder_Member_Rebuy(this.BeginDate.Value, this.EndDate.Value);
            DataTable table = set.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                this.OldMember += base.GetFieldIntValue(row, "OldBuy");
                this.BuyerNumber += base.GetFieldIntValue(row, "totalBuy");
            }
            this.OldMemberPercent = "0";
            if (this.BuyerNumber > 0)
            {
                this.OldMemberPercent = ((Convert.ToDecimal(this.OldMember) / Convert.ToDecimal(this.BuyerNumber)) * 100M).ToString("N2");
            }
            DataTable table2 = set.Tables[1];
            DataTable table3 = new DataTable();
            table3.Columns.Add("PayDate", typeof(string));
            table3.Columns.Add("NewMemberQty", typeof(int));
            table3.Columns.Add("OldMemberQty", typeof(int));
            TimeSpan span = this.EndDate.Value - this.BeginDate.Value;
            int num = span.Days + 1;
            for (int i = 0; i < num; i++)
            {
                DataRow row2 = table3.NewRow();
                row2["PayDate"] = this.BeginDate.Value.AddDays((double) i).ToString("yyyy-MM-dd");
                row2["OldMemberQty"] = 0;
                row2["NewMemberQty"] = 0;
                DataRow[] rowArray = table2.Select("gpDate='" + this.BeginDate.Value.AddDays((double) i).ToString("yyyy-MM-dd") + "' ");
                if (rowArray.Length > 0)
                {
                    row2["OldMemberQty"] = rowArray[0]["OldBuy"];
                    row2["NewMemberQty"] = (int.Parse(rowArray[0]["TotalBuy"].ToString()) - int.Parse(rowArray[0]["OldBuy"].ToString())).ToString();
                }
                table3.Rows.Add(row2);
                this.DateList = this.DateList + "'" + Convert.ToDateTime(row2["PayDate"].ToString()).ToString("yyyy-MM-dd") + "'";
                this.QtyListNew = this.QtyListNew + base.GetFieldIntValue(row2, "NewMemberQty").ToString();
                this.QtyListOld = this.QtyListOld + base.GetFieldDecimalValue(row2, "OldMemberQty").ToString();
                int fieldIntValue = base.GetFieldIntValue(row2, "NewMemberQty");
                int num4 = base.GetFieldIntValue(row2, "OldMemberQty");
                this.QtyListAll = this.QtyListAll + ((fieldIntValue + num4)).ToString();
                if (i < (num - 1))
                {
                    this.DateList = this.DateList + ",";
                    this.QtyListNew = this.QtyListNew + ",";
                    this.QtyListOld = this.QtyListOld + ",";
                    this.QtyListAll = this.QtyListAll + ",";
                }
            }
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
                this.Top = 10;
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
            base.ReloadPage(queryStrings);
        }
    }
}

