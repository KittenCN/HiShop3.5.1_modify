namespace Hidistro.UI.Web.OpenAPI.Test
{
    using global::Hishop.Open.Api;
    using Hidistro.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.OpenAPI.Impl;
    using Hishop.Open.Api;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class TestProductApi : AdminPage
    {
        protected Button btnTestGetProduct;
        protected Button btnTestGetSoldProducts;
        protected Button btnTestUpdateProductApproveStatus;
        protected Button btnTestUpdateProductQuantity;
        protected DropDownList ddlStatus;
        protected HtmlForm form1;
        protected Label lbId;
        protected Label lbPId;
        protected Label lbProductId;
        protected Label lbProductSku;
        protected Label lbProductSkuAmount;
        protected Label lbStatus;
        protected Label lbUpdateType;
        private IProduct productApi;
        protected TextBox txtId;
        protected TextBox txtPId;
        protected TextBox txtProductId;
        protected TextBox txtProductSku;
        protected TextBox txtProductSkuAmount;
        protected TextBox txtTestGetProduct;
        protected TextBox txtTestGetSoldProducts;
        protected TextBox txtTestUpdateProductQuantity;
        protected TextBox txtUpdateProductApproveStatus;
        protected TextBox txtUpdateType;

        protected TestProductApi() : base("m03", "00000")
        {
            this.productApi = new ProductApi();
        }

        protected void btnTestGetProduct_Click(object sender, EventArgs e)
        {
            int num = Globals.ToNum(this.txtProductId.Text);
            string product = this.productApi.GetProduct(num);
            this.txtTestGetProduct.Text = product;
        }

        protected void btnTestGetSoldProducts_Click(object sender, EventArgs e)
        {
            string str = this.productApi.GetSoldProducts(null, null, "", "", "", 1, 10);
            this.txtTestGetSoldProducts.Text = str;
        }

        protected void btnTestUpdateProductApproveStatus_Click(object sender, EventArgs e)
        {
            int num = Globals.ToNum(this.txtId.Text);
            string selectedValue = this.ddlStatus.SelectedValue;
            string str2 = this.productApi.UpdateProductApproveStatus(num, selectedValue);
            this.txtUpdateProductApproveStatus.Text = str2;
        }

        protected void btnTestUpdateProductQuantity_Click(object sender, EventArgs e)
        {
            int num = Globals.ToNum(this.txtPId.Text);
            string text = this.txtProductSku.Text;
            int quantity = Globals.ToNum(this.txtProductSkuAmount.Text);
            int type = Globals.ToNum(this.txtUpdateType.Text);
            string str2 = this.productApi.UpdateProductQuantity(num, text, quantity, type);
            this.txtTestUpdateProductQuantity.Text = str2;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

