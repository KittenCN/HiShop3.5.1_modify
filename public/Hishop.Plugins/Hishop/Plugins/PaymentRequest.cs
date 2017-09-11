namespace Hishop.Plugins
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Xml;

    public abstract class PaymentRequest : ConfigablePlugin, IPlugin
    {
        private const string FormFormat = "<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">{1}</form>";
        private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";

        protected PaymentRequest()
        {
        }

        protected virtual string CreateField(string name, string strValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", new object[] { name, strValue });
        }

        protected virtual string CreateForm(string content, string action)
        {
            content = content + "<input type=\"submit\" value=\"在线支付\" style=\"display:none;\">";
            return string.Format(CultureInfo.InvariantCulture, "<form id=\"payform\" name=\"payform\" action=\"{0}\" method=\"POST\">{1}</form>", new object[] { action, content });
        }

        public static PaymentRequest CreateInstance(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            Type plugin = PaymentPlugins.Instance().GetPlugin("PaymentRequest", name);
            if (plugin == null)
            {
                return null;
            }
            return (Activator.CreateInstance(plugin) as PaymentRequest);
        }

        public static PaymentRequest CreateInstance(string name, string configXml, string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            object[] args = new object[] { orderId, amount, subject, body, buyerEmail, date, showUrl, returnUrl, notifyUrl, attach };
            Type plugin = PaymentPlugins.Instance().GetPlugin("PaymentRequest", name);
            if (plugin == null)
            {
                return null;
            }
            PaymentRequest request = Activator.CreateInstance(plugin, args) as PaymentRequest;
            if (!((request == null) || string.IsNullOrEmpty(configXml)))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                request.InitConfig(document.FirstChild);
            }
            return request;
        }

        protected virtual void RedirectToGateway(string url)
        {
            HttpContext.Current.Response.Redirect(url, true);
        }

        public abstract void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType);
        public abstract void SendRequest();
        protected virtual void SubmitPaymentForm(string formContent)
        {
            string s = formContent + "<script>document.forms['payform'].submit();</script>";
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }

        public abstract bool IsMedTrade { get; }
    }
}

