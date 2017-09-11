namespace Hishop.Plugins.Payment.Chinabank
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class ChinabankNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public ChinabankNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return string.Empty;
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["v_amount"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["v_oid"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            try
            {
                string str = this.parameters["v_oid"];
                string str2 = this.parameters["v_pstatus"];
                string str3 = this.parameters["v_pstring"];
                string str4 = this.parameters["v_pmode"];
                string str5 = this.parameters["v_md5str"];
                string str6 = this.parameters["v_amount"];
                string str7 = this.parameters["v_moneytype"];
                string str8 = this.parameters["remark1"];
                if (((((str == null) || (str2 == null)) || ((str3 == null) || (str4 == null))) || (((str5 == null) || (str6 == null)) || (str8 == null))) || (str7 == null))
                {
                    PayLog.AppendLog_Collection(this.parameters, "", "1", configXml, LogType.ChinaBank);
                    this.OnNotifyVerifyFaild();
                    return;
                }
                if (!str2.Equals("20"))
                {
                    PayLog.AppendLog_Collection(this.parameters, "", "2", configXml, LogType.ChinaBank);
                    this.OnNotifyVerifyFaild();
                    return;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                string str9 = FormsAuthentication.HashPasswordForStoringInConfigFile(str + str2 + str6 + str7 + document.FirstChild.SelectSingleNode("Key").InnerText, "MD5").ToUpper(CultureInfo.InvariantCulture);
                if (!str5.Equals(str9))
                {
                    PayLog.AppendLog_Collection(this.parameters, str9, "3", configXml, LogType.ChinaBank);
                    this.OnNotifyVerifyFaild();
                    return;
                }
            }
            catch (Exception exception)
            {
                PayLog.AppendLog_Collection(this.parameters, "", "3", configXml + "---" + exception.Message, LogType.ChinaBank);
            }
            this.OnFinished(false);
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

