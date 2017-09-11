namespace Hidistro.UI.Web.Admin.settings
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageLogs : AdminPage
    {
        protected Button btnClear;
        protected Button btnDel;
        protected Button btnQueryLogs;
        protected Button Button1;
        protected Button Button4;
        protected ucDateTimePicker calFromDate;
        protected ucDateTimePicker calToDate;
        protected Repeater dlstLog1;
        protected LogsUserNameDropDownList dropOperationUserNames;
        private string FromDate;
        public int lastDay;
        protected Pager pager;
        protected PageSize PageSize1;
        protected Script Script5;
        protected Script Script6;
        protected Script Script7;
        protected Script Script9;
        protected HtmlForm thisForm;
        private string ToDate;

        protected ManageLogs() : base("m09", "szp12")
        {
            this.FromDate = "";
            this.ToDate = "";
        }

        public void BindLogs()
        {
            DbQueryResult logs = EventLogs.GetLogs(this.GetOperationLogQuery());
            this.dlstLog1.DataSource = logs.Data;
            this.dlstLog1.DataBind();
            this.SetSearchControl();
            this.pager.TotalRecords = logs.TotalRecords;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (EventLogs.DeleteAllLogs())
            {
                this.BindLogs();
                this.ShowMsg("成功删除了所有操作日志", true);
            }
            else
            {
                this.ShowMsg("在删除过程中出现未知错误", false);
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            this.DeleteCheck();
        }

        private void btnQueryLogs_Click(object sender, EventArgs e)
        {
            this.lastDay = 0;
            this.ReloadManagerLogs(true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.Button4.BorderColor = ColorTranslator.FromHtml("");
            this.Button1.BorderColor = ColorTranslator.FromHtml("#1CA47D");
            DateTime now = DateTime.Now;
            this.calToDate.SelectedDate = new DateTime?(now);
            this.calFromDate.SelectedDate = new DateTime?(now.AddDays(-6.0));
            this.ToDate = now.ToString("yyyy-MM-dd");
            this.FromDate = now.AddDays(-6.0).ToString("yyyy-MM-dd");
            this.lastDay = 7;
            this.ReloadManagerLogs(true);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.calToDate.SelectedDate = new DateTime?(now);
            this.calFromDate.SelectedDate = new DateTime?(now.AddMonths(-1));
            this.ToDate = now.ToString("yyyy-MM-dd");
            this.FromDate = now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.lastDay = 30;
            this.ReloadManagerLogs(true);
        }

        private void DeleteCheck()
        {
            string strIds = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                strIds = base.Request["CheckBoxGroup"];
            }
            if (strIds.Length <= 0)
            {
                this.ShowMsg("请先选择要删除的操作日志项", false);
            }
            else
            {
                int num = EventLogs.DeleteLogs(strIds);
                this.BindLogs();
                this.ShowMsg(string.Format("成功删除了{0}个操作日志", num), true);
            }
        }

        protected void DeleteLog(object sender, EventArgs e)
        {
            long result = 0L;
            if (long.TryParse(((Button) sender).CommandArgument, out result))
            {
                if (EventLogs.DeleteLog(result))
                {
                    this.BindLogs();
                    this.ShowMsg("成功删除了单个操作日志", true);
                }
                else
                {
                    this.ShowMsg("在删除过程中出现未知错误", false);
                }
            }
            else
            {
                this.ShowMsg("非正常删除！", true);
            }
        }

        private OperationLogQuery GetOperationLogQuery()
        {
            OperationLogQuery query = new OperationLogQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
            {
                query.OperationUserName = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FromDate"]))
            {
                query.FromDate = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["FromDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ToDate"]))
            {
                query.ToDate = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["ToDate"]));
            }
            query.Page.PageIndex = this.pager.PageIndex;
            query.Page.PageSize = this.pager.PageSize;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
            {
                query.Page.SortBy = this.Page.Request.QueryString["SortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
            {
                query.Page.SortOrder = SortAction.Desc;
            }
            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnQueryLogs.Click += new EventHandler(this.btnQueryLogs_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropOperationUserNames.DataBind();
                this.BindLogs();
            }
        }

        private void ReloadManagerLogs(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OperationUserName", this.dropOperationUserNames.SelectedValue);
            if (this.lastDay > 0)
            {
                queryStrings.Add("FromDate", this.FromDate);
                queryStrings.Add("ToDate", this.ToDate);
            }
            else
            {
                if (this.calFromDate.SelectedDate.HasValue)
                {
                    queryStrings.Add("FromDate", this.calFromDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                }
                if (this.calToDate.SelectedDate.HasValue)
                {
                    queryStrings.Add("ToDate", this.calToDate.SelectedDate.Value.ToString("yyyy-MM-dd"));
                }
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            queryStrings.Add("lastDay", this.lastDay.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!this.Page.IsCallback)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
                {
                    this.dropOperationUserNames.SelectedValue = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
                }
                try
                {
                    if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FromDate"]))
                    {
                        this.calFromDate.SelectedDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["FromDate"]));
                    }
                    if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ToDate"]))
                    {
                        this.calToDate.SelectedDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["ToDate"]));
                    }
                }
                catch (Exception)
                {
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
                {
                    int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
                    if (this.lastDay == 30)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("#1CA47D");
                    }
                    else if (this.lastDay == 7)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("#1CA47D");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                    else
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                }
            }
        }
    }
}

