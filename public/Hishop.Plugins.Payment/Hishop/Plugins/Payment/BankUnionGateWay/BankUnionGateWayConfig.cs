namespace Hishop.Plugins.Payment.BankUnionGateWay
{
    using System;

    public class BankUnionGateWayConfig
    {
        private string accessType = "0";
        private string backUrl;
        private string bizType = "000201";
        private string certId;
        private string channelType = "08";
        private string currencyCode = "156";
        private string encoding = "UTF-8";
        private string frontUrl;
        private string merId;
        private string orderId;
        private string signature;
        private string signMethod = "01";
        private string txnAmt;
        private string txnSubType = "01";
        private string txnTime;
        private string txnType = "01";
        private string version = "5.0.0";

        public string AccessType
        {
            get
            {
                return this.accessType;
            }
            set
            {
                this.accessType = value;
            }
        }

        public string BackUrl
        {
            get
            {
                return this.backUrl;
            }
            set
            {
                this.backUrl = value;
            }
        }

        public string BizType
        {
            get
            {
                return this.bizType;
            }
            set
            {
                this.bizType = value;
            }
        }

        public string CertId
        {
            get
            {
                return this.certId;
            }
            set
            {
                this.certId = value;
            }
        }

        public string ChannelType
        {
            get
            {
                return this.channelType;
            }
            set
            {
                this.channelType = value;
            }
        }

        public string CurrencyCode
        {
            get
            {
                return this.currencyCode;
            }
            set
            {
                this.currencyCode = value;
            }
        }

        public string Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }

        public string FrontUrl
        {
            get
            {
                return this.frontUrl;
            }
            set
            {
                this.frontUrl = value;
            }
        }

        public string MerId
        {
            get
            {
                return this.merId;
            }
            set
            {
                this.merId = value;
            }
        }

        public string OrderId
        {
            get
            {
                return this.orderId;
            }
            set
            {
                this.orderId = value;
            }
        }

        public string Signature
        {
            get
            {
                return this.signature;
            }
            set
            {
                this.signature = value;
            }
        }

        public string SignMethod
        {
            get
            {
                return this.signMethod;
            }
            set
            {
                this.signMethod = value;
            }
        }

        public string TxnAmt
        {
            get
            {
                return this.txnAmt;
            }
            set
            {
                this.txnAmt = value;
            }
        }

        public string TxnSubType
        {
            get
            {
                return this.txnSubType;
            }
            set
            {
                this.txnSubType = value;
            }
        }

        public string TxnTime
        {
            get
            {
                return this.txnTime;
            }
            set
            {
                this.txnTime = value;
            }
        }

        public string TxnType
        {
            get
            {
                return this.txnType;
            }
            set
            {
                this.txnType = value;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }
    }
}

