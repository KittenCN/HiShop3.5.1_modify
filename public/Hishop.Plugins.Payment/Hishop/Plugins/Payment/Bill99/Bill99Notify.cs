namespace Hishop.Plugins.Payment.Bill99
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class Bill99Notify : PaymentNotify
    {
        private readonly NameValueCollection parameters;

        public Bill99Notify(NameValueCollection parameters)
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
            return this.parameters["dealId"];
        }

        public override decimal GetOrderAmount()
        {
            return (decimal.Parse(this.parameters["payAmount"]) / 100M);
        }

        public override string GetOrderId()
        {
            return this.parameters["orderId"];
        }

        public override void VerifyNotify(int timeout, string configXml)
        {
            string paramValue = this.parameters["merchantAcctId"];
            string str2 = this.parameters["version"];
            string str3 = this.parameters["language"];
            string str4 = this.parameters["signType"];
            string str5 = this.parameters["payType"];
            string str6 = this.parameters["bankId"];
            string str7 = this.parameters["orderId"];
            string str8 = this.parameters["orderTime"];
            string str9 = this.parameters["orderAmount"];
            string str10 = this.parameters["dealId"];
            string str11 = this.parameters["bankDealId"];
            string str12 = this.parameters["dealTime"];
            string str13 = this.parameters["payAmount"];
            string str14 = this.parameters["fee"];
            string str15 = this.parameters["ext1"];
            string str16 = this.parameters["ext2"];
            string str17 = this.parameters["payResult"];
            string str18 = this.parameters["errCode"];
            string s = this.parameters["signMsg"];
            if (!str17.Equals("10"))
            {
                this.OnNotifyVerifyFaild();
            }
            else
            {
                new XmlDocument().LoadXml(configXml);
                string returnStr = "";
                returnStr = AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(AppendParam(returnStr, "merchantAcctId", paramValue), "version", str2), "language", str3), "signType", str4), "payType", str5), "bankId", str6), "orderId", str7), "orderTime", str8), "orderAmount", str9), "dealId", str10), "bankDealId", str11), "dealTime", str12), "payAmount", str13), "fee", str14), "ext1", str15), "ext2", str16), "payResult", str17), "errCode", str18);
                byte[] bytes = Encoding.UTF8.GetBytes(returnStr);
                byte[] rgbSignature = Convert.FromBase64String(s);
                X509Certificate2 certificate = new X509Certificate2(HttpContext.Current.Server.MapPath("~/plugins/payment/Cert/99bill.cer"), "");
                RSACryptoServiceProvider key = (RSACryptoServiceProvider) certificate.PublicKey.Key;
                key.ImportCspBlob(key.ExportCspBlob(false));
                RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(key);
                deformatter.SetHashAlgorithm("SHA1");
                byte[] rgbHash = new SHA1CryptoServiceProvider().ComputeHash(bytes);
                if (!deformatter.VerifySignature(rgbHash, rgbSignature))
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
                int index = base.ReturnUrl.IndexOf("?");
                if (index > 0)
                {
                    base.ReturnUrl = base.ReturnUrl.Substring(0, index);
                }
                context.Response.Clear();
                context.Response.Write(success ? string.Format("<result>1</result><redirecturl>{0}</redirecturl>", base.ReturnUrl) : string.Format("<result>0</result><redirecturl>{0}</redirecturl>", base.ReturnUrl));
                context.Response.End();
            }
        }
    }
}

