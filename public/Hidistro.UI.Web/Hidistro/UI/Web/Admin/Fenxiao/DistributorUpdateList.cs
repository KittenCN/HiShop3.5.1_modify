namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.FengXiao;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.FenXiao;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Web.UI.WebControls;

    public class DistributorUpdateList : AdminPage
    {
        protected string ArticleTitle;
        protected Button btnSearch;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected string htmlMenuTitleAdd;
        protected string localUrl;
        private int pageno;
        protected Pager pager;
        protected int recordcount;
        protected Repeater rptList;
        protected int sendType;
        private string title;
        protected TextBox txtKey;

        protected DistributorUpdateList() : base("m05", "fxp12")
        {
            this.localUrl = string.Empty;
            this.htmlMenuTitleAdd = string.Empty;
            this.ArticleTitle = string.Empty;
            this.title = string.Empty;
        }

        private void BindData(int pageno, int sendtype)
        {
            DistributorGradeCommissionQuery entity = new DistributorGradeCommissionQuery {
                SortBy = "ID",
                SortOrder = SortAction.Desc
            };
            Globals.EntityCoding(entity, true);
            entity.PageIndex = pageno;
            entity.PageSize = this.pager.PageSize;
            string str = Globals.RequestQueryStr("starttime");
            string str2 = Globals.RequestQueryStr("endtime");
            string str3 = Globals.RequestQueryStr("title");
            if (!string.IsNullOrEmpty(str3))
            {
                entity.Title = str3;
                this.txtKey.Text = str3;
            }
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    entity.StartTime = new DateTime?(DateTime.Parse(str));
                    this.calendarStartDate.Text = entity.StartTime.Value.ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    entity.EndTime = new DateTime?(DateTime.Parse(str2));
                    this.calendarEndDate.Text = entity.EndTime.Value.ToString("yyyy-MM-dd");
                }
            }
            catch
            {
            }
            DbQueryResult result = DistributorGradeCommissionHelper.DistributorGradeCommission(entity);
            this.rptList.DataSource = result.Data;
            this.rptList.DataBind();
            int totalRecords = result.TotalRecords;
            this.pager.TotalRecords = totalRecords;
            this.recordcount = totalRecords;
            if (this.pager.TotalRecords <= this.pager.PageSize)
            {
                this.pager.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string s = this.txtKey.Text.Trim();
            string str2 = Globals.RequestFormStr("ctl00$ContentPlaceHolder1$calendarStartDate$txtDateTimePicker");
            string str3 = Globals.RequestFormStr("ctl00$ContentPlaceHolder1$calendarEndDate$txtDateTimePicker");
            string url = "DistributorUpdateList.aspx?title=" + base.Server.UrlEncode(s) + "&starttime=" + str2 + "&endtime=" + str3;
            base.Response.Redirect(url);
            base.Response.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.pageno = Globals.RequestQueryNum("pageindex");
            if (this.pageno < 1)
            {
                this.pageno = 1;
            }
            this.localUrl = base.Request.Url.ToString();
            if (!base.IsPostBack)
            {
                this.BindData(this.pageno, this.sendType);
            }
        }
    }
}

