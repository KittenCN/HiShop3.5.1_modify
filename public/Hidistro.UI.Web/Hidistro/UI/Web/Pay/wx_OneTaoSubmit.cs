namespace Hidistro.UI.Web.Pay
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Web.UI;

    public class wx_OneTaoSubmit : Page
    {
        public string CheckValue = "";
        public string pay_json = "{\"msg\":\"非正常访问!\"}";
        public int shareid;

        public string ConvertPayJson(PayRequestInfo req)
        {
            string str = "{";
            return (((((((str + "\"appId\":\"" + req.appId + "\",") + "\"timeStamp\":\"" + req.timeStamp + "\",") + "\"nonceStr\":\"" + req.nonceStr + "\",") + "\"package\":\"" + req.package + "\",") + "\"signType\":\"" + req.signType + "\",") + "\"paySign\":\"" + req.paySign + "\"") + "}");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString.Get("orderId");
            if (string.IsNullOrEmpty(str))
            {
                this.pay_json = "{\"msg\":\"订单参数错误!\"}";
            }
            else
            {
                OneyuanTaoParticipantInfo info = OneyuanTaoHelp.GetAddParticipant(0, str, "");
                if (info == null)
                {
                    this.pay_json = "{\"msg\":\"订单不存在了!\"}";
                }
                else
                {
                    OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(info.ActivityId);
                    if (oneyuanTaoInfoById == null)
                    {
                        this.pay_json = "{\"msg\":\"活动已取消，禁止参与!\"}";
                    }
                    else
                    {
                        OneTaoState state = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
                        if (state != OneTaoState.进行中)
                        {
                            this.pay_json = "{\"msg\":\"您来晚了，活动已结束!\"}";
                        }
                        else if (state != OneTaoState.进行中)
                        {
                            this.pay_json = "{\"msg\":\"您来晚了，活动已结束!\"}";
                        }
                        else if ((oneyuanTaoInfoById.ReachType == 1) && ((oneyuanTaoInfoById.FinishedNum + info.BuyNum) > oneyuanTaoInfoById.ReachNum))
                        {
                            this.pay_json = "{\"msg\":\"活动已满员，您来晚了!\"}";
                        }
                        else
                        {
                            PayClient client;
                            decimal totalPrice = info.TotalPrice;
                            PackageInfo package = new PackageInfo {
                                Body = Globals.SubStr(oneyuanTaoInfoById.ProductTitle, 0x24, "...") + "。活动编号：" + info.ActivityId,
                                NotifyUrl = string.Format("http://{0}/pay/wx_Pay.aspx", base.Request.Url.Host),
                                OutTradeNo = str,
                                TotalFee = (int) (totalPrice * 100M)
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
                            else
                            {
                                this.pay_json = "{\"msg\":\"用户OPENID不存在，无法支付!\"}";
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
                            }
                        }
                    }
                }
            }
        }
    }
}

