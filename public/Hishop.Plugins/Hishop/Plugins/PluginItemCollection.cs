namespace Hishop.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    public class PluginItemCollection
    {
        private Dictionary<string, PluginItem> plugins = new Dictionary<string, PluginItem>();

        public void Add(PluginItem item)
        {
            if (!this.plugins.ContainsKey(item.FullName))
            {
                this.plugins.Add(item.FullName, item);
            }
        }

        public bool ContainsKey(string fullName)
        {
            return this.plugins.ContainsKey(fullName);
        }

        public void Remove(string fullName)
        {
            this.plugins.Remove(fullName);
        }

        public virtual string ToJsonString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.AppendFormat("\"qty\":{0}", this.plugins.Count.ToString(CultureInfo.InvariantCulture));
            if (this.plugins.Count > 0)
            {
                builder.Append(",\"items\":[");
                foreach (string str in this.plugins.Keys)
                {
                    PluginItem item = this.plugins[str];
                    builder.Append("{");
                    builder.AppendFormat("\"FullName\":\"{0}\",\"DisplayName\":\"{1}\",\"Logo\":\"{2}\",\"ShortDescription\":\"{3}\",\"Description\":\"{4}\"", new object[] { item.FullName, item.DisplayName, item.Logo, item.ShortDescription, item.Description });
                    builder.Append("},");
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append("]");
            }
            builder.Append("}");
            return builder.ToString();
        }

        public virtual string ToXmlString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<xml>");
            builder.AppendFormat("<qty>{0}</qty>", this.plugins.Count.ToString(CultureInfo.InvariantCulture));
            foreach (string str in this.plugins.Keys)
            {
                PluginItem item = this.plugins[str];
                builder.Append("<item>");
                builder.AppendFormat("<FullName>{0}</FullName><DisplayName>{1}</DisplayName><Logo>{2}</Logo><ShortDescription>{3}</ShortDescription><Description>{4}</Description>", new object[] { item.FullName, item.DisplayName, item.Logo, item.ShortDescription, item.Description });
                builder.Append("</item>");
            }
            builder.Append("</xml>");
            return builder.ToString();
        }

        public int Count
        {
            get
            {
                return this.plugins.Count;
            }
        }

        public PluginItem[] Items
        {
            get
            {
                PluginItem[] array = new PluginItem[this.plugins.Count];
                this.plugins.Values.CopyTo(array, 0);
                return array;
            }
        }
    }
}

