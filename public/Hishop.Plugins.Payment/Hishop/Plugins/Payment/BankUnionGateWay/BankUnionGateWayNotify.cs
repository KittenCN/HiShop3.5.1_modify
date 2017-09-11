namespace Hishop.Plugins.Payment.BankUnionGateWay
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment.BankUnionGateWay.sdk;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web;

    public class BankUnionGateWayNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public BankUnionGateWayNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
            parameters.Remove("HIGW");
        }

        private void AddDictionary(Dictionary<string, string> resData, string paramname)
        {
            string str = this.parameters[paramname];
            if (!string.IsNullOrEmpty(str))
            {
                resData.Add(paramname, str);
            }
        }

        public static string CoverDictionaryToString(Dictionary<string, string> data)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (KeyValuePair<string, string> pair in data)
            {
                dictionary.Add(pair.Key, pair.Value);
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

        public override decimal GetOrderAmount()
        {
            return (decimal.Parse(this.parameters["txnAmt"]) / 100M);
        }

        public override string GetOrderId()
        {
            return this.parameters["orderId"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            string[] allKeys = this.parameters.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                data.Add(allKeys[i], this.parameters[allKeys[i]]);
            }
            if (SDKUtil.Validate(data, Encoding.UTF8))
            {
                this.OnFinished(false);
            }
            else
            {
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

