namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class DistributorStatistics : AdminPage
    {
        protected Button btnQueryLogs;
        protected Button Button1;
        protected Button Button4;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        private string EndTime;
        private int i;
        public int lastDay;
        protected Pager pager;
        protected Repeater reDistributor;
        private int rows;
        private string StartTime;

        protected DistributorStatistics() : base("m05", "fxp05")
        {
            this.StartTime = "";
            this.EndTime = "";
        }

        private void BindData()
        {
            DbQueryResult result = VShopHelper.GetDistributorsRankings(this.StartTime, this.EndTime, this.pager.PageSize, this.pager.PageIndex);
            this.reDistributor.DataSource = result.Data;
            this.reDistributor.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
        }

        protected void btnQueryLogs_Click(object sender, EventArgs e)
        {
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            this.lastDay = 0;
            this.ReBind(true);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
            this.lastDay = 7;
            this.ReBind(true);
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
            this.lastDay = 30;
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
                {
                    this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
                    this.calendarStartDate.SelectedDate = new DateTime?(DateTime.Parse(this.StartTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
                {
                    this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
                    this.calendarEndDate.SelectedDate = new DateTime?(DateTime.Parse(this.EndTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
                {
                    int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
                    if (this.lastDay == 30)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("#FF00CC");
                    }
                    else if (this.lastDay == 7)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("#FF00CC");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                    else
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                }
            }
            else
            {
                if (this.calendarStartDate.SelectedDate.HasValue)
                {
                    this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (this.calendarEndDate.SelectedDate.HasValue)
                {
                    this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.reDistributor.ItemDataBound += new RepeaterItemEventHandler(this.reDistributor_ItemDataBound);
            this.BindData();
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("StartTime", this.StartTime);
            queryStrings.Add("EndTime", this.EndTime);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("lastDay", this.lastDay.ToString());
            base.ReloadPage(queryStrings);
        }

        private void reDistributor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.FindControl("litph");
                this.i++;
                this.rows = ((this.pager.PageIndex - 1) * this.pager.PageSize) + this.i;
                if (this.rows == 1)
                {
                    literal.Text = "<img src=\"../images/0001.gif\"></img>";
                }
                else if (this.rows == 2)
                {
                    literal.Text = "<img src=\"../images/0002.gif\"></img>";
                }
                else if (this.rows == 3)
                {
                    literal.Text = "<img src=\"../images/0003.gif\"></img>";
                }
                else
                {
                    literal.Text = (int.Parse(literal.Text) + this.rows).ToString();
                }
            }
        }
    }
}

