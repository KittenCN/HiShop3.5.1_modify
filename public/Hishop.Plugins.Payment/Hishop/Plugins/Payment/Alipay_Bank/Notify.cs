namespace Hishop.Plugins.Payment.Alipay_Bank
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    public class Notify
    {
        private string _input_charset = "";
        private string _key = "";
        private string _partner = "";
        private string _sign_type = "";
        private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";

        public Notify()
        {
            this._partner = Config.Partner.Trim();
            this._key = Config.Key.Trim();
            this._input_charset = Config.Input_charset.Trim().ToLower();
            this._sign_type = Config.Sign_type.Trim().ToUpper();
        }

        private string Get_Http(string strUrl, int timeout)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(strUrl);
                request.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                StringBuilder builder = new StringBuilder();
                while (-1 != reader.Peek())
                {
                    builder.Append(reader.ReadLine());
                }
                return builder.ToString();
            }
            catch (Exception exception)
            {
                return ("错误：" + exception.Message);
            }
        }

        private string GetPreSignStr(SortedDictionary<string, string> inputPara)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            return Core.CreateLinkString(Core.FilterPara(inputPara));
        }

        private string GetResponseTxt(string notify_id)
        {
            string strUrl = this.Https_veryfy_url + "partner=" + this._partner + "&notify_id=" + notify_id;
            return this.Get_Http(strUrl, 0x1d4c0);
        }

        private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign)
        {
            string str;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string prestr = Core.CreateLinkString(Core.FilterPara(inputPara));
            bool flag = false;
            if (((sign != null) && (sign != "")) && (((str = this._sign_type) != null) && (str == "MD5")))
            {
                flag = AlipayMD5.Verify(prestr, sign, this._key, this._input_charset);
            }
            return flag;
        }

        public bool Verify(SortedDictionary<string, string> inputPara, string notify_id, string sign)
        {
            bool signVeryfy = this.GetSignVeryfy(inputPara, sign);
            string responseTxt = "true";
            if ((notify_id != null) && (notify_id != ""))
            {
                responseTxt = this.GetResponseTxt(notify_id);
            }
            return ((responseTxt == "true") && signVeryfy);
        }
    }
}

