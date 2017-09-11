namespace Hishop.Plugins.Payment.AlipayAssure
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    internal static class Globals
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

        internal static string CreateSendGoodsUrl(string partner, string trade_no, string logistics_name, string invoice_no, string transport_type, string key, string _input_charset)
        {
            int num;
            string[] strArray2 = BubbleSort(new string[] { "service=send_goods_confirm_by_platform", "partner=" + partner, "trade_no=" + trade_no, "logistics_name=" + logistics_name, "invoice_no=" + invoice_no, "transport_type=" + transport_type, "_input_charset=" + _input_charset });
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
            builder2.Append("https://mapi.alipay.com/gateway.do?");
            for (num = 0; num < strArray2.Length; num++)
            {
                builder2.Append(strArray2[num].Split(separator)[0] + "=" + HttpUtility.UrlEncode(strArray2[num].Split(separator)[1]) + "&");
            }
            builder2.Append("sign=" + str + "&sign_type=MD5");
            return builder2.ToString();
        }

        public static string CreatUrl(string gateway, string service, string partner, string sign_type, string out_trade_no, string subject, string body, string payment_type, string total_fee, string show_url, string seller_email, string key, string return_url, string _input_charset, string notify_url, string logistics_type, string logistics_fee, string logistics_payment, string quantity, string agent, string extend_param, string token)
        {
            int num;
            string[] strArray;
            if (string.IsNullOrEmpty(token))
            {
                strArray = new string[] { 
                    "service=" + service, "partner=" + partner, "agent=" + agent, "extend_param=" + extend_param, "subject=" + subject, "body=" + body, "out_trade_no=" + out_trade_no, "price=" + total_fee, "show_url=" + show_url, "payment_type=" + payment_type, "seller_email=" + seller_email, "notify_url=" + notify_url, "_input_charset=" + _input_charset, "return_url=" + return_url, "quantity=" + quantity, "logistics_type=" + logistics_type, 
                    "logistics_fee=" + logistics_fee, "logistics_payment=" + logistics_payment
                 };
            }
            else
            {
                strArray = new string[] { 
                    "service=" + service, "partner=" + partner, "agent=" + agent, "extend_param=" + extend_param, "subject=" + subject, "body=" + body, "out_trade_no=" + out_trade_no, "price=" + total_fee, "show_url=" + show_url, "payment_type=" + payment_type, "seller_email=" + seller_email, "notify_url=" + notify_url, "_input_charset=" + _input_charset, "return_url=" + return_url, "quantity=" + quantity, "logistics_type=" + logistics_type, 
                    "logistics_fee=" + logistics_fee, "logistics_payment=" + logistics_payment, "token=" + token
                 };
            }
            string[] strArray2 = BubbleSort(strArray);
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

