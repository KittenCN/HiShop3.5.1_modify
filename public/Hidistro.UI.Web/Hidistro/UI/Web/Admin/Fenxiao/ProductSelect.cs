namespace Hidistro.UI.Web.Admin.Fenxiao
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

        private void grdProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "check")
            {
                string str = e.CommandArgument.ToString();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(masterSettings.DistributorProducts))
                {
                    string str2 = "";
                    if (masterSettings.DistributorProducts.Contains(str))
                    {
                        foreach (string str3 in masterSettings.DistributorProducts.Split(new char[] { ',' }))
                        {
                            if (!str3.Equals(str))
                            {
                                str2 = str2 + str3 + ",";
                            }
                        }
                        if (str2.Length > 0)
                        {
                            str2 = str2.Substring(0, str2.Length - 1);
                        }
                    }
                    else
                    {
                        str2 = masterSettings.DistributorProducts + "," + str;
                    }
                    masterSettings.DistributorProducts = str2;
                }
                else
                {
                    masterSettings.DistributorProducts = str;
                }
                SettingsManager.Save(masterSettings);
                this.BindProducts();
            }
        }

        private void grdProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (!string.IsNullOrEmpty(masterSettings.DistributorProducts))
                {
                    string str = DataBinder.Eval(e.Item.DataItem, "productid").ToString();
                    Button button = e.Item.FindControl("btnCheck") as Button;
                    HtmlControl control = e.Item.FindControl("trId") as HtmlControl;
                    if (masterSettings.DistributorProducts.Contains(str))
                    {
                        button.Text = "取消";
                        button.Attributes.CssStyle.Remove("background-color");
                        button.Attributes.CssStyle.Remove("border-color");
                        button.Attributes.CssStyle.Add("background-color", "#5cb85c");
                        button.Attributes.CssStyle.Add("border-color", "#4cae4c");
                        control.Attributes.Add("class", "selRow");
                    }
                    else
                    {
                        button.Text = "选择";
                        button.Attributes.CssStyle.Remove("background-color");
                        button.Attributes.CssStyle.Remove("border-color");
                        button.Attributes.CssStyle.Add("background-color", "#286090");
                        button.Attributes.CssStyle.Add("border-color", "#204d74");
                        control.Attributes.Remove("class");
                    }
                }
            }
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
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdProducts.ItemCommand += new RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
            this.grdProducts.ItemDataBound += new RepeaterItemEventHandler(this.grdProducts_ItemDataBound);
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

