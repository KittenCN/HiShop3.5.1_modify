namespace Hidistro.UI.Web.Pay
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;

    public class wx_Submit : Page
    {
        public string CheckValue = "";
        public string pay_json = string.Empty;
        public int shareid;

        public string ConvertPayJson(PayRequestInfo req)
        {
            string str = "{";
            return (((((((str + "\"appId\":\"" + req.appId + "\",") + "\"timeStamp\":\"" + req.timeStamp + "\",") + "\"nonceStr\":\"" + req.nonceStr + "\",") + "\"package\":\"" + req.package + "\",") + "\"signType\":\"" + req.signType + "\",") + "\"paySign\":\"" + req.paySign + "\"") + "}");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString.Get("orderId");
            if (!string.IsNullOrEmpty(str))
            {
                List<OrderInfo> orderMarkingOrderInfo = ShoppingProcessor.GetOrderMarkingOrderInfo(str, true);
                if (orderMarkingOrderInfo.Count != 0)
                {
                    PayClient client;
                    decimal num = 0M;
                    if (orderMarkingOrderInfo[0].UserId != 0)
                    {
                        foreach (OrderInfo info in orderMarkingOrderInfo)
                        {
                            if (info.BargainDetialId > 0)
                            {
                                string str2 = BargainHelper.IsCanBuyByBarginDetailId(info.BargainDetialId);
                                if (str2 != "1")
                                {
                                    info.OrderStatus = OrderStatus.Closed;
                                    info.CloseReason = str2;
                                    OrderHelper.UpdateOrder(info);
                                    base.Response.Write("<script>alert('" + str2 + "，订单自动关闭！');location.href='/Vshop/MemberOrders.aspx'</script>");
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
                    }
                    else
                    {
                        num = orderMarkingOrderInfo[0].Amount;
                    }
                    PackageInfo package = new PackageInfo {
                        Body = str,
                        NotifyUrl = string.Format("http://{0}/pay/wx_Pay.aspx", base.Request.Url.Host),
                        OutTradeNo = str,
                        TotalFee = (int) (num * 100M)
                    };
                    if (package.TotalFee < 1M)
                    {
                        package.TotalFee = 1M;
                    }

                  
                    string openId = "";
                    MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                    if (currentMember != null)
                    {
                        openId = currentMember.OpenId;
                    }
                    package.OpenId = openId;
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                    if (masterSettings.EnableSP)
                    {
                        client = new PayClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
                    }
                    else
                    {
                        client = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
                    }
                    if (client.checkSetParams(out this.CheckValue) && client.checkPackage(package, out this.CheckValue))
                    {
                        PayRequestInfo req = client.BuildPayRequest(package);
                        this.pay_json = this.ConvertPayJson(req);
                        if (!req.package.ToLower().StartsWith("prepay_id=wx"))
                        {
                            this.CheckValue = req.package;
                        }
                        DataTable shareActivity = ShareActHelper.GetShareActivity();
                        int num2 = 0;
                        decimal num3 = 0M;
                        if (shareActivity.Rows.Count > 0)
                        {
                            for (int i = 0; i < shareActivity.Rows.Count; i++)
                            {
                                if ((num != 0M) && (num >= decimal.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["MeetValue"].ToString())))
                                {
                                    num2 = int.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["Id"].ToString());
                                    num3 = decimal.Parse(shareActivity.Rows[shareActivity.Rows.Count - 1]["MeetValue"].ToString());
                                    break;
                                }
                                if ((num != 0M) && (num <= decimal.Parse(shareActivity.Rows[0]["MeetValue"].ToString())))
                                {
                                    num2 = int.Parse(shareActivity.Rows[0]["Id"].ToString());
                                    num3 = decimal.Parse(shareActivity.Rows[0]["MeetValue"].ToString());
                                    break;
                                }
                                if ((num != 0M) && (num >= decimal.Parse(shareActivity.Rows[i]["MeetValue"].ToString())))
                                {
                                    num2 = int.Parse(shareActivity.Rows[i]["Id"].ToString());
                                    num3 = decimal.Parse(shareActivity.Rows[i]["MeetValue"].ToString());
                                }
                            }
                            if (num >= num3)
                            {
                                this.shareid = num2;
                            }
                        }
                    }
                }
            }
        }
    }
}

