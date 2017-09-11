namespace Hidistro.ControlPanel.OutPay.App
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Notify
    {
        private string _public_key = "";
        private string Https_veryfy_url = "https://mapi.alipay.com/gateway.do?service=notify_verify&";

        public Notify()
        {
            this._public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
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
            string strUrl = this.Https_veryfy_url + "partner=" + Core._partner + "&notify_id=" + notify_id;
            return this.Get_Http(strUrl, 0x1d4c0);
        }

        private bool GetSignVeryfy(SortedDictionary<string, string> inputPara, string sign, string _private_key = "")
        {
            string str2;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string content = Core.CreateLinkString(Core.FilterPara(inputPara));
            bool flag = false;
            if (((sign == null) || !(sign != "")) || ((str2 = Core._sign_type) == null))
            {
                return flag;
            }
            if (!(str2 == "RSA"))
            {
                if (str2 != "MD5")
                {
                    return flag;
                }
            }
            else
            {
                return RSAFromPkcs8.verify(content, sign, this._public_key, Core._input_charset);
            }
            return (Core.GetMD5(content + Core._private_key, Core._input_charset) == sign);
        }

        public bool Verify(SortedDictionary<string, string> inputPara, string notify_id, string sign)
        {
            bool flag = this.GetSignVeryfy(inputPara, sign, "");
            string responseTxt = "true";
            if ((notify_id != null) && (notify_id != ""))
            {
                responseTxt = this.GetResponseTxt(notify_id);
            }
            return ((responseTxt == "true") && flag);
        }
    }
}

