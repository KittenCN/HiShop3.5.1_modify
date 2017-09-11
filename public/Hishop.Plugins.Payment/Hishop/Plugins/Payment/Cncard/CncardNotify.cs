namespace Hishop.Plugins.Payment.Cncard
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class CncardNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public CncardNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return this.parameters["c_transnum"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["c_orderamount"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["c_order"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this.parameters["c_mid"];
            string str2 = this.parameters["c_order"];
            string str3 = this.parameters["c_orderamount"];
            string str4 = this.parameters["c_ymd"];
            string str5 = this.parameters["c_moneytype"];
            string str6 = this.parameters["c_transnum"];
            string str7 = this.parameters["c_succmark"];
            string str8 = this.parameters["c_cause"];
            string str9 = this.parameters["c_memo1"];
            string str10 = this.parameters["c_memo2"];
            string str11 = this.parameters["c_signstr"];
            if ((((((str == null) || (str2 == null)) || ((str3 == null) || (str4 == null))) || (((str5 == null) || (str6 == null)) || ((str7 == null) || (str8 == null)))) || ((str9 == null) || (str10 == null))) || (str11 == null))
            {
                this.OnNotifyVerifyFaild();
            }
            else if (!str7.Equals("Y"))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                if (str != document.FirstChild.SelectSingleNode("Cmid").InnerText)
                {
                    this.OnNotifyVerifyFaild();
                }
                else
                {
                    string password = str + str2 + str3 + str4 + str6 + str7 + str5 + str9 + str10 + document.FirstChild.SelectSingleNode("Cpass").InnerText;
                    if (!str11.Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToLower()))
                    {
                        this.OnNotifyVerifyFaild();
                    }
                    else
                    {
                        this.OnFinished(false);
                    }
                }
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
        }
    }
}

