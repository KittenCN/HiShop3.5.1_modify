namespace Hishop.Plugins.Payment.Allbuy
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class AllbuyNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public AllbuyNotify(NameValueCollection parameters)
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
            string str = this.parameters["merchant"];
            string str2 = this.parameters["billno"];
            string str3 = this.parameters["v_pstring"];
            string str4 = this.parameters["amount"];
            string str5 = this.parameters["success"];
            string str6 = this.parameters["remark"];
            string str7 = this.parameters["sign"];
            if (((((str == null) || (str2 == null)) || ((str3 == null) || (str4 == null))) || ((str5 == null) || (str6 == null))) || (str7 == null))
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
                string password = str + str2 + str4 + str5 + document.FirstChild.SelectSingleNode("Key").InnerText;
                if (!str7.Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToLower(CultureInfo.InvariantCulture)))
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

