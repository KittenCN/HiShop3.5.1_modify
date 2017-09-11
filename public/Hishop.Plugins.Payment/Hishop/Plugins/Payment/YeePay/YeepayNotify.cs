namespace Hishop.Plugins.Payment.YeePay
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Xml;

    public class YeepayNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public YeepayNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return this.parameters["r2_TrxId"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["r3_Amt"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["r6_Order"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string sCmd = this.parameters["r0_Cmd"];
            string merchantId = this.parameters["p1_MerId"];
            string sErrorCode = this.parameters["r1_Code"];
            string sTrxId = this.parameters["r2_TrxId"];
            string amount = this.parameters["r3_Amt"];
            string cur = this.parameters["r4_Cur"];
            string productId = this.parameters["r5_Pid"];
            string orderId = this.parameters["r6_Order"];
            string userId = this.parameters["r7_Uid"];
            string mp = this.parameters["r8_MP"];
            string bType = this.parameters["r9_BType"];
            string hmac = this.parameters["hmac"];
            if (sErrorCode != "1")
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(configXml);
                if (!Buy.VerifyCallback(merchantId, document.FirstChild.SelectSingleNode("KeyValue").InnerText, sCmd, sErrorCode, sTrxId, amount, cur, productId, orderId, userId, mp, bType, hmac))
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
        }
    }
}

