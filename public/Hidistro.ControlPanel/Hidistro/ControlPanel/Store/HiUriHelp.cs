namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using System;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;

    public class HiUriHelp
    {
        private NameValueCollection queryStrings;

        public HiUriHelp(NameValueCollection query)
        {
            this.QueryStrings = new NameValueCollection(query);
        }

        public HiUriHelp(string query)
        {
            this.CreateQueryString(query);
        }

        public void AddQueryString(string key, string value)
        {
            if (this.QueryStrings == null)
            {
                this.QueryStrings = new NameValueCollection();
            }
            this.QueryStrings.Add(key, value);
        }

        public void CreateQueryString(string queryString)
        {
            queryString = queryString.Replace("?", "");
            new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                int length = queryString.Length;
                for (int i = 0; i < length; i++)
                {
                    int startIndex = i;
                    int num4 = -1;
                    while (i < length)
                    {
                        char ch = queryString[i];
                        if (ch == '=')
                        {
                            if (num4 < 0)
                            {
                                num4 = i;
                            }
                        }
                        else if (ch == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string str2 = null;
                    if (num4 >= 0)
                    {
                        key = queryString.Substring(startIndex, num4 - startIndex);
                        str2 = queryString.Substring(num4 + 1, (i - num4) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    this.AddQueryString(key, str2);
                    if ((i == (length - 1)) && (queryString[i] == '&'))
                    {
                        this.AddQueryString(key, string.Empty);
                    }
                }
            }
        }

        public string GetNewQuery()
        {
            string str = "?";
            foreach (string str2 in this.QueryStrings.AllKeys)
            {
                Globals.Debuglog(str2 + "：" + this.QueryStrings[str2] + "\r\n", "_Debuglog.txt");
                string str3 = str;
                str = str3 + str2 + "=" + this.QueryStrings[str2] + "&";
            }
            return str.TrimEnd(new char[] { '&' });
        }

        public string GetQueryString(string key)
        {
            if (!string.IsNullOrEmpty(key) && (this.queryStrings != null))
            {
                return this.QueryStrings[key];
            }
            return "";
        }

        public void RemoveQueryString(string key)
        {
            if ((this.QueryStrings != null) || (this.QueryStrings.Count > 1))
            {
                this.QueryStrings.Remove(key);
            }
        }

        public void SetQueryString(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && (this.queryStrings != null))
            {
                this.QueryStrings[key] = value;
            }
        }

        public string NewUrl { get; set; }

        public NameValueCollection QueryStrings
        {
            get
            {
                return this.queryStrings;
            }
            set
            {
                this.queryStrings = value;
            }
        }
    }
}

