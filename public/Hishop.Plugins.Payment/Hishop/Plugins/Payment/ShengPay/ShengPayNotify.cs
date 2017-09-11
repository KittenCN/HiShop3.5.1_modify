namespace Hishop.Plugins.Payment.ShengPay
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class ShengPayNotify : PaymentNotify
    {
        private readonly NameValueCollection _parameters;

        public ShengPayNotify(NameValueCollection parameters)
        {
            this._parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return this._parameters["TransNo"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this._parameters["TransAmount"]);
        }

        public override string GetOrderId()
        {
            return this._parameters["OrderNo"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this._parameters["Name"];
            string str2 = this._parameters["Version"];
            string str3 = this._parameters["Charset"];
            string str4 = this._parameters["TraceNo"];
            string str5 = this._parameters["MsgSender"];
            string str6 = this._parameters["SendTime"];
            string str7 = this._parameters["InstCode"];
            string str8 = this._parameters["OrderNo"];
            string str9 = this._parameters["OrderAmount"];
            string str10 = this._parameters["TransNo"];
            string str11 = this._parameters["TransAmount"];
            string str12 = this._parameters["TransStatus"];
            string str13 = this._parameters["TransType"];
            string str14 = this._parameters["TransTime"];
            string str15 = this._parameters["MerchantNo"];
            string str16 = this._parameters["PaymentNo"];
            string str17 = this._parameters["ErrorCode"];
            string str18 = this._parameters["ErrorMsg"];
            string str19 = this._parameters["PayableFee"];
            string str20 = this._parameters["ReceivableFee"];
            string str21 = this._parameters["PayChannel"];
            string str22 = this._parameters["Ext1"];
            string str23 = this._parameters["BankSerialNo"];
            string str24 = this._parameters["SignType"];
            string str25 = this._parameters["SignMsg"];
            try
            {
                IDictionary<string, string> param = new Dictionary<string, string>();
                foreach (string str26 in this._parameters.AllKeys)
                {
                    param.Add(str26, this._parameters[str26]);
                }
                string msg = "";
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                string innerText = document.FirstChild.SelectSingleNode("Key").InnerText;
                string str30 = str + str2 + str3 + str4 + str5 + str6 + str7 + str8 + str9 + str10 + str11 + str12 + str13 + str14 + str15 + str17 + str18 + str22 + str24;
                msg = str30;
                string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(str30 + innerText, "MD5");
                string str32 = msg;
                msg = str32 + "|status:" + str12 + "|sign:" + sign + "|mac:" + str25;
                if ((str12 != "01") || (str25 != sign))
                {
                    PayLog.AppendLog(param, sign, "", msg, LogType.ShengPay);
                    this.OnNotifyVerifyFaild();
                }
                else
                {
                    this.OnFinished(false);
                }
            }
            catch (Exception exception)
            {
                this.writeXML(configXml, exception.Message + exception.StackTrace);
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

        public void writeXML(string Data, string ErrorMsg)
        {
            DataTable table = new DataTable {
                TableName = "Error"
            };
            table.Columns.Add("DateTime");
            table.Columns.Add("Error");
            table.Columns.Add("Data");
            DataRow row = table.NewRow();
            row["DateTime"] = DateTime.Now;
            row["Error"] = ErrorMsg;
            row["Data"] = Data;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Request.MapPath("/PayNotifyLog.xml"));
        }
    }
}

