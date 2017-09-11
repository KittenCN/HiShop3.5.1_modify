namespace Hishop.Plugins.Payment.AlipayDirect
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    internal static class Globals
    {
        internal static string[] BubbleSort(string[] r)
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

        internal static string CreatDirectUrl(string gateway, string service, string partner, string sign_type, string out_trade_no, string subject, string body, string payment_type, string total_fee, string show_url, string seller_email, string key, string return_url, string _input_charset, string notify_url, string agent, string extend_param)
        {
            int num;
            string[] strArray2 = BubbleSort(new string[] { "service=" + service, "partner=" + partner, "agent=" + agent, "extend_param=" + extend_param, "subject=" + subject, "body=" + body, "out_trade_no=" + out_trade_no, "total_fee=" + total_fee, "show_url=" + show_url, "payment_type=" + payment_type, "seller_email=" + seller_email, "notify_url=" + notify_url, "_input_charset=" + _input_charset, "return_url=" + return_url });
            StringBuilder builder = new StringBuilder();
            for (num = 0; num < strArray2.Length; num++)
            {
                if (num == (strArray2.Length - 1))
                {
                    builder.Append(strArray2[num]);
                }
                else
                {
                    builder.Append(strArray2[num] + "&");
                }
            }
            builder.Append(key);
            string str = GetMD5(builder.ToString(), _input_charset);
            char[] separator = new char[] { '=' };
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(gateway);
            for (num = 0; num < strArray2.Length; num++)
            {
                builder2.Append(strArray2[num].Split(separator)[0] + "=" + HttpUtility.UrlEncode(strArray2[num].Split(separator)[1]) + "&");
            }
            builder2.Append("sign=" + str + "&sign_type=" + sign_type);
            return builder2.ToString();
        }

        internal static string CreatRefundUrl(string gateway, string service, string partner, string sign_type, string out_trade_no, string refundOrderId, string total_fee, string refundFee, string seller_email, string key, string return_url, string _input_charset, string notify_url, string refund_date, string batch_no, string batch_num, string detail_data)
        {
            int num;
            string[] strArray2 = BubbleSort(new string[] { "service=" + service, "partner=" + partner, "refund_date=" + refund_date, "batch_no=" + batch_no, "batch_num=" + batch_num, "detail_data=" + detail_data, "out_trade_no=" + out_trade_no, "refundOrderId=" + refundOrderId, "total_fee=" + total_fee, "refundFee=" + refundFee, "seller_email=" + seller_email, "notify_url=" + notify_url, "_input_charset=" + _input_charset, "return_url=" + return_url });
            StringBuilder builder = new StringBuilder();
            for (num = 0; num < strArray2.Length; num++)
            {
                if (num == (strArray2.Length - 1))
                {
                    builder.Append(strArray2[num]);
                }
                else
                {
                    builder.Append(strArray2[num] + "&");
                }
            }
            builder.Append(key);
            string str = GetMD5(builder.ToString(), _input_charset);
            char[] separator = new char[] { '=' };
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(gateway);
            for (num = 0; num < strArray2.Length; num++)
            {
                builder2.Append(strArray2[num].Split(separator)[0] + "=" + HttpUtility.UrlEncode(strArray2[num].Split(separator)[1]) + "&");
            }
            builder2.Append("sign=" + str + "&sign_type=" + sign_type);
            return builder2.ToString();
        }

        internal static string GetMD5(string s, string _input_charset)
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

