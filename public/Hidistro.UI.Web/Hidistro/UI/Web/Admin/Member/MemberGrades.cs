namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MemberGrades)]
    public class MemberGrades : AdminPage
    {
        protected Repeater rptList;
        protected HtmlForm thisForm;

        protected MemberGrades() : base("m04", "hyp03")
        {
        }

        private void BindMemberRanks()
        {
            this.rptList.DataSource = MemberHelper.GetMemberGrades();
            this.rptList.DataBind();
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindMemberRanks();
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteGrade")
            {
                if (MemberHelper.DeleteMemberGrade(Globals.ToNum(e.CommandArgument.ToString())))
                {
                    this.BindMemberRanks();
                    this.ShowMsg("已经成功删除选择的会员等级", true);
                }
                else
                {
                    this.ShowMsg("不能删除默认的会员等级或有会员的等级", false);
                }
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem)) && ((bool) DataBinder.Eval(e.Item.DataItem, "IsDefault")))
            {
                Label label = e.Item.FindControl("lblSplit") as Label;
                label.Visible = false;
                Button button = e.Item.FindControl("btnDel") as Button;
                button.Visible = false;
            }
        }

        public string SelectUserCountGrades(int gid)
        {
            return MemberHelper.SelectUserCountGrades(gid).ToString();
        }
    }
}

