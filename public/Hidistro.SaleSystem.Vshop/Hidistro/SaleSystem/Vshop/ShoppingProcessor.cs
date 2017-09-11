namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.Promotions;
    using Hidistro.SqlDal.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Runtime.InteropServices;

    public static class ShoppingProcessor
    {
        private static object BalanceUpdateLock = new object();
        private static object createOrderLocker = new object();

        public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal price = 0M;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            decimal num3 = totalWeight;
            int num4 = 1;
            if (((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddWeight.HasValue) && (shippingModeInfo.AddWeight.Value > 0M))
            {
                decimal num5 = num3 - shippingModeInfo.Weight;
                if ((num5 % shippingModeInfo.AddWeight) == 0M)
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal) ((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)));
                }
                else
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal) ((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value))) + 1;
                }
            }
            if ((shippingModeInfo.ModeGroup == null) || (shippingModeInfo.ModeGroup.Count == 0))
            {
                if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
                {
                    return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
                }
                return shippingModeInfo.Price;
            }
            int? nullable = null;
            foreach (ShippingModeGroupInfo info in shippingModeInfo.ModeGroup)
            {
                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                {
                    if (topRegionId == info2.RegionId)
                    {
                        nullable = new int?(info2.GroupId);
                        break;
                    }
                }
                if (nullable.HasValue)
                {
                    if (num3 > shippingModeInfo.Weight)
                    {
                        price = (num4 * info.AddPrice) + info.Price;
                    }
                    else
                    {
                        price = info.Price;
                    }
                    break;
                }
            }
            if (nullable.HasValue)
            {
                return price;
            }
            if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
            {
                return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
            }
            return shippingModeInfo.Price;
        }

        public static decimal CalcPayCharge(decimal cartMoney, PaymentModeInfo paymentModeInfo)
        {
            if (!paymentModeInfo.IsPercent)
            {
                return paymentModeInfo.Charge;
            }
            return (cartMoney * (paymentModeInfo.Charge / 100M));
        }

        private static void checkCanGroupBuy(int quantity, int groupBuyId)
        {
            GroupBuyInfo info = null;
            if (info.Status != GroupBuyStatus.UnderWay)
            {
                throw new OrderException("当前团购状态不允许购买");
            }
            if ((info.StartDate > DateTime.Now) || (info.EndDate < DateTime.Now))
            {
                throw new OrderException("当前不在团购时间范围内");
            }
            int num = info.MaxCount - info.SoldCount;
            if (quantity > num)
            {
                throw new OrderException("剩余可购买团购数量不够");
            }
        }

        public static bool CombineOrderToPay(string orderIds, string orderMarking)
        {
            return new OrderDao().CombineOrderToPay(orderIds, orderMarking);
        }

        public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isCountDown, bool isSignBuy)
        {
            if (shoppingCart.LineItems.Count == 0)
            {
                return null;
            }
            OrderInfo info = new OrderInfo {
                Points = shoppingCart.GetPoint(),
                ReducedPromotionId = shoppingCart.ReducedPromotionId,
                ReducedPromotionName = shoppingCart.ReducedPromotionName,
                ReducedPromotionAmount = shoppingCart.ReducedPromotionAmount,
                IsReduced = shoppingCart.IsReduced,
                SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId,
                SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName,
                IsSendTimesPoint = shoppingCart.IsSendTimesPoint,
                TimesPoint = shoppingCart.TimesPoint,
                FreightFreePromotionId = shoppingCart.FreightFreePromotionId,
                FreightFreePromotionName = shoppingCart.FreightFreePromotionName,
                IsFreightFree = shoppingCart.IsFreightFree
            };
            string str = string.Empty;
            if (shoppingCart.LineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems)
                {
                    str = str + string.Format("'{0}',", info2.SkuId);
                }
            }
            if (shoppingCart.LineItems.Count > 0)
            {
                foreach (ShoppingCartItemInfo info3 in shoppingCart.LineItems)
                {
                    LineItemInfo info4 = new LineItemInfo {
                        SkuId = info3.SkuId,
                        ProductId = info3.ProductId,
                        SKU = info3.SKU,
                        Quantity = info3.Quantity,
                        ShipmentQuantity = info3.ShippQuantity,
                        CommissionDiscount = 100
                    };
                    if (info3.LimitedTimeDiscountId > 0)
                    {
                        bool flag = true;
                        LimitedTimeDiscountInfo discountInfo = new LimitedTimeDiscountDao().GetDiscountInfo(info3.LimitedTimeDiscountId);
                        if (discountInfo == null)
                        {
                            flag = false;
                        }
                        else
                        {
                            info4.CommissionDiscount = discountInfo.CommissionDiscount;
                        }
                        if (!flag)
                        {
                            info3.LimitedTimeDiscountId = 0;
                        }
                    }
                    info4.ItemCostPrice = new SkuDao().GetSkuItem(info3.SkuId).CostPrice;
                    info4.ItemListPrice = info3.MemberPrice;
                    info4.ItemAdjustedPrice = info3.AdjustedPrice;
                    info4.ItemDescription = info3.Name;
                    info4.ThumbnailsUrl = info3.ThumbnailUrl60;
                    info4.ItemWeight = info3.Weight;
                    info4.SKUContent = info3.SkuContent;
                    info4.PromotionId = info3.PromotionId;
                    info4.PromotionName = info3.PromotionName;
                    info4.MainCategoryPath = info3.MainCategoryPath;
                    info4.Type = info3.Type;
                    info4.ExchangeId = info3.ExchangeId;
                    info4.PointNumber = info3.PointNumber * info4.Quantity;
                    info4.ThirdCommission = info3.ThirdCommission;
                    info4.SecondCommission = info3.SecondCommission;
                    info4.FirstCommission = info3.FirstCommission;
                    info4.IsSetCommission = info3.IsSetCommission;
                    info4.LimitedTimeDiscountId = info3.LimitedTimeDiscountId;
                    info.LineItems.Add(info4.SkuId + info4.Type + info4.LimitedTimeDiscountId, info4);
                }
            }
            info.Tax = 0.00M;
            info.InvoiceTitle = "";
            return info;
        }

        public static int CreatOrder(OrderInfo orderInfo, bool isUseBalance, decimal remainingMondy)
        {
            int num = 0;
            if (orderInfo.GetTotal() <= 0M)
            {
                orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
                orderInfo.PayDate = new DateTime?(DateTime.Now);
            }
            if ((orderInfo.PaymentType == null) && (orderInfo.PointExchange > 0))
            {
                orderInfo.PaymentType = "积分抵现";
                orderInfo.Gateway = "hishop.plugins.payment.pointtocach";
                orderInfo.PaymentTypeId = 0x4d;
            }
            else if (((orderInfo.PaymentType == null) && orderInfo.RedPagerID.HasValue) && (orderInfo.RedPagerID.Value > 0))
            {
                orderInfo.PaymentType = "优惠券抵扣";
                orderInfo.Gateway = "hishop.plugins.payment.coupontocach";
                orderInfo.PaymentTypeId = 0x37;
            }
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            Database database = DatabaseFactory.CreateDatabase();
            int quantity = orderInfo.LineItems.Sum<KeyValuePair<string, LineItemInfo>>((Func<KeyValuePair<string, LineItemInfo>, int>) (item => item.Value.Quantity));
            lock (createOrderLocker)
            {
                if (orderInfo.GroupBuyId > 0)
                {
                    checkCanGroupBuy(quantity, orderInfo.GroupBuyId);
                }
                using (DbConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();
                    try
                    {
                        orderInfo.ClientShortType = (ClientShortType) Globals.GetClientShortType();
                        if (!new OrderDao().CreatOrder(orderInfo, dbTran))
                        {
                            dbTran.Rollback();
                            return 0;
                        }
                        if (orderInfo.LineItems.Count > 0)
                        {
                            if (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                            {
                                foreach (LineItemInfo info2 in orderInfo.LineItems.Values)
                                {
                                    info2.OrderItemsStatus = OrderStatus.BuyerAlreadyPaid;
                                }
                            }
                            if (!new LineItemDao().AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTran))
                            {
                                dbTran.Rollback();
                                return 0;
                            }
                        }
                        if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !new CouponDao().AddCouponUseRecord(orderInfo, dbTran))
                        {
                            dbTran.Rollback();
                            return 0;
                        }
                        foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
                        {
                            if ((info3.Type == 1) && (info3.ExchangeId > 0))
                            {
                                PointExchangeChangedInfo info4=new PointExchangeChangedInfo ();
                                info4 = new PointExchangeChangedInfo {
                                    exChangeId = info3.ExchangeId,
                                    exChangeName = new OrderDao().GetexChangeName(info4.exChangeId),
                                    ProductId = info3.ProductId,
                                    PointNumber = info3.PointNumber,
                                    MemberID = orderInfo.UserId,
                                    Date = DateTime.Now,
                                    MemberGrades = currentMember.GradeId
                                };
                                if (!new OrderDao().InsertPointExchange_Changed(info4, dbTran, info3.Quantity))
                                {
                                    dbTran.Rollback();
                                    return 0;
                                }
                                IntegralDetailInfo point = new IntegralDetailInfo {
                                    IntegralChange = -info3.PointNumber,
                                    IntegralSource = "积分兑换商品-订单号：" + orderInfo.OrderMarking,
                                    IntegralSourceType = 2,
                                    Remark = "积分兑换商品",
                                    Userid = orderInfo.UserId,
                                    GoToUrl = Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + orderInfo.OrderId,
                                    IntegralStatus = Convert.ToInt32(IntegralDetailStatus.IntegralExchange)
                                };
                                if (!new IntegralDetailDao().AddIntegralDetail(point, dbTran))
                                {
                                    dbTran.Rollback();
                                    return 0;
                                }
                            }
                        }
                        if (orderInfo.PointExchange > 0)
                        {
                            IntegralDetailInfo info6 = new IntegralDetailInfo {
                                IntegralChange = -orderInfo.PointExchange,
                                IntegralSource = "积分抵现，订单号：" + orderInfo.OrderId,
                                IntegralSourceType = 2,
                                Remark = "",
                                Userid = orderInfo.UserId,
                                GoToUrl = Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + orderInfo.OrderId,
                                IntegralStatus = Convert.ToInt32(IntegralDetailStatus.NowArrived)
                            };
                            if (!new IntegralDetailDao().AddIntegralDetail(info6, dbTran))
                            {
                                dbTran.Rollback();
                                return 0;
                            }
                        }
                        if ((orderInfo.RedPagerID > 0) && !new OrderDao().UpdateCoupon_MemberCoupons(orderInfo, dbTran))
                        {
                            dbTran.Rollback();
                            return 0;
                        }
                        dbTran.Commit();
                        num = 1;
                        if (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                        {
                            num = 2;
                        }
                    }
                    catch
                    {
                        dbTran.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            if (isUseBalance && (num == 1))
            {
                OrderDao dao = new OrderDao();
                orderInfo = dao.GetOrderInfo(orderInfo.OrderId);
                lock (BalanceUpdateLock)
                {
                    num = OrderBalanceUpdate(orderInfo, currentMember.UserId, remainingMondy);
                    if (num != 2)
                    {
                        return num;
                    }
                    dao.UpdatePayOrderStock(orderInfo);
                    string str = "";
                    foreach (LineItemInfo info7 in orderInfo.LineItems.Values)
                    {
                        ProductDao dao2 = new ProductDao();
                        str = str + "'" + info7.SkuId + "',";
                        ProductInfo productDetails = dao2.GetProductDetails(info7.ProductId);
                        productDetails.SaleCounts += info7.Quantity;
                        productDetails.ShowSaleCounts += info7.Quantity;
                        dao2.UpdateProduct(productDetails, null);
                    }
                }
            }
            return num;
        }

        public static OrderInfo GetCalculadtionCommission(OrderInfo order)
        {
            return new OrderDao().GetCalculadtionCommission(order, 0);
        }

        public static DataTable GetCoupon(decimal orderAmount)
        {
            return null;
        }

        public static CouponInfo GetCoupon(string couponCode)
        {
            return new CouponDao().GetCouponDetails(int.Parse(couponCode));
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfoForLineItems(orderId);
        }

        public static List<OrderInfo> GetOrderMarkingOrderInfo(string OrderMarking, bool IsWaitPay = true)
        {
            List<OrderInfo> list = new List<OrderInfo>();
            DataTable orderMarkingAllOrderID = new OrderDao().GetOrderMarkingAllOrderID(OrderMarking, IsWaitPay);
            for (int i = 0; i < orderMarkingAllOrderID.Rows.Count; i++)
            {
                list.Add(new OrderDao().GetOrderInfo(orderMarkingAllOrderID.Rows[i]["OrderId"].ToString()));
            }
            return list;
        }

        public static DataTable GetOrderReturnTable(int userid, string ReturnsId, int type)
        {
            return new RefundDao().GetOrderReturnTable(userid, ReturnsId, type);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return new PaymentModeDao().GetPaymentMode(modeId);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return new PaymentModeDao().GetPaymentModes();
        }

        public static SKUItem GetProductAndSku(MemberInfo member, int productId, string options)
        {
            return new SkuDao().GetProductAndSku(member, productId, options);
        }

        public static bool GetReturnInfo(int userid, string OrderId, int ProductId, string SkuID)
        {
            return new RefundDao().GetReturnInfo(userid, OrderId, ProductId, SkuID);
        }

        public static bool GetReturnMes(int userid, string OrderId, int ProductId, int HandleStatus)
        {
            return new RefundDao().GetReturnMes(userid, OrderId, ProductId, HandleStatus);
        }

        public static int GetUserOrders(int userId)
        {
            return new OrderDao().GetUserOrders(userId);
        }

        public static bool InsertCalculationCommission(ArrayList UserIdList, ArrayList ReferralBlanceList, string orderid, ArrayList OrdersTotalList, string userid)
        {
            return new OrderDao().InsertCalculationCommission(UserIdList, ReferralBlanceList, orderid, OrdersTotalList, userid);
        }

        public static bool InsertOrderRefund(RefundInfo refundInfo)
        {
            return new RefundDao().InsertOrderRefund(refundInfo);
        }

        public static int OrderBalanceUpdate(OrderInfo orderInfo, int userid, decimal remainingMondy)
        {
            MemberInfo member = new MemberDao().GetMember(userid);
            if (member == null)
            {
                return 0;
            }
            decimal availableAmount = member.AvailableAmount;
            decimal num2 = 0M;
            ICollection values = orderInfo.LineItems.Values;
            decimal num3 = 0M;
            OrderStatus orderStatus = orderInfo.OrderStatus;
            foreach (LineItemInfo info2 in values)
            {
                if (info2.Type == 0)
                {
                    num3 = (info2.ItemAdjustedPrice * info2.Quantity) - info2.DiscountAverage;
                    decimal num4 = 0M;
                    if (availableAmount >= num3)
                    {
                        availableAmount -= num3;
                        num4 = num3;
                    }
                    else
                    {
                        num4 = availableAmount;
                        availableAmount = 0M;
                    }
                    if (num4 <= 0M)
                    {
                        break;
                    }
                    num2 += num4;
                    info2.BalancePayMoney = num4;
                }
            }
            if (availableAmount > 0M)
            {
                decimal num5 = 0M;
                decimal num6 = orderInfo.AdjustedFreight - orderInfo.CouponFreightMoneyTotal;
                if (remainingMondy > 0M)
                {
                    num6 -= remainingMondy;
                    if (num6 < 0M)
                    {
                        num6 = 0M;
                    }
                }
                if (availableAmount > num6)
                {
                    num5 = num6;
                }
                else
                {
                    num5 = availableAmount;
                }
                orderInfo.BalancePayFreightMoneyTotal = num5;
                num2 += num5;
            }
            if (orderInfo.GetCashPayMoney() <= 0M)
            {
                orderStatus = OrderStatus.BuyerAlreadyPaid;
            }
            foreach (LineItemInfo info3 in values)
            {
                new LineItemDao().UpdateBalancePayMoney(info3.ID, info3.BalancePayMoney, orderStatus, null);
            }
            int num7 = 1;
            if (orderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
                orderInfo.PayDate = new DateTime?(DateTime.Now);
                num7 = 2;
                if (orderInfo.GetBalancePayMoneyTotal() > 0M)
                {
                    orderInfo.PaymentTypeId = 0x42;
                    orderInfo.PaymentType = "余额支付";
                    orderInfo.Gateway = "hishop.plugins.payment.balancepayrequest";
                }
            }
            new OrderDao().UpdateOrder(orderInfo, null);
            if (num2 > 0M)
            {
                MemberAmountDetailedInfo info4 = new MemberAmountDetailedInfo();
                info4 = new MemberAmountDetailedInfo {
                    UserId = member.UserId,
                    UserName = member.UserName,
                    PayId = Globals.GetGenerateId(),
                    TradeAmount = -num2,
                    TradeType = TradeType.Payment,
                    TradeTime = DateTime.Now,
                    TradeWays = TradeWays.Balance,
                    State = 1,
                    AvailableAmount = member.AvailableAmount + info4.TradeAmount,
                    Remark = "订单号：" + orderInfo.OrderId
                };
                new AmountDao().UseBalance(info4);
            }
            return num7;
        }

        public static string UpdateAdjustCommssions(string orderId, string itemid, decimal commssionmoney, decimal adjustcommssion)
        {
            string str = string.Empty;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    OrderInfo orderInfo = GetOrderInfo(orderId);
                    if (orderId == null)
                    {
                        return "订单编号不合法";
                    }
                    int userId = DistributorsBrower.GetCurrentDistributors(true).UserId;
                    if ((orderInfo.ReferralUserId != userId) || (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay))
                    {
                        return "不是您的订单";
                    }
                    LineItemInfo lineItem = orderInfo.LineItems[itemid];
                    if ((lineItem == null) || (lineItem.ItemsCommission < adjustcommssion))
                    {
                        if (lineItem.ItemsCommission.ToString("F2") != adjustcommssion.ToString("F2"))
                        {
                            return "修改金额过大";
                        }
                        adjustcommssion = lineItem.ItemsCommission;
                    }
                    lineItem.ItemAdjustedCommssion = adjustcommssion;
                    lineItem.IsAdminModify = false;
                    if (!new LineItemDao().UpdateLineItem(orderId, lineItem, dbTran))
                    {
                        dbTran.Rollback();
                    }
                    if (!new OrderDao().UpdateOrder(orderInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return "更新订单信息失败";
                    }
                    dbTran.Commit();
                    str = "1";
                }
                catch (Exception exception)
                {
                    str = exception.ToString();
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
                return str;
            }
        }

        public static bool UpdateCalculadtionCommission(OrderInfo order)
        {
            return new OrderDao().UpdateCalculadtionCommission(order, null);
        }

        public static bool UpdateOrder(OrderInfo order, DbTransaction dbTran = null)
        {
            return new OrderDao().UpdateOrder(order, dbTran);
        }

        public static bool UpdateOrderGoodStatu(string orderid, string skuid, int OrderItemsStatus, int itemid)
        {
            return new RefundDao().UpdateOrderGoodStatu(orderid, skuid, OrderItemsStatus, itemid);
        }

        public static CouponInfo UseCoupon(decimal orderAmount, string claimCode)
        {
            if (!string.IsNullOrEmpty(claimCode))
            {
                CouponInfo coupon = GetCoupon(claimCode);
                if (coupon.ConditionValue <= orderAmount)
                {
                    return coupon;
                }
            }
            return null;
        }
    }
}

