namespace Hishop.Plugins.Payment.WXQRCode
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class WXQRCodeNotify : PaymentNotify
    {
        private string appid = "";
        private string bank_type = "";
        private string cash_fee = "";
        private string fee_type = "";
        private string is_subscribe = "";
        private string mch_id = "";
        private Dictionary<string, string> nofifyData = new Dictionary<string, string>();
        private string nonce_str = "";
        private string openid = "";
        private string out_trade_no = "";
        private readonly NameValueCollection parameters;
        private string result_code = "";
        private string return_code = "";
        private string sign = "";
        private string sub_appid = "";
        private string sub_is_subscribe = "";
        private string sub_mch_id = "";
        private string sub_openid = "";
        private string time_end = "";
        private string total_fee = "";
        private string trade_type = "";
        private string transaction_id = "";

        public WXQRCodeNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
            this.LoadNotifydata(parameters["notify_data"]);
        }

        public string CoverDictionaryToString(Dictionary<string, string> data)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (KeyValuePair<string, string> pair in data)
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    dictionary.Add(pair.Key, pair.Value);
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair2 in dictionary)
            {
                builder.Append(pair2.Key + "=" + pair2.Value + "&");
            }
            return builder.ToString().Substring(0, builder.Length - 1);
        }

        public override string GetGatewayOrderId()
        {
            return string.Empty;
        }

        private string GetMD5String(string encypStr)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(encypStr);
            return BitConverter.ToString(provider.ComputeHash(bytes)).Replace("-", "").ToUpper();
        }

        public override decimal GetOrderAmount()
        {
            return (decimal.Parse(this.total_fee) / 100M);
        }

        public override string GetOrderId()
        {
            return this.out_trade_no;
        }

        public void LoadNotifydata(string notifydata)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(notifydata);
            XmlNode node = document.SelectSingleNode("xml/out_trade_no");
            if (node != null)
            {
                this.out_trade_no = node.InnerText;
                this.nofifyData.Add("out_trade_no", this.out_trade_no);
            }
            node = document.SelectSingleNode("xml/total_fee");
            if (node != null)
            {
                this.total_fee = node.InnerText;
                this.nofifyData.Add("total_fee", this.total_fee);
            }
            node = document.SelectSingleNode("xml/appid");
            if (node != null)
            {
                this.appid = node.InnerText;
                this.nofifyData.Add("appid", this.appid);
            }
            node = document.SelectSingleNode("xml/bank_type");
            if (node != null)
            {
                this.bank_type = node.InnerText;
                this.nofifyData.Add("bank_type", this.bank_type);
            }
            node = document.SelectSingleNode("xml/cash_fee");
            if (node != null)
            {
                this.cash_fee = node.InnerText;
                this.nofifyData.Add("cash_fee", this.cash_fee);
            }
            node = document.SelectSingleNode("xml/fee_type");
            if (node != null)
            {
                this.fee_type = node.InnerText;
                this.nofifyData.Add("fee_type", this.fee_type);
            }
            node = document.SelectSingleNode("xml/is_subscribe");
            if (node != null)
            {
                this.is_subscribe = node.InnerText;
                this.nofifyData.Add("is_subscribe", this.is_subscribe);
            }
            node = document.SelectSingleNode("xml/mch_id");
            if (node != null)
            {
                this.mch_id = node.InnerText;
                this.nofifyData.Add("mch_id", this.mch_id);
            }
            node = document.SelectSingleNode("xml/nonce_str");
            if (node != null)
            {
                this.nonce_str = node.InnerText;
                this.nofifyData.Add("nonce_str", this.nonce_str);
            }
            node = document.SelectSingleNode("xml/openid");
            if (node != null)
            {
                this.openid = node.InnerText;
                this.nofifyData.Add("openid", this.openid);
            }
            node = document.SelectSingleNode("xml/result_code");
            if (node != null)
            {
                this.result_code = node.InnerText;
                this.nofifyData.Add("result_code", this.result_code);
            }
            node = document.SelectSingleNode("xml/return_code");
            if (node != null)
            {
                this.return_code = node.InnerText;
                this.nofifyData.Add("return_code", this.return_code);
            }
            node = document.SelectSingleNode("xml/sign");
            if (node != null)
            {
                this.sign = node.InnerText;
            }
            node = document.SelectSingleNode("xml/time_end");
            if (node != null)
            {
                this.time_end = node.InnerText;
                this.nofifyData.Add("time_end", this.time_end);
            }
            node = document.SelectSingleNode("xml/trade_type");
            if (node != null)
            {
                this.trade_type = node.InnerText;
                this.nofifyData.Add("trade_type", this.trade_type);
            }
            node = document.SelectSingleNode("xml/sub_mch_id");
            if (node != null)
            {
                this.sub_mch_id = node.InnerText;
                this.nofifyData.Add("sub_mch_id", this.sub_mch_id);
            }
            node = document.SelectSingleNode("xml/sub_appid");
            if (node != null)
            {
                this.sub_appid = node.InnerText;
                this.nofifyData.Add("sub_appid", this.sub_appid);
            }
            node = document.SelectSingleNode("xml/sub_is_subscribe");
            if (node != null)
            {
                this.sub_is_subscribe = node.InnerText;
                this.nofifyData.Add("sub_is_subscribe", this.sub_is_subscribe);
            }
            node = document.SelectSingleNode("xml/sub_openid");
            if (node != null)
            {
                this.sub_openid = node.InnerText;
                this.nofifyData.Add("sub_openid", this.sub_openid);
            }
            node = document.SelectSingleNode("xml/transaction_id");
            if (node != null)
            {
                this.transaction_id = node.InnerText;
                this.nofifyData.Add("transaction_id", this.transaction_id);
            }
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            string innerText = document.FirstChild.SelectSingleNode("AppSecret").InnerText;
            if (this.GetMD5String(this.CoverDictionaryToString(this.nofifyData) + "&key=" + innerText).ToUpper().Equals(this.sign))
            {
                this.OnFinished(false);
            }
            else
            {
                PayLog.AppendLog(null, configXml, "4", string.Concat(new object[] { "sign:", this.sign, "-", this.sign.Length }), LogType.WXQRCode);
                this.OnNotifyVerifyFaild();
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
            if (context != null)
            {
                context.Response.Clear();
                context.Response.Write(success ? "ok" : "error");
                context.Response.End();
            }
        }
    }
}

