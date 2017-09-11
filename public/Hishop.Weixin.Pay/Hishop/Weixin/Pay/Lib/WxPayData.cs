namespace Hishop.Weixin.Pay.Lib
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;

    public class WxPayData
    {
        private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

        public bool CheckSign(string key)
        {
            if (!this.IsSet("sign"))
            {
                return true;
            }
            if ((this.GetValue("sign") == null) || (this.GetValue("sign").ToString() == ""))
            {
                this.SetValue("sign", "");
            }
            string str = this.GetValue("sign").ToString();
            return (this.MakeSign(key) == str);
        }

        public SortedDictionary<string, object> FromXml(string xml, string key)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            foreach (XmlNode node2 in document.FirstChild.ChildNodes)
            {
                XmlElement element = (XmlElement) node2;
                this.m_values[element.Name] = element.InnerText;
            }
            try
            {
                if (!this.CheckSign(key))
                {
                    this.m_values.Clear();
                    this.m_values["return_code"] = "FAIL";
                    this.m_values["return_msg"] = "签名验证异常";
                }
            }
            catch (WxPayException exception)
            {
                this.m_values.Clear();
                this.m_values["return_code"] = "FAIL";
                this.m_values["return_msg"] = "签名异常" + exception.Message;
            }
            return this.m_values;
        }

        public IDictionary<string, string> GetParam()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string str in this.m_values.Keys)
            {
                dictionary.Add(str, this.m_values[str].ToString());
            }
            return dictionary;
        }

        public object GetValue(string key)
        {
            object obj2 = null;
            this.m_values.TryGetValue(key, out obj2);
            return obj2;
        }

        public SortedDictionary<string, object> GetValues()
        {
            return this.m_values;
        }

        public bool IsSet(string key)
        {
            object obj2 = null;
            this.m_values.TryGetValue(key, out obj2);
            return (obj2 != null);
        }

        public string MakeSign(string key)
        {
            string s = this.ToUrl() + "&key=" + key;
            byte[] buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder builder = new StringBuilder();
            foreach (byte num in buffer)
            {
                builder.Append(num.ToString("x2"));
            }
            return builder.ToString().ToUpper();
        }

        public void SetValue(string key, object value)
        {
            this.m_values[key] = value;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this.m_values);
        }

        public string ToPrintStr()
        {
            string str = "";
            foreach (KeyValuePair<string, object> pair in this.m_values)
            {
                str = str + string.Format("{0}={1}<br>", pair.Key, pair.Value.ToString());
            }
            return str;
        }

        public string ToUrl()
        {
            string str = "";
            foreach (KeyValuePair<string, object> pair in this.m_values)
            {
                if ((pair.Key != "sign") && (pair.Value.ToString() != ""))
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, pair.Key, "=", pair.Value, "&" });
                }
            }
            return str.Trim(new char[] { '&' });
        }

        public string ToXml()
        {
            StringBuilder builder = new StringBuilder("<?xml version=\"1.0\" standalone=\"true\"?>");
            builder.AppendLine("<xml>");
            foreach (KeyValuePair<string, object> pair in this.m_values)
            {
                if (pair.Value == null)
                {
                    return "";
                }
                if (pair.Value.GetType() == typeof(int))
                {
                    builder.AppendLine(string.Concat(new object[] { "<", pair.Key, ">", pair.Value, "</", pair.Key, ">" }));
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    builder.AppendLine(string.Concat(new object[] { "<", pair.Key, "><![CDATA[", pair.Value, "]]></", pair.Key, ">" }));
                }
            }
            builder.AppendLine("</xml>");
            return builder.ToString();
        }
    }
}

