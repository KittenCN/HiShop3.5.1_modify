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
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using Hishop.TransferManager;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductBatchExport)]
    public class ExportToTB : AdminPage
    {
        private int? _categoryId;
        private DateTime? _endDate;
        private bool _includeInStock;
        private bool _includeOnSales;
        private bool _includeUnSales;
        private string _isMakeTaobao;
        private string _productCode;
        private string _productName;
        private DateTime? _startDate;
        protected Button btnExport;
        protected Button btnSearch;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected CheckBox chkExportStock;
        protected CheckBox chkInStock;
        protected CheckBox chkOnSales;
        protected CheckBox chkUnSales;
        protected DropDownList dpTaoBao;
        protected ProductCategoriesDropDownList dropCategories;
        protected DropDownList dropExportVersions;
        protected Grid grdProducts;
        protected Label lblTotals;
        protected Pager pager;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        protected ExportToTB() : base("m02", "spp05")
        {
        }

        private void BindExporter()
        {
            this.dropExportVersions.Items.Clear();
            this.dropExportVersions.Items.Add(new ListItem("-请选择-", ""));
            Dictionary<string, string> exportAdapters = TransferHelper.GetExportAdapters(new YfxTarget("1.2"), "淘宝助理");
            foreach (string str in exportAdapters.Keys)
            {
                this.dropExportVersions.Items.Add(new ListItem(exportAdapters[str], str));
            }
        }

        private void BindProducts()
        {
            if ((!this._includeUnSales && !this._includeOnSales) && !this._includeInStock)
            {
                this.ShowMsg("至少要选择包含一个商品状态", false);
            }
            else
            {
                DbQueryResult exportProducts = ProductHelper.GetExportProducts(this.GetQuery(), (string) this.ViewState["RemoveProductIds"]);
                this.grdProducts.DataSource = exportProducts.Data;
                this.grdProducts.DataBind();
                this.pager.TotalRecords = exportProducts.TotalRecords;
                this.lblTotals.Text = exportProducts.TotalRecords.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if ((!this._includeUnSales && !this._includeOnSales) && !this._includeInStock)
            {
                this.ShowMsg("至少要选择包含一个商品状态", false);
            }
            else
            {
                string selectedValue = this.dropExportVersions.SelectedValue;
                if (string.IsNullOrEmpty(selectedValue) || (selectedValue.Length == 0))
                {
                    this.ShowMsg("请选择一个导出版本", false);
                }
                else
                {
                    bool includeCostPrice = false;
                    bool includeStock = this.chkExportStock.Checked;
                    bool flag3 = true;
                    string str2 = "http://" + HttpContext.Current.Request.Url.Host + ((HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + HttpContext.Current.Request.Url.Port)) + Globals.ApplicationPath;
                    string applicationPath = Globals.ApplicationPath;
                    DataSet set = ProductHelper.GetExportProducts(this.GetQuery(), includeCostPrice, includeStock, (string) this.ViewState["RemoveProductIds"]);
                    TransferHelper.GetExporter(selectedValue, new object[] { set, includeCostPrice, includeStock, flag3, str2, applicationPath }).DoExport();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReSearchProducts();
        }

        private AdvancedProductQuery GetQuery()
        {
            AdvancedProductQuery entity = new AdvancedProductQuery {
                Keywords = this._productName,
                ProductCode = this._productCode,
                CategoryId = this._categoryId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SaleStatus = ProductSaleStatus.OnSale,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                StartDate = this._startDate,
                EndDate = this._endDate,
                IncludeInStock = this._includeInStock,
                IncludeOnSales = this._includeOnSales,
                IncludeUnSales = this._includeUnSales,
                IsMakeTaobao = new int?(Convert.ToInt32(this._isMakeTaobao))
            };
            if (this._categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(this._categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            return entity;
        }

        private void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                int num2 = (int) this.grdProducts.DataKeys[rowIndex].Value;
                string str = (string) this.ViewState["RemoveProductIds"];
                if (string.IsNullOrEmpty(str))
                {
                    str = num2.ToString();
                }
                else
                {
                    str = str + "," + num2.ToString();
                }
                this.ViewState["RemoveProductIds"] = str;
                this.BindProducts();
            }
        }

        private void LoadParameters()
        {
            int num;
            DateTime time;
            DateTime time2;
            this._productName = this.txtSearchText.Text.Trim();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this._productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
                this.txtSearchText.Text = this._productName;
            }
            this._productCode = this.txtSKU.Text.Trim();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                this._productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
                this.txtSKU.Text = this._productCode;
            }
            this._categoryId = this.dropCategories.SelectedValue;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]) && int.TryParse(this.Page.Request.QueryString["categoryId"], out num))
            {
                this._categoryId = new int?(num);
                this.dropCategories.SelectedValue = this._categoryId;
            }
            this._startDate = this.calendarStartDate.SelectedDate;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]) && DateTime.TryParse(this.Page.Request.QueryString["startDate"], out time))
            {
                this._startDate = new DateTime?(time);
                this.calendarStartDate.SelectedDate = this._startDate;
            }
            this._endDate = this.calendarEndDate.SelectedDate;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]) && DateTime.TryParse(this.Page.Request.QueryString["endDate"], out time2))
            {
                this._endDate = new DateTime?(time2);
                this.calendarEndDate.SelectedDate = this._endDate;
            }
            this._includeOnSales = this.chkOnSales.Checked;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeOnSales"]))
            {
                bool.TryParse(this.Page.Request.QueryString["includeOnSales"], out this._includeOnSales);
                this.chkOnSales.Checked = this._includeOnSales;
            }
            this._includeUnSales = this.chkUnSales.Checked;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeUnSales"]))
            {
                bool.TryParse(this.Page.Request.QueryString["includeUnSales"], out this._includeUnSales);
                this.chkUnSales.Checked = this._includeUnSales;
            }
            this._includeInStock = this.chkInStock.Checked;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeInStock"]))
            {
                bool.TryParse(this.Page.Request.QueryString["includeInStock"], out this._includeInStock);
                this.chkInStock.Checked = this._includeInStock;
            }
            this._isMakeTaobao = this.dpTaoBao.SelectedValue;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isMakeTaobao"]))
            {
                this._isMakeTaobao = this.Page.Request.QueryString["isMakeTaobao"];
                this.dpTaoBao.SelectedValue = this.Page.Request.QueryString["isMakeTaobao"];
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.grdProducts.RowCommand += new GridViewCommandEventHandler(this.grdProducts_RowCommand);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
            }
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindExporter();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindProducts();
            }
        }

        private void ReSearchProducts()
        {
            NameValueCollection values2 = new NameValueCollection();
            values2.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            values2.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
            values2.Add("pageSize", this.pager.PageSize.ToString());
            values2.Add("includeOnSales", this.chkOnSales.Checked.ToString());
            values2.Add("includeUnSales", this.chkUnSales.Checked.ToString());
            values2.Add("includeInStock", this.chkInStock.Checked.ToString());
            values2.Add("isMakeTaobao", this.dpTaoBao.SelectedValue);
            NameValueCollection queryStrings = values2;
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

