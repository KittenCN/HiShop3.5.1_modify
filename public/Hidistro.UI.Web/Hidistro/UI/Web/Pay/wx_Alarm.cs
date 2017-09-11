namespace Hidistro.UI.Web.Pay
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Notify;
    using System;
    using System.Web.UI;

    public class wx_Alarm : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NotifyClient client;
            base.Response.Write("success");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings.EnableSP)
            {
                client = new NotifyClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
            }
            else
            {
                client = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
            }
            AlarmNotify alarmNotify = client.GetAlarmNotify(base.Request.InputStream);
            if (alarmNotify != null)
            {
                AlarmInfo info = new AlarmInfo {
                    AlarmContent = alarmNotify.AlarmContent,
                    AppId = alarmNotify.AppId,
                    Description = alarmNotify.Description
                };
                VShopHelper.SaveAlarm(info);
            }
        }
    }
}

