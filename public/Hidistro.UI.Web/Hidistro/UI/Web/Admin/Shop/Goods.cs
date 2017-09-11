namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Goods : AdminPage
    {
        protected Button btnSearch;
        protected HtmlForm form1;
        protected Grid grdProducts;
        protected Grid grdTopCategries;
        protected Pager pager;
        private string productName;
        protected TextBox txtSearchText;

        protected Goods() : base("m01", "00000")
        {
        }

        private void BindData()
        {
            this.grdTopCategries.DataSource = CatalogHelper.GetSequenceCategories();
            this.grdTopCategries.DataBind();
        }

        private void BindProducts()
        {
            this.LoadParameters();
            ProductSaleStatus all = ProductSaleStatus.All;
            ProductQuery entity = new ProductQuery {
                Keywords = this.productName,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SaleStatus = all
            };
            this.txtSearchText.Text = entity.Keywords;
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!base.IsPostBack)
            {
                this.BindProducts();
                this.BindData();
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
            this.ReloadPage(queryStrings);
        }
    }
}

