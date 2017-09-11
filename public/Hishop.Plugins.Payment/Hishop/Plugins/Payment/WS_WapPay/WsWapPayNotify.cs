namespace Hishop.Plugins.Payment.WS_WapPay
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Xml;

    public class WsWapPayNotify : PaymentNotify
    {
        private string buyer_email = "";
        private string buyer_id;
        private const string InpuitCharset = "utf-8";
        private bool IsNotify = true;
        private string notify_data = "";
        private string notify_id;
        private DateTime notify_time;
        private string notify_type = "";
        private string out_trade_no = "";
        private readonly NameValueCollection parameters;
        private decimal price = 0M;
        private int quantity = 0;
        private string seller_email = "";
        private string seller_id = "";
        private decimal total_fee = 0M;
        private string trade_no = "";
        private string trade_status = "";

        public WsWapPayNotify(NameValueCollection parameters)
        {
            if ((parameters["IsReturn"] != null) && (parameters["IsReturn"].ToLower() == "true"))
            {
                parameters.Remove("IsReturn");
                this.IsNotify = false;
            }
            this.parameters = parameters;
            if (this.IsNotify)
            {
                this.notify_data = parameters["notify_data"];
                this.LoadNotifydata(this.notify_data);
            }
            else
            {
                this.LoadReturndata();
            }
        }

        public static string BuildQuery(IDictionary<string, string> dict, bool encode)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(dict);
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            IEnumerator<KeyValuePair<string, string>> enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                current = enumerator.Current;
                string str2 = current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str2))
                {
                    if (flag)
                    {
                        builder.Append("&");
                    }
                    builder.Append(key);
                    builder.Append("=");
                    if ((encode && (key.ToLower() != "sign")) && (key.ToLower() != "sign_type"))
                    {
                        builder.Append(HttpUtility.UrlEncode(str2, Encoding.UTF8));
                    }
                    else
                    {
                        builder.Append(str2);
                    }
                    flag = true;
                }
            }
            return builder.ToString();
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

        public SortedDictionary<string, string> GetRequestGet()
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            string str = HttpContext.Current.Request.Url.Query.Replace("?", "");
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArray = str.Split(new char[] { '&' });
                string[] strArray2 = new string[0];
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray2 = strArray[i].Split(new char[] { '=' });
                    dictionary.Add(strArray2[0], strArray2[1]);
                }
            }
            return dictionary;
        }

        public void LoadNotifydata(string notifydata)
        {
            try
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
                node = document.SelectSingleNode("notify/seller_email");
                if (node != null)
                {
                    this.seller_email = node.InnerText;
                }
                node = document.SelectSingleNode("notify/quantity");
                if (node != null)
                {
                    int.TryParse(node.InnerText, out this.quantity);
                }
                node = document.SelectSingleNode("notify/price");
                if (node != null)
                {
                    decimal.TryParse(node.InnerText, out this.price);
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
                node = document.SelectSingleNode("notify/seller_email");
                if (node != null)
                {
                    this.seller_email = node.InnerText;
                }
                node = document.SelectSingleNode("notify/buyer_id");
                if (node != null)
                {
                    this.buyer_id = node.InnerText;
                }
            }
            catch (Exception exception)
            {
                PayLog.writeLog_Collection(this.parameters, "", "", "加载notifyData错误：" + exception.Message, LogType.WS_WapPay);
            }
        }

        public void LoadReturndata()
        {
            this.trade_no = this.parameters["trade_no"];
            this.out_trade_no = this.parameters["out_trade_no"];
            this.trade_status = this.parameters["result"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            if (!this.IsNotify)
            {
                this.VerifyReturn(timeout, configXml);
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                this.parameters.Remove("HIGW");
                string[] allKeys = this.parameters.AllKeys;
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (string str in this.parameters.AllKeys)
                {
                    if ((str != "sign") && (str != "sign_type"))
                    {
                        dictionary.Add(str, this.parameters[str]);
                    }
                }
                string prestr = "service=" + this.parameters["service"] + "&v=" + this.parameters["v"] + "&sec_id=" + this.parameters["sec_id"] + "&notify_data=" + this.parameters["notify_data"] + document.FirstChild.SelectSingleNode("Key").InnerText.Trim();
                string sign = Function.Sign(prestr, "MD5", "utf-8");
                string str4 = (this.parameters["sign"] == null) ? "" : this.parameters["sign"].ToLower();
                if (!(str4 == sign))
                {
                    PayLog.writeLog_Collection(this.parameters, sign, "", "验签失败Return---" + document.FirstChild.SelectSingleNode("Key").InnerText + "---" + prestr, LogType.WS_WapPay);
                    this.OnNotifyVerifyFaild();
                }
                else if (this.trade_status.ToLower() != "trade_success")
                {
                    PayLog.writeLog_Collection(this.parameters, sign, "", "通知结果不为成功---" + prestr, LogType.WS_WapPay);
                    this.OnNotifyVerifyFaild();
                }
                else
                {
                    this.OnFinished(false);
                }
            }
        }

        public void VerifyReturn(int timeout, string configXml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string str in this.parameters.AllKeys)
            {
                if (((str != "sign") && (str != "sign_type")) && (str.ToLower() != "higw"))
                {
                    dict.Add(str, this.parameters[str]);
                }
            }
            string prestr = BuildQuery(dict, false) + document.FirstChild.SelectSingleNode("Key").InnerText;
            string sign = Function.Sign(prestr, "MD5", "utf-8");
            string str4 = (this.parameters["sign"] == null) ? "" : this.parameters["sign"].ToLower();
            if (sign != str4)
            {
                PayLog.writeLog_Collection(this.parameters, sign, "", "验签失败Return---" + document.FirstChild.SelectSingleNode("Key").InnerText + "---" + prestr, LogType.WS_WapPay);
                this.OnNotifyVerifyFaild();
            }
            else if (this.trade_status.ToLower() != "success")
            {
                PayLog.writeLog_Collection(this.parameters, sign, "", "通知结果不为成功---" + prestr, LogType.WS_WapPay);
                this.OnNotifyVerifyFaild();
            }
            else
            {
                this.OnFinished(false);
            }
        }

        public override void WriteBack(HttpContext context, bool success)
        {
            try
            {
                if (context != null)
                {
                    context.Response.Clear();
                    context.Response.Write(success ? "success" : "fail");
                    context.Response.End();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {
            }
        }

        public void WriteError(string Error, NameValueCollection parameters)
        {
            DataTable table = new DataTable {
                TableName = "alipay"
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
            if (this.IsNotify)
            {
                table.WriteXml(HttpContext.Current.Request.MapPath("/alipaynotify_.xml"));
            }
            else
            {
                table.WriteXml(HttpContext.Current.Request.MapPath("/alipayreturn_.xml"));
            }
        }
    }
}

