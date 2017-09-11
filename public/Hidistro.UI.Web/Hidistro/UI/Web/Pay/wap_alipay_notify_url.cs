namespace Hidistro.UI.Web.Pay
{
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    public class wap_alipay_notify_url : Page
    {
        protected string OrderId;
        protected List<OrderInfo> orderlist;

        private void DoOneTao(string out_trade_no, string trade_status, string trade_no, SortedDictionary<string, string> sPara)
        {
            if ((trade_status == "TRADE_SUCCESS") || (trade_status == "TRADE_FINISHED"))
            {
                OneyuanTaoParticipantInfo info = OneyuanTaoHelp.GetAddParticipant(0, out_trade_no, "");
                if (info == null)
                {
                    base.Response.Write("success");
                    Globals.Debuglog(base.Request.Form.ToString(), "_Debuglog.txt");
                    return;
                }
                info.PayTime = new DateTime?(DateTime.Now);
                info.PayWay = "alipay";
                info.PayNum = trade_no;
                info.Remark = "订单已支付：支付金额为￥" + sPara["total_fee"];
                if (!info.IsPay && OneyuanTaoHelp.SetPayinfo(info))
                {
                    OneyuanTaoHelp.SetOneyuanTaoFinishedNum(info.ActivityId, 0);
                }
                else
                {
                    Globals.Debuglog(base.Request.Form.ToString(), "_Debuglog.txt");
                }
            }
            else
            {
                Globals.Debuglog(base.Request.Form.ToString(), "_Debuglog.txt");
            }
            base.Response.Write("success");
        }

        private SortedDictionary<string, string> GetRequestPost()
        {
            int index = 0;
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            string[] allKeys = base.Request.Form.AllKeys;
            for (index = 0; index < allKeys.Length; index++)
            {
                dictionary.Add(allKeys[index], base.Request.Form[allKeys[index]]);
            }
            return dictionary;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> requestPost = this.GetRequestPost();
            if (requestPost.Count > 0)
            {
                Notify notify = new Notify();
                if (notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]))
                {
                    string str = base.Request.Form["out_trade_no"];
                    string str2 = base.Request.Form["trade_no"];
                    string str3 = base.Request.Form["trade_status"];
                    if (!string.IsNullOrEmpty(str) && str.StartsWith("B"))
                    {
                        this.DoOneTao(str, str3, str2, requestPost);
                        base.Response.End();
                    }
                    if ((str3 == "TRADE_SUCCESS") || (str3 == "TRADE_FINISHED"))
                    {
                        this.OrderId = str;
                        this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId, true);
                        if (this.orderlist.Count == 0)
                        {
                            base.Response.Write("success");
                            return;
                        }
                        foreach (OrderInfo info in this.orderlist)
                        {
                            info.GatewayOrderId = str2;
                        }
                        foreach (OrderInfo info2 in this.orderlist)
                        {
                            if (info2.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                            {
                                base.Response.Write("success");
                                return;
                            }
                        }
                        foreach (OrderInfo info3 in this.orderlist)
                        {
                            if (info3.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(info3))
                            {
                                info3.OnPayment();
                                base.Response.Write("success");
                            }
                        }
                    }
                    base.Response.Write("success");
                }
                else
                {
                    base.Response.Write("fail");
                }
            }
            else
            {
                base.Response.Write("无通知参数");
            }
        }
    }
}

