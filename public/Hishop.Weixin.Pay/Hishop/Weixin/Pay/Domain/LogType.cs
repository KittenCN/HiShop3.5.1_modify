namespace Hishop.Weixin.Pay.Domain
{
    using System;

    public enum LogType
    {
        Pay,
        PayNotify,
        NativePay,
        NativePayNotify,
        MicroPay,
        MicroPayNotify,
        Refund,
        RefundNotify,
        RefundQuery,
        OrderQuery,
        DownLoadBill,
        CloseOrder,
        GetTokenOrOpenID,
        GetOrEditAddress,
        ShortUrl,
        UnifiedOrder,
        Report,
        Error,
        GetPrepayID
    }
}

