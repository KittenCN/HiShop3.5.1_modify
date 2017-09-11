namespace Hishop.Plugins.Payment.Alipay_Bank
{
    using System;

    public class Config
    {
        private static string input_charset = "";
        private static string key = "";
        private static string partner = "";
        private static string sign_type = "";

        static Config()
        {
            partner = "";
            key = "";
            input_charset = "utf-8";
            sign_type = "MD5";
        }

        public static string Input_charset
        {
            get
            {
                return input_charset;
            }
        }

        public static string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        public static string Partner
        {
            get
            {
                return partner;
            }
            set
            {
                partner = value;
            }
        }

        public static string Sign_type
        {
            get
            {
                return sign_type;
            }
        }
    }
}

