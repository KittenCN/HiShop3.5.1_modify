namespace Hidistro.UI.Web.Pay
{
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    public class wap_alipay_return_url : Page
    {
        protected decimal Amount;
        protected OrderInfo Order;
        protected string OrderId;

        public SortedDictionary<string, string> GetRequestGet()
        {
            int index = 0;
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            string[] allKeys = base.Request.QueryString.AllKeys;
            for (index = 0; index < allKeys.Length; index++)
            {
                dictionary.Add(allKeys[index], base.Request.QueryString[allKeys[index]]);
            }
            return dictionary;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.Response.Write("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no\"/>");
            SortedDictionary<string, string> requestGet = this.GetRequestGet();
            if (requestGet.Count > 0)
            {
                Notify notify = new Notify();
                if (notify.Verify(requestGet, base.Request.QueryString["notify_id"], base.Request.QueryString["sign"]))
                {
                    this.OrderId = base.Request.QueryString["out_trade_no"];
                    string text1 = base.Request.QueryString["trade_no"];
                    string text2 = base.Request.QueryString["trade_status"];
                    if (ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId, true).Count == 0)
                    {
                        base.Response.Write("<p style=\"font-size:16px;\">找不到对应的订单，你付款的订单可能已经被删除</p>");
                    }
                    else if ((base.Request.QueryString["trade_status"] == "TRADE_FINISHED") || (base.Request.QueryString["trade_status"] == "TRADE_SUCCESS"))
                    {
                        this.Amount = decimal.Parse(base.Request.QueryString["total_fee"]);
                        this.UserPayOrder();
                    }
                    else
                    {
                        base.Response.Write(string.Format("<p style=\"font-size:16px;color:#ff0000;\">支付失败，<br><a href=\"{0}\">查看订单</a></p>", "Vshop/MemberOrders.aspx?Status=3"));
                    }
                }
                else
                {
                    base.Response.Write("<p style=\"font-size:16px;\">签名验证失败，可能支付密钥已经被修改</p>");
                }
            }
            else
            {
                base.Response.Write("<p style=\"font-size:16px;\">参数为空，支付异常！</p>");
            }
        }

        private void UserPayOrder()
        {
            if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                base.Response.Write(string.Format("<p style=\"font-size:16px;\">恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}<br><a href=\"{2}\">查看订单</a></p>", this.OrderId, this.Amount.ToString("F"), "/Vshop/MemberOrders.aspx?Status=3"));
            }
        }
    }
}

