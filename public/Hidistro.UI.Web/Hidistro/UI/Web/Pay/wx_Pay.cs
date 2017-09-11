namespace Hidistro.UI.Web.Pay
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using Hishop.Weixin.Pay.Notify;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    public class wx_Pay : Page
    {
        protected string OrderId;
        protected List<OrderInfo> orderlist;

        private void DoOneTao(string Pid, PayInfo PayInfo)
        {
            OneyuanTaoParticipantInfo info = OneyuanTaoHelp.GetAddParticipant(0, Pid, "");
            if (info == null)
            {
                base.Response.Write("success");
            }
            else
            {
                info.PayTime = new DateTime?(DateTime.Now);
                info.PayWay = "weixin";
                info.PayNum = Pid;
                info.Remark = "订单已支付：支付金额为￥" + PayInfo.TotalFee.ToString();
                if (!info.IsPay && OneyuanTaoHelp.SetPayinfo(info))
                {
                    OneyuanTaoHelp.SetOneyuanTaoFinishedNum(info.ActivityId, 0);
                }
                else
                {
                    Globals.Debuglog(JsonConvert.SerializeObject(PayInfo), "_Debuglog.txt");
                }
                base.Response.Write("success");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            NotifyClient client;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings.EnableSP)
            {
                client = new NotifyClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
            }
            else
            {
                client = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
            }
            PayNotify payNotify = client.GetPayNotify(base.Request.InputStream);
            if (payNotify != null)
            {
                this.OrderId = payNotify.PayInfo.OutTradeNo;
                if (this.OrderId.StartsWith("B"))
                {
                    this.DoOneTao(this.OrderId, payNotify.PayInfo);
                    base.Response.End();
                }
                this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId, true);
                if (this.orderlist.Count == 0)
                {
                    base.Response.Write("success");
                }
                else
                {
                    foreach (OrderInfo info in this.orderlist)
                    {
                        info.GatewayOrderId = payNotify.PayInfo.TransactionId;
                    }
                    this.UserPayOrder();
                }
            }
        }

        private void UserPayOrder()
        {
            foreach (OrderInfo info in this.orderlist)
            {
                if (info.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                {
                    base.Response.Write("success");
                    return;
                }
            }
            foreach (OrderInfo info2 in this.orderlist)
            {
                if (info2.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(info2))
                {
                    info2.OnPayment();
                    base.Response.Write("success");
                }
            }
        }
    }
}

