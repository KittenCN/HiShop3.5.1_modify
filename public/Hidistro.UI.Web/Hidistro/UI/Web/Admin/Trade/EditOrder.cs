namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditOrders)]
    public class EditOrder : AdminPage
    {
        protected HiddenField hdAdjustedFreight;
        protected HiddenField hdRemainToPayMoney;
        protected int iframeHeight;
        protected Literal litAdjustedCommssion;
        protected Literal litItemTotalPrice;
        protected Literal litLogistic;
        protected Literal litOrderGoodsTotalPrice;
        protected Literal litOrderTotal;
        protected Literal litOtherShow;
        private OrderInfo order;
        protected string orderId;
        protected string ReUrl;
        protected Repeater rptItemList;
        protected TextBox txtAdjustedFreight;

        protected EditOrder() : base("m03", "ddp03")
        {
            this.iframeHeight = 300;
            this.ReUrl = Globals.RequestQueryStr("reurl");
        }

        protected string FormatAdjustedCommssion(object itemType, object itemAdjustedCommssion, object isAdminModify)
        {
            decimal num = decimal.Parse(itemAdjustedCommssion.ToString());
            if (!bool.Parse(isAdminModify.ToString()))
            {
                num = 0M;
            }
            if (Globals.ToNum(itemType) == 0)
            {
                string[] strArray = new string[] { " <input type=\"hidden\" name=\"oldadjustedcommssion\" value=\"", (num * -1M).ToString("F2"), "\"/><input type=\"text\" name=\"adjustedcommssion\" value=\"", (num * -1M).ToString("F2"), "\" title=\"输入负数为优惠金额，正常输入为涨价\" maxlength=\"7\" />" };
                return string.Concat(strArray);
            }
            string str2 = (num * -1M).ToString("F2");
            string[] strArray2 = new string[] { str2, " <input type=\"hidden\" name=\"oldadjustedcommssion\" value=\"", (num * -1M).ToString("F2"), "\"/><input type=\"hidden\" name=\"adjustedcommssion\" value=\"", (num * -1M).ToString("F2"), "\" />" };
            return string.Concat(strArray2);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.litOtherShow.Text = "";
            string str = Globals.RequestFormStr("posttype");
            if (string.IsNullOrEmpty(this.ReUrl))
            {
                this.ReUrl = "manageorder.aspx";
            }
            if (str == "updateorder")
            {
                string str2 = Globals.RequestFormStr("data");
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"0\",\"tips\":\"修改失败！\"}";
                JArray array = (JArray) JsonConvert.DeserializeObject(str2);
                OrderInfo order = null;
                foreach (JObject obj2 in array)
                {
                    string str4 = obj2["o"].ToString();
                    decimal num = decimal.Parse(obj2["f"].ToString());
                    JArray array2 = (JArray) obj2["data"];
                    if (array2.Count > 0)
                    {
                        string itemid = string.Empty;
                        decimal num2 = 0.00M;
                        if (!string.IsNullOrEmpty(str4))
                        {
                            order = OrderHelper.GetOrderInfo(str4);
                            if ((order != null) && (order.OrderStatus == OrderStatus.WaitBuyerPay))
                            {
                                foreach (JObject obj3 in array2)
                                {
                                    itemid = obj3["skuid"].ToString();
                                    num2 = decimal.Parse(obj3["adjustedcommssion"].ToString());
                                    OrderHelper.UpdateAdjustCommssions(str4, itemid, num2 * -1M);
                                }
                                if (((num >= 0M) && (order != null)) && (order.AdjustedFreight != num))
                                {
                                    order.AdjustedFreight = num;
                                    OrderHelper.UpdateOrder(order);
                                }
                                if (order.BalancePayMoneyTotal > 0M)
                                {
                                    OrderHelper.UpdateOrderItemBalance(str4);
                                }
                                OrderHelper.UpdateCalculadtionCommission(str4);
                                s = "{\"type\":\"1\",\"tips\":\"订单价格修改成功！\"}";
                            }
                            else
                            {
                                s = "{\"type\":\"0\",\"tips\":\"当前订单状态不允许修改价格！\"}";
                            }
                        }
                    }
                }
                base.Response.Write(s);
                base.Response.End();
            }
            else
            {
                this.orderId = Globals.RequestQueryStr("OrderId");
                if (string.IsNullOrEmpty(this.orderId))
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.order = OrderHelper.GetOrderInfo(this.orderId);
                    if (this.order == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.rptItemList.DataSource = this.order.LineItems.Values;
                        this.rptItemList.DataBind();
                        decimal num3 = ((this.order.GetTotal() + this.order.GetAdjustCommssion()) - this.order.AdjustedFreight) - this.order.BalancePayMoneyTotal;
                        this.hdRemainToPayMoney.Value = num3.ToString("F2");
                        this.hdAdjustedFreight.Value = this.order.Freight.ToString("F2");
                        if (this.order.BalancePayMoneyTotal > 0M)
                        {
                            this.iframeHeight = 330;
                            string[] strArray = new string[] { "<p>买家已使用余额支付 <span class='red'>￥", this.order.BalancePayMoneyTotal.ToString("F2"), "</span>，调整优惠不能小于 <span class='red'>-￥", (num3 - 0.01M).ToString("F2"), "</span></p>" };
                            this.litOtherShow.Text = string.Concat(strArray);
                        }
                        this.litLogistic.Text = string.IsNullOrEmpty(this.order.RealModeName) ? this.order.ModeName : this.order.RealModeName;
                        this.txtAdjustedFreight.Text = this.order.AdjustedFreight.ToString("F", CultureInfo.InvariantCulture);
                        decimal num4 = 0M;
                        if (!string.IsNullOrEmpty(this.order.ActivitiesName))
                        {
                            num4 += this.order.DiscountAmount;
                        }
                        if (!string.IsNullOrEmpty(this.order.ReducedPromotionName))
                        {
                            num4 += this.order.ReducedPromotionAmount;
                        }
                        if (!string.IsNullOrEmpty(this.order.CouponName))
                        {
                            num4 += this.order.CouponAmount;
                        }
                        if (!string.IsNullOrEmpty(this.order.RedPagerActivityName))
                        {
                            num4 += this.order.RedPagerAmount;
                        }
                        if (this.order.PointToCash > 0M)
                        {
                            num4 += this.order.PointToCash;
                        }
                        this.litOrderGoodsTotalPrice.Text = this.litItemTotalPrice.Text = this.order.GetAmount().ToString("F2");
                        this.litAdjustedCommssion.Text = this.order.GetTotalDiscountAverage().ToString("F2");
                        this.litOrderTotal.Text = (this.order.GetAmount() - num4).ToString("F", CultureInfo.InvariantCulture);
                    }
                }
            }
        }
    }
}

