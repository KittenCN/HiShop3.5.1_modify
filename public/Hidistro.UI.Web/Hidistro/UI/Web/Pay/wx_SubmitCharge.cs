namespace Hidistro.UI.Web.Pay
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Web.UI;

    public class wx_SubmitCharge : Page
    {
        public string CheckValue = "";
        public string pay_json = string.Empty;

        public string ConvertPayJson(PayRequestInfo req)
        {
            string str = "{";
            return (((((((str + "\"appId\":\"" + req.appId + "\",") + "\"timeStamp\":\"" + req.timeStamp + "\",") + "\"nonceStr\":\"" + req.nonceStr + "\",") + "\"package\":\"" + req.package + "\",") + "\"signType\":\"" + req.signType + "\",") + "\"paySign\":\"" + req.paySign + "\"") + "}");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString.Get("PayId");
            if (!string.IsNullOrEmpty(str))
            {
                MemberAmountDetailedInfo amountDetailByPayId = MemberAmountProcessor.GetAmountDetailByPayId(str);
                if (amountDetailByPayId != null)
                {
                    PayClient client;
                    decimal tradeAmount = amountDetailByPayId.TradeAmount;
                    PackageInfo package = new PackageInfo {
                        Body = str,
                        NotifyUrl = string.Format("http://{0}/pay/wx_PayCharge.aspx", base.Request.Url.Host),
                        OutTradeNo = str,
                        TotalFee = (int) (tradeAmount * 100M)
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
                    }
                }
            }
        }
    }
}

