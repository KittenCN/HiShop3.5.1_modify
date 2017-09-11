namespace Hishop.Plugins.Payment.WS_WapPay
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    public class Service
    {
        public string alipay_Wap_Auth_AuthAndExecute(string requrl, string secid, string partner, string callbackurl, string format, string version, string service, string token, string input_charset, string gatway, string key, string sign_type)
        {
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            string str = "<auth_and_execute_req><request_token>" + token + "</request_token></auth_and_execute_req>";
            sParaTemp.Add("req_data", str);
            sParaTemp.Add("service", service);
            sParaTemp.Add("sec_id", secid);
            sParaTemp.Add("partner", partner);
            sParaTemp.Add("format", format);
            sParaTemp.Add("v", version);
            return Submit.SendPostRedirect(requrl, sParaTemp, gatway, input_charset, key, sign_type);
        }

        public string alipay_wap_trade_create_direct(string requrl, string subject, string outTradeNo, string totalFee, string sellerAccountName, string notifyUrl, string outUser, string merchantUrl, string callbackurl, string service, string secid, string partner, string reqid, string format, string version, string input_charset, string gatway, string key, string sign_type)
        {
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            string str = "<direct_trade_create_req><subject>" + subject + "</subject><out_trade_no>" + outTradeNo + "</out_trade_no><total_fee>" + totalFee + "</total_fee><seller_account_name>" + sellerAccountName + "</seller_account_name><notify_url>" + notifyUrl + "</notify_url><out_user>" + outUser + "</out_user><merchant_url>" + merchantUrl + "</merchant_url><call_back_url>" + callbackurl + "</call_back_url></direct_trade_create_req>";
            sParaTemp.Add("req_data", str);
            sParaTemp.Add("service", service);
            sParaTemp.Add("sec_id", secid);
            sParaTemp.Add("partner", partner);
            sParaTemp.Add("req_id", reqid);
            sParaTemp.Add("format", format);
            sParaTemp.Add("v", version);
            string[] strArray = HttpUtility.UrlDecode(Submit.SendPostInfo(sParaTemp, gatway, input_charset, key, sign_type), Encoding.GetEncoding(input_charset)).Split(new char[] { '&' });
            string xmlDoc = string.Empty;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].IndexOf("res_data=") >= 0)
                {
                    xmlDoc = strArray[i].Replace("res_data=", string.Empty);
                }
            }
            string strForXmlDoc = string.Empty;
            try
            {
                strForXmlDoc = Function.GetStrForXmlDoc(xmlDoc, "direct_trade_create_res/request_token");
            }
            catch
            {
                return string.Empty;
            }
            return strForXmlDoc;
        }
    }
}

