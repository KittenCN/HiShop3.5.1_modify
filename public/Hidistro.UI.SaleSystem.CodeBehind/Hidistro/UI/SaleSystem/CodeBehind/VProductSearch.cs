namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VProductSearch : VshopTemplatedWebControl
    {
        private int categoryId;
        private HiImage imgUrl;
        private string keyWord;
        private Literal litContent;
        private VshopTemplatedRepeater rptCategories;
        private VshopTemplatedRepeater rptProducts;

        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            this.imgUrl = (HiImage) this.FindControl("imgUrl");
            this.litContent = (Literal) this.FindControl("litContent");
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.rptCategories = (VshopTemplatedRepeater) this.FindControl("rptCategories");
            this.Page.Session["stylestatus"] = "4";
            this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, 0x3e8);
            this.rptCategories.DataSource = maxSubCategories;
            this.rptCategories.DataBind();
            PageTitle.AddSiteNameTitle("分类搜索页");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vProductSearch.html";
            }
            base.OnInit(e);
        }

        private void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.Controls[0].FindControl("litpromotion");
                if (!string.IsNullOrEmpty(literal.Text))
                {
                    literal.Text = "<img src='" + literal.Text + "'></img>";
                }
                else
                {
                    literal.Text = "<img src='/Storage/master/default.png'></img>";
                }
            }
        }
    }
}

