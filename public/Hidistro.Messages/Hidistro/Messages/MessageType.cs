namespace Hidistro.Messages
{
    using System;

    internal static class MessageType
    {
        internal const string AccountLock = "AccountLock";
        internal const string AccountUnLock = "AccountUnLock";
        internal const string CouponWillExpired = "CouponWillExpired";
        internal const string DistributorCancel = "DistributorCancel";
        internal const string DistributorCreate = "DistributorCreate";
        internal const string DistributorGradeChange = "DistributorGradeChange";
        internal const string DrawCashReject = "DrawCashReject";
        internal const string DrawCashRelease = "DrawCashRelease";
        internal const string DrawCashRequest = "DrawCashRequest";
        internal const string MemberAmountDrawCashRefuse = "MemberAmountDrawCashRefuse";
        internal const string MemberAmountDrawCashRelease = "MemberAmountDrawCashRelease";
        internal const string MemberAmountDrawCashRequest = "MemberAmountDrawCashRequest";
        internal const string MemberGradeChange = "MemberGradeChange";
        internal const string MemberRegister = "MemberRegister";
        internal const string OrderCreate = "OrderCreate";
        internal const string OrderDeliver = "OrderDeliver";
        internal const string OrderGetCommission = "OrderGetCommission";
        internal const string OrderGetCoupon = "OrderGetCoupon";
        internal const string OrderGetPoint = "OrderGetPoint";
        internal const string OrderPay = "OrderPay";
        internal const string PasswordReset = "PasswordReset";
        internal const string PrizeRelease = "PrizeRelease";
        internal const string ProductAsk = "ProductAsk";
        internal const string ProductCreate = "ProductCreate";
        internal const string RefundSuccess = "RefundSuccess";
        internal const string ServiceRequest = "ServiceRequest";
        internal const string SubMemberRegister = "SubMemberRegister";
    }
}

