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

    public class LimitedTimeDiscountProduct : AdminPage
    {
        protected string actionName;
        protected Button btnSeach;
        protected ProductCategoriesDropDownListNew dropCategories;
        protected Repeater grdProducts;
        protected PageSize hrefPageSize;
        protected int id;
        protected Pager pager;
        protected HtmlForm thisForm;
        protected TextBox txtProductName;

        protected LimitedTimeDiscountProduct() : base("m08", "yxp24")
        {
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            string str = this.txtProductName.Text.Trim();
            int num = this.dropCategories.SelectedValue.HasValue ? Globals.ToNum(this.dropCategories.SelectedValue.Value) : 0;
            this.id = Globals.RequestQueryNum("id");
            int num2 = Globals.RequestQueryNum("pagesize");
            string url = "LimitedTimeDiscountProduct.aspx?id=" + this.id;
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
            int? nullable = null;
            if (num > 0)
            {
                nullable = new int?(num);
                this.dropCategories.SelectedValue = new int?(num);
            }
            if (this.id > 0)
            {
                LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(this.id);
                if (discountInfo != null)
                {
                    this.actionName = discountInfo.ActivityName;
                }
                ProductQuery query = new ProductQuery {
                    Keywords = str,
                    ProductCode = "",
                    CategoryId = nullable,
                    PageSize = this.pager.PageSize,
                    PageIndex = this.pager.PageIndex,
                    SortOrder = SortAction.Desc
                };
                if (num > 0)
                {
                    query.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
                }
                DbQueryResult discountProducted = LimitedTimeDiscountHelper.GetDiscountProducted(query, this.id);
                this.grdProducts.DataSource = discountProducted.Data;
                this.grdProducts.DataBind();
                this.pager.TotalRecords = discountProducted.TotalRecords;
            }
            else
            {
                base.Response.Redirect("LimitedTimeDiscountList.aspx");
            }
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

        protected string GetStatus(string status)
        {
            string str = "";
            string str2 = status;
            if (str2 == null)
            {
                return str;
            }
            if (!(str2 == "1"))
            {
                if (str2 != "2")
                {
                    if (str2 != "3")
                    {
                        return str;
                    }
                    return "已暂停";
                }
            }
            else
            {
                return "进行中";
            }
            return "删除";
        }

        private void grdProducts_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MoveProduct")
            {
                object commandArgument = e.CommandArgument;
                if (!string.IsNullOrEmpty(commandArgument.ToString()) && LimitedTimeDiscountHelper.DeleteDiscountProduct(commandArgument.ToString()))
                {
                    this.ShowMsgAndReUrl("移除成功", true, "LimitedTimeDiscountProduct.aspx?id=" + Globals.RequestQueryNum("id"));
                }
            }
            if (e.CommandName == "Stop")
            {
                int result = 0;
                if (int.TryParse(e.CommandArgument.ToString(), out result))
                {
                    int status = (LimitedTimeDiscountHelper.GetDiscountProductInfoById(result).Status == 3) ? 1 : 3;
                    if (!string.IsNullOrEmpty(result.ToString()) && LimitedTimeDiscountHelper.ChangeDiscountProductStatus(result.ToString(), status))
                    {
                        this.ShowMsgAndReUrl("状态修改成功", true, "LimitedTimeDiscountProduct.aspx?id=" + Globals.RequestQueryNum("id"));
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdProducts.ItemCommand += new RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
            if (!base.IsPostBack)
            {
                this.dropCategories.IsUnclassified = true;
                this.dropCategories.DataBind();
                this.DataBindDiscount();
            }
        }
    }
}

