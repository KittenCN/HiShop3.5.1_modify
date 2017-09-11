namespace Hidistro.UI.Web.Admin.Member
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MemberRanking)]
    public class MemberRanking_1 : AdminPage
    {
        protected LinkButton btnCreateReport;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected DropDownList ddlSort;
        protected Grid grdProductSaleStatistics;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string sortBy;
        protected HtmlForm thisForm;

        protected MemberRanking_1() : base("m04", "hyp05")
        {
            this.sortBy = "SaleTotals";
        }

        private void BindMemberRanking()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery {
                StartDate = this.dateStart,
                EndDate = this.dateEnd,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortBy = this.sortBy,
                SortOrder = SortAction.Desc
            };
            int totalProductSales = 0;
            DataTable memberStatistics = SalesHelper.GetMemberStatistics(query, out totalProductSales);
            this.grdProductSaleStatistics.DataSource = memberStatistics;
            this.grdProductSaleStatistics.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = totalProductSales;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery {
                StartDate = this.dateStart,
                EndDate = this.dateEnd,
                SortBy = this.sortBy,
                SortOrder = SortAction.Desc
            };
            DataTable memberStatisticsNoPage = SalesHelper.GetMemberStatisticsNoPage(query);
            string s = (string.Empty + "会员") + ",订单数" + ",消费金额\r\n";
            foreach (DataRow row in memberStatisticsNoPage.Rows)
            {
                s = s + row["UserName"].ToString();
                s = s + "," + row["OrderCount"].ToString();
                s = s + "," + row["SaleTotals"].ToString() + "\r\n";
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberRanking.csv");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void grdProductSaleStatistics_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["dateEnd"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
                {
                    this.sortBy = base.Server.UrlDecode(this.Page.Request.QueryString["sortBy"]);
                }
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
                this.ddlSort.SelectedValue = this.sortBy;
            }
            else
            {
                this.dateStart = this.calendarStartDate.SelectedDate;
                this.dateEnd = this.calendarEndDate.SelectedDate;
                this.sortBy = this.ddlSort.SelectedValue;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleStatistics_ReBindData);
            this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ddlSort.Items.Add(new ListItem("消费金额", "SaleTotals"));
            this.ddlSort.Items.Add(new ListItem("订单数", "OrderCount"));
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindMemberRanking();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("sortBy", this.ddlSort.SelectedValue);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

