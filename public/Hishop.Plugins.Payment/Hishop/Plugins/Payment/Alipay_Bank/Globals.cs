namespace Hishop.Plugins.Payment.Alipay_Bank
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public static class Globals
    {
        public static string[] BubbleSort(string[] r)
        {
            for (int i = 0; i < r.Length; i++)
            {
                bool flag = false;
                for (int j = r.Length - 2; j >= i; j--)
                {
                    if (string.CompareOrdinal(r[j + 1], r[j]) < 0)
                    {
                        string str = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = str;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return r;
                }
            }
            return r;
        }

        public static string CreatBankUrl(string gateway, string service, string partner, string sign_type, string out_trade_no, string subject, string body, string payment_type, string total_fee, string show_url, string seller_email, string key, string return_url, string _input_charset, string notify_url, string agent, string extend_param, string need_ctu_check, string defaultBank)
        {
            return CreatBankUrl(gateway, service, partner, sign_type, out_trade_no, subject, body, payment_type, total_fee, show_url, seller_email, key, return_url, _input_charset, notify_url, agent, extend_param, need_ctu_check, "", "", "", defaultBank, "bankPay", "10", "", "", "", "", "", "");
        }

        private static string CreatBankUrl(string gateway, string service, string partner, string sign_type, string out_trade_no, string subject, string body, string payment_type, string total_fee, string show_url, string seller_email, string key, string return_url, string _input_charset, string notify_url, string agent, string extend_param, string need_ctu_check, string seller_id, string seller_account_name, string error_notify_url, string defaultbank, string paymethod, string royalty_type, string royalty_parameters, string anti_phishing_key, string exter_invoke_ip, string extra_common_param, string it_b_pay, string product_type)
        {
            int num;
            string[] strArray = BubbleSort(new string[] { 
                "service=" + service, "partner=" + partner, "_input_charset=" + _input_charset, "extend_param=" + extend_param, "subject=" + subject, "body=" + body.Replace("-", ""), "out_trade_no=" + out_trade_no, "total_fee=" + total_fee, "show_url=" + show_url, "payment_type=" + payment_type, "seller_email=" + seller_email, "notify_url=" + notify_url, "defaultbank=" + defaultbank, "return_url=" + return_url, "need_ctu_check=" + need_ctu_check, "seller_id=" + seller_id, 
                "seller_account_name=" + seller_account_name, "error_notify_url=" + error_notify_url, "paymethod=" + paymethod, "royalty_type=" + royalty_type, "royalty_parameters=" + royalty_parameters, "anti_phishing_key=" + anti_phishing_key, "exter_invoke_ip=" + exter_invoke_ip, "extra_common_param=" + extra_common_param, "it_b_pay=" + it_b_pay, "product_type=" + product_type
             });
            StringBuilder builder = new StringBuilder();
            for (num = 0; num < strArray.Length; num++)
            {
                if (!string.IsNullOrEmpty(strArray[num]))
                {
                    builder.Append(strArray[num] + "&");
                }
            }
            char ch = builder[builder.Length - 1];
            if (ch.Equals("&"))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append(key);
            string str = GetMD5(builder.ToString(), _input_charset);
            char[] separator = new char[] { '=' };
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(gateway);
            if (strArray.Length > 0)
            {
                builder2.Append("?");
            }
            for (num = 0; num < strArray.Length; num++)
            {
                if (!string.IsNullOrEmpty(strArray[num].Split(separator)[1]))
                {
                    builder2.Append(strArray[num].Split(separator)[0] + "=" + HttpUtility.UrlEncode(strArray[num].Split(separator)[1]) + "&");
                }
            }
            builder2.Append("sign=" + str + "&sign_type=" + sign_type);
            return builder2.ToString();
        }

        public static string GetMD5(string s, string _input_charset)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder builder = new StringBuilder(0x20);
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
            }
            return builder.ToString();
        }
    }
}

