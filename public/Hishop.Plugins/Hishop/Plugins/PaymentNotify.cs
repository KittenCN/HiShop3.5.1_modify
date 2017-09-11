namespace Hishop.Plugins
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Web;

    public abstract class PaymentNotify : IPlugin
    {
        public event EventHandler<FinishedEventArgs> Finished;

        public event EventHandler NotifyVerifyFaild;

        public event EventHandler Payment;

        protected PaymentNotify()
        {
        }

        public static PaymentNotify CreateInstance(string name, NameValueCollection parameters)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            object[] args = new object[] { parameters };
            PaymentPlugins plugins = PaymentPlugins.Instance();
            Type plugin = plugins.GetPlugin("PaymentRequest", name);
            if (plugin == null)
            {
                return null;
            }
            Type pluginWithNamespace = plugins.GetPluginWithNamespace("PaymentNotify", plugin.Namespace);
            if (pluginWithNamespace == null)
            {
                return null;
            }
            return (Activator.CreateInstance(pluginWithNamespace, args) as PaymentNotify);
        }

        public abstract string GetGatewayOrderId();
        public abstract decimal GetOrderAmount();
        public abstract string GetOrderId();
        public virtual string GetRemark1()
        {
            return string.Empty;
        }

        public virtual string GetRemark2()
        {
            return string.Empty;
        }

        protected virtual string GetResponse(string url, int timeout)
        {
            string str;
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                    {
                        StringBuilder builder = new StringBuilder();
                        while (-1 != reader.Peek())
                        {
                            builder.Append(reader.ReadLine());
                        }
                        return builder.ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                str = "Error:" + exception.Message;
            }
            return str;
        }

        protected virtual void OnFinished(bool isMedTrade)
        {
            if (this.Finished != null)
            {
                this.Finished(this, new FinishedEventArgs(isMedTrade));
            }
        }

        protected virtual void OnNotifyVerifyFaild()
        {
            if (this.NotifyVerifyFaild != null)
            {
                this.NotifyVerifyFaild(this, null);
            }
        }

        protected virtual void OnPayment()
        {
            if (this.Payment != null)
            {
                this.Payment(this, null);
            }
        }

        public abstract void VerifyNotify(int timeout, string configXml);
        public abstract void WriteBack(HttpContext context, bool success);

        public string ReturnUrl { get; set; }
    }
}

