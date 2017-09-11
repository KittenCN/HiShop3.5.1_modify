namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;

    public class PayConfigHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (Globals.GetCurrentManagerUserId() <= 0)
                {
                    context.Response.Write("请先登录");
                    context.Response.End();
                }
                string str = context.Request["type"].ToString();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                switch (str)
                {
                    case "0":
                    {
                        string str2 = context.Request["name"].ToString();
                        string str3 = context.Request["card"].ToString();
                        string str4 = context.Request["bank"].ToString();
                        masterSettings.OfflinePay_BankCard_Name = str2;
                        masterSettings.OfflinePay_BankCard_CardNo = str3;
                        masterSettings.OfflinePay_BankCard_BankName = str4;
                        break;
                    }
                    case "1":
                    {
                        string str5 = context.Request["mid"].ToString();
                        masterSettings.OfflinePay_Alipay_id = str5;
                        break;
                    }
                    case "2":
                    {
                        string str6 = context.Request["content"].ToString();
                        masterSettings.OffLinePayContent = str6;
                        break;
                    }
                    case "3":
                    {
                        string str7 = context.Request["mid"].ToString();
                        string str8 = context.Request["key"].ToString();
                        masterSettings.ShenPay_mid = str7;
                        masterSettings.ShenPay_key = str8;
                        break;
                    }
                    case "4":
                    {
                        string str9 = context.Request["mid"].ToString();
                        string str10 = context.Request["name"].ToString();
                        string str11 = context.Request["pid"].ToString();
                        string str12 = context.Request["key"].ToString();
                        masterSettings.Alipay_mid = str9;
                        masterSettings.Alipay_mName = str10;
                        masterSettings.Alipay_Pid = str11;
                        masterSettings.Alipay_Key = str12;
                        break;
                    }
                    case "5":
                    {
                        string str13 = context.Request["appid"].ToString();
                        string str14 = context.Request["appsecret"].ToString();
                        string str15 = context.Request["mch_id"].ToString();
                        string str16 = context.Request["key"].ToString();
                        masterSettings.WeixinAppId = str13;
                        masterSettings.WeixinAppSecret = str14;
                        masterSettings.WeixinPartnerID = str15;
                        masterSettings.WeixinPartnerKey = str16;
                        break;
                    }
                    case "6":
                    {
                        string str17 = context.Request["mid"].ToString();
                        string str18 = context.Request["md5"].ToString();
                        string str19 = context.Request["des"].ToString();
                        masterSettings.ChinaBank_mid = str17;
                        masterSettings.ChinaBank_MD5 = str18;
                        masterSettings.ChinaBank_DES = str19;
                        break;
                    }
                    case "7":
                    {
                        string str20 = context.Request["key"].ToString();
                        masterSettings.WeixinCertPassword = str20;
                        break;
                    }
                    case "-1":
                    {
                        bool flag = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.EnablePodRequest = flag;
                        break;
                    }
                    case "-2":
                    {
                        bool flag2 = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.EnableOffLineRequest = flag2;
                        break;
                    }
                    case "-3":
                    {
                        bool flag3 = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.EnableWapShengPay = flag3;
                        break;
                    }
                    case "-4":
                    {
                        bool flag4 = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.EnableAlipayRequest = flag4;
                        break;
                    }
                    case "-5":
                    {
                        bool flag5 = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.EnableWeiXinRequest = flag5;
                        break;
                    }
                    case "-6":
                    {
                        bool flag6 = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.ChinaBank_Enable = flag6;
                        break;
                    }
                    case "-7":
                    {
                        bool flag7 = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.EnableWeixinRed = flag7;
                        break;
                    }
                }
                SettingsManager.Save(masterSettings);
                context.Response.Write("保存成功");
            }
            catch (Exception exception)
            {
                context.Response.Write("保存失败！（" + exception.Message + ")");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

