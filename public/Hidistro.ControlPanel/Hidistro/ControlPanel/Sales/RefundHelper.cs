namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SqlDal;
    using Hidistro.SqlDal.Orders;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public static class RefundHelper
    {
        public static bool CloseTransaction(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            order.OrderStatus = OrderStatus.Closed;
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                new OrderDao().UpdateItemsStatus(order.OrderId, 4, "all");
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "关闭了订单“{0}”", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool DelRefundApply(string[] ReturnsIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
            bool flag = true;
            count = 0;
            foreach (string str in ReturnsIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (new RefundDao().DelRefundApply(int.Parse(str)))
                    {
                        count++;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        public static RefundInfo GetByOrderIdAndProductID(string orderId, int productid, string skuid, int orderitemid)
        {
            return new RefundDao().GetByOrderIdAndProductID(orderId, productid, skuid, orderitemid);
        }

        public static DataTable GetOrderItemsReFundByOrderID(string orderid)
        {
            return new RefundDao().GetOrderItemsReFundByOrderID(orderid);
        }

        public static RefundInfo GetOrderReturnsByReturnsID(int returnsid)
        {
            return new RefundDao().GetOrderReturnsByReturnsID(returnsid);
        }

        public static void GetRefundType(string orderId, out int refundType, out string remark)
        {
            new RefundDao().GetRefundType(orderId, out refundType, out remark);
        }

        public static DbQueryResult GetReturnOrderAll(ReturnsApplyQuery returnsapplyquery)
        {
            return new RefundDao().GetReturnOrderAll(returnsapplyquery);
        }

        public static bool UpdateByAuditReturnsId(RefundInfo refundInfo)
        {
            return new RefundDao().UpdateByAuditReturnsId(refundInfo);
        }

        public static bool UpdateByReturnsId(RefundInfo refundInfo)
        {
            bool flag = new RefundDao().UpdateByReturnsId(refundInfo);
            if (refundInfo.HandleStatus == RefundInfo.Handlestatus.Refunded)
            {
                try
                {
                    if (OrderHelper.GetOrderInfo(refundInfo.OrderId) != null)
                    {
                        Messenger.SendWeiXinMsg_RefundSuccess(refundInfo);
                    }
                }
                catch (Exception)
                {
                }
            }
            return flag;
        }

        public static bool UpdateOrderGoodStatu(string orderid, string skuid, int OrderItemsStatus, int itemid)
        {
            return new RefundDao().UpdateOrderGoodStatu(orderid, skuid, OrderItemsStatus, itemid);
        }

        public static bool UpdateRefundMoney(string orderid, int productid, decimal refundMoney)
        {
            return new RefundDao().UpdateRefundMoney(orderid, productid, refundMoney);
        }

        public static bool UpdateRefundOrderStock(string Stock, string SkuId)
        {
            return new RefundDao().UpdateRefundOrderStock(Stock, SkuId);
        }
    }
}

