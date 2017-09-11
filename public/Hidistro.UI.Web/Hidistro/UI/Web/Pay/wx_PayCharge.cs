namespace Hidistro.UI.Web.Pay
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Notify;
    using System;
    using System.Web.UI;

    public class wx_PayCharge : Page
    {
        protected MemberAmountDetailedInfo model;
        protected string PayId;

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
                this.PayId = payNotify.PayInfo.OutTradeNo;
                this.model = MemberAmountProcessor.GetAmountDetailByPayId(this.PayId);
                if (this.model == null)
                {
                    base.Response.Write("success");
                }
                else
                {
                    this.model.GatewayPayId = payNotify.PayInfo.TransactionId;
                    this.UserPayOrder();
                }
            }
        }

        private void UserPayOrder()
        {
            if (this.model.State == 1)
            {
                base.Response.Write("success");
            }
            else if ((this.model.TradeType == TradeType.Recharge) && MemberAmountProcessor.UserPayOrder(this.model))
            {
                base.Response.Write("success");
            }
        }
    }
}

