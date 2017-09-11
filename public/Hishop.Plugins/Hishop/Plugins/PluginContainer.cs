namespace Hishop.Plugins
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;

    public abstract class PluginContainer
    {
        protected static volatile Cache pluginCache = HttpRuntime.Cache;

        protected PluginContainer()
        {
            pluginCache.Remove(this.IndexCacheKey);
            pluginCache.Remove(this.TypeCacheKey);
        }

        private void AddPlugin(Type t, string filename, XmlDocument catalog, XmlNode mapNode)
        {
            XmlNode newChild = mapNode.SelectSingleNode(t.BaseType.Name);
            if (newChild == null)
            {
                newChild = catalog.CreateElement(t.BaseType.Name);
                mapNode.AppendChild(newChild);
            }
            XmlNode node2 = catalog.CreateElement("item");
            XmlAttribute node = catalog.CreateAttribute("identity");
            node.Value = t.FullName.ToLower();
            node2.Attributes.Append(node);
            XmlAttribute attribute2 = catalog.CreateAttribute("file");
            attribute2.Value = filename;
            node2.Attributes.Append(attribute2);
            PluginAttribute customAttribute = (PluginAttribute) Attribute.GetCustomAttribute(t, typeof(PluginAttribute));
            if (customAttribute != null)
            {
                XmlAttribute attribute4 = catalog.CreateAttribute("name");
                attribute4.Value = customAttribute.Name;
                node2.Attributes.Append(attribute4);
                XmlAttribute attribute5 = catalog.CreateAttribute("seq");
                attribute5.Value = (customAttribute.Sequence > 0) ? customAttribute.Sequence.ToString(CultureInfo.InvariantCulture) : "0";
                node2.Attributes.Append(attribute5);
                ConfigablePlugin plugin = Activator.CreateInstance(t) as ConfigablePlugin;
                XmlAttribute attribute6 = catalog.CreateAttribute("logo");
                if (string.IsNullOrEmpty(plugin.Logo) || (plugin.Logo.Trim().Length == 0))
                {
                    attribute6.Value = "";
                }
                else
                {
                    attribute6.Value = this.PluginVirtualPath + "/images/" + plugin.Logo.Trim();
                }
                node2.Attributes.Append(attribute6);
                XmlAttribute attribute7 = catalog.CreateAttribute("shortDescription");
                attribute7.Value = plugin.ShortDescription;
                node2.Attributes.Append(attribute7);
                XmlAttribute attribute8 = catalog.CreateAttribute("description");
                attribute8.Value = plugin.Description;
                node2.Attributes.Append(attribute8);
            }
            XmlAttribute attribute9 = catalog.CreateAttribute("namespace");
            attribute9.Value = t.Namespace.ToLower();
            node2.Attributes.Append(attribute9);
            if ((customAttribute != null) && (customAttribute.Sequence > 0))
            {
                XmlNode refChild = FindNode(newChild.ChildNodes, customAttribute.Sequence);
                if (refChild == null)
                {
                    newChild.AppendChild(node2);
                }
                else
                {
                    newChild.InsertBefore(node2, refChild);
                }
            }
            else
            {
                newChild.AppendChild(node2);
            }
        }

        private void BuildIndex(XmlDocument catalog, XmlNode mapNode)
        {
            if (Directory.Exists(this.PluginLocalPath))
            {
                string[] strArray = Directory.GetFiles(this.PluginLocalPath, "*.dll", SearchOption.AllDirectories);
                string fullName = typeof(IPlugin).FullName;
                foreach (string str2 in strArray)
                {
                    Assembly assembly = Assembly.Load(LoadPlugin(str2));
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        if (CheckIsPlugin(type, fullName))
                        {
                            this.AddPlugin(type, str2, catalog, mapNode);
                        }
                    }
                }
            }
        }

        private static bool CheckIsPlugin(Type t, string interfaceName)
        {
            try
            {
                if ((((t == null) || !t.IsClass) || (!t.IsPublic || t.IsAbstract)) || (t.GetInterface(interfaceName) == null))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static XmlNode FindNode(XmlNodeList nodeList, int sequence)
        {
            if (((nodeList != null) && (nodeList.Count != 0)) && (sequence > 0))
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (int.Parse(nodeList[i].Attributes["seq"].Value) > sequence)
                    {
                        return nodeList[i];
                    }
                }
            }
            return null;
        }

        internal virtual Type GetPlugin(string baseName, string name)
        {
            return this.GetPlugin(baseName, name, "identity");
        }

        private Type GetPlugin(string baseName, string name, string attname)
        {
            Hashtable pluginCache = this.GetPluginCache();
            name = name.ToLower();
            Type type = pluginCache[name] as Type;
            if (type == null)
            {
                if (PluginContainer.pluginCache.Get(this.IndexCacheKey) == null)
                {
                    return null;
                }
                XmlDocument document = PluginContainer.pluginCache.Get(this.IndexCacheKey) as XmlDocument;
                XmlNode node = document.DocumentElement.SelectSingleNode("//" + baseName + "/item[@" + attname + "='" + name + "']");
                if (!((node != null) && File.Exists(node.Attributes["file"].Value)))
                {
                    return null;
                }
                type = Assembly.Load(LoadPlugin(node.Attributes["file"].Value)).GetType(node.Attributes["identity"].Value, false, true);
                if (type != null)
                {
                    pluginCache[name] = type;
                }
            }
            return type;
        }

        private Hashtable GetPluginCache()
        {
            Hashtable hashtable = pluginCache.Get(this.TypeCacheKey) as Hashtable;
            if (hashtable == null)
            {
                hashtable = new Hashtable();
                pluginCache.Insert(this.TypeCacheKey, hashtable, new CacheDependency(this.PluginLocalPath));
            }
            return hashtable;
        }

        public abstract PluginItem GetPluginItem(string fullName);
        protected PluginItem GetPluginItem(string baseName, string fullName)
        {
            PluginItem item = null;
            XmlNode node = (pluginCache.Get(this.IndexCacheKey) as XmlDocument).SelectSingleNode("//" + baseName + "/item[@identity='" + fullName + "']");
            if (node != null)
            {
                item = new PluginItem {
                    FullName = node.Attributes["identity"].Value,
                    DisplayName = node.Attributes["name"].Value,
                    Logo = node.Attributes["logo"].Value,
                    ShortDescription = node.Attributes["shortDescription"].Value,
                    Description = node.Attributes["description"].Value
                };
            }
            return item;
        }

        public abstract PluginItemCollection GetPlugins();
        protected PluginItemCollection GetPlugins(string baseName)
        {
            PluginItemCollection items = new PluginItemCollection();
            XmlNodeList list = (pluginCache.Get(this.IndexCacheKey) as XmlDocument).SelectNodes("//" + baseName + "/item");
            if ((list != null) && (list.Count > 0))
            {
                foreach (XmlNode node in list)
                {
                    PluginItem item = new PluginItem {
                        FullName = node.Attributes["identity"].Value,
                        DisplayName = node.Attributes["name"].Value,
                        Logo = node.Attributes["logo"].Value,
                        ShortDescription = node.Attributes["shortDescription"].Value,
                        Description = node.Attributes["description"].Value
                    };
                    items.Add(item);
                }
            }
            return items;
        }

        internal virtual Type GetPluginWithNamespace(string baseName, string name)
        {
            return this.GetPlugin(baseName, name, "namespace");
        }

        private static byte[] LoadPlugin(string filename)
        {
            byte[] buffer;
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                buffer = new byte[(int) stream.Length];
                stream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }

        protected void VerifyIndex()
        {
            if (pluginCache.Get(this.IndexCacheKey) == null)
            {
                XmlDocument catalog = new XmlDocument();
                XmlNode mapNode = catalog.CreateElement("Plugins");
                this.BuildIndex(catalog, mapNode);
                catalog.AppendChild(mapNode);
                pluginCache.Insert(this.IndexCacheKey, catalog, new CacheDependency(this.PluginLocalPath));
            }
        }

        protected abstract string IndexCacheKey { get; }

        protected abstract string PluginLocalPath { get; }

        protected abstract string PluginVirtualPath { get; }

        protected abstract string TypeCacheKey { get; }
    }
}

