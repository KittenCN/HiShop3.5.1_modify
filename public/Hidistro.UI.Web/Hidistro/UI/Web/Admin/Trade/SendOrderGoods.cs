namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.Vshop;
    using Hishop.Plugins;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditOrders)]
    public class SendOrderGoods : AdminPage
    {
        protected Literal litOrdersCount;
        protected string orderIds;
        protected string ReUrl;
        protected Repeater rptItemList;
        protected string type;

        protected SendOrderGoods() : base("m03", "00000")
        {
            this.orderIds = string.Empty;
            this.ReUrl = Globals.RequestQueryStr("reurl");
            this.type = "sendorders";
        }

        public bool CheckOrderCompany(string oid, string companycode, string companyname, string shipNumber)
        {
            bool flag = true;
            return (((!string.IsNullOrEmpty(oid) && !string.IsNullOrEmpty(companycode)) && (!string.IsNullOrEmpty(companyname) && !string.IsNullOrEmpty(shipNumber))) && flag);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str2;
            string str3;
            JArray array;
            string str4;
            if (Globals.RequestQueryStr("type") == "saveorders")
            {
                this.type = "saveorders";
            }
            string str = Globals.RequestFormStr("posttype");
            this.orderIds = Globals.RequestQueryStr("OrderId").Trim(new char[] { ',' });
            if (string.IsNullOrEmpty(this.ReUrl))
            {
                this.ReUrl = "manageorder.aspx";
            }
            switch (str)
            {
                case "saveorders":
                    str2 = Globals.RequestFormStr("data");
                    base.Response.ContentType = "application/json";
                    str3 = "{\"type\":\"0\",\"tips\":\"指定物流失败！\"}";
                    array = (JArray) JsonConvert.DeserializeObject(str2);
                    str4 = string.Empty;
                    if (array != null)
                    {
                        if (array.Count > 1)
                        {
                            str4 = "批量";
                        }
                        bool flag = true;
                        foreach (JObject obj2 in array)
                        {
                            if (!this.CheckOrderCompany(obj2["orderid"].ToString(), obj2["companycode"].ToString(), obj2["compname"].ToString(), obj2["shipordernumber"].ToString()))
                            {
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            foreach (JObject obj3 in array)
                            {
                                OrderHelper.UpdateOrderCompany(obj3["orderid"].ToString(), obj3["companycode"].ToString(), obj3["compname"].ToString(), obj3["shipordernumber"].ToString());
                            }
                            str3 = "{\"type\":\"1\",\"tips\":\"" + str4 + "指定物流成功！\"}";
                        }
                        else
                        {
                            str3 = "{\"type\":\"0\",\"tips\":\"" + str4 + "指定物流失败，请检测数据的正确性！\"}";
                        }
                    }
                    base.Response.Write(str3);
                    base.Response.End();
                    return;

                case "saveoneorders":
                    str2 = Globals.RequestFormStr("data");
                    base.Response.ContentType = "application/json";
                    str3 = "{\"type\":\"0\",\"tips\":\"指定物流失败！\"}";
                    array = (JArray) JsonConvert.DeserializeObject(str2);
                    str4 = string.Empty;
                    if (array != null)
                    {
                        bool flag2 = true;
                        string shipNumber = "1111111111";
                        foreach (JObject obj4 in array)
                        {
                            if (!this.CheckOrderCompany(obj4["orderid"].ToString(), obj4["companycode"].ToString(), obj4["compname"].ToString(), shipNumber))
                            {
                                flag2 = false;
                            }
                        }
                        if (flag2)
                        {
                            foreach (JObject obj5 in array)
                            {
                                OrderHelper.UpdateOrderCompany(obj5["orderid"].ToString(), obj5["companycode"].ToString(), obj5["compname"].ToString(), "");
                            }
                            str3 = "{\"type\":\"1\",\"tips\":\"" + str4 + "指定物流成功！\"}";
                        }
                        else
                        {
                            str3 = "{\"type\":\"0\",\"tips\":\"" + str4 + "指定物流失败，请检测数据的正确性！\"}";
                        }
                    }
                    base.Response.Write(str3);
                    base.Response.End();
                    return;

                case "sendorders":
                    str2 = Globals.RequestFormStr("data");
                    base.Response.ContentType = "application/json";
                    str3 = "{\"type\":\"0\",\"tips\":\"发货失败！\"}";
                    array = (JArray) JsonConvert.DeserializeObject(str2);
                    str4 = string.Empty;
                    if (array != null)
                    {
                        if (array.Count > 1)
                        {
                            str4 = "批量";
                        }
                        bool flag3 = true;
                        foreach (JObject obj6 in array)
                        {
                            if (!this.CheckOrderCompany(obj6["orderid"].ToString(), obj6["companycode"].ToString(), obj6["compname"].ToString(), obj6["shipordernumber"].ToString()))
                            {
                                flag3 = false;
                            }
                        }
                        if (flag3)
                        {
                            int num = 0;
                            foreach (JObject obj7 in array)
                            {
                                OrderInfo orderInfo = OrderHelper.GetOrderInfo(obj7["orderid"].ToString());
                                if ((((orderInfo.GroupBuyId <= 0) || (orderInfo.GroupBuyStatus == GroupBuyStatus.Success)) && (((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay) && (orderInfo.Gateway == "hishop.plugins.payment.podrequest")) || (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid))) && (!string.IsNullOrEmpty(obj7["shipordernumber"].ToString().Trim()) && (obj7["shipordernumber"].ToString().Trim().Length <= 30)))
                                {
                                    orderInfo.ExpressCompanyAbb = obj7["companycode"].ToString();
                                    orderInfo.ExpressCompanyName = obj7["compname"].ToString();
                                    orderInfo.ShipOrderNumber = obj7["shipordernumber"].ToString();
                                    if (OrderHelper.SendGoods(orderInfo))
                                    {
                                        SendNoteInfo info2 = new SendNoteInfo() ;
                                        Express.SubscribeExpress100(obj7["companycode"].ToString(), obj7["shipordernumber"].ToString());
                                        info2 = new SendNoteInfo {
                                            NoteId = Globals.GetGenerateId() + num,
                                            OrderId = obj7["orderid"].ToString(),
                                            Operator = ManagerHelper.GetCurrentManager().UserName,
                                            Remark = "后台" + info2.Operator + "发货成功"
                                        };
                                        OrderHelper.SaveSendNote(info2);
                                        if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && (orderInfo.GatewayOrderId.Trim().Length > 0))
                                        {
                                            if (orderInfo.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
                                            {
                                                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                                                if (paymentMode != null)
                                                {
                                                    PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                                                }
                                            }
                                            if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
                                            {
                                                PayClient client;
                                                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                                                if (masterSettings.EnableSP)
                                                {
                                                    client = new PayClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
                                                }
                                                else
                                                {
                                                    client = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
                                                }
                                                DeliverInfo deliver = new DeliverInfo {
                                                    TransId = orderInfo.GatewayOrderId,
                                                    OutTradeNo = orderInfo.OrderId,
                                                    OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
                                                };
                                                client.DeliverNotify(deliver);
                                            }
                                        }
                                        orderInfo.OnDeliver();
                                        num++;
                                    }
                                }
                            }
                            if (num == 0)
                            {
                                str3 = "{\"type\":\"0\",\"tips\":\"" + str4 + "发货失败！\"}";
                            }
                            else
                            {
                                str3 = "{\"type\":\"1\",\"tips\":\"" + num + "个订单发货成功！\"}";
                            }
                        }
                        else
                        {
                            str3 = "{\"type\":\"0\",\"tips\":\"" + str4 + "发货失败，请检测数据的正确性！\"}";
                        }
                    }
                    base.Response.Write(str3);
                    base.Response.End();
                    return;

                case "getcompany":
                {
                    base.Response.ContentType = "application/json";
                    str2 = "[{\"type\":\"0\",\"data\":[]}]";
                    IList<ExpressCompanyInfo> allExpress = ExpressHelper.GetAllExpress();
                    int num2 = 0;
                    StringBuilder builder = new StringBuilder();
                    foreach (ExpressCompanyInfo info5 in allExpress)
                    {
                        if (num2 == 0)
                        {
                            builder.Append("{\"code\":\"" + String2Json(info5.Kuaidi100Code) + "\",\"name\":\"" + String2Json(info5.Name) + "\"}");
                        }
                        else
                        {
                            builder.Append(",{\"code\":\"" + String2Json(info5.Kuaidi100Code) + "\",\"name\":\"" + String2Json(info5.Name) + "\"}");
                        }
                        num2++;
                    }
                    if (!string.IsNullOrEmpty(builder.ToString()))
                    {
                        str2 = "[{\"type\":\"1\",\"data\":[" + builder.ToString() + "]}]";
                    }
                    base.Response.Write(str2);
                    base.Response.End();
                    return;
                }
                case "updateExpress":
                {
                    str2 = Globals.RequestFormStr("data");
                    base.Response.ContentType = "application/json";
                    str3 = "{\"type\":\"0\",\"tips\":\"修改失败！\"}";
                    array = (JArray) JsonConvert.DeserializeObject(str2);
                    bool flag4 = true;
                    foreach (JObject obj8 in array)
                    {
                        if (!this.CheckOrderCompany(obj8["orderid"].ToString(), obj8["companycode"].ToString(), obj8["compname"].ToString(), obj8["shipordernumber"].ToString()))
                        {
                            flag4 = false;
                        }
                    }
                    if (flag4)
                    {
                        bool flag5 = false;
                        foreach (JObject obj9 in array)
                        {
                            OrderInfo order = OrderHelper.GetOrderInfo(obj9["orderid"].ToString());
                            order.ExpressCompanyAbb = obj9["companycode"].ToString();
                            order.ExpressCompanyName = obj9["compname"].ToString();
                            order.ShipOrderNumber = obj9["shipordernumber"].ToString();
                            flag5 = OrderHelper.UpdateOrder(order);
                            if (flag5)
                            {
                                Express.SubscribeExpress100(obj9["companycode"].ToString(), obj9["shipordernumber"].ToString());
                            }
                        }
                        if (flag5)
                        {
                            str3 = "{\"type\":\"1\",\"tips\":\"修改成功！\"}";
                        }
                    }
                    else
                    {
                        str3 = "{\"type\":\"0\",\"tips\":\"数据验证失败！\"}";
                    }
                    base.Response.Write(str3);
                    base.Response.End();
                    return;
                }
            }
            if (string.IsNullOrEmpty(this.orderIds))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                string[] strArray = this.orderIds.Split(new char[] { ',' });
                bool flag6 = true;
                foreach (string str6 in strArray)
                {
                    if (!Globals.IsOrdersID(str6))
                    {
                        flag6 = false;
                        break;
                    }
                }
                if (flag6)
                {
                    DataSet ordersByOrderIDList = OrderHelper.GetOrdersByOrderIDList(this.orderIds);
                    this.rptItemList.DataSource = ordersByOrderIDList;
                    this.rptItemList.DataBind();
                    this.litOrdersCount.Text = ordersByOrderIDList.Tables[0].Rows.Count.ToString();
                }
                else
                {
                    base.Response.Write("非法参数请求！");
                    base.Response.End();
                }
            }
        }

        private static string String2Json(string s)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s.ToCharArray()[i];
                switch (ch)
                {
                    case '/':
                    {
                        builder.Append(@"\/");
                        continue;
                    }
                    case '\\':
                    {
                        builder.Append(@"\\");
                        continue;
                    }
                    case '\b':
                    {
                        builder.Append(@"\b");
                        continue;
                    }
                    case '\t':
                    {
                        builder.Append(@"\t");
                        continue;
                    }
                    case '\n':
                    {
                        builder.Append(@"\n");
                        continue;
                    }
                    case '\f':
                    {
                        builder.Append(@"\f");
                        continue;
                    }
                    case '\r':
                    {
                        builder.Append(@"\r");
                        continue;
                    }
                    case '"':
                    {
                        builder.Append("\\\"");
                        continue;
                    }
                }
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}

