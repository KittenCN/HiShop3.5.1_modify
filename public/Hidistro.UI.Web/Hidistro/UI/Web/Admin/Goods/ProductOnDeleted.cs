namespace Hidistro.UI.Web.Admin.Goods
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Products)]
    public class ProductOnDeleted : AdminPage
    {
        protected LinkButton btnInStock;
        protected LinkButton btnOffShelf;
        protected Button btnOK;
        protected Button btnSearch;
        protected LinkButton btnUpShelf;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        private int? categoryId;
        protected CheckBox chkDeleteImage;
        protected HtmlInputHidden currentProductId;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductCategoriesDropDownList dropCategories;
        private DateTime? endDate;
        protected Repeater grdProducts;
        protected HtmlInputHidden hdPenetrationStatus;
        protected PageSize hrefPageSize;
        protected Pager pager;
        private string productCode;
        private string productName;
        private DateTime? startDate;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        protected ProductOnDeleted() : base("m02", "spp11")
        {
        }

        private void BindProducts()
        {
            this.LoadParameters();
            ProductQuery entity = new ProductQuery {
                Keywords = this.productName,
                ProductCode = this.productCode,
                CategoryId = this.categoryId,
                StartDate = this.startDate,
                EndDate = this.endDate,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SaleStatus = ProductSaleStatus.Delete,
                SortOrder = SortAction.Desc,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                SortBy = "DisplaySequence"
            };
            if (this.categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.txtSearchText.Text = entity.Keywords;
            this.txtSKU.Text = entity.ProductCode;
            this.dropCategories.SelectedValue = entity.CategoryId;
            this.pager.TotalRecords = products.TotalRecords;
        }

        private void btnInStock_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要入库的商品", false);
            }
            else if (ProductHelper.InStock(str) > 0)
            {
                this.ShowMsg("成功入库了选择的商品，您可以在仓库里的商品里面找到入库以后的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("入库商品失败，未知错误", false);
            }
        }

        private void btnOffShelf_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要下架的商品", false);
            }
            else if (ProductHelper.OffShelf(str) > 0)
            {
                this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("下架商品失败，未知错误", false);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string str = this.currentProductId.Value;
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要删除的商品", false);
            }
            else if (ProductHelper.DeleteProduct(str, this.hdPenetrationStatus.Value.Equals("1")) > 0)
            {
                this.ShowMsg("成功的删除了商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("删除商品失败，未知错误", false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private void btnUpShelf_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要上架的商品", false);
            }
            else
            {
                string[] strArray = str.Split(new char[] { ',' });
                string str2 = "";
                foreach (string str3 in strArray)
                {
                    if (ProductHelper.GetProductSumStock(Convert.ToInt32(str3)) <= 0L)
                    {
                        str = str.Replace(str3 + ",", "").Replace("," + str3, "").Replace(str3, "");
                        str2 = str2 + str3 + ",";
                    }
                }
                if (string.IsNullOrEmpty(str))
                {
                    this.ShowMsg("库存为0的商品不能上架！", false);
                }
                else if (ProductHelper.UpShelf(str) > 0)
                {
                    if (!string.IsNullOrEmpty(str2))
                    {
                        this.ShowMsg("成功上架了库存不为0的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                        this.BindProducts();
                    }
                    else
                    {
                        this.ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                        this.BindProducts();
                    }
                }
                else
                {
                    this.ShowMsg("上架商品失败，未知错误", false);
                }
            }
        }

        private void grdProducts_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "UpShelf")
            {
                string str = e.CommandArgument.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    this.ShowMsg("请先选择要上架的商品", false);
                    return;
                }
                if (ProductHelper.GetProductSumStock(Convert.ToInt32(str)) <= 0L)
                {
                    str = str.Replace(str, "");
                }
                if (string.IsNullOrEmpty(str))
                {
                    this.ShowMsg("库存为0的商品不能上架！", false);
                    return;
                }
                if (ProductHelper.UpShelf(str) > 0)
                {
                    this.ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("上架商品失败，未知错误", false);
                }
            }
            if (e.CommandName == "InStock")
            {
                string str2 = e.CommandArgument.ToString();
                if (string.IsNullOrEmpty(str2))
                {
                    this.ShowMsg("请先选择要入库的商品", false);
                }
                else if (ProductHelper.InStock(str2) > 0)
                {
                    this.ShowMsg("成功入库了选择的商品，您可以在仓库里的商品里面找到入库以后的商品", true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("入库商品失败，未知错误", false);
                }
            }
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
            }
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
            {
                this.categoryId = new int?(result);
            }
            int num2 = 0;
            if (int.TryParse(this.Page.Request.QueryString["brandId"], out num2))
            {
                this.dropBrandList.SelectedValue = new int?(num2);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.startDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.endDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            this.txtSearchText.Text = this.productName;
            this.txtSKU.Text = this.productCode;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;
            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnUpShelf.Click += new EventHandler(this.btnUpShelf_Click);
            this.btnOffShelf.Click += new EventHandler(this.btnOffShelf_Click);
            this.btnInStock.Click += new EventHandler(this.btnInStock_Click);
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropBrandList.DataBind();
                this.dropCategories.DataBind();
                this.BindProducts();
            }
            this.grdProducts.ItemCommand += new RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadProductOnSales(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            queryStrings.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            if (this.dropBrandList.SelectedValue.HasValue)
            {
                queryStrings.Add("brandId", this.dropBrandList.SelectedValue.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

