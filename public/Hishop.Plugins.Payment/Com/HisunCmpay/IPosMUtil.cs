namespace Com.HisunCmpay
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;

    public class IPosMUtil
    {
        public static string getIpAddress()
        {
            try
            {
                string userHostAddress;
                HttpRequest request = HttpContext.Current.Request;
                if (request.ServerVariables["HTTP_VIA"] != null)
                {
                    userHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(new char[] { ',' })[0].Trim();
                }
                else
                {
                    userHostAddress = request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = "unknown";
                }
                return userHostAddress;
            }
            catch (Exception)
            {
                return "unknown";
            }
        }

        public static string getRedirectUrl(string payUrl)
        {
            Hashtable hashtable = new Hashtable();
            string str2 = payUrl.Replace("<hi:$$>", "&").Replace("<hi:=>", "*");
            if (payUrl != null)
            {
                char[] separator = "&".ToCharArray();
                string[] strArray = str2.Split(separator);
                if (strArray != null)
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        string str3 = strArray[i];
                        if (str3 != null)
                        {
                            char[] chArray2 = "*".ToCharArray();
                            string[] strArray2 = str3.Split(chArray2);
                            if ((strArray2 != null) && (strArray2.Length == 2))
                            {
                                hashtable.Add(strArray2[0], strArray2[1]);
                            }
                        }
                    }
                }
            }
            return (((string) hashtable["url"]) + "?sessionId=" + ((string) hashtable["sessionId"]));
        }

        public static string getTicks()
        {
            return string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
        }

        public static string httpRequest(string url, string data)
        {
            string message;
            string str = "";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            try
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.Default))
                {
                    writer.Write(data);
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        str = reader.ReadToEnd();
                    }
                    message = str;
                }
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public static string keyValueToString(NameValueCollection param)
        {
            string str = "";
            for (int i = 0; i < param.Count; i++)
            {
                string str2 = param.Keys[i];
                string str3 = param[str2];
                if (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3))
                {
                    if (str.Length <= 0)
                    {
                        str = str + str2 + "=" + str3;
                    }
                    else
                    {
                        string str5 = str;
                        str = str5 + "&" + str2 + "=" + str3;
                    }
                }
            }
            return str;
        }

        public static Hashtable parseStringToMap(string source)
        {
            Hashtable hashtable = new Hashtable();
            string[] strArray = source.Split(new char[] { '&' });
            string[] strArray2 = new string[2];
            string str2 = "";
            try
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    strArray2 = strArray[i].Split(new char[] { '=' });
                    if ("amtItem".Equals(strArray2[0]) || "payUrl".Equals(strArray2[0]))
                    {
                        str2 = "";
                        for (int j = 1; j < strArray2.Length; j++)
                        {
                            if (j < (strArray2.Length - 1))
                            {
                                str2 = str2 + strArray2[j] + "=";
                            }
                            else
                            {
                                str2 = str2 + strArray2[j];
                            }
                        }
                    }
                    else
                    {
                        str2 = strArray2[1];
                    }
                    hashtable.Add(strArray2[0], str2);
                }
            }
            catch (Exception)
            {
                hashtable.Add("message", source);
                return hashtable;
            }
            return hashtable;
        }
    }
}

