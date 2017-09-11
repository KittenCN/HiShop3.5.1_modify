namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class LimitedTimeDiscountAddProduct : AdminPage
    {
        protected string actionName;
        protected Button btnSeach;
        private int? categoryId;
        protected ProductCategoriesDropDownList dropCategories;
        protected Repeater grdProducts;
        protected PageSize hrefPageSize;
        protected int id;
        protected Pager pager;
        protected HtmlForm thisForm;
        protected TextBox txtProductName;

        protected LimitedTimeDiscountAddProduct() : base("m08", "yxp24")
        {
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            string str = this.txtProductName.Text.Trim();
            int num = this.dropCategories.SelectedValue.HasValue ? Globals.ToNum(this.dropCategories.SelectedValue.Value) : 0;
            this.id = Globals.RequestQueryNum("id");
            int num2 = Globals.RequestQueryNum("pagesize");
            string url = "LimitedTimeDiscountAddProduct.aspx?id=" + this.id;
            if (num2 > 0)
            {
                url = url + "&pagesize=" + num2;
            }
            if (num > 0)
            {
                url = url + "&cid=" + num;
            }
            if (!string.IsNullOrEmpty(str))
            {
                url = url + "&key=" + base.Server.UrlEncode(str);
            }
            base.Response.Redirect(url);
        }

        private void DataBindDiscount()
        {
            string str = Globals.RequestQueryStr("key").Trim();
            if (!string.IsNullOrEmpty(str))
            {
                this.txtProductName.Text = str;
            }
            int num = Globals.RequestQueryNum("cid");
            this.id = Globals.RequestQueryNum("id");
            if (this.id > 0)
            {
                LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(this.id);
                if (discountInfo != null)
                {
                    this.actionName = discountInfo.ActivityName;
                }
                int? nullable = null;
                if (num > 0)
                {
                    nullable = new int?(num);
                    this.dropCategories.SelectedValue = new int?(num);
                }
                ProductQuery query = new ProductQuery {
                    Keywords = str,
                    ProductCode = "",
                    CategoryId = nullable,
                    PageSize = this.pager.PageSize,
                    PageIndex = this.pager.PageIndex,
                    SortOrder = SortAction.Desc,
                    SortBy = "DisplaySequence"
                };
                if (num > 0)
                {
                    query.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
                }
                DbQueryResult discountProduct = LimitedTimeDiscountHelper.GetDiscountProduct(query);
                this.grdProducts.DataSource = discountProduct.Data;
                this.grdProducts.DataBind();
                this.pager.TotalRecords = discountProduct.TotalRecords;
            }
            else
            {
                base.Response.Redirect("LimitedTimeDiscountList.aspx");
            }
        }

        protected string GetDisable(string ActivityName, object limitedTimeDiscountId, int discountId)
        {
            if (!string.IsNullOrEmpty(ActivityName) && (Globals.ToNum(limitedTimeDiscountId) != discountId))
            {
                return "disabled";
            }
            return "";
        }

        protected string GetDisplay(object obj)
        {
            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                return "";
            }
            return "none";
        }

        protected string GetDisplayValue(object obj)
        {
            decimal num;
            if (decimal.TryParse(obj.ToString(), out num) && (num > 0M))
            {
                return "";
            }
            return "none";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.dropCategories.IsUnclassified = true;
                this.dropCategories.DataBind();
                this.DataBindDiscount();
            }
        }
    }
}

