namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class NoticeEdit : AdminPage
    {
        protected string adminName;
        protected string ArticleTitle;
        protected string htmlMenuTitleAdd;
        protected int Id;
        protected string menuTitle;
        protected RadioButtonList rbSendTolist;
        protected int recordcount;
        protected string reUrl;
        protected int sendType;
        protected HtmlForm thisForm;
        protected ucUeditor txtMemo;
        protected TextBox txtTitle;

        protected NoticeEdit() : base("m01", "dpp11")
        {
            this.adminName = string.Empty;
            this.reUrl = string.Empty;
            this.htmlMenuTitleAdd = string.Empty;
            this.ArticleTitle = string.Empty;
            this.menuTitle = "公告";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str2;
            ManagerInfo currentManager;
            switch (Globals.RequestFormStr("posttype"))
            {
                case "save":
                {
                    int num=0;
                    base.Response.ContentType = "application/json";
                    str2 = "{\"success\":\"0\",\"tips\":\"操作失败！\"}";
                    string str3 = Globals.RequestFormStr("title");
                    this.sendType = Globals.RequestFormNum("type");
                    if (this.sendType != 1)
                    {
                        this.sendType = 0;
                    }
                    switch (Globals.RequestFormNum("sendto"))
                    {
                        case 0:
                        case 1:
                        case 2:
                            break;

                        default:
                            num = 0;
                            break;
                    }
                    this.Id = Globals.RequestFormNum("Id");
                    string str4 = Globals.RequestFormStr("memo");
                    NoticeInfo info = new NoticeInfo {
                        Id = this.Id,
                        Title = str3,
                        Memo = str4,
                        AddTime = DateTime.Now,
                        SendType = this.sendType,
                        SendTo = num
                    };
                    currentManager = ManagerHelper.GetCurrentManager();
                    this.adminName = currentManager.UserName;
                    info.Author = this.adminName;
                    if (num == 2)
                    {
                        DataTable table = NoticeHelper.GetTempSelectedUser(this.adminName).Tables[0];
                        int count = table.Rows.Count;
                        if (count == 0)
                        {
                            str2 = "{\"success\":\"0\",\"tips\":\"请先选择用户！\"}";
                            base.Response.Write(str2);
                            base.Response.End();
                        }
                        else
                        {
                            List<NoticeUserInfo> list = new List<NoticeUserInfo>();
                            for (int i = 0; i < count; i++)
                            {
                                NoticeUserInfo item = new NoticeUserInfo {
                                    UserId = Globals.ToNum(table.Rows[i]["UserID"]),
                                    NoticeId = 0
                                };
                                list.Add(item);
                            }
                            info.NoticeUserInfo = list;
                        }
                    }
                    int num4 = NoticeHelper.SaveNotice(info);
                    if (num4 > 0)
                    {
                        str2 = "{\"success\":\"1\",\"id\":" + num4 + "}";
                    }
                    base.Response.Write(str2);
                    base.Response.End();
                    return;
                }
                case "getselecteduser":
                {
                    base.Response.ContentType = "application/json";
                    currentManager = ManagerHelper.GetCurrentManager();
                    this.adminName = currentManager.UserName;
                    DataTable table2 = NoticeHelper.GetSelectedUser(this.adminName).Tables[0];
                    int num5 = table2.Rows.Count;
                    StringBuilder builder = new StringBuilder();
                    if (num5 > 0)
                    {
                        int num6 = 0;
                        builder.Append("{\"name\":\"" + Globals.String2Json(table2.Rows[num6]["username"].ToString()) + "\",\"tel\":\"" + Globals.String2Json(table2.Rows[num6]["CellPhone"].ToString()) + "\",\"bindname\":\"" + Globals.String2Json(table2.Rows[num6]["UserBindName"].ToString()) + "\"}");
                        for (num6 = 1; num6 < num5; num6++)
                        {
                            builder.Append(",{\"name\":\"" + Globals.String2Json(table2.Rows[num6]["username"].ToString()) + "\",\"tel\":\"" + Globals.String2Json(table2.Rows[num6]["CellPhone"].ToString()) + "\",\"bindname\":\"" + Globals.String2Json(table2.Rows[num6]["UserBindName"].ToString()) + "\"}");
                        }
                    }
                    str2 = string.Concat(new object[] { "{\"success\":\"1\",\"icount\":", num5, ",\"userlist\":[", builder.ToString(), "]}" });
                    base.Response.Write(str2);
                    base.Response.End();
                    return;
                }
            }
            if (!base.IsPostBack)
            {
                this.Id = Globals.RequestQueryNum("Id");
                if (this.Id > 0)
                {
                    NoticeInfo noticeInfo = NoticeHelper.GetNoticeInfo(this.Id);
                    if (noticeInfo != null)
                    {
                        this.txtTitle.Text = noticeInfo.Title;
                        this.txtMemo.Text = noticeInfo.Memo;
                        this.rbSendTolist.SelectedValue = noticeInfo.SendTo.ToString();
                    }
                }
                this.reUrl = Globals.RequestQueryStr("reurl");
                if (string.IsNullOrEmpty(this.reUrl))
                {
                    this.reUrl = "noticelist.aspx";
                }
                this.sendType = Globals.RequestQueryNum("type");
                this.rbSendTolist.Items[0].Attributes.Add("onclick", "CancelShowUserList()");
                this.rbSendTolist.Items[1].Attributes.Add("onclick", "CancelShowUserList()");
                if (this.sendType == 1)
                {
                    this.menuTitle = "消息";
                    this.rbSendTolist.Items[2].Attributes.Add("onclick", "ShowUserList()");
                }
                else
                {
                    this.rbSendTolist.Items[2].Attributes.Add("class", "hide");
                    this.rbSendTolist.Width = 0xaf;
                    this.sendType = 0;
                }
                currentManager = ManagerHelper.GetCurrentManager();
                this.adminName = currentManager.UserName;
            }
        }
    }
}

