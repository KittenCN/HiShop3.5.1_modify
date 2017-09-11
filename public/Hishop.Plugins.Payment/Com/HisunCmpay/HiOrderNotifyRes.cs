namespace Com.HisunCmpay
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;

    public class HiOrderNotifyRes
    {
        private string mAccountDate;
        private string mAmount;
        private string mAmtItem;
        private string mBankAbbr;
        private string mErrMsg;
        private string mFee;
        private string mHmac;
        private string mMerchantId;
        private string mMessage;
        private string mMobile;
        private string mOrderDate;
        private string mOrderId;
        private string mPayDate;
        private string mPayNo;
        private string mReserved1;
        private string mReserved2;
        private string mReturnCode;
        private string mServerCert;
        private string mSignType;
        private string mStatus;
        private string mType;
        private string mVersion;

        public HiOrderNotifyRes(NameValueCollection param)
        {
            string source = IPosMUtil.keyValueToString(param);
            this.parseSource(source);
            string str2 = this.getSource();
            if ("000000".Equals(this.mReturnCode))
            {
                if (!"MD5".Equals(GlobalParam.getInstance().signType))
                {
                    return;
                }
                if (!SignUtil.verifySign(str2, GlobalParam.getInstance().signKey, this.mHmac))
                {
                    this.mErrMsg = "verify signature failed：returnCode = " + this.mReturnCode + "&message = " + this.mMessage;
                    throw new Exception(this.mErrMsg);
                }
            }
            this.mErrMsg = "notify failed：returnCode = " + this.mReturnCode + "&message = " + this.mMessage;
            throw new Exception(this.mErrMsg);
        }

        private string getSource()
        {
            string str = "";
            return ((((((((((str + this.mMerchantId) + this.mPayNo + this.mReturnCode) + this.mMessage + this.mSignType) + this.mType + this.mVersion) + this.mAmount + this.mAmtItem) + this.mBankAbbr + this.mMobile) + this.mOrderId + this.mPayDate) + this.mAccountDate + this.mReserved1) + this.mReserved2 + this.mStatus) + this.mOrderDate + this.mFee);
        }

        private bool parseSource(string source)
        {
            Hashtable hashtable = IPosMUtil.parseStringToMap(source);
            this.mHmac = (string) hashtable["hmac"];
            this.mMerchantId = (string) hashtable["merchantId"];
            this.mPayNo = (string) hashtable["payNo"];
            this.mReturnCode = (string) hashtable["returnCode"];
            this.mMessage = (string) hashtable["message"];
            this.mSignType = (string) hashtable["signType"];
            this.mType = (string) hashtable["type"];
            this.mVersion = (string) hashtable["version"];
            this.mAmount = (string) hashtable["amount"];
            this.mAmtItem = (string) hashtable["amtItem"];
            this.mBankAbbr = (string) hashtable["bankAbbr"];
            this.mMobile = (string) hashtable["mobile"];
            this.mOrderId = (string) hashtable["orderId"];
            this.mPayDate = (string) hashtable["payDate"];
            this.mAccountDate = (string) hashtable["accountDate"];
            this.mReserved1 = (string) hashtable["reserved1"];
            this.mReserved2 = (string) hashtable["reserved2"];
            this.mStatus = (string) hashtable["status"];
            this.mOrderDate = (string) hashtable["orderDate"];
            this.mFee = (string) hashtable["fee"];
            return true;
        }

        public string AccountDate
        {
            get
            {
                return this.mAccountDate;
            }
            set
            {
                this.mAccountDate = value;
            }
        }

        public string Amount
        {
            get
            {
                return this.mAmount;
            }
            set
            {
                this.mAmount = value;
            }
        }

        public string AmtItem
        {
            get
            {
                return this.mAmtItem;
            }
            set
            {
                this.mAmtItem = value;
            }
        }

        public string BankAbbr
        {
            get
            {
                return this.mBankAbbr;
            }
            set
            {
                this.mBankAbbr = value;
            }
        }

        public string ErrMsg
        {
            get
            {
                return this.mErrMsg;
            }
            set
            {
                this.mErrMsg = value;
            }
        }

        public string Fee
        {
            get
            {
                return this.mFee;
            }
            set
            {
                this.mFee = value;
            }
        }

        public string Hmac
        {
            get
            {
                return this.mHmac;
            }
            set
            {
                this.mHmac = value;
            }
        }

        public string MerchantId
        {
            get
            {
                return this.mMerchantId;
            }
            set
            {
                this.mMerchantId = value;
            }
        }

        public string Message
        {
            get
            {
                return this.mMessage;
            }
            set
            {
                this.mMessage = value;
            }
        }

        public string Mobile
        {
            get
            {
                return this.mMobile;
            }
            set
            {
                this.mMobile = value;
            }
        }

        public string OrderDate
        {
            get
            {
                return this.mOrderDate;
            }
            set
            {
                this.mOrderDate = value;
            }
        }

        public string OrderId
        {
            get
            {
                return this.mOrderId;
            }
            set
            {
                this.mOrderId = value;
            }
        }

        public string PayDate
        {
            get
            {
                return this.mPayDate;
            }
            set
            {
                this.mPayDate = value;
            }
        }

        public string PayNo
        {
            get
            {
                return this.mPayNo;
            }
            set
            {
                this.mPayNo = value;
            }
        }

        public string Reserved1
        {
            get
            {
                return this.mReserved1;
            }
            set
            {
                this.mReserved1 = value;
            }
        }

        public string Reserved2
        {
            get
            {
                return this.mReserved2;
            }
            set
            {
                this.mReserved2 = value;
            }
        }

        public string ReturnCode
        {
            get
            {
                return this.mReturnCode;
            }
            set
            {
                this.mReturnCode = value;
            }
        }

        public string ServerCert
        {
            get
            {
                return this.mServerCert;
            }
            set
            {
                this.mServerCert = value;
            }
        }

        public string SignType
        {
            get
            {
                return this.mSignType;
            }
            set
            {
                this.mSignType = value;
            }
        }

        public string Status
        {
            get
            {
                return this.mStatus;
            }
            set
            {
                this.mStatus = value;
            }
        }

        public string Type
        {
            get
            {
                return this.mType;
            }
            set
            {
                this.mType = value;
            }
        }

        public string Version
        {
            get
            {
                return this.mVersion;
            }
            set
            {
                this.mVersion = value;
            }
        }
    }
}

