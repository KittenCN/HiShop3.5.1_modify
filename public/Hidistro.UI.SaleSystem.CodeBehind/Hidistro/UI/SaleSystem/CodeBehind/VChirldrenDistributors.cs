namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VChirldrenDistributors : VMemberTemplatedWebControl
    {
        private HtmlInputHidden hiddPageIndex;
        private HtmlInputHidden hiddTotal;
        private Literal litMysubFirst;
        private Literal litMysubMember;
        private Literal litMysubSecond;
        private Literal litUserId;
        private Panel onedistributor;
        private VshopTemplatedRepeater rpdistributor;
        private Panel twodistributor;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("下级分销商");
            this.litMysubMember = (Literal) this.FindControl("litMysubMember");
            this.litMysubFirst = (Literal) this.FindControl("litMysubFirst");
            this.litMysubSecond = (Literal) this.FindControl("litMysubSecond");
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false));
            this.litUserId = (Literal) this.FindControl("litUserId");
            this.litUserId.Text = userIdDistributors.UserId.ToString();
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
            this.rpdistributor = (VshopTemplatedRepeater) this.FindControl("rpdistributor");
            this.hiddTotal = (HtmlInputHidden) this.FindControl("hiddTotal");
            DistributorsQuery query = new DistributorsQuery {
                PageIndex = 0,
                PageSize = 10
            };
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(false), true);
            if (currentDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                query.GradeId = 2;
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["gradeId"], out result))
                {
                    query.GradeId = result;
                }
                query.ReferralPath = currentDistributors.UserId.ToString();
                query.UserId = currentDistributors.UserId;
                int total = 0;
                string str = this.Page.Request.QueryString["sort"];
                if (string.IsNullOrWhiteSpace(str))
                {
                    str = "CreateTime";
                }
                string str2 = this.Page.Request.QueryString["order"];
                if (string.IsNullOrWhiteSpace(str2))
                {
                    str2 = "desc";
                }
                DataTable table2 = DistributorsBrower.GetDownDistributors(query, out total, str, str2);
                this.hiddTotal.Value = total.ToString();
                this.rpdistributor.DataSource = table2;
                this.rpdistributor.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VChirldrenDistributors.html";
            }
            base.OnInit(e);
        }
    }
}

