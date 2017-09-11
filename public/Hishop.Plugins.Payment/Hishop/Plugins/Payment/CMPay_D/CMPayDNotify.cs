namespace Hishop.Plugins.Payment.CMPay_D
{
    using Com.HisunCmpay;
    using Hishop.Plugins;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Xml;

    public class CMPayDNotify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public CMPayDNotify(NameValueCollection parameters)
        {
            this.parameters = parameters;
        }

        private static string AppendParam(string returnStr, string paramId, string paramValue)
        {
            if (returnStr != "")
            {
                if (paramValue != "")
                {
                    string str2 = returnStr;
                    returnStr = str2 + "&" + paramId + "=" + paramValue;
                }
                return returnStr;
            }
            if (paramValue != "")
            {
                returnStr = paramId + "=" + paramValue;
            }
            return returnStr;
        }

        public override string GetGatewayOrderId()
        {
            return (string) IPosMUtil.parseStringToMap(IPosMUtil.keyValueToString(this.parameters))["payNo"];
        }

        public override decimal GetOrderAmount()
        {
            string s = (string) IPosMUtil.parseStringToMap(IPosMUtil.keyValueToString(this.parameters))["amount"];
            return (decimal.Parse(s) / 100M);
        }

        public override string GetOrderId()
        {
            return (string) IPosMUtil.parseStringToMap(IPosMUtil.keyValueToString(this.parameters))["orderId"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            Hashtable hashtable = IPosMUtil.parseStringToMap(IPosMUtil.keyValueToString(this.parameters));
            XmlDocument document = new XmlDocument();
            document.LoadXml(configXml);
            string innerText = document.FirstChild.SelectSingleNode("Key").InnerText;
            string str3 = (string) hashtable["merchantId"];
            string str4 = (string) hashtable["payNo"];
            string str5 = (string) hashtable["returnCode"];
            string str6 = (string) hashtable["message"];
            string str7 = (string) hashtable["signType"];
            string str8 = (string) hashtable["type"];
            string str9 = (string) hashtable["version"];
            string str10 = (string) hashtable["amount"];
            string str11 = (string) hashtable["amtItem"];
            string str12 = (string) hashtable["bankAbbr"];
            string str13 = (string) hashtable["mobile"];
            string str14 = (string) hashtable["orderId"];
            string str15 = (string) hashtable["payDate"];
            string str16 = (string) hashtable["accountDate"];
            string str17 = (string) hashtable["reserved1"];
            string str18 = (string) hashtable["reserved2"];
            string str19 = (string) hashtable["status"];
            string str20 = (string) hashtable["orderDate"];
            string str21 = (string) hashtable["fee"];
            string hmac = (string) hashtable["hmac"];
            string source = str3 + str4 + str5 + str6 + str7 + str8 + str9 + str10 + str11 + str12 + str13 + str14 + str15 + str16 + str17 + str18 + str19 + str20 + str21;
            if (!"000000".Equals(str5))
            {
                this.OnNotifyVerifyFaild();
            }
            else if ("MD5".Equals(GlobalParam.getInstance().signType) && !SignUtil.verifySign(source, innerText, hmac))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                this.OnFinished(false);
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
    }
}

