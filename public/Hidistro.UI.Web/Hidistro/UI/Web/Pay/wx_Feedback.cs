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

    public class wx_Feedback : Page
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
            FeedBackNotify feedBackNotify = client.GetFeedBackNotify(base.Request.InputStream);
            if (feedBackNotify != null)
            {
                string msgType = feedBackNotify.MsgType;
                if (msgType != null)
                {
                    if (!(msgType == "request"))
                    {
                        if (msgType == "confirm")
                        {
                            feedBackNotify.MsgType = "已完成";
                        }
                    }
                    else
                    {
                        feedBackNotify.MsgType = "未处理";
                    }
                }
                if (VShopHelper.GetFeedBack(feedBackNotify.FeedBackId) != null)
                {
                    VShopHelper.UpdateFeedBackMsgType(feedBackNotify.FeedBackId, feedBackNotify.MsgType);
                }
                else
                {
                    FeedBackInfo info = new FeedBackInfo {
                        AppId = feedBackNotify.AppId,
                        ExtInfo = feedBackNotify.ExtInfo,
                        FeedBackId = feedBackNotify.FeedBackId,
                        MsgType = feedBackNotify.MsgType,
                        OpenId = feedBackNotify.OpenId,
                        Reason = feedBackNotify.Reason,
                        Solution = feedBackNotify.Solution,
                        TransId = feedBackNotify.TransId
                    };
                    VShopHelper.SaveFeedBack(info);
                }
            }
        }
    }
}

