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

    [ParseChildren(true)]
    public class VChirldrenDistributorStores : VMemberTemplatedWebControl
    {
        private HtmlInputHidden hiddTotal;
        private VshopTemplatedRepeater rpdistributor;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("下级分销商");
            this.rpdistributor = (VshopTemplatedRepeater) this.FindControl("rpdistributor");
            this.hiddTotal = (HtmlInputHidden) this.FindControl("hiddTotal");
            DistributorsQuery query = new DistributorsQuery {
                PageIndex = 0,
                PageSize = 10
            };
            if (DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentMemberUserId(false), true).ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                query.GradeId = 3;
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["UserId"], out result))
                {
                    query.UserId = result;
                }
                query.ReferralPath = result.ToString();
                int total = 0;
                DataTable threeDistributors = DistributorsBrower.GetThreeDistributors(query, out total);
                this.hiddTotal.Value = total.ToString();
                this.rpdistributor.DataSource = threeDistributors;
                this.rpdistributor.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VChirldrenDistributorStores.html";
            }
            base.OnInit(e);
        }
    }
}

