namespace Hishop.Plugins.Payment.BankUnionGateWay
{
    using Hishop.Plugins;
    using Hishop.Plugins.Payment;
    using Hishop.Plugins.Payment.BankUnionGateWay.sdk;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Plugin("银联全渠道支付")]
    public class BankUnionGetWayRequest : PaymentRequest
    {
        public static BankUnionGateWayConfig BUConfig;
        private const string Gateway = "https://gateway.95516.com/gateway/api/frontTransReq.do";

        public BankUnionGetWayRequest()
        {
        }

        public BankUnionGetWayRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            if (BUConfig == null)
            {
                BUConfig = new BankUnionGateWayConfig();
            }
            BUConfig.OrderId = orderId;
            BUConfig.TxnAmt = Math.Round((decimal) (amount * 100M), 0).ToString();
            BUConfig.TxnTime = date.ToString("yyyyMMddHHmmss");
            BUConfig.FrontUrl = returnUrl;
            BUConfig.BackUrl = notifyUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            BUConfig.MerId = SDKConfig.MerId = this.Vmid;
            SDKConfig.SignCertPwd = this.Key;
            SDKConfig.signCertPath = Path.Combine(SDKConfig.validateCertDir, this.SignCertFileName);
            BUConfig.CertId = CertUtil.GetSignCertId();
            data.Add("version", BUConfig.Version);
            data.Add("encoding", BUConfig.Encoding);
            data.Add("certId", BUConfig.CertId);
            data.Add("signMethod", BUConfig.SignMethod);
            data.Add("txnType", BUConfig.TxnType);
            data.Add("txnSubType", BUConfig.TxnSubType);
            data.Add("bizType", BUConfig.BizType);
            data.Add("channelType", BUConfig.ChannelType);
            data.Add("frontUrl", BUConfig.FrontUrl);
            data.Add("backUrl", BUConfig.BackUrl);
            data.Add("accessType", BUConfig.AccessType);
            data.Add("merId", BUConfig.MerId);
            data.Add("orderId", BUConfig.OrderId);
            data.Add("txnTime", BUConfig.TxnTime);
            data.Add("txnAmt", BUConfig.TxnAmt);
            data.Add("currencyCode", BUConfig.CurrencyCode);
            data.Add("userMac", "userMac");
            SDKUtil.Sign(data, Encoding.UTF8);
            string msg = SDKUtil.CreateAutoSubmitForm("https://gateway.95516.com/gateway/api/frontTransReq.do", data, Encoding.UTF8);
            PayLog.writeLog(data, "", "", msg, LogType.BankUnion_GateWay);
            this.SubmitPaymentForm(msg);
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
                return false;
            }
        }

        [ConfigElement("证书密码", Nullable=false)]
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

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }

        [ConfigElement("证书文件名", Nullable=true)]
        public string SignCertFileName { get; set; }

        [ConfigElement("商户号", Nullable=false)]
        public string Vmid { get; set; }
    }
}

