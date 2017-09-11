namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyMembers : VMemberTemplatedWebControl
    {
        private HtmlInputHidden hiddPageIndex;
        private HtmlInputHidden hiddTotal;
        private Literal litMysubFirst;
        private Literal litMysubMember;
        private Literal litMysubSecond;
        private Literal litUserId;
        private VshopTemplatedRepeater rpMyMemberList;

        protected override void AttachChildControls()
        {
            int num2;
            int num3;
            PageTitle.AddSiteNameTitle("店铺会员");
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false));
            if (userIdDistributors == null)
            {
                this.Context.Response.Redirect("/default.aspx");
                this.Context.Response.End();
            }
            this.litUserId = (Literal) this.FindControl("litUserId");
            this.litUserId.Text = userIdDistributors.UserId.ToString();
            this.litMysubMember = (Literal) this.FindControl("litMysubMember");
            this.litMysubFirst = (Literal) this.FindControl("litMysubFirst");
            this.litMysubSecond = (Literal) this.FindControl("litMysubSecond");
            DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(userIdDistributors.UserId);
            if ((distributorsSubStoreNum != null) || (distributorsSubStoreNum.Rows.Count > 0))
            {
                this.litMysubMember.Text = distributorsSubStoreNum.Rows[0]["memberCount"].ToString();
                this.litMysubFirst.Text = distributorsSubStoreNum.Rows[0]["firstV"].ToString();
                this.litMysubSecond.Text = distributorsSubStoreNum.Rows[0]["secondV"].ToString();
            }
            else
            {
                this.litMysubMember.Text = "0";
                this.litMysubFirst.Text = "0";
                this.litMysubSecond.Text = "0";
            }
            this.rpMyMemberList = (VshopTemplatedRepeater) this.FindControl("rpMyMemberList");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num2))
            {
                num2 = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num3))
            {
                num3 = 10;
            }
            this.hiddTotal = (HtmlInputHidden) this.FindControl("hiddTotal");
            this.hiddPageIndex = (HtmlInputHidden) this.FindControl("hiddPageIndex");
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserID"]))
            {
                int referralUserId = int.Parse(this.Page.Request.QueryString["UserID"]);
                int total = 0;
                string str = this.Page.Request.QueryString["sort"];
                if (string.IsNullOrWhiteSpace(str))
                {
                    str = "createDate";
                }
                string str2 = this.Page.Request.QueryString["order"];
                if (string.IsNullOrWhiteSpace(str2))
                {
                    str2 = "desc";
                }
                DataTable table2 = MemberProcessor.GetMembersByUserId(referralUserId, num2, num3, out total, str, str2);
                this.hiddTotal.Value = total.ToString();
                this.hiddPageIndex.Value = num2.ToString();
                this.rpMyMemberList.DataSource = table2;
                this.rpMyMemberList.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMyMembers.html";
            }
            base.OnInit(e);
        }
    }
}

