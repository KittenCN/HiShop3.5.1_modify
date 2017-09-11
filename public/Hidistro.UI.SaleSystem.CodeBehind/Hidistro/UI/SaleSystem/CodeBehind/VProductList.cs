namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VProductList : VshopTemplatedWebControl
    {
        private int categoryId;
        private HtmlInputHidden hdfKeyword;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private string pIds;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptCategoryList;
        private VshopTemplatedRepeater rptProducts;
        private HtmlInputHidden txtTotalPages;

        protected override void AttachChildControls()
        {
            int num2;
            int num3;
            int num4;
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            int num = Globals.RequestQueryNum("isLimitedTimeDiscountId");
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            this.pIds = this.Page.Request.QueryString["pIds"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.hdfKeyword = (HtmlInputHidden) this.FindControl("hdfKeyword");
            this.hdfKeyword.Value = this.keyWord;
            this.imgUrl = (HiImage) this.FindControl("imgUrl");
            this.litContent = (Literal) this.FindControl("litContent");
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater) this.FindControl("rptCategories");
            this.rptCategoryList = (VshopTemplatedRepeater) this.FindControl("rptCategoryList");
            this.txtTotalPages = (HtmlInputHidden) this.FindControl("txtTotal");
            string str = this.Page.Request.QueryString["sort"];
            if (string.IsNullOrWhiteSpace(str))
            {
                str = "DisplaySequence";
            }
            string str2 = this.Page.Request.QueryString["order"];
            if (string.IsNullOrWhiteSpace(str2))
            {
                str2 = "desc";
            }
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num2))
            {
                num2 = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num3))
            {
                num3 = 20;
            }
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 0x3e8);
            this.rptCategories.DataSource = maxSubCategories;
            this.rptCategories.DataBind();
            DataSet categoryList = CategoryBrowser.GetCategoryList();
            this.rptCategoryList.ItemDataBound += new RepeaterItemEventHandler(this.rptCategoryList_ItemDataBound);
            this.rptCategoryList.DataSource = categoryList;
            this.rptCategoryList.DataBind();
            this.rptProducts.DataSource = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, num2, num3, out num4, str, str2, this.pIds, num == 1);
            this.rptProducts.DataBind();
            this.txtTotalPages.SetWhenIsNotNull(num4.ToString());
            string title = "商品列表";
            if (this.categoryId > 0)
            {
                CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
                if (category != null)
                {
                    title = category.Name;
                }
            }
            PageTitle.AddSiteNameTitle(title);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProductList.html";
            }
            base.OnInit(e);
        }

        private void rptCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataView view = (DataView) DataBinder.Eval(e.Item.DataItem, "SubCategories");
            DataRowView dataItem = (DataRowView) e.Item.DataItem;
            Convert.ToInt32(dataItem["CategoryId"]);
            Literal literal = (Literal) e.Item.Controls[0].FindControl("litPlus");
            if ((view == null) || (view.ToTable().Rows.Count == 0))
            {
                literal.Visible = false;
            }
            else
            {
                literal.Visible = true;
            }
        }
    }
}

