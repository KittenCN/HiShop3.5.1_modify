namespace Hishop.Plugins.Payment.YeePay
{
    using System;
    using System.Text;
    using System.Web;

    internal static class Buy
    {
        private static string nodeAuthorizationURL = "https://www.yeepay.com/app-merchant-proxy/node";

        internal static string CreateUrl(string merchantId, string keyValue, string orderId, string amount, string cur, string productId, string merchantCallbackURL, string addressFlag, string sMctProperties, string frpId)
        {
            string str = "Buy";
            string str2 = "1";
            Digest digest = new Digest();
            string aValue = "";
            aValue = (((((aValue + str) + merchantId + orderId) + amount + cur) + productId + merchantCallbackURL) + addressFlag + sMctProperties) + frpId + str2;
            string str4 = digest.HmacSign(aValue, keyValue);
            string str5 = "";
            return (((((((((((((str5 + nodeAuthorizationURL) + "?p0_Cmd=" + str) + "&p1_MerId=" + merchantId) + "&p2_Order=" + orderId) + "&p3_Amt=" + amount) + "&p4_Cur=" + cur) + "&p5_Pid=" + productId) + "&p8_Url=" + HttpUtility.UrlEncode(merchantCallbackURL, Encoding.GetEncoding("gb2312"))) + "&p9_SAF=" + addressFlag) + "&pa_MP=" + sMctProperties) + "&pd_FrpId=" + frpId) + "&pr_NeedResponse=" + str2) + "&hmac=" + str4);
        }

        internal static string GetQueryString(string strArgName, string strUrl)
        {
            strUrl = strUrl.Replace("?", "&");
            string str = "";
            string[] strArray = strUrl.Split(new char[] { '&' });
            int length = strArray.Length;
            for (int i = 0; i < length; i++)
            {
                int index = strArray[i].ToString().IndexOf("=");
                if ((index != -1) && (strArray[i].ToString().Substring(0, index) == strArgName))
                {
                    str = strArray[i].ToString().Substring(index + 1);
                }
            }
            return HttpUtility.UrlDecode(str, Encoding.GetEncoding("gb2312"));
        }

        internal static bool VerifyCallback(string merchantId, string keyValue, string sCmd, string sErrorCode, string sTrxId, string amount, string cur, string productId, string orderId, string userId, string mp, string bType, string hmac)
        {
            Digest digest = new Digest();
            string aValue = "";
            aValue = (((((aValue + merchantId) + sCmd + sErrorCode) + sTrxId + amount) + cur + productId) + orderId + userId) + mp + bType;
            string str2 = digest.HmacSign(aValue, keyValue);
            return (hmac == str2);
        }
    }
}

