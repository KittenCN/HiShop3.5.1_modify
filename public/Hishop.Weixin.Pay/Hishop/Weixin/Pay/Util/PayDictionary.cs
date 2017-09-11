namespace Hishop.Weixin.Pay.Util
{
    using System;
    using System.Collections.Generic;

    internal class PayDictionary : Dictionary<string, string>
    {
        public void Add(string key, object value)
        {
            string str;
            if (value == null)
            {
                str = null;
            }
            else if (value is string)
            {
                str = (string) value;
            }
            else if (value is DateTime)
            {
                str = ((DateTime) value).ToString("yyyyMMddHHmmss");
            }
            else if (value is DateTime?)
            {
                DateTime? nullable = value as DateTime?;
                str = nullable.Value.ToString("yyyyMMddHHmmss");
            }
            else if (value is decimal)
            {
                str = string.Format("{0:F2}", value);
            }
            else if (value is decimal?)
            {
                decimal? nullable2 = value as decimal?;
                str = string.Format("{0:F0}", nullable2.Value);
            }
            else
            {
                str = value.ToString();
            }
            this.Add(key, str);
        }

        public void Add(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                base.Add(key, value);
            }
        }
    }
}

