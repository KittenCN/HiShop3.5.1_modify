namespace Hishop.Plugins.OpenId.AliPay
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Xml;

    public class AliPayNotify : OpenIdNotify
    {
        private readonly SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();

        public AliPayNotify(NameValueCollection _parameters)
        {
            string[] allKeys = _parameters.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                this.parameters.Add(allKeys[i], _parameters[allKeys[i]]);
            }
            this.parameters.Remove("HIGW");
            this.parameters.Remove("HITO");
        }

        private string CreateUrl(XmlNode configNode)
        {
            return string.Format(CultureInfo.InvariantCulture, "https://mapi.alipay.com/gateway.do?service=notify_verify&partner={0}&notify_id={1}", new object[] { configNode.SelectSingleNode("Partner").InnerText, this.parameters["notify_id"] });
        }

        public override void Verify(int timeout, string configXml)
        {
            bool flag;
            string message = null;
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            try
            {
                flag = bool.Parse(this.GetResponse(this.CreateUrl(document.FirstChild), timeout));
            }
            catch
            {
                flag = false;
                message = "支付宝通知消息验证未通过";
            }
            if (flag)
            {
                Dictionary<string, string> dicArray = Globals.Parameterfilter(this.parameters);
                Globals.CreateLinkstring(dicArray);
                flag = Globals.BuildSign(dicArray, document.FirstChild.SelectSingleNode("Key").InnerText, "MD5", "utf-8") == this.parameters["sign"];
                if (!flag)
                {
                    message = "支付宝签名验证未通过";
                }
            }
            if (flag)
            {
                this.OnAuthenticated(this.parameters["user_id"]);
            }
            else
            {
                this.OnFailed(message);
            }
        }
    }
}

