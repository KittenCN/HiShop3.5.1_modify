namespace Hishop.Plugins.Payment.AlipayAssure
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    [Plugin("支付宝担保交易", Sequence=1)]
    public class AssureRequest : PaymentRequest
    {
        private const string Agent = "C4335302345904805116";
        private readonly string body;
        private const string extend_param = "isv^yf31";
        private const string Gateway = "https://mapi.alipay.com/gateway.do?";
        private const string InputCharset = "utf-8";
        private const string LogisticsFee = "0.00";
        private const string LogisticsPayment = "BUYER_PAY";
        private const string LogisticsType = "EXPRESS";
        private readonly string notifyUrl;
        private readonly string outTradeNo;
        private const string PaymentType = "1";
        private readonly string price;
        private const string Quantity = "1";
        private readonly string returnUrl;
        private const string Service = "create_partner_trade_by_buyer";
        private readonly string showUrl;
        private const string SignType = "MD5";
        private readonly string subject;
        private string token;

        public AssureRequest()
        {
        }

        public AssureRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.returnUrl = returnUrl;
            this.notifyUrl = notifyUrl;
            this.body = body;
            this.outTradeNo = orderId;
            this.subject = subject;
            this.price = amount.ToString("F", CultureInfo.InvariantCulture);
            this.showUrl = showUrl;
            this.token = attach;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
            string url = Globals.CreateSendGoodsUrl(this.Partner, tradeno, logisticsName, invoiceno, transportType, this.Key, "utf-8");
            XmlDocument document = SendGoodsRequest(url);
            if (document != null)
            {
                XmlNode node = document.SelectSingleNode("alipay");
                if ((node != null) && !(node.SelectSingleNode("is_success").InnerText == "T"))
                {
                    XmlNode node2 = node.SelectSingleNode("error");
                    if ((node2 != null) && (node2.InnerText == "RETRY"))
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            document = SendGoodsRequest(url);
                            if ((document == null) || (document.SelectSingleNode("alipay").SelectSingleNode("is_success").InnerText == "T"))
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static XmlDocument SendGoodsRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Timeout = 0x1388;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StringBuilder builder = new StringBuilder();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                    {
                        while (-1 != reader.Peek())
                        {
                            builder.Append(reader.ReadLine());
                        }
                    }
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(builder.ToString());
                return document;
            }
            catch (Exception exception)
            {
                PayLog.AppendLog(null, "", url, exception.Message, LogType.Alipay_Assure);
                return null;
            }
        }

        public override void SendRequest()
        {
            this.RedirectToGateway(Globals.CreatUrl("https://mapi.alipay.com/gateway.do?", "create_partner_trade_by_buyer", this.Partner, "MD5", this.outTradeNo, this.subject, this.body, "1", this.price, this.showUrl, this.SellerEmail, this.Key, this.returnUrl, "utf-8", this.notifyUrl, "EXPRESS", "0.00", "BUYER_PAY", "1", "C4335302345904805116", "isv^yf31", this.token));
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool IsMedTrade
        {
            get
            {
                return true;
            }
        }

        [ConfigElement("安全校验码(Key)", Nullable=false)]
        public string Key { get; set; }

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        [ConfigElement("合作者身份(PID)", Nullable=false)]
        public string Partner { get; set; }

        [ConfigElement("收款支付宝账号", Nullable=false)]
        public string SellerEmail { get; set; }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

