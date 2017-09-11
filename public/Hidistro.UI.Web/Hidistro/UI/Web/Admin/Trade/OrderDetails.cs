namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.Vshop;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Orders)]
    public class OrderDetails : AdminPage
    {
        protected decimal BalancePayFreightMoneyTotal;
        protected decimal BalancePayMoneyTotal;
        protected Button btnAgreeConfirm;
        protected HtmlInputButton btnClocsOrder;
        protected Button btnCloseOrder;
        protected HtmlInputButton btnConfirmOrder;
        protected Button btnConfirmPay;
        protected HtmlInputButton btnModifyAddr;
        protected HtmlInputButton btnModifyPrice;
        protected Button btnMondifyPay;
        protected Button btnMondifyShip;
        protected Button btnRefuseConfirm;
        protected Button btnRemark;
        protected HtmlInputButton btnSendGoods;
        protected HtmlInputButton btnUpdateExpress;
        protected HtmlInputButton btnViewLogistic;
        protected string comCode;
        protected CloseTranReasonDropDownList ddlCloseReason;
        protected PaymentDropDownList ddlpayment;
        protected HtmlGenericControl divOrderProcess;
        protected HtmlGenericControl divRemarkShow;
        protected HiddenField hdBalance;
        protected HiddenField hdCompanyCode;
        protected HiddenField hdExpressUrl;
        protected HiddenField hdfOrderID;
        protected HiddenField hdHasNewKey;
        protected HiddenField hdOrderItemId;
        protected HiddenField hdProductID;
        protected HiddenField hdReturnsId;
        protected HiddenField hdSkuID;
        protected Label lbCloseReason;
        protected FormatedTimeLabel lblorderDateForRemark;
        protected OrderStatusLabel lblOrderStatus;
        protected FormatedMoneyLabel lblorderTotalForRemark;
        protected Literal lblOriAddress;
        protected Label lbReason;
        protected Literal litActivityShow;
        protected Literal litAddress;
        protected Literal litCommissionInfo;
        protected Literal litCompanyName;
        protected Literal litExOrderId;
        protected Literal litFinishDate;
        protected Literal litFreight;
        protected Literal litManagerRemark;
        protected Literal litModeName;
        protected Literal litName;
        protected Literal litOrderDate;
        protected Literal litOrderId;
        protected Literal litPayDate;
        protected Literal litPayType;
        protected Literal litRealName;
        protected Literal litRemark;
        protected Literal litShipOrderNumber;
        protected Literal litShippingDate;
        protected Literal litShippingRegion;
        protected Literal litShipToDate;
        protected Literal litSiteName;
        protected Literal litUserName;
        protected Literal litUserTel;
        protected Literal litWeiXinNickName;
        private UpdateStatistics myEvent;
        private StatisticNotifier myNotifier;
        private OrderInfo order;
        protected string orderId;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        protected OrderRemarkImage OrderRemarkImageLink;
        protected OrderStatusLabel OrderStatusLabelHtml;
        protected decimal otherDiscountPrice;
        protected HtmlGenericControl pLoginsticInfo;
        protected HtmlGenericControl pNewAddress;
        protected HtmlAnchor power;
        protected string ProcessClass2;
        protected string ProcessClass3;
        protected string ProcessClass4;
        private string reurl;
        protected Repeater rptItemList;
        protected Repeater rptRefundList;
        public bool showOrderExpress;
        protected Literal spanOrderId;
        protected TextBox txtAdminMemo;
        protected HtmlInputText txtcategoryId;
        protected TextBox txtMemo;
        protected TextBox txtMoney;
        protected TextBox txtRemark;
        protected TextBox txtShipOrderNumber;

        protected OrderDetails() : base("m03", "00000")
        {
            this.myNotifier = new StatisticNotifier();
            this.myEvent = new UpdateStatistics();
            this.orderId = Globals.RequestQueryStr("OrderId");
            this.comCode = "";
            this.reurl = string.Empty;
            this.ProcessClass2 = string.Empty;
            this.ProcessClass3 = string.Empty;
            this.ProcessClass4 = string.Empty;
        }

        private void BindRemark(OrderInfo order)
        {
            this.spanOrderId.Text = order.OrderId;
            this.lblorderDateForRemark.Time = order.OrderDate;
            this.lblorderTotalForRemark.Money = order.GetTotal();
            this.txtRemark.Text = Globals.HtmlDecode(order.ManagerRemark);
            this.orderRemarkImageForRemark.SelectedValue = order.ManagerMark;
        }

        protected void btnAgreeConfirm_Click(object sender, EventArgs e)
        {
            decimal result = 0M;
            decimal.TryParse(this.txtMoney.Text.Trim(), out result);
            decimal balancePayMoneyTotal = 0M;
            int num3 = 0;
            int num4 = Globals.ToNum(this.hdProductID.Value);
            int returnsid = Globals.ToNum(this.hdReturnsId.Value);
            this.hdSkuID.Value.Trim();
            if (num4 <= 0)
            {
                this.ShowMsg("服务器错误，请刷新页面重试！", false);
            }
            else
            {
                decimal num6 = 0M;
                decimal.TryParse(Globals.RequestFormStr("ctl00$ContentPlaceHolder1$hdBalance"), out num6);
                RefundInfo orderReturnsByReturnsID = RefundHelper.GetOrderReturnsByReturnsID(returnsid);
                if (orderReturnsByReturnsID != null)
                {
                    orderReturnsByReturnsID.AdminRemark = this.txtMemo.Text.Trim();
                    orderReturnsByReturnsID.HandleTime = DateTime.Now;
                    orderReturnsByReturnsID.RefundTime = DateTime.Now.ToString();
                    orderReturnsByReturnsID.HandleStatus = RefundInfo.Handlestatus.Refunded;
                    orderReturnsByReturnsID.Operator = Globals.GetCurrentManagerUserId().ToString();
                    if (result < 0M)
                    {
                        this.ShowMsg("输入的金额格式不正确", false);
                    }
                    else
                    {
                        orderReturnsByReturnsID.RefundMoney = result;
                        orderReturnsByReturnsID.RefundId = orderReturnsByReturnsID.ReturnsId;
                        orderReturnsByReturnsID.BalanceReturnMoney = num6;
                        if (RefundHelper.UpdateByReturnsId(orderReturnsByReturnsID))
                        {
                            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderReturnsByReturnsID.OrderId);
                            balancePayMoneyTotal = orderInfo.BalancePayMoneyTotal;
                            if (result > orderInfo.BalancePayMoneyTotal)
                            {
                                orderInfo.BalancePayMoneyTotal = 0M;
                                orderInfo.BalancePayFreightMoneyTotal = 0M;
                                foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                                {
                                    info3.BalancePayMoney = 0M;
                                }
                            }
                            else if ((result >= num6) && (result <= orderInfo.BalancePayMoneyTotal))
                            {
                                orderInfo.BalancePayMoneyTotal -= result;
                                orderInfo.BalancePayFreightMoneyTotal = ((orderInfo.BalancePayFreightMoneyTotal - (result - num6)) <= 0M) ? 0M : (orderInfo.BalancePayFreightMoneyTotal - (result - num6));
                                decimal num7 = (((result - num6) - orderInfo.BalancePayFreightMoneyTotal) <= 0M) ? 0M : ((result - num6) - orderInfo.BalancePayFreightMoneyTotal);
                                foreach (LineItemInfo info4 in orderInfo.LineItems.Values)
                                {
                                    if ((!string.IsNullOrEmpty(orderReturnsByReturnsID.SkuId) && (info4.SkuId == orderReturnsByReturnsID.SkuId)) || ((info4.ProductId == orderReturnsByReturnsID.ProductId) && string.IsNullOrEmpty(info4.SkuId)))
                                    {
                                        info4.BalancePayMoney = 0M;
                                    }
                                    else if ((num7 > 0M) && (info4.BalancePayMoney > 0M))
                                    {
                                        info4.BalancePayMoney = (info4.BalancePayMoney <= num7) ? 0M : (info4.BalancePayMoney - num7);
                                        num7 -= info4.BalancePayMoney;
                                    }
                                }
                            }
                            else if (result < num6)
                            {
                                orderInfo.BalancePayMoneyTotal -= result;
                                foreach (LineItemInfo info5 in orderInfo.LineItems.Values)
                                {
                                    if ((!string.IsNullOrEmpty(orderReturnsByReturnsID.SkuId) && (info5.SkuId == orderReturnsByReturnsID.SkuId)) || ((info5.ProductId == orderReturnsByReturnsID.ProductId) && string.IsNullOrEmpty(info5.SkuId)))
                                    {
                                        info5.BalancePayMoney -= result;
                                    }
                                }
                            }
                            string skuid = null;
                            string stock = null;
                            foreach (LineItemInfo info6 in orderInfo.LineItems.Values)
                            {
                                if ((!string.IsNullOrEmpty(orderReturnsByReturnsID.SkuId) && (info6.SkuId == orderReturnsByReturnsID.SkuId)) || ((info6.ProductId == orderReturnsByReturnsID.ProductId) && string.IsNullOrEmpty(info6.SkuId)))
                                {
                                    skuid = info6.SkuId;
                                    stock = info6.Quantity.ToString();
                                    info6.OrderItemsStatus = OrderStatus.Refunded;
                                    break;
                                }
                            }
                            if (!RefundHelper.UpdateOrderGoodStatu(this.hdfOrderID.Value, skuid, 9, orderReturnsByReturnsID.OrderItemID))
                            {
                                return;
                            }
                            RefundHelper.UpdateRefundOrderStock(stock, skuid);
                            foreach (LineItemInfo info7 in orderInfo.LineItems.Values)
                            {
                                if ((info7.OrderItemsStatus.ToString() == OrderStatus.Refunded.ToString()) || (info7.OrderItemsStatus.ToString() == OrderStatus.Returned.ToString()))
                                {
                                    num3++;
                                }
                            }
                            OrderHelper.UpdateOrderAmount(orderInfo);
                            if (orderInfo.LineItems.Values.Count == num3)
                            {
                                this.CloseOrder(this.hdfOrderID.Value);
                                orderInfo.OrderStatus = OrderStatus.Closed;
                            }
                            OrderHelper.UpdateCalculadtionCommission(this.hdfOrderID.Value);
                            int orderItemID = orderReturnsByReturnsID.OrderItemID;
                            decimal d = 0M;
                            if (orderItemID > 0)
                            {
                                if (OrderSplitHelper.GetLineItemInfo(orderItemID, "") != null)
                                {
                                    d = (result > balancePayMoneyTotal) ? balancePayMoneyTotal : result;
                                }
                                if (d > 0M)
                                {
                                    orderReturnsByReturnsID = RefundHelper.GetOrderReturnsByReturnsID(returnsid);
                                    RefundHelper.UpdateByReturnsId(orderReturnsByReturnsID);
                                    OrderHelper.MemberAmountAddByRefund(MemberHelper.GetMember(orderInfo.UserId), decimal.Round(d, 2), this.hdfOrderID.Value);
                                    OrderHelper.UpdateOrder(orderInfo);
                                    OrderHelper.UpdateOrderItems(orderInfo);
                                }
                            }
                            try
                            {
                                Messenger.SendWeiXinMsg_RefundSuccess(orderReturnsByReturnsID);
                            }
                            catch (Exception exception)
                            {
                                Globals.Debuglog("订单退款成功提醒消息异常" + exception.ToString(), "_DebuglogSendMsgRefund.txt");
                            }
                            this.ShowMsgAndReUrl("同意退款成功!", true, "OrderDetails.aspx?OrderId=" + this.hdfOrderID.Value + "&t=" + DateTime.Now.ToString("HHmmss"));
                            try
                            {
                                this.myNotifier.updateAction = UpdateAction.OrderUpdate;
                                this.myNotifier.actionDesc = "同意退款成功";
                                this.myNotifier.RecDateUpdate = orderInfo.PayDate.HasValue ? orderInfo.PayDate.Value : DateTime.Today;
                                this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                                this.myNotifier.UpdateDB();
                                return;
                            }
                            catch (Exception exception2)
                            {
                                Globals.Debuglog(exception2.Message, "_Debuglog.txt");
                                return;
                            }
                        }
                        this.ShowMsg("退款失败，请重试。", false);
                    }
                }
            }
        }

        private void btnCloseOrder_Click(object sender, EventArgs e)
        {
            this.order.CloseReason = this.ddlCloseReason.SelectedValue;
            if ("请选择关闭的理由" == this.order.CloseReason)
            {
                this.ShowMsg("请选择关闭的理由", false);
            }
            else if (OrderHelper.CloseTransaction(this.order))
            {
                this.order.OnClosed();
                this.ShowMsgAndReUrl("关闭订单成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
            }
            else
            {
                this.ShowMsg("关闭订单失败", false);
            }
        }

        protected void btnConfirmPay_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
            if ((orderInfo != null) && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
            {
                if (OrderHelper.ConfirmPay(orderInfo))
                {
                    DebitNoteInfo info2 = new DebitNoteInfo();
                    info2 = new DebitNoteInfo {
                        NoteId = Globals.GetGenerateId(),
                        OrderId = this.orderId,
                        Operator = ManagerHelper.GetCurrentManager().UserName,
                        Remark = "后台" + info2.Operator + "收款成功"
                    };
                    OrderHelper.SaveDebitNote(info2);
                    orderInfo.OnPayment();
                    this.ShowMsgAndReUrl("成功的确认了订单收款", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
                }
                else
                {
                    this.ShowMsg("确认订单收款失败", false);
                }
            }
        }

        private void btnMondifyPay_Click(object sender, EventArgs e)
        {
            this.order = OrderHelper.GetOrderInfo(this.orderId);
            if (this.ddlpayment.SelectedValue.HasValue && (this.ddlpayment.SelectedValue == -1))
            {
                this.order.PaymentTypeId = 0;
                this.order.PaymentType = "货到付款";
                this.order.Gateway = "hishop.plugins.payment.podrequest";
            }
            else if (this.ddlpayment.SelectedValue.HasValue && (this.ddlpayment.SelectedValue == 0x63))
            {
                this.order.PaymentTypeId = 0x63;
                this.order.PaymentType = "线下付款";
                this.order.Gateway = "hishop.plugins.payment.offlinerequest";
            }
            else if (this.ddlpayment.SelectedValue.HasValue && (this.ddlpayment.SelectedValue == 0x42))
            {
                this.order.PaymentTypeId = 0x42;
                this.order.PaymentType = "余额支付";
                this.order.Gateway = "hishop.plugins.payment.balancepayrequest";
            }
            else if (this.ddlpayment.SelectedValue.HasValue && (this.ddlpayment.SelectedValue == 0x4d))
            {
                this.order.PaymentTypeId = 0x4d;
                this.order.PaymentType = "积分抵现";
                this.order.Gateway = "hishop.plugins.payment.pointtocash";
            }
            else if (this.ddlpayment.SelectedValue.HasValue && (this.ddlpayment.SelectedValue == 0x58))
            {
                this.order.PaymentTypeId = 0x58;
                this.order.PaymentType = "微信支付";
                this.order.Gateway = "hishop.plugins.payment.weixinrequest";
            }
            else
            {
                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(this.ddlpayment.SelectedValue.Value);
                this.order.PaymentTypeId = paymentMode.ModeId;
                this.order.PaymentType = paymentMode.Name;
                this.order.Gateway = paymentMode.Gateway;
            }
            if (OrderHelper.UpdateOrderPaymentType(this.order))
            {
                this.ShowMsgAndReUrl("修改支付方式成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
            }
            else
            {
                this.ShowMsg("修改支付方式失败", false);
            }
        }

        protected void btnRefuseConfirm_Click(object sender, EventArgs e)
        {
            int num = Globals.ToNum(this.hdProductID.Value);
            this.hdSkuID.Value.Trim();
            Globals.ToNum(this.hdOrderItemId.Value);
            int returnsid = Globals.ToNum(this.hdReturnsId.Value);
            if (num <= 0)
            {
                this.ShowMsg("服务器错误，请刷新页面重试！", false);
            }
            else
            {
                RefundInfo orderReturnsByReturnsID = RefundHelper.GetOrderReturnsByReturnsID(returnsid);
                orderReturnsByReturnsID.RefundId = orderReturnsByReturnsID.ReturnsId;
                orderReturnsByReturnsID.AdminRemark = this.txtAdminMemo.Text.Trim();
                orderReturnsByReturnsID.HandleTime = DateTime.Now;
                orderReturnsByReturnsID.HandleStatus = RefundInfo.Handlestatus.RefuseRefunded;
                orderReturnsByReturnsID.Operator = Globals.GetCurrentManagerUserId().ToString();
                orderReturnsByReturnsID.BalanceReturnMoney = 0M;
                if (!RefundHelper.UpdateByReturnsId(orderReturnsByReturnsID))
                {
                    this.ShowMsg("操作失败，请重试。", false);
                }
                else
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hdfOrderID.Value);
                    string skuid = null;
                    foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                    {
                        if (info3.ProductId == num)
                        {
                            skuid = info3.SkuId;
                            OrderStatus orderItemsStatus = info3.OrderItemsStatus;
                            break;
                        }
                    }
                    if (RefundHelper.UpdateOrderGoodStatu(this.hdfOrderID.Value, skuid, 3, orderReturnsByReturnsID.OrderItemID))
                    {
                        this.ShowMsgAndReUrl("拒绝退款成功!", true, "OrderDetails.aspx?OrderId=" + this.hdfOrderID.Value + "&t=" + DateTime.Now.ToString("HHmmss"));
                    }
                }
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备注长度限制在300个字符以内", false);
            }
            else
            {
                this.order.OrderId = this.orderId;
                if (this.orderRemarkImageForRemark.SelectedItem != null)
                {
                    this.order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                }
                this.order.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                if (OrderHelper.SaveRemark(this.order))
                {
                    this.BindRemark(this.order);
                    this.ShowMsgAndReUrl("保存备注成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss"));
                }
                else
                {
                    this.ShowMsg("保存失败", false);
                }
            }
        }

        private void CloseOrder(string orderid)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderid);
            orderInfo.CloseReason = "客户要求退货(款)！";
            if (RefundHelper.CloseTransaction(orderInfo))
            {
                orderInfo.OnClosed();
                MemberHelper.GetMember(orderInfo.UserId);
            }
        }

        private void LoadUserControl(OrderInfo order)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ExpressSet expressSet = ExpressHelper.GetExpressSet();
            this.hdHasNewKey.Value = "0";
            this.hdExpressUrl.Value = "";
            if (expressSet != null)
            {
                if (!string.IsNullOrEmpty(expressSet.NewKey))
                {
                    this.hdHasNewKey.Value = "1";
                }
                if (!string.IsNullOrEmpty(expressSet.Url.Trim()))
                {
                    this.hdExpressUrl.Value = expressSet.Url.Trim();
                }
            }
            if (Globals.RequestFormStr("posttype") == "modifyRefundMondy")
            {
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                decimal result = 0M;
                decimal.TryParse(Globals.RequestFormStr("price"), out result);
                int productid = Globals.RequestFormNum("pid");
                string str3 = Globals.RequestFormStr("oid");
                if (((result > 0M) && (productid > 0)) && !string.IsNullOrEmpty(str3))
                {
                    if (RefundHelper.UpdateRefundMoney(str3, productid, result))
                    {
                        s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                    }
                }
                else if (result <= 0M)
                {
                    s = "{\"type\":\"0\",\"tips\":\"退款金额需大于0！\"}";
                }
                base.Response.Write(s);
                base.Response.End();
            }
            else
            {
                this.reurl = "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + DateTime.Now.ToString("HHmmss");
                this.btnMondifyPay.Click += new EventHandler(this.btnMondifyPay_Click);
                this.btnCloseOrder.Click += new EventHandler(this.btnCloseOrder_Click);
                this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
                this.order = OrderHelper.GetOrderInfo(this.orderId);
                if (this.order == null)
                {
                    base.Response.Write("原订单已删除！");
                    base.Response.End();
                }
                else if (!base.IsPostBack)
                {
                    this.btnUpdateExpress.Visible = this.order.OrderStatus == OrderStatus.SellerAlreadySent;
                    if (string.IsNullOrEmpty(this.orderId))
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.hdfOrderID.Value = this.orderId;
                        this.litOrderDate.Text = this.order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
                        if (this.order.PayDate.HasValue)
                        {
                            this.litPayDate.Text = DateTime.Parse(this.order.PayDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (this.order.ShippingDate.HasValue)
                        {
                            this.litShippingDate.Text = DateTime.Parse(this.order.ShippingDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (this.order.FinishDate.HasValue)
                        {
                            this.litFinishDate.Text = DateTime.Parse(this.order.FinishDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        this.lblOrderStatus.OrderStatusCode = this.order.OrderStatus;
                        switch (this.order.OrderStatus)
                        {
                            case OrderStatus.WaitBuyerPay:
                                this.ProcessClass2 = "active";
                                if (this.order.Gateway != "hishop.plugins.payment.podrequest")
                                {
                                    this.btnConfirmPay.Visible = true;
                                }
                                this.btnModifyAddr.Attributes.Add("onclick", "DialogFrame('../trade/ShipAddress.aspx?action=update&OrderId=" + this.orderId + "','修改收货地址',620,410)");
                                this.btnModifyAddr.Visible = true;
                                break;

                            case OrderStatus.BuyerAlreadyPaid:
                                this.ProcessClass2 = "ok";
                                this.ProcessClass3 = "active";
                                this.btnModifyAddr.Attributes.Add("onclick", "DialogFrame('../trade/ShipAddress.aspx?action=update&OrderId=" + this.orderId + "','修改收货地址',620,410)");
                                this.btnModifyAddr.Visible = true;
                                break;

                            case OrderStatus.SellerAlreadySent:
                                this.ProcessClass2 = "ok";
                                this.ProcessClass3 = "ok";
                                this.ProcessClass4 = "active";
                                break;

                            case OrderStatus.Finished:
                                this.ProcessClass2 = "ok";
                                this.ProcessClass3 = "ok";
                                this.ProcessClass4 = "ok";
                                break;
                        }
                        if (this.order.ManagerMark.HasValue)
                        {
                            this.OrderRemarkImageLink.ManagerMarkValue = (int)this.order.ManagerMark.Value;
                            this.litManagerRemark.Text = this.order.ManagerRemark;
                        }
                        else
                        {
                            this.divRemarkShow.Visible = false;
                        }
                        this.comCode = this.order.ExpressCompanyAbb;
                        this.litRemark.Text = this.order.Remark;
                        string orderId = this.order.OrderId;
                        string orderMarking = this.order.OrderMarking;
                        if (!string.IsNullOrEmpty(orderMarking))
                        {
                            orderId = orderId + " (" + this.order.PaymentType + "流水号：" + orderMarking + ")";
                        }
                        this.litOrderId.Text = orderId;
                        this.litUserName.Text = this.order.Username;
                        if (this.order.BalancePayMoneyTotal > 0M)
                        {
                            this.litPayType.Text = "余额支付 ￥" + this.order.BalancePayMoneyTotal.ToString("F2");
                        }
                        decimal num3 = this.order.GetTotal() - this.order.BalancePayMoneyTotal;
                        if (num3 > 0M)
                        {
                            if (!string.IsNullOrEmpty(this.litPayType.Text.Trim()))
                            {
                                this.litPayType.Text = this.litPayType.Text;
                            }
                            if ((num3 - this.order.CouponFreightMoneyTotal) > 0M)
                            {
                                if (!string.IsNullOrEmpty(this.litPayType.Text))
                                {
                                    this.litPayType.Text = this.litPayType.Text + "<br>";
                                }
                                this.litPayType.Text = this.litPayType.Text + (!this.order.PayDate.HasValue ? "待支付" : this.order.PaymentType) + " ￥" + num3.ToString("F2");
                            }
                        }
                        else if (this.order.PaymentTypeId == 0x4d)
                        {
                            this.litPayType.Text = this.order.PaymentType + " ￥" + this.order.PointToCash.ToString("F2");
                        }
                        else if (this.order.PaymentTypeId == 0x37)
                        {
                            this.litPayType.Text = this.order.PaymentType + " ￥" + this.order.RedPagerAmount.ToString("F2");
                        }
                        this.litShipToDate.Text = this.order.ShipToDate;
                        this.litRealName.Text = this.order.ShipTo;
                        this.litName.Text = this.order.ShipTo;
                        this.litExOrderId.Text = this.order.OrderId;
                        this.txtShipOrderNumber.Text = this.order.ShipOrderNumber;
                        this.OrderStatusLabelHtml.OrderStatusCode = this.order.OrderStatus;
                        this.litUserTel.Text = string.IsNullOrEmpty(this.order.CellPhone) ? this.order.TelPhone : this.order.CellPhone;
                        this.litShippingRegion.Text = this.order.ShippingRegion;
                        this.litFreight.Text = Globals.FormatMoney(this.order.AdjustedFreight);
                        this.BalancePayFreightMoneyTotal = this.order.BalancePayFreightMoneyTotal;
                        this.BalancePayMoneyTotal = this.order.BalancePayMoneyTotal;
                        if (this.order.ReferralUserId == 0)
                        {
                            this.litSiteName.Text = "主站";
                        }
                        else
                        {
                            DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(this.order.ReferralUserId);
                            if (distributorInfo != null)
                            {
                                this.litSiteName.Text = distributorInfo.StoreName;
                            }
                        }
                        StringBuilder builder = new StringBuilder();
                        if (!string.IsNullOrEmpty(this.order.ActivitiesName))
                        {
                            this.otherDiscountPrice += this.order.DiscountAmount;
                            builder.Append("<p>" + this.order.ActivitiesName + ":￥" + this.order.DiscountAmount.ToString("F2") + "</p>");
                        }
                        if (!string.IsNullOrEmpty(this.order.ReducedPromotionName))
                        {
                            this.otherDiscountPrice += this.order.ReducedPromotionAmount;
                            builder.Append("<p>" + this.order.ReducedPromotionName + ":￥" + this.order.ReducedPromotionAmount.ToString("F2") + "</p>");
                        }
                        if (!string.IsNullOrEmpty(this.order.CouponName))
                        {
                            this.otherDiscountPrice += this.order.CouponAmount;
                            builder.Append("<p>" + this.order.CouponName + ":￥" + this.order.CouponAmount.ToString("F2") + "</p>");
                        }
                        if (!string.IsNullOrEmpty(this.order.RedPagerActivityName))
                        {
                            this.otherDiscountPrice += this.order.RedPagerAmount;
                            builder.Append("<p>" + this.order.RedPagerActivityName + ":￥" + this.order.RedPagerAmount.ToString("F2") + "</p>");
                        }
                        if (this.order.PointToCash > 0M)
                        {
                            this.otherDiscountPrice += this.order.PointToCash;
                            builder.Append("<p>积分抵现:￥" + this.order.PointToCash.ToString("F2") + "</p>");
                        }
                        this.order.GetAdjustCommssion();
                        decimal num4 = 0M;
                        decimal num5 = 0M;
                        foreach (LineItemInfo info2 in this.order.LineItems.Values)
                        {
                            if (info2.IsAdminModify)
                            {
                                num4 += info2.ItemAdjustedCommssion;
                            }
                            else
                            {
                                num5 += info2.ItemAdjustedCommssion;
                            }
                        }
                        if (num4 != 0M)
                        {
                            if (num4 > 0M)
                            {
                                builder.Append("<p>管理员调价减:￥" + num4.ToString("F2") + "</p>");
                            }
                            else
                            {
                                builder.Append("<p>管理员调价加:￥" + ((num4 * -1M)).ToString("F2") + "</p>");
                            }
                        }
                        if (num5 != 0M)
                        {
                            if (num5 > 0M)
                            {
                                builder.Append("<p>分销商调价减:￥" + num5.ToString("F2") + "</p>");
                            }
                            else
                            {
                                builder.Append("<p>分销商调价加:￥" + ((num5 * -1M)).ToString("F2") + "</p>");
                            }
                        }
                        this.litActivityShow.Text = builder.ToString();
                        if (((int) this.lblOrderStatus.OrderStatusCode) != 4)
                        {
                            this.lbCloseReason.Visible = false;
                        }
                        else
                        {
                            this.divOrderProcess.Visible = false;
                            this.lbReason.Text = this.order.CloseReason;
                        }
                        if ((this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid) || ((this.order.OrderStatus == OrderStatus.WaitBuyerPay) && (this.order.Gateway == "hishop.plugins.payment.podrequest")))
                        {
                            this.btnSendGoods.Visible = true;
                        }
                        else
                        {
                            this.btnSendGoods.Visible = false;
                        }
                        if (((this.order.OrderStatus == OrderStatus.SellerAlreadySent) || (this.order.OrderStatus == OrderStatus.Finished)) && !string.IsNullOrEmpty(this.order.ExpressCompanyAbb))
                        {
                            this.pLoginsticInfo.Visible = true;
                            this.btnViewLogistic.Visible = true;
                            if ((Express.GetExpressType() == "kuaidi100") && (this.power != null))
                            {
                                this.power.Visible = true;
                            }
                        }
                        if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
                        {
                            this.btnClocsOrder.Visible = true;
                            this.btnModifyPrice.Visible = true;
                        }
                        else
                        {
                            this.btnClocsOrder.Visible = false;
                            this.btnModifyPrice.Visible = false;
                        }
                        this.btnModifyPrice.Attributes.Add("onclick", "DialogFrame('../trade/EditOrder.aspx?OrderId=" + this.orderId + "&reurl=" + base.Server.UrlEncode(this.reurl) + "','修改订单价格',900,450)");
                        this.BindRemark(this.order);
                        this.ddlpayment.DataBind();
                        this.ddlpayment.SelectedValue = new int?(this.order.PaymentTypeId);
                        this.rptItemList.DataSource = this.order.LineItems.Values;
                        this.rptItemList.DataBind();
                        string oldAddress = this.order.OldAddress;
                        string str7 = string.Empty;
                        if (!string.IsNullOrEmpty(this.order.ShippingRegion))
                        {
                            str7 = this.order.ShippingRegion.Replace(',', ' ');
                        }
                        if (!string.IsNullOrEmpty(this.order.Address))
                        {
                            str7 = str7 + this.order.Address;
                        }
                        if (!string.IsNullOrEmpty(this.order.ShipTo))
                        {
                            str7 = str7 + "，" + this.order.ShipTo;
                        }
                        if (!string.IsNullOrEmpty(this.order.TelPhone))
                        {
                            str7 = str7 + "，" + this.order.TelPhone;
                        }
                        if (!string.IsNullOrEmpty(this.order.CellPhone))
                        {
                            str7 = str7 + "，" + this.order.CellPhone;
                        }
                        if (string.IsNullOrEmpty(oldAddress))
                        {
                            this.lblOriAddress.Text = str7;
                            this.pNewAddress.Visible = false;
                        }
                        else
                        {
                            this.lblOriAddress.Text = oldAddress;
                            this.litAddress.Text = str7;
                        }
                        if ((this.order.OrderStatus == OrderStatus.Finished) || (this.order.OrderStatus == OrderStatus.SellerAlreadySent))
                        {
                            string realModeName = this.order.RealModeName;
                            if (string.IsNullOrEmpty(realModeName))
                            {
                                realModeName = this.order.ModeName;
                            }
                            this.litModeName.Text = realModeName;
                            this.litShipOrderNumber.Text = this.order.ShipOrderNumber;
                        }
                        else
                        {
                            this.litModeName.Text = this.order.ModeName;
                        }
                        if (!string.IsNullOrEmpty(this.order.ExpressCompanyName))
                        {
                            this.litCompanyName.Text = this.order.ExpressCompanyName;
                            this.hdCompanyCode.Value = this.order.ExpressCompanyAbb;
                        }
                        MemberInfo member = MemberProcessor.GetMember(this.order.UserId, true);
                        if (member != null)
                        {
                            if (!string.IsNullOrEmpty(member.OpenId))
                            {
                                this.litWeiXinNickName.Text = member.UserName;
                            }
                            if (!string.IsNullOrEmpty(member.UserBindName))
                            {
                                this.litUserName.Text = member.UserBindName;
                            }
                        }
                        if (this.order.ReferralUserId > 0)
                        {
                            builder = new StringBuilder();
                            builder.Append("<div class=\"commissionInfo mb20\"><h3>佣金信息</h3><div class=\"commissionInfoInner\">");
                            decimal num6 = 0M;
                            decimal totalCommssion = 0M;
                            decimal secondTotalCommssion = 0M;
                            decimal thirdTotalCommssion = 0M;
                            if (this.order.OrderStatus != OrderStatus.Closed)
                            {
                                totalCommssion = this.order.GetTotalCommssion();
                                secondTotalCommssion = this.order.GetSecondTotalCommssion();
                                thirdTotalCommssion = this.order.GetThirdTotalCommssion();
                            }
                            num6 += totalCommssion;
                            string storeName = string.Empty;
                            DistributorsInfo info4 = DistributorsBrower.GetDistributorInfo(this.order.ReferralUserId);
                            if (info4 != null)
                            {
                                storeName = info4.StoreName;
                                if ((this.order.ReferralPath != null) && (this.order.ReferralPath.Length > 0))
                                {
                                    string[] strArray = this.order.ReferralPath.Trim().Split(new char[] { '|' });
                                    int distributorid = 0;
                                    if (strArray.Length > 1)
                                    {
                                        distributorid = Globals.ToNum(strArray[0]);
                                        if (distributorid > 0)
                                        {
                                            info4 = DistributorsBrower.GetDistributorInfo(distributorid);
                                            if (info4 != null)
                                            {
                                                num6 += thirdTotalCommssion;
                                                builder.Append("<p class=\"mb5\"><span>上二级分销商：</span> " + info4.StoreName + "<i> ￥" + thirdTotalCommssion.ToString("F2") + "</i></p>");
                                            }
                                        }
                                        distributorid = Globals.ToNum(strArray[1]);
                                        if (distributorid > 0)
                                        {
                                            info4 = DistributorsBrower.GetDistributorInfo(distributorid);
                                            if (info4 != null)
                                            {
                                                num6 += secondTotalCommssion;
                                                builder.Append("<p class=\"mb5\"><span>上一级分销商：</span> " + info4.StoreName + "<i> ￥" + secondTotalCommssion.ToString("F2") + "</i></p>");
                                            }
                                        }
                                    }
                                    else if (strArray.Length == 1)
                                    {
                                        distributorid = Globals.ToNum(strArray[0]);
                                        if (distributorid > 0)
                                        {
                                            info4 = DistributorsBrower.GetDistributorInfo(distributorid);
                                            if (info4 != null)
                                            {
                                                builder.Append("<p class=\"mb5\"><span>上二级分销商：</span>-</p>");
                                                num6 += secondTotalCommssion;
                                                builder.Append("<p class=\"mb5\"><span>上一级分销商：</span>" + info4.StoreName + " <i> ￥" + secondTotalCommssion.ToString("F2") + "</i></p>");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    builder.Append("<p class=\"mb5\"><span>上二级分销商：</span>-</p>");
                                    builder.Append("<p class=\"mb5\"><span>上一级分销商：</span>-</p>");
                                }
                            }
                            builder.Append("<div class=\"clearfix\">");
                            if (num5 > 0M)
                            {
                                string str10 = " (改价让利￥" + num5.ToString("F2") + ")";
                                string[] strArray2 = new string[7];
                                strArray2[0] = "<p><span>成交店铺：</span> ";
                                strArray2[1] = storeName;
                                strArray2[2] = " <i>￥";
                                decimal num11 = totalCommssion - num5;
                                strArray2[3] = num11.ToString("F2");
                                strArray2[4] = "</i>";
                                strArray2[5] = str10;
                                strArray2[6] = "</p>";
                                builder.Append(string.Concat(strArray2));
                                builder.Append("<p><span>佣金总额：</span><i>￥" + ((num6 - num5)).ToString("F2") + "</i></p>");
                            }
                            else
                            {
                                builder.Append("<p><span>成交店铺：</span> " + storeName + " <i>￥" + totalCommssion.ToString("F2") + "</i></p>");
                                builder.Append("<p><span>佣金总额：</span><i>￥" + num6.ToString("F2") + "</i></p>");
                            }
                            builder.Append("</div></div></div>");
                            this.litCommissionInfo.Text = builder.ToString();
                        }
                        DataTable orderItemsReFundByOrderID = RefundHelper.GetOrderItemsReFundByOrderID(this.orderId);
                        if (orderItemsReFundByOrderID.Rows.Count > 0)
                        {
                            this.rptRefundList.DataSource = orderItemsReFundByOrderID;
                            this.rptRefundList.DataBind();
                        }
                    }
                }
            }
        }

        protected void rptRefundList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HtmlInputButton button = (HtmlInputButton) e.Item.FindControl("btnAgree");
                HtmlInputButton button2 = (HtmlInputButton) e.Item.FindControl("btnRefuce");
                Label label = (Label) e.Item.FindControl("lblIsAgree");
                RefundInfo.Handlestatus handlestatus = (RefundInfo.Handlestatus) DataBinder.Eval(e.Item.DataItem, "HandleStatus");
                HtmlAnchor anchor = (HtmlAnchor) e.Item.FindControl("linkModify");
                switch (handlestatus)
                {
                    case RefundInfo.Handlestatus.Applied:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "已申请";
                        anchor.Visible = false;
                        return;

                    case RefundInfo.Handlestatus.Refunded:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "已退款";
                        anchor.Visible = false;
                        return;

                    case RefundInfo.Handlestatus.Refused:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "拒绝申请";
                        anchor.Visible = false;
                        return;

                    case RefundInfo.Handlestatus.NoneAudit:
                    case RefundInfo.Handlestatus.HasTheAudit:
                    case RefundInfo.Handlestatus.NoRefund:
                        return;

                    case RefundInfo.Handlestatus.AuditNotThrough:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "审核不通过";
                        anchor.Visible = false;
                        return;

                    case RefundInfo.Handlestatus.RefuseRefunded:
                        button.Visible = false;
                        button2.Visible = false;
                        label.Visible = true;
                        label.Text = "拒绝退款";
                        anchor.Visible = false;
                        return;
                }
            }
        }
    }
}

