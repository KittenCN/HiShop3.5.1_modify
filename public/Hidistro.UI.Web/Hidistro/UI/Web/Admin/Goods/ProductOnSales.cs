namespace Hidistro.UI.Web.Admin.Goods
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Products)]
    public class ProductOnSales : AdminPage
    {
        protected Button btnCancelFreeShip;
        protected Button btnDelete;
        protected HyperLink btnDownTaobao;
        protected Button btnInStock;
        protected Button btnSearch;
        protected Button btnSetFreeShip;
        protected Button BtnTemplate;
        protected Button btnUnSale;
        protected Button btnUpdateProductTags;
        protected Button btnUpSale;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        private int? categoryId;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductCategoriesDropDownList dropCategories;
        private DateTime? endDate;
        protected FreightTemplateDownList FreightTemplateDownList1;
        protected Repeater grdProducts;
        protected PageSize hrefPageSize;
        protected Literal LitOnSale;
        protected Literal LitOnStock;
        protected ProductTagsLiteral litralProductTag;
        protected Literal LitZero;
        protected string LocalUrl;
        private UpdateStatistics myEvent;
        private StatisticNotifier myNotifier;
        protected Pager pager;
        private string productCode;
        private string productName;
        private ProductSaleStatus saleStatus;
        private DateTime? startDate;
        private int? tagId;
        protected TrimTextBox txtProductTag;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;
        private int? typeId;

        protected ProductOnSales() : base("m02", "spp02")
        {
            this.saleStatus = ProductSaleStatus.OnSale;
            this.LocalUrl = string.Empty;
            this.myNotifier = new StatisticNotifier();
            this.myEvent = new UpdateStatistics();
        }

        private void BindProducts()
        {
            this.LoadParameters();
            ProductQuery entity = new ProductQuery {
                Keywords = this.productName,
                ProductCode = this.productCode,
                CategoryId = this.categoryId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                StartDate = this.startDate,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TypeId = this.typeId,
                SaleStatus = this.saleStatus,
                EndDate = this.endDate
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
            this.txtSKU.Text = entity.ProductCode;
            this.dropCategories.SelectedValue = entity.CategoryId;
            DataTable productNum = ProductHelper.GetProductNum();
            this.LitOnSale.Text = "出售中(" + productNum.Rows[0]["OnSale"].ToString() + ")";
            this.LitOnStock.Text = "仓库中(" + productNum.Rows[0]["OnStock"].ToString() + ")";
            this.LitZero.Text = "已售罄(" + productNum.Rows[0]["Zero"].ToString() + ")";
            this.pager.TotalRecords = products.TotalRecords;
        }

        private void btnCancelFreeShip_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要删除的商品", false);
            }
            else if (ProductHelper.RemoveProduct(str) > 0)
            {
                try
                {
                    this.myNotifier.updateAction = UpdateAction.ProductUpdate;
                    this.myNotifier.actionDesc = "批量删除商品";
                    this.myNotifier.RecDateUpdate = DateTime.Today;
                    this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                    this.myNotifier.UpdateDB();
                }
                catch (Exception)
                {
                }
                this.ShowMsg("成功删除了选择的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("删除商品失败，未知错误", false);
            }
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
                this.ShowMsg("成功入库选择的商品，您可以在仓库区的商品里面找到入库以后的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("入库商品失败，未知错误", false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private void btnSetFreeShip_Click(object sender, EventArgs e)
        {
            bool isFree = ((Button) sender).ID == "btnSetFreeShip";
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要设置为包邮的商品", false);
            }
            else if (ProductHelper.SetFreeShip(str, isFree) > 0)
            {
                this.ShowMsg("成功" + (isFree ? "设置" : "取消") + "了商品包邮状态", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg((isFree ? "设置" : "取消") + "商品包邮状态失败，未知错误", false);
            }
        }

        private void BtnTemplate_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要设置为运费的商品", false);
            }
            else if (string.IsNullOrEmpty(this.FreightTemplateDownList1.SelectedValue.ToString()) || (this.FreightTemplateDownList1.SelectedValue == 0))
            {
                this.ShowMsg("请选择运费模板", false);
            }
            else if (ProductHelper.UpdateProductFreightTemplate(str, int.Parse(this.FreightTemplateDownList1.SelectedValue.ToString())) > 0)
            {
                this.ShowMsg("操作成功", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("操作失败", false);
            }
        }

        private void btnUnSale_Click(object sender, EventArgs e)
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

        private void btnUpdateProductTags_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要关联的商品", false);
            }
            else
            {
                IList<int> list = new List<int>();
                if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
                {
                    string str2 = this.txtProductTag.Text.Trim();
                    string[] strArray = null;
                    if (str2.Contains(","))
                    {
                        strArray = str2.Split(new char[] { ',' });
                    }
                    else
                    {
                        strArray = new string[] { str2 };
                    }
                    foreach (string str3 in strArray)
                    {
                        list.Add(Convert.ToInt32(str3));
                    }
                }
                string[] strArray2 = null;
                if (str.Contains(","))
                {
                    strArray2 = str.Split(new char[] { ',' });
                }
                else
                {
                    strArray2 = new string[] { str };
                }
                int num = 0;
                string[] strArray4 = strArray2;
                for (int i = 0; i < strArray4.Length; i++)
                {
                    string text1 = strArray4[i];
                }
                if (num > 0)
                {
                    this.ShowMsg(string.Format("已成功修改了{0}件商品的商品标签", num), true);
                }
                else
                {
                    this.ShowMsg("已成功取消了商品的关联商品标签", true);
                }
                this.txtProductTag.Text = "";
            }
        }

        private void btnUpSale_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要上架的商品", false);
            }
            else if (ProductHelper.UpShelf(str) > 0)
            {
                this.ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("上架商品失败，未知错误", false);
            }
        }

        private void dropSaleStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private void grdProducts_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (ProductHelper.RemoveProduct(e.CommandArgument.ToString()) > 0)
                {
                    try
                    {
                        this.myNotifier.updateAction = UpdateAction.ProductUpdate;
                        this.myNotifier.actionDesc = "单个删除商品";
                        this.myNotifier.RecDateUpdate = DateTime.Today;
                        this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                        this.myNotifier.UpdateDB();
                    }
                    catch (Exception)
                    {
                    }
                    this.ShowMsg("删除商品成功", true);
                    this.ReloadProductOnSales(false);
                }
            }
            else if (e.CommandName == "UnSaleProduct")
            {
                if (ProductHelper.OffShelf(e.CommandArgument.ToString()) > 0)
                {
                    this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("下架商品失败，未知错误", false);
                }
            }
        }

        private void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = (Literal) e.Row.FindControl("litSaleStatus");
                Literal literal2 = (Literal) e.Row.FindControl("litMarketPrice");
                if (literal.Text == "1")
                {
                    literal.Text = "出售中";
                }
                else if (literal.Text == "2")
                {
                    literal.Text = "下架区";
                }
                else
                {
                    literal.Text = "仓库中";
                }
                if (string.IsNullOrEmpty(literal2.Text))
                {
                    literal2.Text = "-";
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
            int num3 = 0;
            if (int.TryParse(this.Page.Request.QueryString["tagId"], out num3))
            {
                this.tagId = new int?(num3);
            }
            int num4 = 0;
            if (int.TryParse(this.Page.Request.QueryString["typeId"], out num4))
            {
                this.typeId = new int?(num4);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
            {
                this.saleStatus = (ProductSaleStatus) Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
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
            this.LocalUrl = base.Server.UrlEncode(base.Request.Url.ToString());
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.BtnTemplate.Click += new EventHandler(this.BtnTemplate_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnUpSale.Click += new EventHandler(this.btnUpSale_Click);
            this.btnUnSale.Click += new EventHandler(this.btnUnSale_Click);
            this.btnInStock.Click += new EventHandler(this.btnInStock_Click);
            this.btnCancelFreeShip.Click += new EventHandler(this.btnSetFreeShip_Click);
            this.btnSetFreeShip.Click += new EventHandler(this.btnSetFreeShip_Click);
            this.btnUpdateProductTags.Click += new EventHandler(this.btnUpdateProductTags_Click);
            this.grdProducts.ItemCommand += new RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.IsUnclassified = true;
                this.dropCategories.DataBind();
                this.dropBrandList.DataBind();
                this.FreightTemplateDownList1.DataBind();
                this.litralProductTag.DataBind();
                this.btnDownTaobao.NavigateUrl = string.Format("http://order1.kuaidiangtong.com/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}", base.Request.Url.Host, Globals.ApplicationPath);
                this.BindProducts();
            }
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
            queryStrings.Add("SaleStatus", "1");
            base.ReloadPage(queryStrings);
        }
    }
}

