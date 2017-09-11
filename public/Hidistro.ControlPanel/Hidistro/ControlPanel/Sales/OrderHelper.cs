namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Hidistro.SqlDal.Orders;
    using Hidistro.SqlDal.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class OrderHelper
    {
        public static bool CloseTransaction(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (!order.CheckAction(OrderActions.SELLER_CLOSE))
            {
                return false;
            }
            order.OrderStatus = OrderStatus.Closed;
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                new OrderDao().UpdateItemsStatus(order.OrderId, 4, "all");
                Point.SetPointAndBalanceByOrderId(order);
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "关闭了订单“{0}”", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_FINISH_TRADE))
            {
                DateTime now = DateTime.Now;
                order.OrderStatus = OrderStatus.Finished;
                order.FinishDate = new DateTime?(now);
                if (!order.PayDate.HasValue)
                {
                    order.PayDate = new DateTime?(now);
                }
                flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的订单", new object[] { order.OrderId }));
                }
            }
            if (flag)
            {
                DistributorsBrower.UpdateCalculationCommission(order);
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    if (info.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
                    {
                        RefundHelper.UpdateOrderGoodStatu(order.OrderId, info.SkuId, 5, info.ID);
                    }
                }
            }
            return flag;
        }

        public static bool ConfirmPay(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.CofimOrderPay);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
            {
                OrderDao dao = new OrderDao();
                order.OrderStatus = OrderStatus.BuyerAlreadyPaid;
                order.PayDate = new DateTime?(DateTime.Now);
                order.PaymentTypeId = 0x63;
                order.PaymentType = "线下支付";
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                string str = string.Empty;
                if (currentManager != null)
                {
                    str = string.Concat(new object[] { currentManager.UserName, "(", currentManager.UserId, ")" });
                }
                order.ManagerRemark = "管理员" + str + "后台收款";
                order.Gateway = "hishop.plugins.payment.offlinerequest";
                flag = dao.UpdateOrder(order, null);
                string str2 = "";
                if (!flag)
                {
                    return flag;
                }
                dao.UpdatePayOrderStock(order);
                foreach (LineItemInfo info2 in order.LineItems.Values)
                {
                    ProductDao dao2 = new ProductDao();
                    str2 = str2 + "'" + info2.SkuId + "',";
                    ProductInfo productDetails = dao2.GetProductDetails(info2.ProductId);
                    productDetails.SaleCounts += info2.Quantity;
                    productDetails.ShowSaleCounts += info2.Quantity;
                    dao2.UpdateProduct(productDetails, null);
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    dao.UpdateItemsStatus(order.OrderId, 2, str2.Substring(0, str2.Length - 1));
                }
                if (!string.IsNullOrEmpty(order.ActivitiesId))
                {
                    new ActivitiesDao().UpdateActivitiesTakeEffect(order.ActivitiesId);
                }
                MemberHelper.SetOrderDate(order.UserId, 1);
                SettingsManager.GetMasterSettings(true);
                MemberInfo member = new MemberDao().GetMember(order.UserId);
                if (VshopBrowser.IsPassAutoToDistributor(member, true))
                {
                    DistributorsBrower.MemberAutoToDistributor(member);
                }
                try
                {
                    OrderInfo info5 = order;
                    if (info5 != null)
                    {
                        Messenger.SendWeiXinMsg_OrderPay(info5);
                    }
                }
                catch (Exception)
                {
                }
                EventLogs.WriteOperationLog(Privilege.CofimOrderPay, string.Format(CultureInfo.InvariantCulture, "确认收款编号为\"{0}\"的订单", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool DelDebitNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= new DebitNoteDao().DelDebitNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static int DeleteOrders(string orderIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            int num = new OrderDao().DeleteOrders(orderIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的订单", new object[] { orderIds }));
            }
            return num;
        }

        public static bool DelSendNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= new SendNoteDao().DelSendNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static bool EditOrderShipNumber(string orderid, string shipnumber)
        {
            return new OrderDao().EditOrderShipNumber(orderid, shipnumber);
        }

        public static bool ExistsOrderByBargainDetialId(int userId, int bargainDetialId)
        {
            return new OrderDao().ExistsOrderByBargainDetialId(userId, bargainDetialId);
        }

        public static string ExportOrderData(string orderidList)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<table  cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            builder.Append("<thead><tr style=\"font-weight: bold; white-space: nowrap;background:#ccc;\">");
            builder.Append("<th style=\"border-right: 1px solid #ccc;\">订单编号</th><th style=\"border-right: 1px solid #ccc;\">商品名称</th><th style=\"border-right: 1px solid #ccc;\">商品编码</th><th style=\"border-right: 1px solid #ccc;\">SKU</th><th style=\"border-right: 1px solid #ccc;\">单价</th><th style=\"border-right: 1px solid #ccc;\">数量</th><th style=\"border-right: 1px solid #ccc;\">涨价或优惠</th><th style=\"border-right: 1px solid #ccc;\">买家会员名</th><th style=\"border-right: 1px solid #ccc;\">买家应付货款</th><th style=\"border-right: 1px solid #ccc;\">买家应付邮费</th><th style=\"border-right: 1px solid #ccc;\">总金额</th><th style=\"border-right: 1px solid #ccc;\">买家实际支付积分</th><th style=\"border-right: 1px solid #ccc;\">订单状态</th><th style=\"border-right: 1px solid #ccc;\">买家留言</th><th style=\"border-right: 1px solid #ccc;\">收货人姓名</th><th style=\"border-right: 1px solid #ccc;\">收货地址</th><th style=\"border-right: 1px solid #ccc;\">运送方式</th><th style=\"border-right: 1px solid #ccc;\">联系电话</th><th style=\"border-right: 1px solid #ccc;\">联系手机</th><th style=\"border-right: 1px solid #ccc;\">订单创建时间</th><th style=\"border-right: 1px solid #ccc;\">订单付款时间</th><th style=\"border-right: 1px solid #ccc;\">物流单号</th><th style=\"border-right: 1px solid #ccc;\">物流公司</th><th style=\"border-right: 1px solid #ccc;\">订单备注</th><th style=\"border-right: 1px solid #ccc;\">宝贝总数量</th><th style=\"border-right: 1px solid #ccc;\">分销商Id</th><th style=\"border-right: 1px solid #ccc;\">分销商店铺名称</th><th style=\"border-right: 1px solid #ccc;\">修改后的收货地址</th><th>积分抵扣</th><th>优惠</th>");
            builder.Append("</tr></thead><tbody>");
            foreach (string str in orderidList.Split(new char[] { ',' }))
            {
                OrderInfo orderInfo = GetOrderInfo(str);
                bool flag = true;
                if (orderInfo != null)
                {
                    int count = orderInfo.LineItems.Values.Count;
                    string sKU = string.Empty;
                    new StringBuilder();
                    int num2 = 0;
                    int num3 = 0;
                    foreach (LineItemInfo info2 in orderInfo.LineItems.Values)
                    {
                        builder.Append("<tr>");
                        if (flag)
                        {
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.OrderId), "</td>" }));
                        }
                        num2 += info2.PointNumber;
                        num3 += info2.Quantity;
                        sKU = info2.SKU;
                        if (string.IsNullOrEmpty(sKU))
                        {
                            ProductInfo productDetails = ProductHelper.GetProductDetails(info2.ProductId);
                            if (productDetails != null)
                            {
                                sKU = productDetails.ProductCode;
                            }
                        }
                        builder.Append("<td>" + FormatOrderStr(info2.ItemDescription) + "</td>");
                        builder.Append("<td>" + FormatOrderStr(sKU) + "</td>");
                        builder.Append("<td>" + FormatOrderStr(info2.SkuId) + "</td>");
                        builder.Append("<td>" + ((info2.Type == 1) ? "0" : FormatOrderStr(info2.ItemAdjustedPrice.ToString("F2"))) + "</td>");
                        builder.Append("<td>" + info2.Quantity.ToString() + "</td>");
                        builder.Append("<td>" + ((info2.Type == 1) ? "0.00" : ((info2.ItemAdjustedCommssion * -1M)).ToString("F2")) + "</td>");
                        if (flag)
                        {
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.Username), "</td>" }));
                            decimal total = orderInfo.GetTotal();
                            decimal adjustedFreight = orderInfo.AdjustedFreight;
                            object[] objArray3 = new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr((total - adjustedFreight).ToString("F2")), "</td>" };
                            builder.Append(string.Concat(objArray3));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.AdjustedFreight.ToString("F2")), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(total.ToString("F2")), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.PointExchange.ToString()), "</td>" }));
                            string str3 = string.Empty;
                            switch (orderInfo.OrderStatus)
                            {
                                case OrderStatus.WaitBuyerPay:
                                    str3 = "等待付款";
                                    break;

                                case OrderStatus.BuyerAlreadyPaid:
                                    str3 = "已付款";
                                    break;

                                case OrderStatus.SellerAlreadySent:
                                    str3 = "已发货";
                                    break;

                                case OrderStatus.Closed:
                                    str3 = "已关闭";
                                    break;

                                case OrderStatus.Finished:
                                    str3 = "交易完成";
                                    break;
                            }
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(str3), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.Remark.ToString()), "</td>" }));
                            string str4 = string.Empty;
                            string oldAddress = orderInfo.OldAddress;
                            if (!string.IsNullOrEmpty(orderInfo.ShippingRegion))
                            {
                                str4 = orderInfo.ShippingRegion.Replace(',', ' ');
                            }
                            if (!string.IsNullOrEmpty(orderInfo.Address))
                            {
                                str4 = str4 + orderInfo.Address;
                            }
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.ShipTo.ToString()), "</td>" }));
                            if (!string.IsNullOrEmpty(orderInfo.ZipCode))
                            {
                                str4 = str4 + " " + orderInfo.ZipCode;
                            }
                            if (!string.IsNullOrEmpty(orderInfo.TelPhone))
                            {
                                str4 = str4 + " " + orderInfo.TelPhone;
                            }
                            if (!string.IsNullOrEmpty(orderInfo.CellPhone))
                            {
                                str4 = str4 + " " + orderInfo.CellPhone;
                            }
                            string str6 = string.Empty;
                            if (string.IsNullOrEmpty(oldAddress))
                            {
                                builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(str4), "</td>" }));
                            }
                            else
                            {
                                str6 = str4;
                                builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(oldAddress), "</td>" }));
                            }
                            string realModeName = string.Empty;
                            if ((orderInfo.OrderStatus == OrderStatus.Finished) || (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent))
                            {
                                realModeName = orderInfo.RealModeName;
                                if (string.IsNullOrEmpty(realModeName))
                                {
                                    realModeName = orderInfo.ModeName;
                                }
                            }
                            else
                            {
                                realModeName = orderInfo.ModeName;
                            }
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(realModeName), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.TelPhone), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.CellPhone), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.OrderDate.ToString()), "</td>" }));
                            string str8 = string.Empty;
                            if (orderInfo.PayDate.HasValue)
                            {
                                str8 = orderInfo.PayDate.ToString();
                            }
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(str8), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.ShipOrderNumber), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(orderInfo.ExpressCompanyName), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr((orderInfo.ManagerRemark == null) ? "" : orderInfo.ManagerRemark.ToString()), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", orderInfo.GetProductTotalNum(), "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr((orderInfo.ReferralUserId > 0) ? orderInfo.ReferralUserId.ToString() : ""), "</td>" }));
                            if (orderInfo.ReferralUserId > 0)
                            {
                                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(orderInfo.ReferralUserId);
                                if (distributorInfo != null)
                                {
                                    builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(distributorInfo.StoreName), "</td>" }));
                                }
                                else
                                {
                                    builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(""), "</td>" }));
                                }
                            }
                            else
                            {
                                builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", FormatOrderStr(""), "</td>" }));
                            }
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", str6, "</td>" }));
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", (orderInfo.PointToCash == 0M) ? "" : orderInfo.PointToCash.ToString(), "</td>" }));
                            OrderInfo info5 = orderInfo;
                            StringBuilder builder2 = new StringBuilder();
                            if (!string.IsNullOrEmpty(info5.ActivitiesName))
                            {
                                builder2.Append(info5.ActivitiesName + ":￥" + info5.DiscountAmount.ToString("F2"));
                            }
                            if (!string.IsNullOrEmpty(info5.ReducedPromotionName))
                            {
                                if (!string.IsNullOrEmpty(builder2.ToString()))
                                {
                                    builder2.Append("\r\n");
                                }
                                builder2.Append(info5.ReducedPromotionName + ":￥" + info5.ReducedPromotionAmount.ToString("F2"));
                            }
                            if (!string.IsNullOrEmpty(info5.CouponName))
                            {
                                if (!string.IsNullOrEmpty(builder2.ToString()))
                                {
                                    builder2.Append("\r\n");
                                }
                                builder2.Append(info5.CouponName + ":￥" + info5.CouponAmount.ToString("F2"));
                            }
                            if (!string.IsNullOrEmpty(info5.RedPagerActivityName))
                            {
                                if (!string.IsNullOrEmpty(builder2.ToString()))
                                {
                                    builder2.Append("\r\n");
                                }
                                builder2.Append(info5.RedPagerActivityName + ":￥" + info5.RedPagerAmount.ToString("F2"));
                            }
                            if (info5.PointToCash > 0M)
                            {
                                if (!string.IsNullOrEmpty(builder2.ToString()))
                                {
                                    builder2.Append("\r\n");
                                }
                                builder2.Append("积分抵现:￥" + info5.PointToCash.ToString("F2"));
                            }
                            info5.GetAdjustCommssion();
                            decimal num6 = 0M;
                            decimal num7 = 0M;
                            foreach (LineItemInfo info6 in info5.LineItems.Values)
                            {
                                if (info6.IsAdminModify)
                                {
                                    num6 += info6.ItemAdjustedCommssion;
                                }
                                else
                                {
                                    num7 += info6.ItemAdjustedCommssion;
                                }
                            }
                            if (num6 != 0M)
                            {
                                if (num6 > 0M)
                                {
                                    if (!string.IsNullOrEmpty(builder2.ToString()))
                                    {
                                        builder2.Append("\r\n");
                                    }
                                    builder2.Append("管理员调价减:￥" + num6.ToString("F2"));
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(builder2.ToString()))
                                    {
                                        builder2.Append("\r\n");
                                    }
                                    decimal num9 = num6 * -1M;
                                    builder2.Append("管理员调价加:￥" + num9.ToString("F2"));
                                }
                            }
                            if (num7 != 0M)
                            {
                                if (num7 > 0M)
                                {
                                    if (!string.IsNullOrEmpty(builder2.ToString()))
                                    {
                                        builder2.Append("\r\n");
                                    }
                                    builder2.Append("分销商调价减:￥" + num7.ToString("F2"));
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(builder2.ToString()))
                                    {
                                        builder2.Append("\r\n");
                                    }
                                    builder2.Append("分销商调价加:￥" + ((num7 * -1M)).ToString("F2"));
                                }
                            }
                            builder.Append(string.Concat(new object[] { "<td rowspan=\"", count, "\"  style=\"vnd.ms-excel.numberformat: @;\">", builder2.ToString(), "</td>" }));
                        }
                        flag = false;
                        builder.Append("</tr>");
                    }
                }
            }
            builder.Append("<tbody></table>");
            return builder.ToString().Replace("<td>", "<td style=\"vnd.ms-excel.numberformat: @;\">");
        }

        public static string FormatOrderStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str.Replace(",", "，").Replace("\n", "").Replace(@"\", "＼");
        }

        public static DbQueryResult GetAllDebitNote(DebitNoteQuery query)
        {
            return new DebitNoteDao().GetAllDebitNote(query);
        }

        public static DataTable GetAllOrderID()
        {
            return new OrderDao().GetAllOrderID();
        }

        public static DbQueryResult GetAllSendNote(RefundApplyQuery query)
        {
            return new SendNoteDao().GetAllSendNote(query);
        }

        public static OrderInfo GetCalculadtionCommission(OrderInfo order)
        {
            return new OrderDao().GetCalculadtionCommission(order, 1);
        }

        public static int GetCountOrderIDByStatus(OrderStatus? orderstatus, OrderStatus? itemstatus)
        {
            return new OrderDao().GetCountOrderIDByStatus(orderstatus, itemstatus);
        }

        public static DbQueryResult GetDeleteOrders(OrderQuery query)
        {
            return new OrderDao().GetDeleteOrders(query);
        }

        private static string getEMSLastNum(string emsno)
        {
            List<char> list = emsno.ToList<char>();
            char ch = list[2];
            int num = int.Parse(ch.ToString()) * 8;
            char ch2 = list[3];
            num += int.Parse(ch2.ToString()) * 6;
            char ch3 = list[4];
            num += int.Parse(ch3.ToString()) * 4;
            char ch4 = list[5];
            num += int.Parse(ch4.ToString()) * 2;
            char ch5 = list[6];
            num += int.Parse(ch5.ToString()) * 3;
            char ch6 = list[7];
            num += int.Parse(ch6.ToString()) * 5;
            char ch7 = list[8];
            num += int.Parse(ch7.ToString()) * 9;
            char ch8 = list[9];
            num += int.Parse(ch8.ToString()) * 7;
            num = 11 - (num % 11);
            switch (num)
            {
                case 10:
                    num = 0;
                    break;

                case 11:
                    num = 5;
                    break;
            }
            return num.ToString();
        }

        private static string getEMSNext(string emsno)
        {
            long num = Convert.ToInt64(emsno.Substring(2, 8));
            if (num < 0x5f5e0ffL)
            {
                num += 1L;
            }
            string str = num.ToString().PadLeft(8, '0');
            string str2 = emsno.Substring(0, 2) + str + emsno.Substring(10, 1);
            return (emsno.Substring(0, 2) + str + getEMSLastNum(str2) + emsno.Substring(11, 2));
        }

        public static int GetItemNumByOrderID(string orderid)
        {
            return new LineItemDao().GetItemNumByOrderID(orderid);
        }

        public static DbQueryResult GetMemberDetailOrders(MemberDetailOrderQuery query)
        {
            return new OrderDao().GetMemberDetailOrders(query);
        }

        private static string GetNextExpress(string ExpressCom, string strno)
        {
            switch (ExpressCom.ToLower())
            {
                case "ems":
                    return getEMSNext(strno);

                case "顺丰快递":
                case "shunfeng":
                    return getSFNext(strno);

                case "宅急送":
                case "zhaijisong":
                    return getZJSNext(strno);
            }
            long num = long.Parse(strno) + 1L;
            return num.ToString();
        }

        public static DataSet GetOrderGoods(string orderIds)
        {
            return new OrderDao().GetOrderGoods(orderIds);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return new OrderDao().GetOrderInfo(orderId);
        }

        public static DbQueryResult GetOrders(OrderQuery query)
        {
            return new OrderDao().GetOrders(query);
        }

        public static DataSet GetOrdersAndLines(string orderIds)
        {
            return new OrderDao().GetOrdersAndLines(orderIds);
        }

        public static DataSet GetOrdersByOrderIDList(string orderIds)
        {
            return new OrderDao().GetOrdersByOrderIDList(orderIds);
        }

        public static DataSet GetProductGoods(string orderIds)
        {
            return new OrderDao().GetProductGoods(orderIds);
        }

        public static DataTable GetSendGoodsOrders(string orderIds)
        {
            return new OrderDao().GetSendGoodsOrders(orderIds);
        }

        private static string getSFNext(string sfno)
        {
            int[] numArray = new int[12];
            int[] numArray2 = new int[12];
            List<char> list = sfno.ToList<char>();
            string str = sfno.Substring(0, 11);
            string source = string.Empty;
            if (sfno.Substring(0, 1) == "0")
            {
                source = "0" + ((Convert.ToInt64(str) + 1L)).ToString();
            }
            else
            {
                source = (Convert.ToInt64(str) + 1L).ToString();
            }
            for (int i = 0; i < 12; i++)
            {
                numArray[i] = int.Parse(list[i].ToString());
            }
            source.ToList<char>();
            for (int j = 0; j < 11; j++)
            {
                numArray2[j] = int.Parse(source[j].ToString());
            }
            if (((numArray2[8] - numArray[8]) == 1) && ((numArray[8] % 2) == 1))
            {
                if ((numArray[11] - 8) >= 0)
                {
                    numArray2[11] = numArray[11] - 8;
                }
                else
                {
                    numArray2[11] = (numArray[11] - 8) + 10;
                }
            }
            else if (((numArray2[8] - numArray[8]) == 1) && ((numArray[8] % 2) == 0))
            {
                if ((numArray[11] - 7) >= 0)
                {
                    numArray2[11] = numArray[11] - 7;
                }
                else
                {
                    numArray2[11] = (numArray[11] - 7) + 10;
                }
            }
            else if (((numArray[9] == 3) || (numArray[9] == 6)) && (numArray[10] == 9))
            {
                if ((numArray[11] - 5) >= 0)
                {
                    numArray2[11] = numArray[11] - 5;
                }
                else
                {
                    numArray2[11] = (numArray[11] - 5) + 10;
                }
            }
            else if (numArray[10] == 9)
            {
                if ((numArray[11] - 4) >= 0)
                {
                    numArray2[11] = numArray[11] - 4;
                }
                else
                {
                    numArray2[11] = (numArray[11] - 4) + 10;
                }
            }
            else if ((numArray[11] - 1) >= 0)
            {
                numArray2[11] = numArray[11] - 1;
            }
            else
            {
                numArray2[11] = (numArray[11] - 1) + 10;
            }
            return (source + numArray2[11].ToString());
        }

        public static int GetSkuStock(string skuId)
        {
            return new SkuDao().GetSkuItem(skuId).Stock;
        }

        public static DataSet GetTradeOrders(OrderQuery query, out int records)
        {
            return new OrderDao().GetTradeOrders(query, 0, out records);
        }

        public static DataTable GetUserOrderPaidWaitFinish(int userId)
        {
            return new OrderDao().GetUserOrderPaidWaitFinish(userId);
        }

        private static string getZJSNext(string zjsno)
        {
            long num = Convert.ToInt64(zjsno) + 11L;
            if ((num % 10L) > 6L)
            {
                num -= 7L;
            }
            return num.ToString().PadLeft(zjsno.Length, '0');
        }

        public static bool MemberAmountAddByRefund(MemberInfo memberInfo, decimal amount, string orderid)
        {
            return new AmountDao().MemberAmountAddByRefund(memberInfo, amount, orderid);
        }

        public static bool MondifyAddress(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (!order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS))
            {
                return false;
            }
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的收货地址", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static int RealDeleteOrders(string orderIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            int num = new OrderDao().RealDeleteOrders(orderIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的订单", new object[] { orderIds }));
            }
            return num;
        }

        public static int RealDeleteOrders(string orderIds, DateTime? orderDate)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            int num = new OrderDao().RealDeleteOrders(orderIds);
            if (num > 0)
            {
                string retInfo = "";
                new ShopStatisticDao().StatisticsOrdersByRecDate(orderDate.Value, UpdateAction.AllUpdate, 0, out retInfo);
                EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的订单", new object[] { orderIds }));
            }
            return num;
        }

        public static int RestoreOrders(string orderIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            int num = new OrderDao().RestoreOrders(orderIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "还原了编号为\"{0}\"的订单", new object[] { orderIds }));
            }
            return num;
        }

        public static bool SaveDebitNote(DebitNoteInfo note)
        {
            return new DebitNoteDao().SaveDebitNote(note);
        }

        public static bool SaveRemark(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.RemarkOrder);
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”进行了备注", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool SaveSendNote(SendNoteInfo note)
        {
            return new SendNoteDao().SaveSendNote(note);
        }

        public static bool SendGoods(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderSendGoods);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                OrderDao dao = new OrderDao();
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                order.ShippingDate = new DateTime?(DateTime.Now);
                flag = dao.UpdateOrder(order, null);
                string str = "";
                if (!flag)
                {
                    return flag;
                }
                bool flag2 = false;
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    OrderStatus orderItemsStatus = info.OrderItemsStatus;
                    switch (orderItemsStatus)
                    {
                        case OrderStatus.WaitBuyerPay:
                        case OrderStatus.BuyerAlreadyPaid:
                            break;

                        default:
                        {
                            if (orderItemsStatus == OrderStatus.ApplyForRefund)
                            {
                                flag2 = true;
                                str = str + "'" + info.SkuId + "',";
                            }
                            continue;
                        }
                    }
                    str = str + "'" + info.SkuId + "',";
                }
                if (flag2)
                {
                    dao.DeleteReturnRecordForSendGoods(order.OrderId);
                }
                if (!string.IsNullOrEmpty(str))
                {
                    dao.UpdateItemsStatus(order.OrderId, 3, str.Substring(0, str.Length - 1));
                }
                bool flag3 = true;
                foreach (LineItemInfo info2 in order.LineItems.Values)
                {
                    if (info2.Type == 0)
                    {
                        flag3 = false;
                        break;
                    }
                }
                if (((order.Gateway == null) || (order.Gateway.ToLower() == "hishop.plugins.payment.podrequest")) || flag3)
                {
                    dao.UpdatePayOrderStock(order);
                    foreach (LineItemInfo info3 in order.LineItems.Values)
                    {
                        str = str + info3.SkuId + ",";
                        ProductDao dao2 = new ProductDao();
                        ProductInfo productDetails = dao2.GetProductDetails(info3.ProductId);
                        productDetails.SaleCounts += info3.Quantity;
                        productDetails.ShowSaleCounts += info3.Quantity;
                        dao2.UpdateProduct(productDetails, null);
                    }
                }
                MemberHelper.GetMember(order.UserId);
                try
                {
                    OrderInfo info5 = order;
                    if (info5 != null)
                    {
                        Messenger.SendWeiXinMsg_OrderDeliver(info5);
                    }
                }
                catch (Exception)
                {
                }
                EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool SetOrderExpressComputerpe(string purchaseOrderIds, string expressCompanyName, string expressCompanyAbb)
        {
            return new OrderDao().SetOrderExpressComputerpe(purchaseOrderIds, expressCompanyName, expressCompanyAbb);
        }

        public static bool SetOrderShipNumber(string orderId, string startNumber)
        {
            OrderInfo orderInfo = new OrderDao().GetOrderInfo(orderId);
            orderInfo.ShipOrderNumber = startNumber;
            return new OrderDao().UpdateOrder(orderInfo, null);
        }

        public static void SetOrderShipNumber(string[] orderIds, string startNumber, string ExpressCom = "")
        {
            string strno = startNumber;
            OrderDao dao = new OrderDao();
            for (int i = 0; i < orderIds.Length; i++)
            {
                if (i != 0)
                {
                    strno = GetNextExpress(ExpressCom, strno);
                }
                else
                {
                    GetNextExpress(ExpressCom, strno);
                }
                dao.EditOrderShipNumber(orderIds[i], strno);
            }
        }

        public static bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            return new OrderDao().SetOrderShippingMode(orderIds, realShippingModeId, realModeName);
        }

        public static bool SetPrintOrderExpress(string orderId, string expressCompanyName, string expressCompanyAbb, string shipOrderNumber)
        {
            return new OrderDao().SetPrintOrderExpress(orderId, expressCompanyName, expressCompanyAbb, shipOrderNumber);
        }

        public static bool UpdateAdjustCommssions(string orderId, string itemid, decimal adjustcommssion)
        {
            bool flag = false;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    OrderInfo orderInfo = GetOrderInfo(orderId);
                    if (orderInfo == null)
                    {
                        return false;
                    }
                    LineItemInfo lineItem = orderInfo.LineItems[itemid];
                    lineItem.ItemAdjustedCommssion = adjustcommssion;
                    if (!new LineItemDao().UpdateLineItem(orderId, lineItem, dbTran))
                    {
                        dbTran.Rollback();
                    }
                    if (!new OrderDao().UpdateOrder(orderInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
                return flag;
            }
        }

        public static bool UpdateCalculadtionCommission(string orderid)
        {
            OrderInfo calculadtionCommission = GetCalculadtionCommission(GetOrderInfo(orderid));
            new OrderDao().UpdateOrder(calculadtionCommission, null);
            return new OrderDao().UpdateCalculadtionCommission(calculadtionCommission, null);
        }

        public static bool UpdateOrder(OrderInfo order)
        {
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag && (order.OrderStatus == OrderStatus.Closed))
            {
                new OrderDao().UpdateItemsStatus(order.OrderId, 4, "all");
            }
            return flag;
        }

        public static bool UpdateOrderAmount(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                flag = new OrderDao().UpdateOrder(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"订单的金额", new object[] { order.OrderId }));
                }
            }
            return flag;
        }

        public static bool UpdateOrderCompany(string orderId, string companycode, string companyname, string shipNumber)
        {
            return new OrderDao().UpdateOrderCompany(orderId, companycode, companyname, shipNumber);
        }

        public static void UpdateOrderItemBalance(string orderid)
        {
            new OrderDao().UpdateOrderItemBalance(orderid);
        }

        public static bool UpdateOrderItems(OrderInfo order)
        {
            return new OrderDao().UpdateOrderItems(order);
        }

        public static bool UpdateOrderPaymentType(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (!order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE))
            {
                return false;
            }
            bool flag = new OrderDao().UpdateOrder(order, null);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的支付方式", new object[] { order.OrderId }));
            }
            return flag;
        }
    }
}

