namespace Hidistro.Entities.Members
{
    using System;
    using System.ComponentModel;

    public enum TradeWays
    {
        [Description("支付宝")]
        Alipay = 1,
        [Description("余额")]
        Balance = 3,
        [Description("线下转账")]
        LineTransfer = 5,
        [Description("盛付通")]
        ShengFutong = 4,
        [Description("店铺佣金")]
        ShopCommission = 2,
        [Description("微信钱包")]
        WeChatWallet = 0
    }
}

