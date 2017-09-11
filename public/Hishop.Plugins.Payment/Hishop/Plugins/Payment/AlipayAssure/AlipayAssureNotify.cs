namespace Hishop.Plugins.Payment.AlipayAssure
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Xml;

    public class AlipayAssureNotify : PaymentNotify
    {
        private const string InpuitCharset = "utf-8";
        private readonly NameValueCollection parameters;

        public AlipayAssureNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        private string CreateUrl(XmlNode configNode)
        {
            return string.Format(CultureInfo.InvariantCulture, "https://mapi.alipay.com/gateway.do?service=notify_verify&partner={0}&notify_id={1}", new object[] { configNode.SelectSingleNode("Partner").InnerText.Trim(), this.parameters["notify_id"] });
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
            catch (Exception exception)
            {
                PayLog.writeLog_Collection(this.parameters, "", HttpContext.Current.Request.Url.ToString(), exception.Message, LogType.Alipay_Direct);
                flag = false;
            }
            if (!flag)
            {
                PayLog.AppendLog_Collection(this.parameters, "", HttpContext.Current.Request.Url.ToString(), "is_success值错了", LogType.Alipay_Direct);
            }
            this.parameters.Remove("HIGW");
            string[] strArray2 = Globals.BubbleSort(this.parameters.AllKeys);
            string s = "";
            for (int i = 0; i < strArray2.Length; i++)
            {
                if ((!string.IsNullOrEmpty(this.parameters[strArray2[i]]) && (strArray2[i] != "sign")) && (strArray2[i] != "sign_type"))
                {
                    if (i == (strArray2.Length - 1))
                    {
                        s = s + strArray2[i] + "=" + this.parameters[strArray2[i]];
                    }
                    else
                    {
                        s = s + strArray2[i] + "=" + this.parameters[strArray2[i]] + "&";
                    }
                }
            }
            s = s + document.FirstChild.SelectSingleNode("Key").InnerText;
            string str2 = Globals.GetMD5(s, "utf-8");
            if (!(flag && this.parameters["sign"].Equals(Globals.GetMD5(s, "utf-8"))))
            {
                PayLog.writeLog_Collection(this.parameters, Globals.GetMD5(s, "utf-8"), HttpContext.Current.Request.Url.ToString(), "签名错误---" + document.FirstChild.SelectSingleNode("Key").InnerText, LogType.Alipay_Direct);
                this.OnNotifyVerifyFaild();
            }
            else
            {
                string str3 = this.parameters["trade_status"];
                if (str3 != null)
                {
                    if (!(str3 == "WAIT_SELLER_SEND_GOODS"))
                    {
                        if (str3 == "TRADE_FINISHED")
                        {
                            this.OnFinished(true);
                        }
                    }
                    else
                    {
                        this.OnPayment();
                    }
                }
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

        public void WriteError(string Error, NameValueCollection parameters, string configXml = "")
        {
            DataTable table = new DataTable {
                TableName = "QRCodealipay"
            };
            table.Columns.Add(new DataColumn("OperTime"));
            table.Columns.Add(new DataColumn("msg"));
            foreach (string str in parameters.AllKeys)
            {
                table.Columns.Add(new DataColumn(str));
            }
            DataRow row = table.NewRow();
            row["OperTime"] = DateTime.Now;
            row["msg"] = Error;
            foreach (string str in parameters.AllKeys)
            {
                row[str] = parameters[str];
            }
            table.Columns.Add(new DataColumn("configxml"));
            row["configxml"] = configXml;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Request.MapPath("/alipynotify.xml"));
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
            table.WriteXml(HttpContext.Current.Request.MapPath("/PayLog.xml"));
        }
    }
}

