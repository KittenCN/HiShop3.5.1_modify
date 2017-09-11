namespace Hidistro.UI.Web.Admin.Bargain
{
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Bargain;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BargainDetial : AdminPage
    {
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected CheckBox ckIsCommission;
        protected TextBox discount;
        protected Image imgeProductName;
        protected Label lbmemberNumber;
        protected Label lbNumberOfParticipants;
        protected Label lbproductName;
        protected Label lbSaleNumber;
        protected Label lbSalePrice;
        protected Label lbStatus;
        protected Label lbStock;
        protected Label lbtitle;
        protected Image productImage;
        public string productInfoHtml;
        protected RadioButton rbtBargainTypeOne;
        protected RadioButton rbtBargainTypeTwo;
        protected HtmlForm thisForm;
        protected TextBox txtBargainTypeOneValue;
        protected TextBox txtBargainTypeTwoValue1;
        protected TextBox txtBargainTypeTwoValue2;
        protected TextBox txtFloorPrice;
        protected TextBox txtInitialPrice;
        protected TextBox txtPurchaseNumber;
        protected TextBox txtRemarks;
        protected TextBox txtTitle;
        protected TextBox txtTranNumber;

        public BargainDetial() : base("m08", "yxp21")
        {
            this.productInfoHtml = "";
        }

        public string GetProductInfoHtml(ProductInfo product)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class='shop-img fl'>");
            builder.Append("<img src='" + (string.IsNullOrEmpty(product.ImageUrl1) ? "/utility/pics/none.gif" : product.ImageUrl1) + "'  width='60' height='60'>");
            builder.Append("</div>");
            builder.Append("<div class='shop-username fl ml10'>");
            builder.Append("<p>" + product.ProductName + "</p>");
            builder.Append("</div>");
            builder.Append(" <p class='fl ml20'>现价：￥" + product.MarketPrice.Value.ToString("f2") + "</p>");
            builder.Append(" <p class='fl ml20'>库存：" + ProductHelper.GetProductSumStock(product.ProductId) + "</p>");
            return builder.ToString();
        }

        private void LoadData()
        {
            if (this.Page.Request.QueryString["Id"] != null)
            {
                BargainInfo bargainInfo = BargainHelper.GetBargainInfo(int.Parse(this.Page.Request.QueryString["Id"]));
                if (bargainInfo != null)
                {
                    if (bargainInfo.ProductId > 0)
                    {
                        ProductInfo productDetails = ProductHelper.GetProductDetails(bargainInfo.ProductId);
                        this.productImage.ImageUrl = string.IsNullOrEmpty(productDetails.ImageUrl1) ? "/utility/pics/none.gif" : productDetails.ImageUrl1;
                        this.lbproductName.Text = productDetails.ProductName;
                        this.lbtitle.Text = bargainInfo.Title;
                        this.productInfoHtml = this.GetProductInfoHtml(productDetails);
                    }
                    this.txtTitle.Text = bargainInfo.Title;
                    this.calendarStartDate.Text = bargainInfo.BeginDate.ToString();
                    this.calendarEndDate.Text = bargainInfo.EndDate.ToString();
                    this.imgeProductName.ImageUrl = string.IsNullOrEmpty(bargainInfo.ActivityCover) ? "/utility/pics/none.gif" : bargainInfo.ActivityCover;
                    this.txtRemarks.Text = bargainInfo.Remarks;
                    this.txtTranNumber.Text = bargainInfo.ActivityStock.ToString();
                    this.txtPurchaseNumber.Text = bargainInfo.PurchaseNumber.ToString();
                    this.txtFloorPrice.Text = bargainInfo.FloorPrice.ToString("f2");
                    this.txtInitialPrice.Text = bargainInfo.InitialPrice.ToString("f2");
                    this.ckIsCommission.Checked = bargainInfo.IsCommission;
                    this.discount.Text = (((double) bargainInfo.CommissionDiscount) / 10.0).ToString();
                    if (bargainInfo.BargainType == 0)
                    {
                        this.rbtBargainTypeOne.Checked = true;
                        this.rbtBargainTypeTwo.Checked = false;
                        this.txtBargainTypeOneValue.Text = bargainInfo.BargainTypeMinVlue.ToString("f2");
                    }
                    else
                    {
                        this.rbtBargainTypeOne.Checked = false;
                        this.rbtBargainTypeTwo.Checked = true;
                        this.txtBargainTypeTwoValue1.Text = bargainInfo.BargainTypeMinVlue.ToString("f2");
                        this.txtBargainTypeTwoValue2.Text = bargainInfo.BargainTypeMaxVlue.ToString("f2");
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.LoadData();
            }
        }
    }
}

