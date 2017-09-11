namespace Hishop.Plugins.Payment.IPSExpress
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class IpsExpressNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public IpsExpressNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return string.Empty;
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["Amount"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["BillNo"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this.parameters["Merchant"];
            string str2 = this.parameters["BillNo"];
            string str3 = this.parameters["Amount"];
            string str4 = this.parameters["Success"];
            string str5 = this.parameters["Remark"];
            string str6 = this.parameters["Sign"];
            if (((((str == null) || (str2 == null)) || ((str3 == null) || (str4 == null))) || (str5 == null)) || (str6 == null))
            {
                this.OnNotifyVerifyFaild();
            }
            else if (!str4.Equals("Y"))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                string password = str + str2 + str3 + str5 + str4 + document.FirstChild.SelectSingleNode("MerPassword").InnerText;
                if (!str6.Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToLower(CultureInfo.InvariantCulture)))
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

