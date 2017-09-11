namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Orders;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.HtmlControls;

    public class BatchPrintSendOrderGoods : AdminPage
    {
        protected HtmlGenericControl divContent;

        protected BatchPrintSendOrderGoods() : base("m03", "00000")
        {
        }

        private List<OrderInfo> GetPrintData(string orderIds)
        {
            List<OrderInfo> list = new List<OrderInfo>();
            foreach (string str in orderIds.Split(new char[] { ',' }))
            {
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(str);
                list.Add(orderInfo);
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request["OrderIds"].Trim(new char[] { ',' });
            if (!string.IsNullOrEmpty(str))
            {
                foreach (OrderInfo info in (from a in this.GetPrintData(str)
                    orderby string.Concat(new object[] { a.ShipTo, a.RegionId, a.ExpressCompanyAbb, a.Address, a.CellPhone }) descending
                    select a).ToList<OrderInfo>())
                {
                    HtmlGenericControl child = new HtmlGenericControl("div");
                    child.Attributes["class"] = "order print";
                    child.Attributes["style"] = "padding-bottom:60px;padding-top:40px;";
                    StringBuilder builder = new StringBuilder("");
                    builder.AppendFormat("<div class=\"info clear\"><ul class=\"sub-info\"><li><span>订单号： </span>{2}</li><li><span>成交时间： </span>{1}</li><li><span>收货人姓名： </span>{0}</li></ul></div>", info.ShipTo, (info.FinishDate.HasValue ? DateTime.Parse(info.FinishDate.ToString()) : info.OrderDate).ToString("yyyy-MM-dd HH:mm:ss"), info.OrderId);
                    builder.Append("<table><col class=\"col-1\" /><col class=\"col-3\" /><col class=\"col-3\" /><col class=\"col-3\" /><col class=\"col-4\" /><col class=\"col-5\" /><thead><tr><th>商品信息</th><th>商品编码</th><th>单价</th><th>数量</th><th>小计</th><th>总价</th></tr></thead><tbody>");
                    Dictionary<string, LineItemInfo> lineItems = info.LineItems;
                    if (lineItems != null)
                    {
                        int num = 0;
                        foreach (string str2 in lineItems.Keys)
                        {
                            LineItemInfo info2 = lineItems[str2];
                            string sKU = string.Empty;
                            if (info2.OrderItemsStatus == OrderStatus.Returned)
                            {
                                sKU = "(已退货，金额￥" + info2.ReturnMoney.ToString("F2") + ")";
                            }
                            else if (info2.OrderItemsStatus == OrderStatus.Refunded)
                            {
                                sKU = "(已退款，金额￥" + info2.ReturnMoney.ToString("F2") + ")";
                            }
                            builder.AppendFormat("<tr><td>{0}</td>", info2.ItemDescription + sKU + (string.IsNullOrEmpty(info2.SKUContent) ? "" : ("<br>" + info2.SKUContent)));
                            sKU = info2.SKU;
                            if (string.IsNullOrEmpty(sKU))
                            {
                                ProductInfo productDetails = ProductHelper.GetProductDetails(info2.ProductId);
                                if (productDetails != null)
                                {
                                    sKU = productDetails.ProductCode;
                                }
                            }
                            if (string.IsNullOrEmpty(sKU))
                            {
                                sKU = "-";
                            }
                            builder.AppendFormat("<td style='text-align:center;'>{0}</td>", sKU);
                            builder.AppendFormat("<td>￥{0}</td>", Math.Round(info2.ItemListPrice, 2));
                            builder.AppendFormat("<td style='padding-left:15px;'>{0}</td>", info2.ShipmentQuantity);
                            builder.AppendFormat("<td style='border-left:1px solid #858585;'>￥{0}</td>", Math.Round((decimal) (info2.GetSubTotal() - info2.DiscountAverage), 2));
                            if (num == 0)
                            {
                                string str4 = string.Empty;
                                StringBuilder builder2 = new StringBuilder();
                                if (!string.IsNullOrEmpty(info.ActivitiesName))
                                {
                                    builder2.Append("<p>" + info.ActivitiesName + ":￥" + info.DiscountAmount.ToString("F2") + "</p>");
                                }
                                if (!string.IsNullOrEmpty(info.ReducedPromotionName))
                                {
                                    builder2.Append("<p>" + info.ReducedPromotionName + ":￥" + info.ReducedPromotionAmount.ToString("F2") + "</p>");
                                }
                                if (!string.IsNullOrEmpty(info.CouponName))
                                {
                                    builder2.Append("<p>" + info.CouponName + ":￥" + info.CouponAmount.ToString("F2") + "</p>");
                                }
                                if (!string.IsNullOrEmpty(info.RedPagerActivityName))
                                {
                                    builder2.Append("<p>" + info.RedPagerActivityName + ":￥" + info.RedPagerAmount.ToString("F2") + "</p>");
                                }
                                if (info.PointToCash > 0M)
                                {
                                    builder2.Append("<p>积分抵现:￥" + info.PointToCash.ToString("F2") + "</p>");
                                }
                                decimal adjustCommssion = info.GetAdjustCommssion();
                                if (adjustCommssion > 0M)
                                {
                                    builder2.Append("<p>管理员调价优惠:￥" + adjustCommssion.ToString("F2") + "</p>");
                                }
                                else if (adjustCommssion < 0M)
                                {
                                    builder2.Append("<p>管理员调价增加:￥" + adjustCommssion.ToString("F2").Trim(new char[] { '-' }) + "</p>");
                                }
                                str4 = builder2.ToString();
                                builder.AppendFormat("<td rowspan='{0}' colspan='1' style='border-left:1px solid #858585;'>{1}</td>", lineItems.Keys.Count, string.Format("<p><strong>￥{0}</strong></p>" + str4 + "<p>含运费￥{1}</p><p></p>", info.GetTotal().ToString("F2"), info.AdjustedFreight.ToString("F2")));
                            }
                            builder.Append("</tr>");
                            num++;
                        }
                    }
                    builder.Append("</tbody></table>");
                    child.InnerHtml = builder.ToString();
                    this.divContent.Controls.AddAt(0, child);
                }
            }
        }
    }
}

