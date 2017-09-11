namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [PrivilegeCheck(Privilege.Orders)]
    public class OrderSplit : AdminPage
    {
        protected string orderId;
        protected string reUrl;

        protected OrderSplit() : base("m03", "00000")
        {
            this.reUrl = Globals.RequestQueryStr("reurl");
            this.orderId = Globals.RequestQueryStr("OrderId");
        }

        private string FormatNewOrderID(string oldorderid, string stradd)
        {
            if (stradd == "1")
            {
                return oldorderid;
            }
            return (oldorderid + "-" + stradd);
        }

        private string GetItemStateName(OrderStatus status)
        {
            string str = string.Empty;
            switch (status)
            {
                case OrderStatus.BuyerAlreadyPaid:
                    return "已付款";

                case OrderStatus.SellerAlreadySent:
                case OrderStatus.Closed:
                case OrderStatus.ApplyForReplacement:
                    return str;

                case OrderStatus.Finished:
                    return "已完成";

                case OrderStatus.ApplyForRefund:
                    return "退款中";

                case OrderStatus.ApplyForReturns:
                    return "退货中";

                case OrderStatus.Refunded:
                    return "已退款";

                case OrderStatus.Returned:
                    return "已退货";
            }
            return str;
        }

        private string GetOrderItemListByOrder(OrderInfo order)
        {
            StringBuilder builder = new StringBuilder();
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                builder.Append(string.Concat(new object[] { 
                    ",{\"ID\":\"", info.ID, "\",\"ProductID\":\"", info.ProductId.ToString(), "\",\"SkuID\":\"", info.SkuId, "\",\"Quantity\":\"", info.Quantity.ToString(), "\",\"ItemListPrice\":\"", info.ItemListPrice.ToString("F2"), "\",\"SKUContent\":\"", Globals.String2Json(info.SKUContent), "\",\"ThumbnailsUrl\":\"", Globals.String2Json(info.ThumbnailsUrl), "\",\"OrderItemsStatus\":\"", this.GetItemStateName(info.OrderItemsStatus), 
                    "\",\"ItemDescription\":\"", Globals.String2Json(info.ItemDescription), "\"}"
                 }));
            }
            return builder.ToString().Trim(new char[] { ',' });
        }

        private string GetOrderItemListData(string itemlist, string orderid)
        {
            StringBuilder builder = new StringBuilder();
            int id = 0;
            if (!string.IsNullOrEmpty(itemlist) && !string.IsNullOrEmpty(orderid))
            {
                foreach (string str in itemlist.Split(new char[] { ',' }))
                {
                    id = Globals.ToNum(str);
                    if (id > 0)
                    {
                        LineItemInfo lineItemInfo = OrderSplitHelper.GetLineItemInfo(id, orderid);
                        if (lineItemInfo != null)
                        {
                            builder.Append(string.Concat(new object[] { 
                                ",{\"ID\":\"", lineItemInfo.ID, "\",\"ProductID\":\"", lineItemInfo.ProductId.ToString(), "\",\"SkuID\":\"", lineItemInfo.SkuId, "\",\"Quantity\":\"", lineItemInfo.Quantity.ToString(), "\",\"ItemListPrice\":\"", lineItemInfo.ItemListPrice.ToString("F2"), "\",\"SKUContent\":\"", Globals.String2Json(lineItemInfo.SKUContent), "\",\"ThumbnailsUrl\":\"", Globals.String2Json(lineItemInfo.ThumbnailsUrl), "\",\"OrderItemsStatus\":\"", this.GetItemStateName(lineItemInfo.OrderItemsStatus), 
                                "\",\"ItemDescription\":\"", Globals.String2Json(lineItemInfo.ItemDescription), "\"}"
                             }));
                        }
                    }
                }
            }
            return builder.ToString().Trim(new char[] { ',' });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str2;
            string str4;
            int num2;
            string str = Globals.RequestFormStr("posttype");
            if (string.IsNullOrEmpty(this.reUrl))
            {
                this.reUrl = "manageorder.aspx";
            }
            switch (str)
            {
                case "getordersplit":
                {
                    base.Response.ContentType = "application/json";
                    str2 = "[]";
                    this.orderId = Globals.RequestFormStr("orderid");
                    IList<OrderSplitInfo> orderSplitItems = OrderSplitHelper.GetOrderSplitItems(this.orderId);
                    if (orderSplitItems.Count <= 0)
                    {
                        OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
                        StringBuilder builder2 = new StringBuilder();
                        if (orderInfo != null)
                        {
                            if (orderInfo.SplitState > 0)
                            {
                                builder2.Append("{\"id\":\"splited\"}");
                            }
                            else
                            {
                                builder2.Append("[");
                                if (orderInfo != null)
                                {
                                    builder2.Append("{\"id\":0,\"orderid\":\"" + orderInfo.OrderId + "\",\"adjustedfreight\":" + orderInfo.AdjustedFreight.ToString("F2") + ",\"data\":[" + this.GetOrderItemListByOrder(orderInfo) + "]}");
                                }
                                builder2.Append("]");
                            }
                        }
                        str2 = builder2.ToString();
                        break;
                    }
                    StringBuilder builder = new StringBuilder();
                    builder.Append("[");
                    int num = 0;
                    foreach (OrderSplitInfo info in orderSplitItems)
                    {
                        if (num != 0)
                        {
                            builder.Append(",");
                        }
                        num++;
                        builder.Append(string.Concat(new object[] { "{\"id\":", info.Id, ",\"orderid\":\"", this.FormatNewOrderID(info.OldOrderId, info.OrderIDNum.ToString()), "\",\"adjustedfreight\":", info.AdjustedFreight.ToString("F2"), ",\"data\":[", this.GetOrderItemListData(info.ItemList, info.OldOrderId), "]}" }));
                    }
                    builder.Append("]");
                    str2 = builder.ToString();
                    break;
                }
                case "savesplit":
                {
                    base.Response.ContentType = "application/json";
                    string neworderid = Globals.RequestFormStr("toorderid");
                    str4 = Globals.RequestFormStr("fromorderid");
                    num2 = Globals.RequestFormNum("itemid");
                    string skuid = Globals.RequestFormStr("fromskuid");
                    str2 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                    OrderInfo oldOrderInfo = OrderHelper.GetOrderInfo(str4);
                    if (oldOrderInfo != null)
                    {
                        if ((oldOrderInfo.OrderStatus != OrderStatus.WaitBuyerPay) && (oldOrderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid))
                        {
                            OrderSplitHelper.DelOrderSplitByOrderID(str4, null);
                            str2 = "{\"type\":\"3\",\"tips\":\"当前订单状态不允许拆分！\"}";
                        }
                        else
                        {
                            decimal num3 = oldOrderInfo.GetTotal() - oldOrderInfo.AdjustedFreight;
                            decimal num4 = 0M;
                            foreach (LineItemInfo info4 in oldOrderInfo.LineItems.Values)
                            {
                                if (info4.ID == num2)
                                {
                                    if (info4.Type != 1)
                                    {
                                        num4 = ((info4.ItemAdjustedPrice * info4.Quantity) - info4.ItemAdjustedCommssion) - info4.DiscountAverage;
                                    }
                                    break;
                                }
                            }
                            if (num4 >= num3)
                            {
                                str2 = "{\"type\":\"0\",\"tips\":\"订单拆分后，原订单的价格将不大于0！\"}";
                            }
                            else if ((num4 == 0M) && (neworderid == "0"))
                            {
                                str2 = "{\"type\":\"0\",\"tips\":\"订单拆分后，新订单的价格必须大于0！\"}";
                            }
                            else
                            {
                                string str10 = OrderSplitHelper.OrderSplitToTemp(oldOrderInfo, skuid, neworderid, num2);
                                if (str10 != null)
                                {
                                    if (!(str10 == "1"))
                                    {
                                        if (str10 == "-1")
                                        {
                                            str2 = "{\"type\":\"0\",\"tips\":\"订单只有一条记录，不允许拆分！\"}";
                                        }
                                        else if (str10 == "-2")
                                        {
                                            str2 = "{\"type\":\"0\",\"tips\":\"非法数据！\"}";
                                        }
                                        else if (str10 == "-3")
                                        {
                                            str2 = "{\"type\":\"0\",\"tips\":\"拆分出去的订单价格必须大于0！\"}";
                                        }
                                    }
                                    else
                                    {
                                        str2 = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                                    }
                                }
                            }
                        }
                    }
                    base.Response.Write(str2);
                    base.Response.End();
                    return;
                }
                case "cancelordersplit":
                {
                    string str11;
                    base.Response.ContentType = "application/json";
                    str4 = Globals.RequestFormStr("fromorderid");
                    num2 = Globals.RequestFormNum("itemid");
                    int fromsplitid = Globals.RequestFormNum("fromsplitid");
                    str2 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                    if ((((OrderHelper.GetOrderInfo(str4) != null) && (num2 > 0)) && ((fromsplitid > 0) && ((str11 = OrderSplitHelper.CancelSplitOrderByID(str4, fromsplitid, num2)) != null))) && (str11 == "1"))
                    {
                        str2 = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                    }
                    base.Response.Write(str2);
                    base.Response.End();
                    return;
                }
                case "editfright":
                {
                    base.Response.ContentType = "application/json";
                    str2 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                    int id = Globals.RequestFormNum("id");
                    decimal result = 0M;
                    decimal.TryParse(Globals.RequestFormStr("val"), out result);
                    if (result >= 0M)
                    {
                        OrderSplitInfo orderSplitInfo = OrderSplitHelper.GetOrderSplitInfo(id);
                        if (orderSplitInfo != null)
                        {
                            orderSplitInfo.AdjustedFreight = result;
                            if (OrderSplitHelper.UpdateOrderSplitFright(orderSplitInfo))
                            {
                                str2 = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                            }
                        }
                    }
                    base.Response.Write(str2);
                    base.Response.End();
                    return;
                }
                case "cancelsplittoorder":
                    base.Response.ContentType = "application/json";
                    str2 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                    if (OrderSplitHelper.DelOrderSplitByOrderID(Globals.RequestFormStr("fromorderid"), null))
                    {
                        str2 = "{\"type\":\"1\",\"tips\":\"取消成功！\"}";
                    }
                    base.Response.Write(str2);
                    base.Response.End();
                    return;

                case "savesplittoorder":
                {
                    base.Response.ContentType = "application/json";
                    str2 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                    str4 = Globals.RequestFormStr("fromorderid");
                    IList<OrderSplitInfo> infoList = OrderSplitHelper.GetOrderSplitItems(str4);
                    if (infoList.Count <= 1)
                    {
                        str2 = "{\"type\":\"0\",\"tips\":\"订单未拆分，不能保存！\"}";
                    }
                    else
                    {
                        OrderInfo oldorderinfo = OrderHelper.GetOrderInfo(str4);
                        if (oldorderinfo == null)
                        {
                            str2 = "{\"type\":\"0\",\"tips\":\"主订单已不存在，不能保存！\"}";
                        }
                        else if ((oldorderinfo.OrderStatus != OrderStatus.WaitBuyerPay) && (oldorderinfo.OrderStatus != OrderStatus.BuyerAlreadyPaid))
                        {
                            str2 = "{\"type\":\"0\",\"tips\":\"待付款和待发货状态的订单才允许拆分！\"}";
                        }
                        else
                        {
                            string str8 = OrderSplitHelper.UpdateAndCreateOrderByOrderSplitInfo(infoList, oldorderinfo);
                            if (str8 == "1")
                            {
                                str2 = "{\"type\":\"1\",\"tips\":\"订单拆分成功！\"}";
                            }
                            else
                            {
                                str2 = "{\"type\":\"0\",\"tips\":\"订单拆分失败，原因是" + str8 + "！\"}";
                            }
                        }
                    }
                    base.Response.Write(str2);
                    base.Response.End();
                    return;
                }
                default:
                    return;
            }
            base.Response.Write(str2);
            base.Response.End();
        }
    }
}

