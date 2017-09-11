namespace Hidistro.UI.Web.Admin.Member
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class CustomDistributorDetail : AdminPage
    {
        protected Button BtnAddMembers;
        protected string clientType;
        protected int currentGroupId;
        protected Grid grdMemberList;
        protected Literal GroupName;
        protected PageSize hrefPageSize;
        protected Literal litActivy;
        protected Literal litAll;
        protected Literal litNew;
        protected Literal litSleep;
        protected Button lkbDelectCheck;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlForm thisForm;
        protected TrimTextBox txtUserNames;

        protected CustomDistributorDetail() : base("m04", "hyp05")
        {
            this.clientType = Globals.RequestQueryStr("ClientType").ToLower();
        }

        protected void BindData()
        {
            MemberQuery query = new MemberQuery {
                PageIndex = this.pager.PageIndex,
                SortBy = this.grdMemberList.SortOrderBy,
                PageSize = this.pager.PageSize,
                ClientType = this.clientType,
                GroupId = new int?(this.currentGroupId),
                Stutas = (UserStatus)1
            };
            if (this.grdMemberList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult members = MemberHelper.GetMembers(query, false);
            this.grdMemberList.DataSource = members.Data;
            this.grdMemberList.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = members.TotalRecords;
        }

        protected void BtnAddMembers_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUserNames.Text.Trim()))
            {
                this.ShowMsg("请输入用户名", false);
            }
            else
            {
                IList<int> userIdList = new List<int>();
                foreach (string str2 in this.txtUserNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", ",").Split(new char[] { ',' }))
                {
                    int memberIdByUserNameOrNiChen = MemberHelper.GetMemberIdByUserNameOrNiChen(str2, "");
                    if (memberIdByUserNameOrNiChen > 0)
                    {
                        userIdList.Add(memberIdByUserNameOrNiChen);
                    }
                }
                this.txtUserNames.Text = "";
                if (userIdList.Count > 0)
                {
                    string str3 = CustomGroupingHelper.AddCustomGroupingUser(userIdList, this.currentGroupId);
                    if (string.IsNullOrEmpty(str3))
                    {
                        this.ShowMsg("添加成功！", true);
                    }
                    else
                    {
                        this.ShowMsg(str3, false);
                    }
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("未找到会员", false);
                }
            }
        }

        protected void grdMemberList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (CustomGroupingHelper.DelGroupUser(this.grdMemberList.DataKeys[e.RowIndex].Value.ToString(), this.currentGroupId))
            {
                this.BindData();
            }
        }

        protected void lkbDelectCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            foreach (GridViewRow row in this.grdMemberList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    str = str + this.grdMemberList.DataKeys[row.RowIndex].Value.ToString() + ",";
                }
            }
            str = str.TrimEnd(new char[] { ',' });
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要移除的会员账号！", false);
            }
            else if (CustomGroupingHelper.DelGroupUser(str, this.currentGroupId))
            {
                this.BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["GroupId"], out this.currentGroupId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.BtnAddMembers.Click += new EventHandler(this.BtnAddMembers_Click);
                this.lkbDelectCheck.Click += new EventHandler(this.lkbDelectCheck_Click);
                this.grdMemberList.RowDeleting += new GridViewDeleteEventHandler(this.grdMemberList_RowDeleting);
                if (!base.IsPostBack)
                {
                    CustomGroupingInfo groupInfoById = CustomGroupingHelper.GetGroupInfoById(this.currentGroupId);
                    if (groupInfoById != null)
                    {
                        this.GroupName.Text = groupInfoById.GroupName;
                        this.BindData();
                    }
                    else
                    {
                        base.Response.Redirect("CustomDistributorList.aspx");
                    }
                }
            }
        }
    }
}

