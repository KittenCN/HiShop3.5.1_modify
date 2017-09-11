namespace Hishop.Weixin.Pay
{
    using Hishop.Weixin.Pay.Domain;
    using Hishop.Weixin.Pay.Util;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Xml;

    public class PayClient
    {
        private PayAccount _payAccount;
        public static readonly string Deliver_Notify_Url = "https://api.weixin.qq.com/pay/delivernotify";
        public static readonly string prepay_id_Url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        public PayClient(PayAccount account) : this(account.AppId, account.AppSecret, account.PartnerId, account.PartnerKey, account.EnableSP, account.Sub_appid, account.Sub_mch_id)
        {
        }

        public PayClient(string appId, string appSecret, string partnerId, string partnerKey, bool enableSP, string sub_appid, string sub_mch_id)
        {
            this._payAccount = new PayAccount(appId, appSecret, partnerId, partnerKey, enableSP, sub_appid, sub_mch_id);
        }

        internal string BuildPackage(PackageInfo package)
        {
            PayDictionary parameters = new PayDictionary();
            parameters.Add("appid", this._payAccount.AppId);
            parameters.Add("mch_id", this._payAccount.PartnerId);
            if (this._payAccount.EnableSP)
            {
                parameters.Add("sub_appid", this._payAccount.Sub_appid);
                parameters.Add("sub_mch_id", this._payAccount.Sub_mch_id);
                parameters.Add("sub_openid", package.OpenId);
            }
            else
            {
                parameters.Add("openid", package.OpenId);
            }
            parameters.Add("device_info", "");
            parameters.Add("nonce_str", Utils.CreateNoncestr());
            parameters.Add("body", package.Body);
            parameters.Add("attach", "");
            parameters.Add("out_trade_no", package.OutTradeNo);
            parameters.Add("total_fee", (int) package.TotalFee);
            parameters.Add("spbill_create_ip", package.SpbillCreateIp);
            parameters.Add("time_start", package.TimeExpire);
            parameters.Add("time_expire", "");
            parameters.Add("goods_tag", package.GoodsTag);
            parameters.Add("notify_url", package.NotifyUrl);
            parameters.Add("trade_type", "JSAPI");
            parameters.Add("product_id", "");
            string sign = SignHelper.SignPackage(parameters, this._payAccount.PartnerKey);
            string str2 = this.GetPrepay_id(parameters, sign);
            if (str2.Length > 0x40)
            {
                str2 = "";
            }
            return string.Format("prepay_id=" + str2, new object[0]);
        }

        public PayRequestInfo BuildPayRequest(PackageInfo package)
        {
            PayRequestInfo info = new PayRequestInfo {
                appId = this._payAccount.AppId,
                package = this.BuildPackage(package),
                timeStamp = Utils.GetCurrentTimeSeconds().ToString(),
                nonceStr = Utils.CreateNoncestr()
            };
            PayDictionary parameters = new PayDictionary();
            parameters.Add("appId", this._payAccount.AppId);
            parameters.Add("timeStamp", info.timeStamp);
            parameters.Add("package", info.package);
            parameters.Add("nonceStr", info.nonceStr);
            parameters.Add("signType", "MD5");
            info.paySign = SignHelper.SignPay(parameters, this._payAccount.PartnerKey);
            return info;
        }

        public bool checkPackage(PackageInfo package, out string errmsg)
        {
            errmsg = "";
            if (string.IsNullOrEmpty(package.NotifyUrl) || (package.NotifyUrl.Length < 5))
            {
                errmsg = "返回地址NotifyUrl未配置！";
                return false;
            }
            if (string.IsNullOrEmpty(package.OpenId) || (package.OpenId.Length < 8))
            {
                errmsg = "用户OPENID不正确！";
                return false;
            }
            if (string.IsNullOrEmpty(package.OutTradeNo))
            {
                errmsg = "交易订单号不能为空";
                return false;
            }
            if (package.TotalFee == 0M)
            {
                errmsg = "支付金额不能为零";
                return false;
            }
            return true;
        }

        public bool checkSetParams(out string errmsg)
        {
            errmsg = "";
            bool flag = true;
            if (this._payAccount == null)
            {
                flag = false;
                errmsg = "微信支付参数未初始化！";
                return flag;
            }
            if (!this._payAccount.EnableSP)
            {
                if (string.IsNullOrEmpty(this._payAccount.AppId) || (this._payAccount.AppId.Length < 15))
                {
                    errmsg = "商户公众号未正确配置！";
                    return false;
                }
                if (string.IsNullOrEmpty(this._payAccount.PartnerId) || (this._payAccount.PartnerId.Length < 8))
                {
                    errmsg = "商户号未正确配置！";
                    return false;
                }
                if (string.IsNullOrEmpty(this._payAccount.PartnerKey) || (this._payAccount.PartnerKey.Length < 8))
                {
                    errmsg = "商户KEY未正确配置！";
                    return false;
                }
                if (!string.IsNullOrEmpty(this._payAccount.AppSecret) && (this._payAccount.AppSecret.Length >= 8))
                {
                    return flag;
                }
                errmsg = "公众号AppSecret未正确配置！";
                return false;
            }
            if (string.IsNullOrEmpty(this._payAccount.AppId) || (this._payAccount.AppId.Length < 15))
            {
                errmsg = "服务商公众号未正确配置！";
                return false;
            }
            if (string.IsNullOrEmpty(this._payAccount.PartnerId) || (this._payAccount.PartnerId.Length < 8))
            {
                errmsg = "服务商商户号未正确配置！";
                return false;
            }
            if (string.IsNullOrEmpty(this._payAccount.PartnerKey) || (this._payAccount.PartnerKey.Length < 8))
            {
                errmsg = "服务商KEY未正确配置！";
                return false;
            }
            if (string.IsNullOrEmpty(this._payAccount.AppSecret) || (this._payAccount.AppSecret.Length < 8))
            {
                errmsg = "公众号AppSecret未正确配置！";
                return false;
            }
            if (string.IsNullOrEmpty(this._payAccount.Sub_appid) || (this._payAccount.Sub_appid.Length < 8))
            {
                errmsg = "商户公众号未正确配置！";
                return false;
            }
            if (!string.IsNullOrEmpty(this._payAccount.Sub_mch_id) && (this._payAccount.Sub_mch_id.Length >= 8))
            {
                return flag;
            }
            errmsg = "子商户号未正确配置！";
            return false;
        }

        public bool DeliverNotify(DeliverInfo deliver)
        {
            string token = Utils.GetToken(this._payAccount.AppId, this._payAccount.AppSecret);
            return this.DeliverNotify(deliver, token);
        }

        public bool DeliverNotify(DeliverInfo deliver, string token)
        {
            PayDictionary parameters = new PayDictionary();
            parameters.Add("appid", this._payAccount.AppId);
            parameters.Add("openid", deliver.OpenId);
            parameters.Add("transid", deliver.TransId);
            parameters.Add("out_trade_no", deliver.OutTradeNo);
            parameters.Add("deliver_timestamp", Utils.GetTimeSeconds(deliver.TimeStamp));
            parameters.Add("deliver_status", deliver.Status ? 1 : 0);
            parameters.Add("deliver_msg", deliver.Message);
            deliver.AppId = this._payAccount.AppId;
            deliver.AppSignature = SignHelper.SignPay(parameters, "");
            parameters.Add("app_signature", deliver.AppSignature);
            parameters.Add("sign_method", deliver.SignMethod);
            string data = JsonConvert.SerializeObject(parameters);
            string url = string.Format("{0}?access_token={1}", Deliver_Notify_Url, token);
            string str3 = new WebUtils().DoPost(url, data);
            return (!string.IsNullOrEmpty(str3) && str3.Contains("ok"));
        }

        internal string GetPrepay_id(PayDictionary dict, string sign)
        {
            dict.Add("sign", sign);
            SignHelper.BuildQuery(dict, false);
            string postData = SignHelper.BuildXml(dict, false);
            return PostData(prepay_id_Url, postData);
        }

        public static string PostData(string url, string postData)
        {
            string xml = string.Empty;
            try
            {
                HttpWebRequest request;
                Uri requestUri = new Uri(url);
                if (url.ToLower().StartsWith("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = (s, c, ch, e) => true;
                    request = (HttpWebRequest) WebRequest.CreateDefault(requestUri);
                }
                else
                {
                    request = (HttpWebRequest) WebRequest.Create(requestUri);
                }
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.KeepAlive = true;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                requestStream.Dispose();
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        xml = new StreamReader(stream2, encoding).ReadToEnd();
                        XmlDocument document = new XmlDocument();
                        try
                        {
                            document.LoadXml(xml);
                        }
                        catch (Exception exception)
                        {
                            xml = string.Format("获取信息错误doc.load：{0}", exception.Message) + xml;
                        }
                        try
                        {
                            if (document == null)
                            {
                                return xml;
                            }
                            XmlNode node = document.SelectSingleNode("xml/return_code");
                            if (node == null)
                            {
                                return xml;
                            }
                            if (node.InnerText == "SUCCESS")
                            {
                                XmlNode node2 = document.SelectSingleNode("xml/prepay_id");
                                if (node2 == null)
                                {
                                    return xml;
                                }
                                return node2.InnerText;
                            }
                            XmlNode node3 = document.SelectSingleNode("xml/return_msg");
                            if (node3 != null)
                            {
                                return node3.InnerText;
                            }
                            return document.InnerXml;
                        }
                        catch (Exception exception2)
                        {
                            xml = string.Format("获取信息错误node.load：{0}", exception2.Message) + xml;
                        }
                        return xml;
                    }
                }
            }
            catch (Exception exception3)
            {
                xml = string.Format("获取信息错误post error：{0}", exception3.Message) + xml;
            }
            return xml;
        }

        public static void writeLog(IDictionary<string, string> param, string sign, string url, string msg)
        {
        }
    }
}

