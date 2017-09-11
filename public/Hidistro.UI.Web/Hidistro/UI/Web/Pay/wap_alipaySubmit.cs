namespace Hidistro.UI.Web.Pay
{
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class wap_alipaySubmit : Page
    {
        protected HtmlGenericControl infos;

        private void DoOneTaoPay(string Pid)
        {
            if (string.IsNullOrEmpty(Pid))
            {
                base.Response.Write("支付参数不正确！<a href='javascript:history.go(-1);'>返回上一页</a>");
                base.Response.End();
            }
            OneyuanTaoParticipantInfo info = OneyuanTaoHelp.GetAddParticipant(0, Pid, "");
            if (info == null)
            {
                base.Response.Write("您的夺宝信息已被删除！<a href='javascript:history.go(-1);'>返回上一页</a>");
                base.Response.End();
            }
            OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(info.ActivityId);
            if (oneyuanTaoInfoById == null)
            {
                base.Response.Write("夺宝活动已被删除，你无法完成支付！<a href='javascript:history.go(-1);'>返回上一页</a>");
                base.Response.End();
            }
            OneTaoState state = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
            if (state != OneTaoState.进行中)
            {
                base.Response.Write("当前活动" + state.ToString() + "，无法支付！<a href='javascript:history.go(-1);'>返回上一页</a>");
                base.Response.End();
            }
            if ((oneyuanTaoInfoById.FinishedNum + info.BuyNum) > oneyuanTaoInfoById.ReachNum)
            {
                base.Response.Write("活动已满员，您可以试下其它活动！<a href='/Vshop/OneyuanList.aspx'>夺宝活动中心</a>");
                base.Response.End();
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string partner = masterSettings.Alipay_Pid;
            string str3 = masterSettings.Alipay_Key;
            string str4 = "utf-8";
            Core.setConfig(partner, "MD5", str3, str4);
            string str5 = "1";
            string str6 = Globals.FullPath("/Vshop/OneTaoPaySuccess.aspx");
            string str7 = Globals.FullPath("/Pay/wap_alipay_notify_url.aspx");
            string str8 = Pid;
            string str9 = "订单支付";
            decimal totalPrice = info.TotalPrice;
            string str10 = Globals.FullPath("/Vshop/OneTaoPayView.aspx?Pid=") + Pid;
            string str11 = SettingsManager.GetMasterSettings(false).SiteName + "支付，当前支付编号-" + Pid + " ...";
            string str12 = "1m";
            string str13 = "";
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", partner);
            sParaTemp.Add("seller_id", partner);
            sParaTemp.Add("_input_charset", str4);
            sParaTemp.Add("service", "alipay.wap.create.direct.pay.by.user");
            sParaTemp.Add("payment_type", str5);
            sParaTemp.Add("notify_url", str7);
            sParaTemp.Add("return_url", str6);
            sParaTemp.Add("out_trade_no", str8);
            sParaTemp.Add("subject", str9);
            sParaTemp.Add("total_fee", totalPrice.ToString("F"));
            sParaTemp.Add("show_url", str10);
            sParaTemp.Add("body", str11);
            sParaTemp.Add("it_b_pay", str12);
            sParaTemp.Add("extern_token", str13);
            string s = Core.BuildRequest(sParaTemp, "get", "确认");
            base.Response.Write(s);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString.Get("orderId");
            string str2 = base.Request.QueryString.Get("Ptype");
            if (string.IsNullOrEmpty(str))
            {
                this.infos.InnerText = "订单号为空，请返回";
            }
            else
            {
                if (!string.IsNullOrEmpty(str2) && (str2 == "OneTao"))
                {
                    this.DoOneTaoPay(str);
                    base.Response.End();
                }
                List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(str, true);
                if (orderMarkingOrderInfo.Count == 0)
                {
                    this.infos.InnerText = "订单信息未找到！";
                }
                else
                {
                    decimal num = 0M;
                    foreach (OrderInfo info in orderMarkingOrderInfo)
                    {
                        if (info.BargainDetialId > 0)
                        {
                            string str3 = BargainHelper.IsCanBuyByBarginDetailId(info.BargainDetialId);
                            if (str3 != "1")
                            {
                                info.OrderStatus = OrderStatus.Closed;
                                info.CloseReason = str3;
                                OrderHelper.UpdateOrder(info);
                                base.Response.Write("<script>alert('" + str3 + "，订单自动关闭！');location.href='/Vshop/MemberOrders.aspx'</script>");
                                base.Response.End();
                                return;
                            }
                        }
                        else
                        {
                            foreach (LineItemInfo info2 in info.LineItems.Values)
                            {
                                if (!ProductHelper.GetProductHasSku(info2.SkuId, info2.Quantity))
                                {
                                    info.OrderStatus = OrderStatus.Closed;
                                    info.CloseReason = "库存不足";
                                    OrderHelper.UpdateOrder(info);
                                    base.Response.Write("<script>alert('库存不足，订单自动关闭！');location.href='/Vshop/MemberOrders.aspx'</script>");
                                    base.Response.End();
                                    return;
                                }
                            }
                        }
                        num += info.GetCashPayMoney();
                    }
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string partner = masterSettings.Alipay_Pid;
                    string str5 = masterSettings.Alipay_Key;
                    string str6 = "utf-8";
                    Core.setConfig(partner, "MD5", str5, str6);
                    string str7 = "1";
                    string str8 = Globals.FullPath("/Pay/wap_alipay_return_url.aspx");
                    string str9 = Globals.FullPath("/Pay/wap_alipay_notify_url.aspx");
                    string str10 = str;
                    string str11 = "订单支付";
                    decimal num2 = num;
                    string str12 = Globals.FullPath("/Vshop/MemberOrderDetails.aspx?orderId=") + orderMarkingOrderInfo[0].OrderId;
                    string str13 = "订单号-" + orderMarkingOrderInfo[0].OrderId + " ...";
                    string str14 = "1m";
                    string str15 = "";
                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("partner", partner);
                    sParaTemp.Add("seller_id", partner);
                    sParaTemp.Add("_input_charset", str6);
                    sParaTemp.Add("service", "alipay.wap.create.direct.pay.by.user");
                    sParaTemp.Add("payment_type", str7);
                    sParaTemp.Add("notify_url", str9);
                    sParaTemp.Add("return_url", str8);
                    sParaTemp.Add("out_trade_no", str10);
                    sParaTemp.Add("subject", str11);
                    sParaTemp.Add("total_fee", num2.ToString("F"));
                    sParaTemp.Add("show_url", str12);
                    sParaTemp.Add("body", str13);
                    sParaTemp.Add("it_b_pay", str14);
                    sParaTemp.Add("extern_token", str15);
                    string s = Core.BuildRequest(sParaTemp, "get", "确认");
                    base.Response.Write(s);
                }
            }
        }
    }
}

