namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VMyConsultations : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            ProductConsultationAndReplyQuery consultationQuery = new ProductConsultationAndReplyQuery {
                UserId = currentMember.UserId,
                IsCount = true,
                PageIndex = num,
                PageSize = num2,
                SortBy = "ConsultationId",
                SortOrder = SortAction.Desc
            };
            DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(consultationQuery);
            this.rptProducts.DataSource = productConsultations.Data;
            this.rptProducts.DataBind();
            this.txtTotal.SetWhenIsNotNull(productConsultations.TotalRecords.ToString());
            PageTitle.AddSiteNameTitle("商品咨询");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMyConsultations.html";
            }
            base.OnInit(e);
        }
    }
}

