namespace Hishop.Plugins.Payment.ShengPayMobile
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class ShengPayMobileNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public ShengPayMobileNotify()
        {
        }

        public ShengPayMobileNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return this.parameters["TraceNo"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["OrderAmount"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["OrderNo"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this.parameters["Name"];
            string str2 = this.parameters["Version"];
            string str3 = this.parameters["Charset"];
            string str4 = this.parameters["TraceNo"];
            string str5 = this.parameters["MsgSender"];
            string str6 = this.parameters["SendTime"];
            string str7 = this.parameters["InstCode"];
            string str8 = this.parameters["OrderNo"];
            string str9 = this.parameters["OrderAmount"];
            string str10 = this.parameters["TransNo"];
            string str11 = this.parameters["TransAmount"];
            string str12 = this.parameters["TransStatus"];
            string str13 = this.parameters["TransType"];
            string str14 = this.parameters["TransTime"];
            string str15 = this.parameters["MerchantNo"];
            string str16 = this.parameters["ErrorCode"];
            string str17 = this.parameters["ErrorMsg"];
            string str18 = this.parameters["Ext1"];
            string str19 = this.parameters["Ext2"];
            string str20 = this.parameters["SignType"];
            string str21 = this.parameters["SignMsg"];
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            string str24 = FormsAuthentication.HashPasswordForStoringInConfigFile((str + str2 + str3 + str4 + str5 + str6 + str7 + str8 + str9 + str10 + str11 + str12 + str13 + str14 + str15 + str16 + str17 + str18 + str19 + str20) + document.FirstChild.SelectSingleNode("SellerKey").InnerText, "MD5");
            if ((str12 != "01") || (str21 != str24))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                this.OnFinished(false);
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
            if ((context != null) && success)
            {
                context.Response.Clear();
                context.Response.Write("OK");
                context.Response.End();
            }
        }
    }
}

