namespace Hishop.Plugins.Payment.Paypal
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;

    public class PaypalNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public PaypalNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return string.Empty;
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["mc_gross"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["item_number"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://www.paypal.com/cgi-bin/webscr");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string str2 = Encoding.ASCII.GetString(bytes) + "&cmd=_notify-validate";
            request.ContentLength = str2.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(str2);
            writer.Close();
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string str3 = reader.ReadToEnd();
            reader.Close();
            string str4 = this.parameters["payment_status"];
            if ((str3 == "VERIFIED") && str4.Equals("Completed"))
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
        }
    }
}

