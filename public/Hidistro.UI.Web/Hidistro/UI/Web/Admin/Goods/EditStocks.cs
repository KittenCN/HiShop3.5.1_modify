namespace Hidistro.UI.Web.Admin.Goods
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditStocks : AdminPage
    {
        protected Button btnOperationOK;
        protected Button btnSaveStock;
        protected Button btnTargetOK;
        protected Grid grdSelectedProducts;
        private string productIds;
        protected TextBox txtAddStock;
        protected TextBox txtTagetStock;

        protected EditStocks() : base("m01", "00000")
        {
            this.productIds = string.Empty;
        }

        private void BindProduct()
        {
            string str = this.Page.Request.QueryString["ProductIds"];
            if (!string.IsNullOrEmpty(str))
            {
                this.grdSelectedProducts.DataSource = ProductHelper.GetSkuStocks(str);
                this.grdSelectedProducts.DataBind();
            }
        }

        private void btnOperationOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsgToTarget("没有要修改的商品", false, "parent");
            }
            else
            {
                int result = 0;
                if (!int.TryParse(this.txtAddStock.Text, out result))
                {
                    this.ShowMsgToTarget("请输入正确的库存格式", false, "parent");
                }
                else if (ProductHelper.AddSkuStock(this.productIds, result))
                {
                    this.BindProduct();
                    this.ShowMsgToTarget("修改商品的库存成功", true, "parent");
                }
                else
                {
                    this.ShowMsgToTarget("修改商品的库存失败", false, "parent");
                }
            }
        }

        private void btnSaveStock_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> skuStocks = null;
            if (this.grdSelectedProducts.Rows.Count > 0)
            {
                skuStocks = new Dictionary<string, int>();
                foreach (GridViewRow row in this.grdSelectedProducts.Rows)
                {
                    int result = 0;
                    TextBox box = row.FindControl("txtStock") as TextBox;
                    if (int.TryParse(box.Text, out result))
                    {
                        string key = (string) this.grdSelectedProducts.DataKeys[row.RowIndex].Value;
                        skuStocks.Add(key, result);
                    }
                }
                if (skuStocks.Count > 0)
                {
                    if (ProductHelper.UpdateSkuStock(skuStocks))
                    {
                        string str2 = Globals.RequestQueryStr("reurl");
                        if (string.IsNullOrEmpty(str2))
                        {
                            str2 = "productonsales.aspx";
                        }
                        this.ShowMsgAndReUrl("保存成功", true, str2, "parent");
                    }
                    else
                    {
                        this.ShowMsgToTarget("批量修改库存失败", false, "parent");
                        this.BindProduct();
                    }
                }
            }
        }

        private void btnTargetOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsgToTarget("没有要修改的商品", false, "parent");
            }
            else
            {
                int result = 0;
                if (!int.TryParse(this.txtTagetStock.Text, out result))
                {
                    this.ShowMsgToTarget("请输入正确的库存格式", false, "parent");
                }
                else if (result < 0)
                {
                    this.ShowMsgToTarget("商品库存不能小于0", false, "parent");
                }
                else if (ProductHelper.UpdateSkuStock(this.productIds, result))
                {
                    this.BindProduct();
                    this.ShowMsgToTarget("修改商品的库存成功", true, "parent");
                }
                else
                {
                    this.ShowMsgToTarget("修改商品的库存失败", false, "parent");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.productIds = this.Page.Request.QueryString["productIds"];
            this.btnSaveStock.Click += new EventHandler(this.btnSaveStock_Click);
            this.btnTargetOK.Click += new EventHandler(this.btnTargetOK_Click);
            this.btnOperationOK.Click += new EventHandler(this.btnOperationOK_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindProduct();
            }
        }
    }
}

