namespace Hidistro.UI.SaleSystem.CodeBehind
{
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
    public class VMyRanking : VMemberTemplatedWebControl
    {
        private Literal liturl;
        private VshopTemplatedRepeater rptMyRanking;
        private VshopTemplatedRepeater rptMyRankingList;
        private VshopTemplatedRepeater rptMyTeamRankingList;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(false), true);
            if (currentDistributors == null)
            {
                HttpContext.Current.Response.Redirect("/Vshop/MemberCenter.aspx");
                HttpContext.Current.Response.End();
            }
            if (currentDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                string url = this.Page.Request.QueryString["returnUrl"];
                if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
                {
                    this.Page.Response.Redirect(url);
                }
                MemberInfo currentMemberInfo = base.CurrentMemberInfo;
                this.liturl = (Literal) this.FindControl("liturl");
                this.liturl.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/Default.aspx?ReferralId=" + currentMemberInfo.UserId;
                DataSet userRanking = DistributorsBrower.GetUserRanking(currentMemberInfo.UserId);
                this.rptMyRankingList = (VshopTemplatedRepeater) this.FindControl("rptMyRankingList");
                this.rptMyTeamRankingList = (VshopTemplatedRepeater) this.FindControl("rptMyTeamRankingList");
                this.rptMyRanking = (VshopTemplatedRepeater) this.FindControl("rptMyRanking");
                if (userRanking.Tables.Count == 3)
                {
                    this.rptMyRanking.DataSource = userRanking.Tables[0];
                    this.rptMyRanking.DataBind();
                    this.rptMyRankingList.DataSource = userRanking.Tables[1];
                    this.rptMyRankingList.DataBind();
                    this.rptMyTeamRankingList.DataSource = userRanking.Tables[2];
                    this.rptMyTeamRankingList.DataBind();
                }
                PageTitle.AddSiteNameTitle("查看排名");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyRanking.html";
            }
            base.OnInit(e);
        }
    }
}

