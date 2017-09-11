namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VBrandDetail : VshopTemplatedWebControl
    {
        private int BrandId;
        private HiImage imgUrl;
        private Literal litBrandDetail;
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            int num3;
            if (!int.TryParse(this.Page.Request.QueryString["BrandId"], out this.BrandId))
            {
                base.GotoResourceNotFound("");
            }
            this.imgUrl = (HiImage) this.FindControl("imgUrl");
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.litBrandDetail = (Literal) this.FindControl("litBrandDetail");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            BrandCategoryInfo brandCategory = CategoryBrowser.GetBrandCategory(this.BrandId);
            this.litBrandDetail.SetWhenIsNotNull(brandCategory.Description);
            this.imgUrl.ImageUrl = brandCategory.Logo;
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            this.rptProducts.DataSource = ProductBrowser.GetBrandProducts(MemberProcessor.GetCurrentMember(), new int?(this.BrandId), num, num2, out num3);
            this.rptProducts.DataBind();
            this.txtTotal.SetWhenIsNotNull(num3.ToString());
            PageTitle.AddSiteNameTitle("品牌详情");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VBrandDetail.html";
            }
            base.OnInit(e);
        }
    }
}

