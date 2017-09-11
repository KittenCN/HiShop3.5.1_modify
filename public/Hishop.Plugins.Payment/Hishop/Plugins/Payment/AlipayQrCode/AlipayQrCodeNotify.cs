namespace Hishop.Plugins.Payment.AlipayQrCode
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Xml;

    public class AlipayQrCodeNotify : PaymentNotify
    {
        private string buyer_email = "";
        private DateTime gmt_create;
        private DateTime gmt_payment;
        private const string InputCharset = "utf-8";
        private string notify_data = "";
        private string out_trade_no = "";
        private readonly NameValueCollection parameters;
        private string partner = "";
        private string qrcode = "";
        private string seller_email = "";
        private string subject = "";
        private decimal total_fee = 0M;
        private string trade_no = "";
        private string trade_status = "";

        public AlipayQrCodeNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
            this.notify_data = parameters["notify_data"];
            if ((this.notify_data != null) && (this.notify_data != ""))
            {
                this.LoadNotifydata(this.notify_data);
            }
        }

        private string CreateUrl(XmlNode configNode)
        {
            return string.Format(CultureInfo.InvariantCulture, "https://mapi.alipay.com/gateway.do?service=notify_verify&partner={0}&notify_id={1}", new object[] { configNode.SelectSingleNode("Partner").InnerText, this.parameters["notify_id"] });
        }

        public override string GetGatewayOrderId()
        {
            return this.trade_no;
        }

        public override decimal GetOrderAmount()
        {
            return this.total_fee;
        }

        public override string GetOrderId()
        {
            return this.out_trade_no;
        }

        public void LoadNotifydata(string notifydata)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(notifydata);
            XmlNode node = document.SelectSingleNode("notify/trade_no");
            if (node != null)
            {
                this.trade_no = node.InnerText;
            }
            node = document.SelectSingleNode("notify/out_trade_no");
            if (node != null)
            {
                this.out_trade_no = node.InnerText;
            }
            node = document.SelectSingleNode("notify/total_fee");
            if (node != null)
            {
                decimal.TryParse(node.InnerText, out this.total_fee);
            }
            node = document.SelectSingleNode("notify/trade_status");
            if (node != null)
            {
                this.trade_status = node.InnerText;
            }
            node = document.SelectSingleNode("notify/buyer_email");
            if (node != null)
            {
                this.buyer_email = node.InnerText;
            }
            node = document.SelectSingleNode("notify/subject");
            if (node != null)
            {
                this.subject = node.InnerText;
            }
            node = document.SelectSingleNode("notify/partner");
            if (node != null)
            {
                this.partner = node.InnerText;
            }
            node = document.SelectSingleNode("notify/qrcode");
            if (node != null)
            {
                this.qrcode = node.InnerText;
            }
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            bool flag = this.trade_status.ToLower() == "trade_success";
            this.parameters.Remove("HIGW");
            string[] allKeys = this.parameters.AllKeys;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string str in this.parameters.AllKeys)
            {
                if ((str != "sign") && (str != "sign_type"))
                {
                    dict.Add(str, this.parameters[str]);
                }
            }
            string password = Globals.BuildQuery(dict, false) + document.FirstChild.SelectSingleNode("Key").InnerText;
            string str3 = (this.parameters["sign"] == null) ? "" : this.parameters["sign"].ToLower();
            if (flag && (str3 == FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToLower()))
            {
                this.OnFinished(false);
            }
            else
            {
                this.OnNotifyVerifyFaild();
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

        public void WriteError(string Error, NameValueCollection parameters)
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
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Request.MapPath("/QRCodealipaynotify.xml"));
        }
    }
}

