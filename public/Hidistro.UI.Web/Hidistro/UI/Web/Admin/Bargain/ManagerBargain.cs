namespace Hidistro.UI.Web.Admin.Bargain
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Bargain;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManagerBargain : AdminPage
    {
        protected Button btnDeleteCheck;
        protected Button btnSearchButton;
        protected ucDateTimePicker calendarBeginDate;
        protected ucDateTimePicker calendarEndDate;
        protected Repeater grdBargainList;
        protected PageSize hrefPageSize;
        protected Literal ListActive;
        protected Literal Listfrozen;
        protected Literal Literal1;
        protected Literal Literal2;
        protected Pager pager;
        private string productName;
        protected Script Script5;
        protected Script Script6;
        protected HtmlForm thisForm;
        private string title;
        protected TextBox txtProductName;
        protected TextBox txtTitle;
        private string type;

        public ManagerBargain() : base("m08", "yxp21")
        {
        }

        private void BindData()
        {
            BargainQuery query = new BargainQuery {
                ProductName = this.productName,
                Title = this.title,
                Type = this.type,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex
            };
            this.pager.TotalRecords = BargainHelper.GetTotal(query);
            DbQueryResult bargainList = BargainHelper.GetBargainList(query);
            this.grdBargainList.DataSource = bargainList.Data;
            this.grdBargainList.DataBind();
        }

        private void btnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要删除的砍价活动！", false);
            }
            else
            {
                str = str.TrimEnd(new char[] { ',' });
                if (BargainHelper.GetBargainById(str).Rows.Count > 0)
                {
                    this.ShowMsg("删除的砍价活动中有正在进行的，不能删除", false);
                }
                else
                {
                    BargainHelper.DeleteBargainById(str);
                    this.ShowMsg("删除成功", true);
                    this.BindData();
                }
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        protected string GetEditHtml(object id, object status, object endDate, object beginDate)
        {
            StringBuilder builder = new StringBuilder();
            string str = status.ToString();
            if (str != null)
            {
                if (!(str == "进行中"))
                {
                    if (str == "未开始")
                    {
                        builder.Append("<a href='/admin/Bargain/AddBargain.aspx?Id=" + id.ToString() + "' class='btn btn-info btn-xs mb5 inputw50' style='display: '>编辑</a>");
                    }
                    else if (str == "已结束")
                    {
                        builder.Append("<a href='#' class='btn btn-info btn-xs mb5 inputw50' style='display:none'>编辑</a>");
                    }
                }
                else
                {
                    builder.Append(string.Concat(new object[] { "<a href='#' onclick=\"OpenEdit('", id.ToString(), "','", (DateTime) endDate, "','", (DateTime) beginDate, "');\" class='btn btn-info btn-xs mb5 inputw50' style='display: '>编辑</a>" }));
                }
            }
            return builder.ToString();
        }

        protected bool GetStatus(object obj)
        {
            bool flag = false;
            if (obj.ToString() == "进行中")
            {
                flag = false;
            }
            if (obj.ToString() == "未开始")
            {
                flag = true;
            }
            if (obj.ToString() == "已结束")
            {
                flag = true;
            }
            return flag;
        }

        private void grdBargainList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string commandName = e.CommandName;
            string str2 = e.CommandArgument.ToString();
            if (((commandName == "Delete") && !string.IsNullOrEmpty(str2)) && BargainHelper.DeleteBargainById(str2))
            {
                this.ShowMsg("删除成功", true);
                this.BindData();
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
                {
                    this.productName = base.Server.UrlDecode(this.Page.Request.QueryString["productName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["title"]))
                {
                    this.title = base.Server.UrlDecode(this.Page.Request.QueryString["title"]);
                }
                this.txtProductName.Text = this.productName;
                this.txtTitle.Text = this.title;
            }
            else
            {
                this.productName = this.txtProductName.Text;
                this.title = this.txtTitle.Text;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.grdBargainList.ItemCommand += new RepeaterCommandEventHandler(this.grdBargainList_ItemCommand);
            this.btnDeleteCheck.Click += new EventHandler(this.btnDeleteCheck_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.ViewState["Type"] = (base.Request.QueryString["Type"] != null) ? base.Request.QueryString["Type"] : null;
                this.type = (this.ViewState["Type"] == null) ? "all" : this.ViewState["Type"].ToString();
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", this.txtProductName.Text);
            queryStrings.Add("title", this.txtTitle.Text);
            queryStrings.Add("type", (this.ViewState["Type"] != null) ? this.ViewState["Type"].ToString() : "");
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

