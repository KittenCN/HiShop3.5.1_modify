namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Commodities;
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

    public class EditProductToActivity : AdminPage
    {
        protected int _id;
        protected ProductSaleStatus _status;
        protected Button btnQuery;
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

        protected EditProductToActivity() : base("m08", "yxp05")
        {
            this._status = ProductSaleStatus.All;
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
                SaleStatus = this._status,
                minPrice = nullable,
                maxPrice = nullable2,
                selectQuery = string.Format("productId in (select productId from Hishop_Activities_Product where ActivitiesId={0})", this._id)
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            DataTable data = (DataTable) products.Data;
            data.Columns.Add("setStatus");
            if (((data != null) && (selectedProducts != null)) && ((data.Rows.Count > 0) && (selectedProducts.Rows.Count > 0)))
            {
                for (int j = 0; j < data.Rows.Count; j++)
                {
                    int num3 = int.Parse(data.Rows[j]["ProductId"].ToString());
                    DataRow[] rowArray = selectedProducts.Select(string.Format("ProductId={0}", num3));
                    if (rowArray.Length > 0)
                    {
                        data.Rows[j]["setStatus"] = (rowArray[0]["status"].ToString() == "0") ? "暂停" : "恢复";
                    }
                }
            }
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.pager.TotalRecords = products.TotalRecords;
            this.lblJoin.Text = (selectedProducts != null) ? selectedProducts.Rows.Count.ToString() : "0";
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
            this.BindProducts(this._id);
        }

        private DataTable GetSelectedProducts(int actId)
        {
            return ActivityHelper.QueryProducts(actId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.Params.AllKeys.Contains<string>("id") && !this.bInt(base.Request["id"].ToString(), ref this._id))
            {
                this._id = 0;
            }
            this.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            if (!base.IsPostBack)
            {
                this.BindProducts(this._id);
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

