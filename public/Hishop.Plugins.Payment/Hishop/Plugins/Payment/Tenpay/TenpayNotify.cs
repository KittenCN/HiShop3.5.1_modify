namespace Hishop.Plugins.Payment.Tenpay
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Xml;

    public class TenpayNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public TenpayNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return this.parameters["transaction_id"];
        }

        public override decimal GetOrderAmount()
        {
            return (decimal.Parse(this.parameters["total_fee"], CultureInfo.InvariantCulture) / 100M);
        }

        public override string GetOrderId()
        {
            return this.parameters["sp_billno"];
        }

        private string UrlDecode(string instr)
        {
            if ((instr == null) || (instr.Trim() == ""))
            {
                return "";
            }
            return instr.Replace("%3d", "=").Replace("%26", "&").Replace("%22", "\"").Replace("%3f", "?").Replace("%27", "'").Replace("%20", " ").Replace("%25", "%");
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this.parameters["cmdno"];
            string str2 = this.parameters["pay_result"];
            string str3 = this.UrlDecode(this.parameters["pay_info"]);
            string str4 = this.parameters["date"];
            string str5 = this.parameters["bargainor_id"];
            string str6 = this.parameters["transaction_id"];
            string str7 = this.parameters["sp_billno"];
            string str8 = this.parameters["total_fee"];
            string str9 = this.parameters["fee_type"];
            string str10 = this.parameters["attach"];
            string str11 = this.parameters["sign"];
            if ((((((str == null) || (str2 == null)) || ((str3 == null) || (str4 == null))) || (((str5 == null) || (str6 == null)) || ((str7 == null) || (str8 == null)))) || ((str9 == null) || (str10 == null))) || (str11 == null))
            {
                this.OnNotifyVerifyFaild();
            }
            else if (!str2.Equals("0"))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                string encypStr = "cmdno=" + str + "&pay_result=" + str2 + "&date=" + str4 + "&transaction_id=" + str6 + "&sp_billno=" + str7 + "&total_fee=" + str8 + "&fee_type=" + str9 + "&attach=" + str10 + "&key=" + document.FirstChild.SelectSingleNode("Key").InnerText;
                if (!str11.Equals(Globals.GetMD5(encypStr)))
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

