namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Order_ChargesList : UserControl
    {
        protected HyperLink hlkFreightFreePromotion;
        protected HyperLink hlkSentTimesPointPromotion;
        protected Literal litCoupon;
        protected Literal litCouponValue;
        protected Literal litDiscount;
        protected Literal litExmition;
        protected Literal litFreight;
        protected Literal litInvoiceTitle;
        protected Literal litPayCharge;
        protected Literal litPayMode;
        protected Literal litPoints;
        protected Literal litRedPager;
        protected Literal litShippingMode;
        protected Literal litTax;
        protected Literal litTotalPrice;
        protected LinkButton lkBtnEditPayMode;
        protected Label lkBtnEditshipingMode;
        private OrderInfo order;

        public void LoadControls()
        {
            if ((this.order.OrderStatus == OrderStatus.WaitBuyerPay) || (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
            {
                this.lkBtnEditshipingMode.Visible = true;
            }
            if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                this.lkBtnEditPayMode.Visible = true;
            }
            this.litFreight.Text = Globals.FormatMoney(this.order.AdjustedFreight);
            if ((this.order.OrderStatus == OrderStatus.Finished) || (this.order.OrderStatus == OrderStatus.SellerAlreadySent))
            {
                this.litShippingMode.Text = this.order.RealModeName;
            }
            else
            {
                this.litShippingMode.Text = this.order.ModeName;
            }
            this.litPayMode.Text = this.order.PaymentType;
            if (this.order.IsFreightFree)
            {
                this.hlkFreightFreePromotion.Text = this.order.FreightFreePromotionName;
                this.hlkFreightFreePromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { this.order.FreightFreePromotionId });
            }
            this.litPayCharge.Text = Globals.FormatMoney(this.order.PayCharge);
            if (!string.IsNullOrEmpty(this.order.CouponName))
            {
                this.litCoupon.Text = "[" + this.order.CouponName + "]-" + Globals.FormatMoney(this.order.CouponValue);
            }
            else
            {
                this.litCoupon.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
            }
            this.litCouponValue.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
            if (this.order.RedPagerID > 0)
            {
                this.litRedPager.Text = Globals.FormatMoney(this.order.RedPagerAmount) + " [" + this.order.RedPagerActivityName + "]";
            }
            else
            {
                this.litRedPager.Text = "0.00";
            }
            this.litDiscount.Text = Globals.FormatMoney(this.order.AdjustedDiscount);
            this.litPoints.Text = this.order.Points.ToString(CultureInfo.InvariantCulture);
            if (this.order.IsSendTimesPoint)
            {
                this.hlkSentTimesPointPromotion.Text = this.order.SentTimesPointPromotionName;
                this.hlkSentTimesPointPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { this.order.SentTimesPointPromotionId });
            }
            this.litTotalPrice.Text = Globals.FormatMoney(this.order.GetTotal());
            this.litExmition.Text = Globals.FormatMoney(this.order.DiscountAmount);
            if (this.order.Tax > 0M)
            {
                this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(this.order.Tax);
                this.litTax.Text = this.litTax.Text + "</span></td></tr>";
            }
            if (this.order.InvoiceTitle.Length > 0)
            {
                this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + this.order.InvoiceTitle;
                this.litInvoiceTitle.Text = this.litInvoiceTitle.Text + "</span></td></tr>";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.LoadControls();
        }

        public OrderInfo Order
        {
            get
            {
                return this.order;
            }
            set
            {
                this.order = value;
            }
        }
    }
}

