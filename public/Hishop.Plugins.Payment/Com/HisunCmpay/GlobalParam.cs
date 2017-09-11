namespace Com.HisunCmpay
{
    using System;
    using System.Xml;

    public class GlobalParam
    {
        private static GlobalParam instance = null;
        private string mCallbackUrl;
        private string mCharacterSet;
        private string mMerchantId;
        private string mNotifyUrl;
        private string mReqUrl;
        private string mSignKey;
        private string mSignType;
        private string mVersion;
        private static string resourceFile = "profile/MerchantInfo.xml";

        private GlobalParam()
        {
            this.init();
        }

        public static GlobalParam getInstance()
        {
            if (instance == null)
            {
                instance = new GlobalParam();
            }
            return instance;
        }

        private string getXMLValue(XmlElement root, string name)
        {
            XmlNodeList elementsByTagName = root.GetElementsByTagName(name);
            if (elementsByTagName.Count <= 0)
            {
                return "";
            }
            return elementsByTagName.Item(0).FirstChild.Value;
        }

        private void init()
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(AppDomain.CurrentDomain.BaseDirectory + resourceFile);
            }
            catch (Exception)
            {
                return;
            }
            XmlElement documentElement = document.DocumentElement;
            if (documentElement != null)
            {
                this.mCharacterSet = this.getXMLValue(documentElement, "characterSet");
                this.mCallbackUrl = this.getXMLValue(documentElement, "callbackUrl");
                this.mNotifyUrl = this.getXMLValue(documentElement, "notifyUrl");
                this.mMerchantId = this.getXMLValue(documentElement, "merchantId");
                this.mSignKey = this.getXMLValue(documentElement, "signKey");
                this.mReqUrl = this.getXMLValue(documentElement, "reqUrl");
                this.mVersion = this.getXMLValue(documentElement, "version");
                this.mSignType = this.getXMLValue(documentElement, "signType");
            }
        }

        public string callbackUrl
        {
            get
            {
                return this.mCallbackUrl;
            }
            set
            {
                this.mCallbackUrl = value;
            }
        }

        public string characterSet
        {
            get
            {
                return this.mCharacterSet;
            }
            set
            {
                this.mCharacterSet = value;
            }
        }

        public string merchantId
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

        public string notifyUrl
        {
            get
            {
                return this.mNotifyUrl;
            }
            set
            {
                this.mNotifyUrl = value;
            }
        }

        public string reqUrl
        {
            get
            {
                return this.mReqUrl;
            }
            set
            {
                this.mReqUrl = value;
            }
        }

        public string signKey
        {
            get
            {
                return this.mSignKey;
            }
            set
            {
                this.mSignKey = value;
            }
        }

        public string signType
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

        public string version
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

