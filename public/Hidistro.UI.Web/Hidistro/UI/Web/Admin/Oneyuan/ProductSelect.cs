namespace Hidistro.UI.Web.Admin.Oneyuan
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ProductSelect : Page
    {
        protected Button btnSearch;
        private int? categoryId;
        protected ProductCategoriesDropDownList dropCategories;
        protected HtmlForm form1;
        protected Repeater grdProducts;
        protected int IsMultil;
        protected Pager pager;
        private string productName;
        private ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
        protected TextBox txtSearchText;

        private void BindProducts()
        {
            this.LoadParameters();
            ProductQuery entity = new ProductQuery {
                Keywords = this.productName,
                CategoryId = this.categoryId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                BrandId = null,
                SaleStatus = this.saleStatus
            };
            if (this.categoryId.HasValue && (this.categoryId > 0))
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.txtSearchText.Text = entity.Keywords;
            this.dropCategories.SelectedValue = entity.CategoryId;
            ProductHelper.GetProductNum();
            this.pager.TotalRecords = products.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return base.Request.Url.AbsolutePath;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(base.Request.Url.AbsolutePath).Append("?");
            foreach (string str2 in queryStrings.Keys)
            {
                string str = queryStrings[str2].Trim().Replace("'", "");
                if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                {
                    builder.Append(str2).Append("=").Append(base.Server.UrlEncode(str)).Append("&");
                }
            }
            queryStrings.Clear();
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
            {
                this.categoryId = new int?(result);
            }
            this.txtSearchText.Text = this.productName;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(base.Request.QueryString["IsMultil"], out this.IsMultil);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.IsUnclassified = true;
                this.dropCategories.DataBind();
                this.BindProducts();
            }
        }

        protected void ReloadPage(NameValueCollection queryStrings)
        {
            base.Response.Redirect(this.GenericReloadUrl(queryStrings));
        }

        private void ReloadProductOnSales(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("SaleStatus", "1");
            this.ReloadPage(queryStrings);
        }
    }
}

