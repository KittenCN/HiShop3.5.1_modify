namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VChooseProducts : VMemberTemplatedWebControl
    {
        private int categoryId;
        private string keyWord = string.Empty;
        private VshopTemplatedRepeater rpCategorys;
        private VshopTemplatedRepeater rpChooseProducts;
        private HtmlInputText txtkeywords;

        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.txtkeywords = (HtmlInputText) this.FindControl("keywords");
            this.rpChooseProducts = (VshopTemplatedRepeater) this.FindControl("rpChooseProducts");
            this.rpCategorys = (VshopTemplatedRepeater) this.FindControl("rpCategorys");
            this.DataBindSoruce();
        }

        private void DataBindSoruce()
        {
            int num;
            this.txtkeywords.Value = this.keyWord;
            this.rpCategorys.DataSource = CategoryBrowser.GetCategories();
            this.rpCategorys.DataBind();
            this.rpChooseProducts.DataSource = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, 1, 0x2710, out num, "DisplaySequence", "desc", false);
            this.rpChooseProducts.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ChooseProducts.html";
            }
            base.OnInit(e);
        }
    }
}

