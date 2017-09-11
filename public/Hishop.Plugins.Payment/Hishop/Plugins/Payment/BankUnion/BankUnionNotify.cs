namespace Hishop.Plugins.Payment.BankUnion
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Xml;

    public class BankUnionNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public BankUnionNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return string.Empty;
        }

        public override decimal GetOrderAmount()
        {
            return (decimal.Parse(this.parameters[QuickPayConf.notifyVo[6]]) / 100M);
        }

        public override string GetOrderId()
        {
            return this.parameters[QuickPayConf.notifyVo[8]];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string[] valueVo = new string[QuickPayConf.notifyVo.Length];
            for (int i = 0; i < QuickPayConf.notifyVo.Length; i++)
            {
                valueVo[i] = this.parameters[QuickPayConf.notifyVo[i]];
            }
            string str = this.parameters[QuickPayConf.signature];
            string str2 = this.parameters[QuickPayConf.signMethod];
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str2))
            {
                this.OnNotifyVerifyFaild();
            }
            else if (!valueVo[10].Equals("00"))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                QuickPayConf.securityKey = document.FirstChild.SelectSingleNode("Key").InnerText;
                if (!new QuickPayUtils().checkSign(valueVo, str2, str))
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
            if (context != null)
            {
                context.Response.Clear();
                context.Response.Write(success ? "ok" : "error");
                context.Response.End();
            }
        }
    }
}

