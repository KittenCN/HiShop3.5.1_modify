namespace Hishop.Plugins.Payment.GoPay
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Xml;

    public class GoPayNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public GoPayNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        public override string GetGatewayOrderId()
        {
            return this.parameters["orderid"];
        }

        public override decimal GetOrderAmount()
        {
            return decimal.Parse(this.parameters["tranAmt"]);
        }

        public override string GetOrderId()
        {
            return this.parameters["merOrderNum"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string str = this.parameters["version"];
            string str2 = this.parameters["charset"];
            string str3 = this.parameters["language"];
            string str4 = this.parameters["signType"];
            string str5 = this.parameters["tranCode"];
            string str6 = this.parameters["merchantID"];
            string str7 = this.parameters["merOrderNum"];
            string str8 = this.parameters["tranAmt"];
            string str9 = this.parameters["feeAmt"];
            string str10 = this.parameters["frontMerUrl"];
            string str11 = this.parameters["backgroundMerUrl"];
            string str12 = this.parameters["tranDateTime"];
            string str13 = this.parameters["tranIP"];
            string str14 = this.parameters["respCode"];
            string str15 = this.parameters["msgExt"];
            string gatewayOrderId = this.GetGatewayOrderId();
            string str17 = this.parameters["gopayOutOrderId"];
            string str18 = this.parameters["bankCode"];
            string str19 = this.parameters["tranFinishTime"];
            string str20 = this.parameters["merRemark1"];
            string str21 = this.parameters["merRemark2"];
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            string innerText = document.FirstChild.SelectSingleNode("VerficationCode").InnerText;
            string str23 = this.parameters["signValue"];
            if (Globals.GetMD5("version=[" + str + "]tranCode=[" + str5 + "]merchantID=[" + str6 + "]merOrderNum=[" + str7 + "]tranAmt=[" + str8 + "]feeAmt=[" + str9 + "]tranDateTime=[" + str12 + "]frontMerUrl=[" + str10 + "]backgroundMerUrl=[" + str11 + "]orderId=[" + gatewayOrderId + "]gopayOutOrderId=[" + str17 + "]tranIP=[" + str13 + "]respCode=[" + str14 + "]gopayServerTime=[]VerficationCode=[" + innerText + "]").Equals(str23) && str14.Equals("0000"))
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
            if ((context != null) && success)
            {
                context.Response.Clear();
                context.Response.Write("OK");
                context.Response.End();
            }
        }
    }
}

