namespace Hidistro.UI.Web.Admin.Goods
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditBaseInfo : AdminPage
    {
        protected Button btnAddOK;
        protected Button btnReplaceOK;
        protected Button btnSaveInfo;
        protected Grid grdSelectedProducts;
        private string productIds;
        protected TextBox txtNewWord;
        protected TextBox txtOleWord;
        protected TextBox txtPrefix;
        protected TextBox txtSuffix;

        protected EditBaseInfo() : base("m02", "00000")
        {
            this.productIds = string.Empty;
        }

        private void BindProduct()
        {
            string str = this.Page.Request.QueryString["ProductIds"];
            if (!string.IsNullOrEmpty(str))
            {
                this.grdSelectedProducts.DataSource = ProductHelper.GetProductBaseInfo(str);
                this.grdSelectedProducts.DataBind();
            }
        }

        private void btnAddOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPrefix.Text.Trim()) && string.IsNullOrEmpty(this.txtSuffix.Text.Trim()))
            {
                this.ShowMsgToTarget("前后缀不能同时为空", false, "parent");
            }
            else
            {
                if (ProductHelper.UpdateProductNames(this.productIds, this.txtPrefix.Text.Trim(), this.txtSuffix.Text.Trim()))
                {
                    this.ShowMsgToTarget("为商品名称添加前后缀成功", true, "parent");
                }
                else
                {
                    this.ShowMsgToTarget("为商品名称添加前后缀失败", false, "parent");
                }
                this.BindProduct();
            }
        }

        private void btnReplaceOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtOleWord.Text.Trim()))
            {
                this.ShowMsgToTarget("查找字符串不能为空", false, "parent");
            }
            else
            {
                if (ProductHelper.ReplaceProductNames(this.productIds, this.txtOleWord.Text.Trim(), this.txtNewWord.Text.Trim()))
                {
                    this.ShowMsgToTarget("为商品名称替换字符串缀成功", true, "parent");
                }
                else
                {
                    this.ShowMsgToTarget("为商品名称替换字符串缀失败", false, "parent");
                }
                this.BindProduct();
            }
        }

        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("MarketPrice");
            if (this.grdSelectedProducts.Rows.Count > 0)
            {
                decimal result = 0M;
                foreach (GridViewRow row in this.grdSelectedProducts.Rows)
                {
                    int num = (int) this.grdSelectedProducts.DataKeys[row.RowIndex].Value;
                    TextBox box = row.FindControl("txtProductName") as TextBox;
                    TextBox box2 = row.FindControl("txtProductCode") as TextBox;
                    TextBox box3 = row.FindControl("txtMarketPrice") as TextBox;
                    if (!string.IsNullOrEmpty(box3.Text.Trim()) && !decimal.TryParse(box3.Text.Trim(), out result))
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(box3.Text.Trim()))
                    {
                        result = 0M;
                    }
                    DataRow row2 = dt.NewRow();
                    row2["ProductId"] = num;
                    row2["ProductName"] = Globals.HtmlEncode(box.Text.Trim());
                    row2["ProductCode"] = Globals.HtmlEncode(box2.Text.Trim());
                    if (result >= 0M)
                    {
                        row2["MarketPrice"] = result;
                    }
                    dt.Rows.Add(row2);
                }
                if (ProductHelper.UpdateProductBaseInfo(dt))
                {
                    string str = Globals.RequestQueryStr("reurl");
                    if (string.IsNullOrEmpty(str))
                    {
                        str = "productonsales.aspx";
                    }
                    this.ShowMsgAndReUrl("修改成功", true, str, "parent");
                }
                else
                {
                    this.ShowMsgToTarget("批量修改商品信息失败,原价输入数据错误！", false, "parent");
                    this.BindProduct();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.productIds = this.Page.Request.QueryString["productIds"];
            this.btnSaveInfo.Click += new EventHandler(this.btnSaveInfo_Click);
            this.btnAddOK.Click += new EventHandler(this.btnAddOK_Click);
            this.btnReplaceOK.Click += new EventHandler(this.btnReplaceOK_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindProduct();
            }
        }
    }
}

