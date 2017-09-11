namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberOrderDetails : VMemberTemplatedWebControl
    {
        private HyperLink hlinkGetRedPager;
        private Literal litActualPrice;
        private Literal litAddPayInfo;
        private Literal litAddress;
        private Literal litBuildPrice;
        private Literal litCounponPrice;
        private Literal litDisCountPrice;
        private Literal litExemption;
        private Literal litOrderDate;
        private Literal litOrderId;
        private OrderStatusLabel litOrderStatus;
        private Literal litPayTime;
        private Literal litPhone;
        private Literal litPointToCash;
        private Literal litRedPagerAmount;
        private Literal litRemark;
        private Literal litShippingCost;
        private Literal litShipTo;
        private Literal litShipToDate;
        private Literal litTotalPrice;
        private string orderId;
        private HtmlInputHidden orderStatus;
        private VshopTemplatedRepeater rptOrderProducts;
        private HtmlInputHidden txtOrderId;

        protected override void AttachChildControls()
        {
            this.orderId = this.Page.Request.QueryString["orderId"];
            this.litShipTo = (Literal) this.FindControl("litShipTo");
            this.litPhone = (Literal) this.FindControl("litPhone");
            this.litAddress = (Literal) this.FindControl("litAddress");
            this.litOrderId = (Literal) this.FindControl("litOrderId");
            this.litAddPayInfo = (Literal) this.FindControl("litAddPayInfo");
            this.litOrderDate = (Literal) this.FindControl("litOrderDate");
            this.litOrderStatus = (OrderStatusLabel) this.FindControl("litOrderStatus");
            this.rptOrderProducts = (VshopTemplatedRepeater) this.FindControl("rptOrderProducts");
            this.litTotalPrice = (Literal) this.FindControl("litTotalPrice");
            this.litPayTime = (Literal) this.FindControl("litPayTime");
            this.hlinkGetRedPager = (HyperLink) this.FindControl("hlinkGetRedPager");
            this.orderStatus = (HtmlInputHidden) this.FindControl("orderStatus");
            this.txtOrderId = (HtmlInputHidden) this.FindControl("txtOrderId");
            this.litRemark = (Literal) this.FindControl("litRemark");
            this.litShipToDate = (Literal) this.FindControl("litShipToDate");
            this.litShippingCost = (Literal) this.FindControl("litShippingCost");
            this.litCounponPrice = (Literal) this.FindControl("litCounponPrice");
            this.litRedPagerAmount = (Literal) this.FindControl("litRedPagerAmount");
            this.litExemption = (Literal) this.FindControl("litExemption");
            this.litPointToCash = (Literal) this.FindControl("litPointToCash");
            this.litBuildPrice = (Literal) this.FindControl("litBuildPrice");
            this.litDisCountPrice = (Literal) this.FindControl("litDisCountPrice");
            this.litActualPrice = (Literal) this.FindControl("litActualPrice");
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            if (orderInfo == null)
            {
                base.GotoResourceNotFound("此订单已不存在");
            }
            this.litShipTo.Text = orderInfo.ShipTo;
            this.litPhone.Text = orderInfo.CellPhone;
            this.litAddress.Text = orderInfo.ShippingRegion + orderInfo.Address;
            if (orderInfo.BargainDetialId > 0)
            {
                this.litOrderId.Text = this.orderId + "<span class='text-danger'> 【砍价】</span>";
            }
            else
            {
                this.litOrderId.Text = this.orderId;
            }
            this.litOrderDate.Text = orderInfo.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
            this.litTotalPrice.SetWhenIsNotNull(orderInfo.GetAmount().ToString("F2"));
            this.litOrderStatus.OrderStatusCode = orderInfo.OrderStatus;
            this.litOrderStatus.Gateway = orderInfo.Gateway;
            this.litPayTime.SetWhenIsNotNull(orderInfo.PayDate.HasValue ? orderInfo.PayDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
            OrderRedPagerInfo orderRedPagerInfo = OrderRedPagerBrower.GetOrderRedPagerInfo(this.orderId);
            if ((orderRedPagerInfo != null) && (orderRedPagerInfo.MaxGetTimes > orderRedPagerInfo.AlreadyGetTimes))
            {
                this.hlinkGetRedPager.NavigateUrl = "/vshop/GetRedShare.aspx?orderid=" + this.orderId;
                this.hlinkGetRedPager.Visible = true;
            }
            this.orderStatus.SetWhenIsNotNull(((int) orderInfo.OrderStatus).ToString());
            this.txtOrderId.SetWhenIsNotNull(this.orderId.ToString());
            decimal couponValue = 0M;
            couponValue = orderInfo.CouponValue;
            if (couponValue > 0M)
            {
                this.litCounponPrice.Text = "-\x00a5" + couponValue.ToString("F2");
            }
            else
            {
                this.litCounponPrice.Text = " \x00a5" + couponValue.ToString("F2").Trim(new char[] { '-' });
            }
            couponValue = orderInfo.RedPagerAmount;
            if (couponValue > 0M)
            {
                this.litRedPagerAmount.Text = "<div><span class=\"span-r-80\">代金券抵扣：</span>- \x00a5" + couponValue.ToString("F2") + "</div>";
            }
            couponValue = orderInfo.GetAdjustCommssion();
            if (couponValue > 0M)
            {
                this.litDisCountPrice.Text = "<div><span class=\"span-r-80\">价格调整：</span>- \x00a5" + couponValue.ToString("F2") + "</div>";
            }
            else
            {
                this.litDisCountPrice.Text = "<div><span class=\"span-r-80\">价格调整：</span> &nbsp;\x00a5" + couponValue.ToString("F2").Trim(new char[] { '-' }) + "</div>";
            }
            couponValue = orderInfo.PointToCash;
            if (couponValue > 0M)
            {
                this.litPointToCash.Text = "<div><span class=\"span-r-80\">积分抵现：</span>- \x00a5" + couponValue.ToString("F2") + "</div>";
            }
            couponValue = orderInfo.DiscountAmount;
            if (couponValue > 0M)
            {
                this.litExemption.Text = "<div><span class=\"span-r-80\">优惠减免：</span>- \x00a5" + couponValue.ToString("F2") + "</div>";
            }
            this.litShippingCost.Text = orderInfo.AdjustedFreight.ToString("F2");
            this.litShipToDate.SetWhenIsNotNull(orderInfo.ShipToDate);
            this.litBuildPrice.SetWhenIsNotNull(orderInfo.GetAmount().ToString("F2"));
            this.litActualPrice.SetWhenIsNotNull(orderInfo.GetTotal().ToString("F2"));
            this.litRemark.SetWhenIsNotNull(orderInfo.Remark);
            this.rptOrderProducts.DataSource = orderInfo.LineItems.Values;
            this.rptOrderProducts.DataBind();
            StringBuilder builder = new StringBuilder();
            decimal num3 = orderInfo.GetTotal() - orderInfo.BalancePayMoneyTotal;
            if (orderInfo.BalancePayMoneyTotal > 0M)
            {
                if (orderInfo.PayDate.HasValue)
                {
                    builder.Append("<hr /><div><span class='span-r-80'>余额支付：</span>&nbsp;&nbsp;<strong class='text-danger'>\x00a5" + orderInfo.BalancePayMoneyTotal.ToString("F2") + "</strong></div>");
                    if ((num3 - orderInfo.CouponFreightMoneyTotal) > 0M)
                    {
                        builder.Append("<div><span class='span-r-80'>" + orderInfo.PaymentType + "：</span>&nbsp;&nbsp;<strong class='text-danger'>\x00a5" + num3.ToString("F2") + "</strong></div>");
                    }
                }
                else
                {
                    decimal cashPayMoney = orderInfo.GetCashPayMoney();
                    builder.Append("<hr /><div><span class='span-r-80'>余额已支付：</span>- \x00a5" + orderInfo.BalancePayMoneyTotal.ToString("F2") + "</div>");
                    builder.Append("<div><span class='span-r-80'>还需支付：</span>&nbsp;<strong class='text-danger'> \x00a5" + cashPayMoney.ToString("F2") + "</strong></div>");
                }
                this.litAddPayInfo.Text = builder.ToString();
            }
            PageTitle.AddSiteNameTitle("订单详情");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberOrderDetails.html";
            }
            base.OnInit(e);
        }
    }
}

