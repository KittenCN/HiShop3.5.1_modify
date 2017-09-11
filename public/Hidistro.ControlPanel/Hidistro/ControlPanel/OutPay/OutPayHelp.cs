namespace Hidistro.ControlPanel.OutPay
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.OutPay;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;

    public class OutPayHelp
    {
        public static string BatchWeixinPayCheckRealName = "";
        public static char[] Chars = new char[] { 
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'R', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
            'w', 'x', 'y', 'z'
         };
        private static string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";
        private static bool IsReadSeting = false;
        private static string WeiPayUrl = string.Format("https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers", new object[0]);
        private static string WeiXinAppid = "";
        private static string WeixinCertPassword = "";
        private static string WeiXinCertPath = "";
        private static string WeiXinKey = "";
        private static string WeiXinMchid = "";

        public static List<WeiPayResult> BatchWeiPay(List<OutPayWeiInfo> BatchUserList)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            WeiXinMchid = masterSettings.WeixinPartnerID;
            WeiXinAppid = masterSettings.WeixinAppId;
            WeiXinKey = masterSettings.WeixinPartnerKey;
            BatchWeixinPayCheckRealName = masterSettings.BatchWeixinPayCheckRealName.ToString();
            WeiXinCertPath = masterSettings.WeixinCertPath;
            WeixinCertPassword = masterSettings.WeixinCertPassword;
            string batchWeixinPayCheckRealName = BatchWeixinPayCheckRealName;
            if (batchWeixinPayCheckRealName != null)
            {
                if (!(batchWeixinPayCheckRealName == "0"))
                {
                    if (batchWeixinPayCheckRealName == "1")
                    {
                        BatchWeixinPayCheckRealName = "FORCE_CHECK";
                    }
                    else if (batchWeixinPayCheckRealName == "2")
                    {
                        BatchWeixinPayCheckRealName = "OPTION_CHECK";
                    }
                }
                else
                {
                    BatchWeixinPayCheckRealName = "NO_CHECK";
                }
            }
            List<WeiPayResult> list = new List<WeiPayResult>();
            WeiPayResult item = new WeiPayResult {
                return_code = "SUCCESS",
                err_code = "",
                return_msg = "微信企业付款参数配置错误"
            };
            if (WeiXinMchid == "")
            {
                item.return_code = "FAIL";
                item.return_msg = "商户号未配置！";
            }
            else if (WeiXinAppid == "")
            {
                item.return_code = "FAIL";
                item.return_msg = "公众号APPID未配置！";
            }
            else if (WeiXinKey == "")
            {
                item.return_code = "FAIL";
                item.return_msg = "商户密钥未配置！";
            }
            if (item.return_code == "FAIL")
            {
                item.return_code = "INITFAIL";
                list.Add(item);
                return list;
            }
            foreach (OutPayWeiInfo info in BatchUserList)
            {
                WeiPayResult result2 = WeiXinPayOut(info, WeiXinAppid, WeiXinMchid, BatchWeixinPayCheckRealName, WeiXinKey);
                list.Add(result2);
                if ((result2.return_code == "SUCCESS") && (((result2.err_code == "NOAUTH") || (result2.err_code == "NOTENOUGH")) || (((result2.err_code == "CA_ERROR") || (result2.err_code == "SIGN_ERROR")) || (result2.err_code == "XML_ERROR"))))
                {
                    list.Add(result2);
                    return list;
                }
            }
            return list;
        }

        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue, string _key, string _input_charset)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary = BuildRequestPara(sParaTemp, _key, _input_charset);
            StringBuilder builder = new StringBuilder();
            builder.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + GATEWAY_NEW + "_input_charset=" + _input_charset + "' method='" + strMethod.ToLower().Trim() + "'>");
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                builder.Append("<input type='hidden' name='" + pair.Key + "' value='" + pair.Value + "'/>");
            }
            builder.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");
            builder.Append("<script>document.forms['alipaysubmit'].submit();</script>");
            return builder.ToString();
        }

        private static string BuildRequestMysign(Dictionary<string, string> sPara, string _key, string _input_charset)
        {
            return Sign(CreateLinkString(sPara), _key, _input_charset);
        }

        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp, string _key, string _input_charset)
        {
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            string str = "";
            sPara = FilterPara(sParaTemp);
            str = BuildRequestMysign(sPara, _key, _input_charset);
            sPara.Add("sign", str);
            sPara.Add("sign_type", "MD5");
            return sPara;
        }

        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dicArray)
            {
                builder.Append(pair.Key + "=" + pair.Value + "&");
            }
            int length = builder.Length;
            builder.Remove(length - 1, 1);
            return builder.ToString();
        }

        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in dicArrayPre)
            {
                if (((pair.Key.ToLower() != "sign") && (pair.Key.ToLower() != "sign_type")) && ((pair.Value != "") && (pair.Value != null)))
                {
                    dictionary.Add(pair.Key, pair.Value);
                }
            }
            return dictionary;
        }

        public static string GetMD5(string myString, string _input_charset = "utf-8")
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.GetEncoding(_input_charset).GetBytes(myString);
            byte[] buffer2 = md.ComputeHash(bytes);
            string str = null;
            for (int i = 0; i < buffer2.Length; i++)
            {
                str = str + buffer2[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }

        public static string GetRandomString(int length)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            string str = DateTime.Now.ToString("yyyyMMdd");
            builder.Append(str);
            for (int i = 0; i < length; i++)
            {
                builder.Append(Chars[random.Next(0, Chars.Length)]);
            }
            return builder.ToString();
        }

        public static string Sign(string prestr, string key, string _input_charset)
        {
            return GetMD5(prestr + key, "utf-8");
        }

        public static WeiPayResult SingleWeiPay(int amount, string desc, string useropenid, string realname, string tradeno, int UserId)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            WeiXinMchid = masterSettings.WeixinPartnerID;
            WeiXinAppid = masterSettings.WeixinAppId;
            WeiXinKey = masterSettings.WeixinPartnerKey;
            BatchWeixinPayCheckRealName = masterSettings.BatchWeixinPayCheckRealName.ToString();
            WeiXinCertPath = masterSettings.WeixinCertPath;
            WeixinCertPassword = masterSettings.WeixinCertPassword;
            string batchWeixinPayCheckRealName = BatchWeixinPayCheckRealName;
            if (batchWeixinPayCheckRealName != null)
            {
                if (!(batchWeixinPayCheckRealName == "0"))
                {
                    if (batchWeixinPayCheckRealName == "1")
                    {
                        BatchWeixinPayCheckRealName = "FORCE_CHECK";
                    }
                    else if (batchWeixinPayCheckRealName == "2")
                    {
                        BatchWeixinPayCheckRealName = "OPTION_CHECK";
                    }
                }
                else
                {
                    BatchWeixinPayCheckRealName = "NO_CHECK";
                }
            }
            WeiPayResult result = new WeiPayResult {
                return_code = "SUCCESS",
                err_code = "",
                return_msg = "微信企业付款参数配置错误"
            };
            if (WeiXinMchid == "")
            {
                result.return_code = "FAIL";
                result.return_msg = "商户号未配置！";
            }
            else if (WeiXinAppid == "")
            {
                result.return_code = "FAIL";
                result.return_msg = "公众号APPID未配置！";
            }
            else if (WeiXinKey == "")
            {
                result.return_code = "FAIL";
                result.return_msg = "商户密钥未配置！";
            }
            if (result.return_code == "FAIL")
            {
                return result;
            }
            result.return_code = "FAIL";
            result.return_msg = "用户参数出错了！";
            OutPayWeiInfo payinfos = new OutPayWeiInfo {
                Amount = amount,
                Partner_Trade_No = tradeno,
                Openid = useropenid,
                Re_User_Name = realname,
                Desc = desc,
                UserId = UserId,
                device_info = "",
                Nonce_Str = GetRandomString(20)
            };
            return WeiXinPayOut(payinfos, WeiXinAppid, WeiXinMchid, BatchWeixinPayCheckRealName, WeiXinKey);
        }

        public static bool Verify(string prestr, string sign, string key, string _input_charset)
        {
            return (Sign(prestr, key, _input_charset) == sign);
        }

        public static bool VerifyNotify(SortedDictionary<string, string> inputPara, string notify_id, string sign, string _key, string _input_charset, string _partner)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            bool flag = Verify(CreateLinkString(FilterPara(inputPara)), sign, _key, _input_charset);
            string str2 = "true";
            if ((notify_id != null) && (notify_id != ""))
            {
                string str3 = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
                str2 = new HttpHelp().DoGet(str3 + "partner=" + _partner + "&notify_id=" + notify_id, null);
            }
            return ((str2 == "true") && flag);
        }

        public static WeiPayResult WeiXinPayOut(OutPayWeiInfo payinfos, string Mch_appid, string Mchid, string Check_Name, string _key)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            dictionary.Add("mch_appid", Mch_appid);
            dictionary.Add("mchid", Mchid);
            dictionary.Add("nonce_str", payinfos.Nonce_Str);
            dictionary.Add("partner_trade_no", payinfos.Partner_Trade_No);
            dictionary.Add("openid", payinfos.Openid);
            dictionary.Add("check_name", Check_Name);
            dictionary.Add("amount", payinfos.Amount.ToString());
            dictionary.Add("desc", payinfos.Desc);
            dictionary.Add("spbill_create_ip", Globals.ServerIP());
            dictionary.Add("re_user_name", payinfos.Re_User_Name);
            dictionary.Add("device_info", "");
            string str = _key;
            string str2 = "";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<xml>");
            foreach (string str3 in dictionary.Keys)
            {
                if (dictionary[str3] != "")
                {
                    string str5 = str2;
                    str2 = str5 + "&" + str3 + "=" + dictionary[str3];
                    builder.AppendLine("<" + str3 + ">" + dictionary[str3] + "</" + str3 + ">");
                }
            }
            builder.AppendLine("<sign>" + GetMD5(str2.Remove(0, 1) + "&key=" + str, "utf-8").ToUpper() + "</sign>");
            builder.AppendLine("</xml>");
            HttpHelp help = new HttpHelp();
            string xml = help.DoPost(WeiPayUrl, builder.ToString(), WeixinCertPassword, WeiXinCertPath);
            WeiPayResult result = new WeiPayResult {
                return_code = "FAIL",
                return_msg = "访问服务器出错了！",
                err_code = "SERVERERR",
                UserId = payinfos.UserId,
                Amount = payinfos.Amount,
                partner_trade_no = payinfos.Partner_Trade_No
            };
            if (help.errstr != "")
            {
                result.return_msg = help.errstr;
                return result;
            }
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                result.return_code = document.SelectSingleNode("/xml/return_code").InnerText;
                result.return_msg = document.SelectSingleNode("/xml/return_msg").InnerText;
                if (result.return_code.ToUpper() == "SUCCESS")
                {
                    result.result_code = document.SelectSingleNode("/xml/result_code").InnerText;
                    if (result.result_code.ToUpper() == "SUCCESS")
                    {
                        result.mch_appid = document.SelectSingleNode("/xml/mch_appid").InnerText;
                        result.mchid = document.SelectSingleNode("/xml/mchid").InnerText;
                        result.device_info = document.SelectSingleNode("/xml/device_info").InnerText;
                        result.nonce_str = document.SelectSingleNode("/xml/nonce_str").InnerText;
                        result.result_code = document.SelectSingleNode("/xml/result_code").InnerText;
                        result.partner_trade_no = document.SelectSingleNode("/xml/partner_trade_no").InnerText;
                        result.payment_no = document.SelectSingleNode("/xml/payment_no").InnerText;
                        result.payment_time = document.SelectSingleNode("/xml/payment_time").InnerText;
                        return result;
                    }
                    result.err_code = document.SelectSingleNode("/xml/err_code").InnerText;
                    return result;
                }
                result.err_code = "FAIL";
            }
            catch (Exception exception)
            {
                Globals.Debuglog(xml, "_DebuglogBatchPayment.txt");
                result.return_code = "FAIL";
                result.return_msg = exception.Message.ToString();
            }
            return result;
        }
    }
}

