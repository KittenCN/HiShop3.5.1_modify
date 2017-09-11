namespace Hishop.Plugins
{
    using System;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Xml;

    public abstract class ConfigablePlugin
    {
        private const string CastMsg = "{0}的输入格式不正确，请按正确格式输入";
        private const string RequiredMsg = "{0}为必填项";

        protected ConfigablePlugin()
        {
        }

        private static void AppendAttrubiteNode(XmlDocument doc, ConfigElementAttribute att, PropertyInfo property)
        {
            XmlNode newChild = doc.CreateElement("att");
            XmlAttribute node = doc.CreateAttribute("Property");
            node.Value = property.Name;
            newChild.Attributes.Append(node);
            XmlAttribute attribute2 = doc.CreateAttribute("Name");
            attribute2.Value = string.IsNullOrEmpty(att.Name) ? property.Name : att.Name;
            newChild.Attributes.Append(attribute2);
            XmlAttribute attribute3 = doc.CreateAttribute("Description");
            attribute3.Value = att.Description;
            newChild.Attributes.Append(attribute3);
            XmlAttribute attribute4 = doc.CreateAttribute("Nullable");
            attribute4.Value = att.Nullable.ToString();
            newChild.Attributes.Append(attribute4);
            XmlAttribute attribute5 = doc.CreateAttribute("InputType");
            attribute5.Value = ((int) att.InputType).ToString();
            newChild.Attributes.Append(attribute5);
            if ((att.Options != null) && (att.Options.Length > 0))
            {
                XmlNode node2 = doc.CreateElement("Options");
                foreach (string str in att.Options)
                {
                    XmlNode node3 = doc.CreateElement("Item");
                    node3.InnerText = str;
                    node2.AppendChild(node3);
                }
                newChild.AppendChild(node2);
            }
            doc.SelectSingleNode("xml").AppendChild(newChild);
        }

        public virtual ConfigData GetConfigData(NameValueCollection form)
        {
            ConfigData data = new ConfigData {
                NeedProtect = this.NeedProtect
            };
            PropertyInfo[] properties = base.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                string str = form[info.Name] ?? "false";
                ConfigElementAttribute customAttribute = (ConfigElementAttribute) Attribute.GetCustomAttribute(info, typeof(ConfigElementAttribute));
                if (customAttribute != null)
                {
                    if (!(customAttribute.Nullable || (!string.IsNullOrEmpty(str) && (str.Length != 0))))
                    {
                        data.IsValid = false;
                        data.ErrorMsgs.Add(string.Format("{0}为必填项", customAttribute.Name));
                    }
                    else if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            Convert.ChangeType(str, info.PropertyType);
                        }
                        catch (FormatException)
                        {
                            data.IsValid = false;
                            data.ErrorMsgs.Add(string.Format("{0}的输入格式不正确，请按正确格式输入", customAttribute.Name));
                            continue;
                        }
                        data.Add(info.Name, str);
                    }
                }
            }
            return data;
        }

        internal virtual XmlDocument GetMetaData()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<xml></xml>");
            PropertyInfo[] properties = base.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                ConfigElementAttribute customAttribute = (ConfigElementAttribute) Attribute.GetCustomAttribute(info, typeof(ConfigElementAttribute));
                if (customAttribute != null)
                {
                    AppendAttrubiteNode(doc, customAttribute, info);
                }
            }
            return doc;
        }

        protected virtual void InitConfig(XmlNode configXml)
        {
            PropertyInfo[] properties = base.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                ConfigElementAttribute customAttribute = (ConfigElementAttribute) Attribute.GetCustomAttribute(info, typeof(ConfigElementAttribute));
                if (customAttribute != null)
                {
                    XmlNode node = configXml.SelectSingleNode(info.Name);
                    if (((node != null) && !string.IsNullOrEmpty(node.InnerText)) && (node.InnerText.Length > 0))
                    {
                        info.SetValue(this, Convert.ChangeType(node.InnerText, info.PropertyType), null);
                    }
                }
            }
        }

        public abstract string Description { get; }

        public abstract string Logo { get; }

        protected abstract bool NeedProtect { get; }

        public abstract string ShortDescription { get; }
    }
}

