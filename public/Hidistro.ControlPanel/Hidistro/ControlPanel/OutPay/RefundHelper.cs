namespace Hidistro.ControlPanel.OutPay
{
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hishop.Weixin.Pay;
    using Hishop.Weixin.Pay.Domain;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class RefundHelper
    {
        public static string AlipayRefundRequest(string _notify_url, List<alipayReturnInfo> RefundList)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (!masterSettings.EnableAlipayRequest)
            {
                return "支付宝支付功能未开启，无法完成支付！";
            }
            if (((masterSettings.Alipay_Pid == "") || (masterSettings.Alipay_Key == "")) || ((masterSettings.Alipay_mid == "") || (masterSettings.Alipay_mName == "")))
            {
                return "支付宝参数设置错误，请检查支付宝配置参数！";
            }
            string partner = masterSettings.Alipay_Pid;
            string str2 = masterSettings.Alipay_Key;
            string text1 = masterSettings.Alipay_mid;
            string text2 = masterSettings.Alipay_mName;
            string str3 = "utf-8";
            Core.setConfig(partner, "MD5", str2, str3);
            string str4 = _notify_url;
            string str5 = masterSettings.Alipay_mid;
            string str6 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string str7 = GenerateRefundOrderId();
            string str8 = RefundList.Count.ToString();
            string str9 = "";
            List<string> values = new List<string>();
            foreach (alipayReturnInfo info in RefundList)
            {
                values.Add(info.alipaynum + "^" + info.refundNum.ToString("F2") + "^" + info.Remark);
            }
            str9 = string.Join("#", values);
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", partner);
            sParaTemp.Add("_input_charset", str3);
            sParaTemp.Add("service", "refund_fastpay_by_platform_pwd");
            sParaTemp.Add("notify_url", str4);
            sParaTemp.Add("seller_email", str5);
            sParaTemp.Add("refund_date", str6);
            sParaTemp.Add("batch_no", str7);
            sParaTemp.Add("batch_num", str8);
            sParaTemp.Add("detail_data", str9);
            return Core.BuildRequest(sParaTemp, "get", "确认");
        }

        public static string GenerateRefundOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        public static string SendWxRefundRequest(string out_trade_no, decimal orderTotal, decimal RefundMoney, string RefundOrderId, out string WxRefundNum)
        {
            if (RefundMoney == 0M)
            {
                RefundMoney = orderTotal;
            }
            RefundInfo info = new RefundInfo();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            WxRefundNum = "";
            if (!masterSettings.EnableWeiXinRequest)
            {
                return "微信支付功能未开启";
            }
            info.out_refund_no = RefundOrderId;
            info.out_trade_no = out_trade_no;
            info.RefundFee = new decimal?((int) (RefundMoney * 100M));
            info.TotalFee = new decimal?((int) (orderTotal * 100M));
            PayConfig config = new PayConfig();
            WxRefundNum = "";
            string str = "";
            try
            {
                if (masterSettings.EnableSP)
                {
                    config.AppId = masterSettings.Main_AppId;
                    config.MchID = masterSettings.Main_Mch_ID;
                    config.Key = masterSettings.Main_PayKey;
                    config.sub_appid = masterSettings.WeixinAppId;
                    config.sub_mch_id = masterSettings.WeixinPartnerID;
                }
                else
                {
                    config.AppId = masterSettings.WeixinAppId;
                    config.MchID = masterSettings.WeixinPartnerID;
                    config.Key = masterSettings.WeixinPartnerKey;
                    config.sub_appid = "";
                    config.sub_mch_id = "";
                }
                config.AppSecret = masterSettings.WeixinAppSecret;
                config.SSLCERT_PATH = masterSettings.WeixinCertPath;
                config.SSLCERT_PASSWORD = masterSettings.WeixinCertPassword;
                if (((config.AppId == "") || (config.MchID == "")) || ((config.AppSecret == "") || (config.Key == "")))
                {
                    str = "微信公众号配置参数错误，不能为空！";
                }
                else if ((config.SSLCERT_PATH == "") && (config.SSLCERT_PASSWORD == ""))
                {
                    str = "微信证书以及密码不能为空！解决办法:请到微信-->微信红包-->上传微信证书和填写证书密码。";
                }
                else
                {
                    str = Refund.SendRequest(info, config, out WxRefundNum);
                }
            }
            catch (Exception)
            {
                str = "ERROR";
            }
            if (str.ToUpper() == "SUCCESS")
            {
                str = "";
            }
            return str;
        }
    }
}

