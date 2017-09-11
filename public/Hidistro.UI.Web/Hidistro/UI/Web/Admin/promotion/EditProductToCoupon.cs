namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditProductToCoupon : AdminPage
    {
        protected Button btnQuery;
        protected int couponId;
        protected Repeater grdProducts;
        protected PageSize hrefPageSize;
        protected Label lblJoin;
        protected Label lbsaleNumber;
        protected Label lbwareNumber;
        protected Pager pager;
        protected HtmlForm thisForm;
        protected TextBox txt_maxPrice;
        protected TextBox txt_minPrice;
        protected TextBox txt_name;

        protected EditProductToCoupon() : base("m08", "yxp01")
        {
        }

        private bool bDecimal(string val, ref decimal i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return decimal.TryParse(val, out i);
        }

        private void BindProducts(int couponId)
        {
            DataTable selectedProducts = this.GetSelectedProducts(couponId);
            string text = this.txt_name.Text;
            string val = this.txt_minPrice.Text;
            string str3 = this.txt_maxPrice.Text;
            decimal? nullable = null;
            decimal? nullable2 = null;
            decimal i = 0M;
            if (!this.bDecimal(val, ref i))
            {
                nullable = null;
            }
            else
            {
                nullable = new decimal?(i);
            }
            if (!this.bDecimal(str3, ref i))
            {
                nullable2 = null;
            }
            else
            {
                nullable2 = new decimal?(i);
            }
            ProductQuery entity = new ProductQuery {
                Keywords = text,
                ProductCode = "",
                CategoryId = null,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                StartDate = null,
                BrandId = null,
                EndDate = null,
                TypeId = null,
                SaleStatus = ProductSaleStatus.All,
                minPrice = nullable,
                maxPrice = nullable2
            };
            string str4 = string.Empty;
            foreach (DataRow row in selectedProducts.Rows)
            {
                str4 = str4 + row["ProductId"].ToString() + ",";
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult productsFromGroup = ProductHelper.GetProductsFromGroup(entity, str4.TrimEnd(new char[] { ',' }));
            DataTable data = (DataTable) productsFromGroup.Data;
            data.Columns.Add("canstopStatus");
            data.Columns.Add("canopenStatus");
            if ((data != null) && (selectedProducts != null))
            {
                if ((data.Rows.Count > 0) && (selectedProducts.Rows.Count > 0))
                {
                    for (int j = 0; j < data.Rows.Count; j++)
                    {
                        string str5 = data.Rows[j]["ProductId"].ToString();
                        if (selectedProducts.Select(" productId='" + str5 + "'")[0]["status"].ToString() == "0")
                        {
                            data.Rows[j]["canstopStatus"] = "''";
                            data.Rows[j]["canopenStatus"] = "none";
                        }
                        else
                        {
                            data.Rows[j]["canstopStatus"] = "none";
                            data.Rows[j]["canopenStatus"] = "''";
                        }
                    }
                }
                else if (data.Rows.Count > 0)
                {
                    for (int k = 0; k < data.Rows.Count; k++)
                    {
                        data.Rows[k]["canstopStatus"] = "none";
                        data.Rows[k]["canopenStatus"] = "''";
                    }
                }
            }
            this.grdProducts.DataSource = productsFromGroup.Data;
            this.grdProducts.DataBind();
            this.pager.TotalRecords = productsFromGroup.TotalRecords;
            this.lblJoin.Text = selectedProducts.Rows.Count.ToString();
            this.setInSaleAndStock();
        }

        private bool bInt(string val, ref int i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            if (val.Contains(".") || val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.BindProducts(this.couponId);
        }

        private DataTable GetSelectedProducts(int couponId)
        {
            return CouponHelper.GetCouponProducts(couponId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.Params.AllKeys.Contains<string>("id") && !this.bInt(base.Request["id"].ToString(), ref this.couponId))
            {
                this.couponId = 0;
            }
            this.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            if (!base.IsPostBack)
            {
                this.BindProducts(this.couponId);
            }
        }

        private void setInSaleAndStock()
        {
            DataTable productNum = ProductHelper.GetProductNum();
            this.lbsaleNumber.Text = productNum.Rows[0]["OnSale"].ToString();
            this.lbwareNumber.Text = productNum.Rows[0]["OnStock"].ToString();
        }
    }
}

