namespace Hidistro.Entities.Members
{
    using System;
    using System.ComponentModel;

    public enum TradeType
    {
        [Description("充值返现")]
        CashBack = 8,
        [Description("佣金转入")]
        CommissionTransfer = 2,
        [Description("订单关闭")]
        OrderClose = 5,
        [Description("在线支付")]
        Payment = 3,
        [Description("充值")]
        Recharge = 1,
        [Description("售后退款")]
        Refund = 4,
        [Description("商铺调整")]
        ShopAdjustment = 7,
        [Description("提现")]
        Withdrawals = 0,
        [Description("提现驳回")]
        WithdrawalsRefuse = 6
    }
}

