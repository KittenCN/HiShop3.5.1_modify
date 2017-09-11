namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SqlDal.Members;
    using System;

    public static class Point
    {
        public static bool MemberAmountAddByRefund(MemberInfo memberInfo, decimal amount, string orderid)
        {
            return new AmountDao().MemberAmountAddByRefund(memberInfo, amount, orderid);
        }

        public static void SetPointAndBalanceByOrderId(OrderInfo orderInfo)
        {
            int pointExchange = 0;
            decimal balancePayMoneyTotal = orderInfo.GetBalancePayMoneyTotal();
            if (orderInfo.PointExchange > 0)
            {
                pointExchange = orderInfo.PointExchange;
            }
            else if (orderInfo.LineItems.Count > 0)
            {
                foreach (LineItemInfo info in orderInfo.LineItems.Values)
                {
                    if (info.PointNumber > 0)
                    {
                        pointExchange += info.PointNumber;
                    }
                }
            }
            if (pointExchange > 0)
            {
                IntegralDetailInfo point = new IntegralDetailInfo {
                    IntegralChange = pointExchange,
                    IntegralSource = "订单取消，积分返还-订单号：" + orderInfo.OrderId,
                    IntegralSourceType = 1,
                    IntegralStatus = 4,
                    Userid = orderInfo.UserId,
                    Remark = "订单取消，积分返还"
                };
                new IntegralDetailDao().AddIntegralDetail(point, null);
            }
            if (balancePayMoneyTotal > 0M)
            {
                MemberAmountAddByRefund(new MemberDao().GetMember(orderInfo.UserId), balancePayMoneyTotal, orderInfo.OrderId);
            }
        }
    }
}

