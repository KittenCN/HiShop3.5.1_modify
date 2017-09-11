namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class NoticeList : AdminPage
    {
        protected string ArticleTitle;
        protected Button btnDel;
        protected Button btnPub;
        protected Button btnSearch;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected DropDownList ddlState;
        protected HyperLink hlinkAdd;
        protected string htmlMenuTitleAdd;
        protected string localUrl;
        private int pageno;
        protected Pager pager;
        protected int recordcount;
        protected Repeater rptList;
        protected int sendType;
        private string title;
        protected TextBox txtTitle;
        protected TextBox txtUserName;

        protected NoticeList() : base("m01", "dpp11")
        {
            this.localUrl = string.Empty;
            this.htmlMenuTitleAdd = string.Empty;
            this.ArticleTitle = string.Empty;
            this.title = string.Empty;
        }

        private void BindData(int pageno, int sendtype)
        {
            string str5;
            NoticeQuery entity = new NoticeQuery {
                SortBy = "ID",
                SortOrder = SortAction.Desc
            };
            Globals.EntityCoding(entity, true);
            entity.PageIndex = pageno;
            entity.SendType = sendtype;
            entity.PageSize = this.pager.PageSize;
            entity.IsDistributor = true;
            this.title = Globals.RequestQueryStr("title");
            string str = Globals.RequestQueryStr("starttime");
            string str2 = Globals.RequestQueryStr("endtime");
            string str3 = Globals.RequestQueryStr("username");
            string s = Globals.RequestQueryStr("state");
            if (!string.IsNullOrEmpty(this.title))
            {
                entity.Title = this.title;
                this.txtTitle.Text = this.title;
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
            if (!string.IsNullOrEmpty(str3))
            {
                entity.Author = str3;
                this.txtUserName.Text = str3;
            }
            if (((str5 = s) != null) && ((str5 == "0") || (str5 == "1")))
            {
                entity.IsPub = new int?(Globals.ToNum(s));
                this.ddlState.SelectedValue = s;
            }
            entity.SortBy = "IsPub asc,AddTime";
            DbQueryResult noticeRequest = NoticeHelper.GetNoticeRequest(entity);
            this.rptList.DataSource = noticeRequest.Data;
            this.rptList.DataBind();
            int totalRecords = noticeRequest.TotalRecords;
            this.pager.TotalRecords = totalRecords;
            this.recordcount = totalRecords;
            if (this.pager.TotalRecords <= this.pager.PageSize)
            {
                this.pager.Visible = false;
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string[] strArray = Globals.RequestFormStr("cbNoticeGroup").Trim().Split(new char[] { ',' });
            int noticeid = 0;
            int num2 = 0;
            foreach (string str2 in strArray)
            {
                noticeid = Globals.ToNum(str2);
                if (noticeid > 0)
                {
                    NoticeHelper.DelNotice(noticeid);
                    num2++;
                }
            }
            if (num2 > 0)
            {
                this.ShowMsg("成功删除了指定的" + ((this.sendType == 1) ? "消息" : "公告"), true);
                this.BindData(this.pageno, this.sendType);
            }
            else
            {
                this.ShowMsg("请选择要删除的" + ((this.sendType == 1) ? "消息" : "公告"), false);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string s = this.txtTitle.Text.Trim();
            string str2 = Globals.RequestFormStr("ctl00$ContentPlaceHolder1$calendarStartDate$txtDateTimePicker");
            string str3 = Globals.RequestFormStr("ctl00$ContentPlaceHolder1$calendarEndDate$txtDateTimePicker");
            string str4 = this.txtUserName.Text.Trim();
            string str5 = this.ddlState.SelectedValue.Trim();
            string url = string.Concat(new object[] { "NoticeList.aspx?type=", this.sendType, "&title=", base.Server.UrlEncode(s), "&starttime=", str2, "&endtime=", str3, "&username=", str4, "&state=", str5 });
            base.Response.Redirect(url);
            base.Response.End();
        }

        protected string FormatIsPub(object ispub)
        {
            string str = string.Empty;
            switch (Globals.ToNum(ispub))
            {
                case 0:
                    return "<span class='red'>待发布</span>";

                case 1:
                    return "已发布";
            }
            return str;
        }

        protected string FormatSendTo(object sendto, object id)
        {
            string str = string.Empty;
            switch (Globals.ToNum(sendto))
            {
                case 0:
                    return "所有用户";

                case 1:
                    return "分销商";

                case 2:
                {
                    str = "指定用户";
                    int selectedUser = NoticeHelper.GetSelectedUser(Globals.ToNum(id));
                    if (selectedUser > 0)
                    {
                        object obj2 = str;
                        str = string.Concat(new object[] { obj2, "(<span style='color:green'>", selectedUser, "</span>)" });
                    }
                    return str;
                }
            }
            return str;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.sendType = Globals.RequestQueryNum("type");
            this.pageno = Globals.RequestQueryNum("pageindex");
            if (this.pageno < 1)
            {
                this.pageno = 1;
            }
            this.localUrl = base.Request.Url.ToString();
            if (this.sendType == 1)
            {
                this.btnDel.OnClientClick = "return HiConform('<strong>消息删除后将不可恢复，并且用户也将看不到内容！</strong><p>确定要批量删除所选择的消息吗？</p>', this);";
                this.hlinkAdd.NavigateUrl = "noticeedit.aspx?type=1&reurl=" + base.Server.UrlEncode(this.localUrl);
                this.hlinkAdd.Text = "发布新消息";
            }
            else
            {
                this.btnDel.OnClientClick = "return HiConform('<strong>公告删除后将不可恢复，并且用户也将看不到内容！</strong><p>确定要批量删除所选择的公告吗？</p>', this);";
                this.hlinkAdd.NavigateUrl = "noticeedit.aspx?reurl=" + base.Server.UrlEncode(this.localUrl);
                this.hlinkAdd.Text = "创建新的公告";
            }
            if (!base.IsPostBack)
            {
                this.BindData(this.pageno, this.sendType);
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string commandName = e.CommandName;
            if (commandName != null)
            {
                if (!(commandName == "delete"))
                {
                    if (!(commandName == "pub"))
                    {
                        return;
                    }
                }
                else
                {
                    NoticeHelper.DelNotice(Globals.ToNum(e.CommandArgument.ToString()));
                    this.ShowMsg("成功删除了指定的" + ((this.sendType == 1) ? "消息" : "公告"), true);
                    this.BindData(this.pageno, this.sendType);
                    return;
                }
                NoticeHelper.NoticePub(Globals.ToNum(e.CommandArgument.ToString()));
                this.ShowMsg("成功发布了指定的" + ((this.sendType == 1) ? "消息" : "公告"), true);
                this.BindData(this.pageno, this.sendType);
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int num = (int) DataBinder.Eval(e.Item.DataItem, "IsPub");
                int num2 = (int) DataBinder.Eval(e.Item.DataItem, "Id");
                Button button = e.Item.FindControl("btnPub") as Button;
                HyperLink link = e.Item.FindControl("hpLinkEdit") as HyperLink;
                button.Visible = num == 0;
                if (num == 0)
                {
                    link.NavigateUrl = string.Concat(new object[] { "noticeedit.aspx?id=", num2, "&reurl=", base.Server.UrlEncode(this.localUrl) });
                    link.Visible = true;
                }
            }
        }
    }
}

