namespace Hidistro.Entities.Orders
{
    using System;
    using System.ComponentModel;

    public enum OrderStatus
    {
        [Description("所有订单|Trade_ALL")]
        All = 0,
        [Description("申请退款|TRADE_APPLY_FOR_REFUND")]
        ApplyForRefund = 6,
        [Description("申请换货|TRADE_APPLY_FOR_REPLACE")]
        ApplyForReplacement = 8,
        [Description("申请退货|TRADE_APPLY_FOR_RETURN")]
        ApplyForReturns = 7,
        [Description("等待发货|WAIT_SELLER_SEND_GOODS")]
        BuyerAlreadyPaid = 2,
        [Description("交易关闭|TRADE_CLOSED")]
        Closed = 4,
        [Description("已删除的订单|TRADE_HISTORY")]
        Deleted = 12,
        [Description("交易完成|TRADE_FINISHED")]
        Finished = 5,
        [Description("历史订单|TRADE_HISTORY")]
        History = 0x63,
        [Description("已退款|TRADE_REFUND_FINISHED")]
        Refunded = 9,
        [Description("已退货|TRADE_RETURNED_FINISHED")]
        Returned = 10,
        [Description("已发货|WAIT_BUYER_CONFIRM_GOODS")]
        SellerAlreadySent = 3,
        [Description("今日订单|TRADE_HISTORY")]
        Today = 11,
        [Description("等待买家付款|WAIT_BUYER_PAY")]
        WaitBuyerPay = 1
    }
}

