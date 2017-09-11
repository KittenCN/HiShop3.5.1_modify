namespace Hishop.Plugins.Payment.IPS
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class IpsNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public IpsNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return string.Empty;
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["amount"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["billno"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this.parameters["billno"];
            string str2 = this.parameters["amount"];
            string str3 = this.parameters["date"];
            string str4 = this.parameters["ipsbillno"];
            string str5 = this.parameters["succ"];
            string str6 = this.parameters["retEncodeType"];
            string str7 = this.parameters["currency_type"];
            string str8 = this.parameters["signature"];
            if (((((str == null) || (str2 == null)) || ((str3 == null) || (str4 == null))) || (((str5 == null) || (str6 == null)) || (str7 == null))) || (str8 == null))
            {
                this.OnNotifyVerifyFaild();
            }
            else if (!str5.Equals("Y"))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("billno{0}currencytype{1}amount{2}date{3}", new object[] { str, str7, str2, str3 });
                builder.AppendFormat("succ{0}ipsbillno{1}retencodetype{2}{3}", new object[] { str5, str4, str6, document.FirstChild.SelectSingleNode("Cert").InnerText });
                string str9 = Globals.GetMD5(builder.ToString()).ToLower(CultureInfo.InvariantCulture);
                if (!str8.Equals(str9))
                {
                    this.OnNotifyVerifyFaild();
                }
                else
                {
                    this.OnFinished(false);
                }
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
        }
    }
}

