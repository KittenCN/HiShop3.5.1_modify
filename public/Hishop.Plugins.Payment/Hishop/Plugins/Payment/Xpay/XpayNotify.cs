namespace Hishop.Plugins.Payment.Xpay
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Xml;

    public class XpayNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public XpayNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return this.parameters["sid"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["prc"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["bid"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this.parameters["tid"];
            string str2 = this.parameters["bid"];
            string str3 = this.parameters["sid"];
            string str4 = this.parameters["prc"];
            string str5 = this.parameters["actionCode"];
            string str6 = this.parameters["actionParameter"];
            string str7 = this.parameters["card"];
            string str8 = this.parameters["success"];
            string str9 = this.parameters["md"];
            if (!str8.Equals("true"))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                string s = document.FirstChild.SelectSingleNode("Key").InnerText + ":" + str2 + "," + str3 + "," + str4 + "," + str5 + "," + str6 + "," + str + "," + str7 + "," + str8;
                if (!str9.Equals(Globals.GetXpayMD5(s)))
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

