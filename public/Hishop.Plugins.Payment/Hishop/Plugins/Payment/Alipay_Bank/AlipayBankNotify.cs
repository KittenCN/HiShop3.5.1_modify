namespace Hishop.Plugins.Payment.Alipay_Bank
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Xml;

    public class AlipayBankNotify : PaymentNotify
    {
        private const string InputCharset = "utf-8";
        private readonly NameValueCollection parameters;

        public AlipayBankNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        private string CreateUrl(XmlNode configNode)
        {
            return string.Format(CultureInfo.InvariantCulture, "https://mapi.alipay.com/gateway.do?service=notify_verify&partner={0}&notify_id={1}", new object[] { configNode.SelectSingleNode("Partner").InnerText, this.parameters["notify_id"] });
        }

        public override string GetGatewayOrderId()
        {
            return this.parameters["trade_no"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["total_fee"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["out_trade_no"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            bool flag;
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            try
            {
                flag = bool.Parse(this.GetResponse(this.CreateUrl(document.FirstChild), timeout));
            }
            catch
            {
                flag = false;
            }
            this.parameters.Remove("HIGW");
            string[] strArray = Globals.BubbleSort(this.parameters.AllKeys);
            string s = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                if ((!string.IsNullOrEmpty(this.parameters[strArray[i]]) && (strArray[i] != "sign")) && (strArray[i] != "sign_type"))
                {
                    if (i == (strArray.Length - 1))
                    {
                        s = s + strArray[i] + "=" + this.parameters[strArray[i]];
                    }
                    else
                    {
                        s = s + strArray[i] + "=" + this.parameters[strArray[i]] + "&";
                    }
                }
            }
            s = s + document.FirstChild.SelectSingleNode("Key").InnerText;
            flag = flag && this.parameters["sign"].Equals(Globals.GetMD5(s, "utf-8"));
            string str2 = this.parameters["trade_status"];
            if (flag && ((str2 == "TRADE_SUCCESS") || (str2 == "TRADE_FINISHED")))
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
                context.Response.Write(success ? "success" : "fail");
                context.Response.End();
            }
        }
    }
}

