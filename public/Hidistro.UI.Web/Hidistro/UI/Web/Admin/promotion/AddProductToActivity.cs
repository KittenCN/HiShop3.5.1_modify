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

    public class AddProductToActivity : AdminPage
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

        protected AddProductToActivity() : base("m08", "yxp05")
        {
            this._status = ProductSaleStatus.OnSale;
        }

        private bool bDecimal(string val, ref decimal i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return decimal.TryParse(val, out i);
        }

        private void BindProducts(int actId)
        {
            DataTable selectedProducts = this.GetSelectedProducts();
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
                maxPrice = nullable2
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            DataTable data = (DataTable) products.Data;
            data.Columns.Add("seledStatus");
            data.Columns.Add("canSelStatus");
            data.Columns.Add("canChkStatus");
            data.Columns.Add("btnCode");
            if (data != null)
            {
                if (data.Rows.Count > 0)
                {
                    string str4 = "0";
                    for (int j = 0; j < data.Rows.Count; j++)
                    {
                        int num3 = int.Parse(data.Rows[j]["ProductId"].ToString());
                        DataRow[] rowArray = selectedProducts.Select(string.Format("ProductId={0}", num3));
                        if (rowArray.Length > 0)
                        {
                            bool flag = false;
                            foreach (DataRow row in rowArray)
                            {
                                if (row["ActivitiesId"].ToString() == this._id.ToString())
                                {
                                    flag = true;
                                    break;
                                }
                                str4 = row["ActivitiesId"].ToString();
                            }
                            if (flag)
                            {
                                data.Rows[j]["btnCode"] = "<button type=\"button\" class=\"btn btn-success resetSize\" name=\"selectBtn\" disabled>已加入本次活动</button>";
                            }
                            else
                            {
                                data.Rows[j]["btnCode"] = "<a title=\"点击新窗口打开该活动\" href=\"AddProductToActivity.aspx?id=" + str4 + "\" target=\"_blank\" class=\"btn btn-warning resetSize\">已加入其他活动</button>";
                            }
                            continue;
                        }
                        data.Rows[j]["btnCode"] = "<input type=\"button\" class=\"btn btn-info resetSize\" name=\"selectBtn\" value=\"选取加入\" onclick=\"var obj=this;HiTipsShow('<strong>确定要加入该活动吗？</strong><p>加入该活动后，其他满减活动将不能选择该商品！</p>', 'confirmII', function () {saveSingle(" + data.Rows[j]["ProductId"].ToString() + ")});\"/>";
                    }
                }
                else if (data.Rows.Count > 0)
                {
                    for (int k = 0; k < data.Rows.Count; k++)
                    {
                    }
                }
            }
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.pager.TotalRecords = products.TotalRecords;
            this.lbsaleNumber.Text = products.TotalRecords.ToString();
            DataTable table3 = this.GetSelectedProducts(actId);
            this.lblJoin.Text = (table3 != null) ? table3.Rows.Count.ToString() : "0";
            this.setInStock();
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

        private DataTable GetSelectedProducts()
        {
            return ActivityHelper.QuerySelProducts();
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

        private void setInStock()
        {
            DataTable productNum = ProductHelper.GetProductNum();
            this.lbwareNumber.Text = productNum.Rows[0]["OnStock"].ToString();
        }
    }
}

