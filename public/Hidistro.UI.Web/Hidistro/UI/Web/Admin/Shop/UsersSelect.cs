namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UsersSelect : AdminPage
    {
        protected string adminname;
        protected Button btnSearch;
        protected Panel divEmpty;
        protected HtmlForm form1;
        protected Pager pager;
        protected Repeater rptList;
        protected string searchName;
        protected TextBox txtKey;

        protected UsersSelect() : base("m01", "dpp11")
        {
            this.adminname = string.Empty;
            this.searchName = string.Empty;
        }

        private void BindData(string title)
        {
            MemberQuery query = new MemberQuery {
                Username = title,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                Stutas = (UserStatus)1,
                EndTime = new DateTime?(DateTime.Now),
                StartTime = new DateTime?(DateTime.Now.AddDays((double) -SettingsManager.GetMasterSettings(false).ActiveDay)),
                CellPhone = (base.Request.QueryString["phone"] != null) ? base.Request.QueryString["phone"] : "",
                ClientType = (base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : ""
            };
            DbQueryResult members = MemberHelper.GetMembers(query, false);
            int totalRecords = members.TotalRecords;
            if (totalRecords > 0)
            {
                this.rptList.DataSource = members.Data;
                this.rptList.DataBind();
                this.pager.TotalRecords = this.pager.TotalRecords = totalRecords;
                if (this.pager.TotalRecords <= this.pager.PageSize)
                {
                    this.pager.Visible = false;
                }
            }
            else
            {
                this.divEmpty.Visible = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string s = this.txtKey.Text.Trim();
            base.Response.Redirect("usersselect.aspx?admin=" + this.adminname + "&key=" + base.Server.UrlEncode(s));
            base.Response.End();
        }

        protected string FormatOper(object userid, object adminname)
        {
            if (NoticeHelper.GetUserIsSel(Globals.ToNum(userid), adminname.ToString()))
            {
                return ("<input type='button' class='btn btn-success btn-xs' value='已选' issel='1' userid='" + userid.ToString() + "' onclick='seluser(this)'/>");
            }
            return ("<input type='button' class='btn btn-primary btn-xs' value='选择' issel='0' userid='" + userid.ToString() + "' onclick='seluser(this)'/>");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.adminname = ManagerHelper.GetCurrentManager().UserName;
            if (Globals.RequestFormStr("posttype") == "sel")
            {
                base.Response.ContentType = "application/json";
                string s = "{\"success\":\"1\",\"tips\":\"操作成功！\"}";
                int userid = Globals.RequestFormNum("userid");
                if (Globals.RequestFormNum("issel") == 1)
                {
                    NoticeHelper.DelUser(userid, this.adminname);
                }
                else
                {
                    NoticeHelper.AddUser(userid, this.adminname);
                }
                base.Response.Write(s);
                base.Response.End();
            }
            if (!base.IsPostBack)
            {
                this.searchName = Globals.RequestQueryStr("key");
                this.txtKey.Text = this.searchName;
                this.BindData(this.searchName);
            }
        }
    }
}

