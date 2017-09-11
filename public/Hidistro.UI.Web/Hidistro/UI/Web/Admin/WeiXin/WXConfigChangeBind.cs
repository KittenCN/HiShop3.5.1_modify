namespace Hidistro.UI.Web.Admin.WeiXin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class WXConfigChangeBind : AdminPage
    {
        protected Button BatchHuifu;
        protected int BindOpenIDAndNoUserNameCount;
        protected Button btnBatchSave;
        protected Button btnSearchButton;
        protected HtmlForm form1;
        protected Grid grdMemberList;
        protected HtmlInputHidden hdUserId;
        protected PageSize hrefPageSize;
        protected Button huifuUser;
        protected Pager pager;
        protected Pager pager1;
        protected Script Script5;
        protected Script Script6;
        private SiteSettings siteSettings;
        protected TextBox txtPhone;
        protected TextBox txtUserName;

        protected WXConfigChangeBind() : base("m06", "wxp01")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        private void BindData()
        {
            MemberQuery query = new MemberQuery {
                PageIndex = this.pager.PageIndex,
                SortBy = this.grdMemberList.SortOrderBy,
                PageSize = this.pager.PageSize,
                Stutas = (UserStatus)1,
                EndTime = new DateTime?(DateTime.Now),
                Username = this.txtUserName.Text.Trim(),
                CellPhone = this.txtPhone.Text.Trim()
            };
            if (this.grdMemberList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult members = MemberHelper.GetMembers(query, true);
            this.grdMemberList.DataSource = members.Data;
            this.grdMemberList.DataBind();
            if (members.TotalRecords == 0)
            {
                base.Response.Redirect("WXConfigBindOK.aspx");
                base.Response.End();
            }
            this.pager1.TotalRecords = this.pager.TotalRecords = members.TotalRecords;
            this.BindOpenIDAndNoUserNameCount = this.pager1.TotalRecords;
            this.ViewState["BindOpenIDAndNoUserNameCount"] = this.BindOpenIDAndNoUserNameCount;
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            int success = 0;
            int fail = 0;
            for (int i = 0; i < this.grdMemberList.Rows.Count; i++)
            {
                CheckBox box = this.grdMemberList.Rows[i].Cells[0].Controls[0] as CheckBox;
                if (box.Checked)
                {
                    TextBox box2 = this.grdMemberList.Rows[i].FindControl("txtUserName") as TextBox;
                    int userId = (int) this.grdMemberList.DataKeys[i].Value;
                    string msg = string.Empty;
                    if (this.UpdateMemeberBindName(box2.Text.Trim(), userId, out msg))
                    {
                        success++;
                    }
                    else
                    {
                        fail++;
                    }
                }
            }
            if ((success + fail) > 0)
            {
                this.BindData();
                this.ShowResult(success, fail);
            }
            else
            {
                try
                {
                    this.BindOpenIDAndNoUserNameCount = (int) this.ViewState["BindOpenIDAndNoUserNameCount"];
                }
                catch (Exception)
                {
                }
                this.ShowMsg("请选择要保存的会员信息！", false);
            }
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        protected void grdMemberList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                e.Row.Visible = false;
            }
        }

        protected void gvList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = (int) this.grdMemberList.DataKeys[e.RowIndex].Value;
            string str = ((TextBox) this.grdMemberList.Rows[e.RowIndex].FindControl("txtUserName")).Text.Trim();
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("用户名不能为空！", false);
                try
                {
                    this.BindOpenIDAndNoUserNameCount = (int) this.ViewState["BindOpenIDAndNoUserNameCount"];
                }
                catch (Exception)
                {
                }
            }
            else
            {
                string msg = string.Empty;
                if (this.UpdateMemeberBindName(str, userId, out msg))
                {
                    this.ShowMsg("保存成功！", true);
                    this.BindData();
                }
                else
                {
                    try
                    {
                        this.BindOpenIDAndNoUserNameCount = (int) this.ViewState["BindOpenIDAndNoUserNameCount"];
                    }
                    catch (Exception)
                    {
                    }
                    if (!string.IsNullOrEmpty(msg))
                    {
                        this.ShowMsg(msg, false);
                    }
                    else
                    {
                        this.ShowMsg("保存失败！", false);
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdMemberList.RowDataBound += new GridViewRowEventHandler(this.grdMemberList_RowDataBound);
            this.grdMemberList.RowUpdating += new GridViewUpdateEventHandler(this.gvList_RowUpdating);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnBatchSave.Click += new EventHandler(this.btnSaveAll_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ShowResult(int success, int fail)
        {
            StringBuilder builder = new StringBuilder("<div class='modal fade' role='dialog' aria-labelledby='mySmallModalLabel' id='myModal'>");
            builder.Append(" <div class='modal-dialog modal-sm'>\r\n                <div class='modal-content'>\r\n                    <div class='w-modalbox'>\r\n                        <h5>保存成功</h5>\r\n                        <div class='titileBorderBox borderSolidB'>\r\n                            <div class='contentBox pl20 modalcontext'>\r\n                         <div>\r\n                             生成会员的用户名后，会员可以通过用户名登录店铺，避免店铺更换绑定微信\r\n                             众账号后，会员的个人信息丢失。\r\n                         </div>");
            if (fail > 0)
            {
                builder.AppendFormat("<p style='text-align:left;text-indent:20px;'><span>{0}</span>位会员用户名设置成功。</p>", success);
                builder.AppendFormat("<p style='text-align:left;text-indent:20px;margin-top:0px;'><span>{0}</span>位会员用户名设置失败，请给设置失败的会员自定义用户名。</p>", fail);
            }
            else
            {
                builder.AppendFormat("<p>您已成功给<span>{0}</span>位会员设置用户名</p>", success);
            }
            builder.Append(" </div></div> <div class='y-ikown pt10 pb10'>");
            builder.AppendFormat("<input type='submit'  value='{0}' class='btn btn-success inputw100' data-dismiss='modal' onclick='return ModifyMemo1();'>", (fail > 0) ? "自定义用户名" : "我知道了");
            builder.Append(" </div> </div> </div> </div> </div>");
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScriptMsg"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScriptMsg", builder.ToString());
            }
        }

        protected bool UpdateMemeberBindName(string bindName, int userId, out string msg)
        {
            msg = string.Empty;
            string password = HiCryptographer.Md5Encrypt("123456");
            if (bindName.Length < 2)
            {
                msg = "用户名至少两个字符";
                return false;
            }
            if (!MemberHelper.IsExistUserBindName(bindName))
            {
                return MemberHelper.BindUserName(userId, bindName, password);
            }
            msg = "用户名重复";
            return false;
        }
    }
}

