namespace Hidistro.Core.Urls
{
    using Hidistro.Core;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Xml;

    public class SiteUrlsData
    {
        private string _locationFilter;
        private Hidistro.Core.Urls.LocationSet _locationSet;
        private NameValueCollection _paths = new NameValueCollection();
        private NameValueCollection _reversePaths = new NameValueCollection();
        private bool enableHtmRewrite;
        private string extension;

        public SiteUrlsData(string siteUrlsXmlFile)
        {
            this.Initialize(siteUrlsXmlFile);
        }

        protected XmlDocument CreateDoc(string siteUrlsXmlFile)
        {
            XmlDocument document = new XmlDocument();
            document.Load(siteUrlsXmlFile);
            return document;
        }

        private Hidistro.Core.Urls.LocationSet CreateLocationSet(XmlNode basePaths, string globalPath)
        {
            Hidistro.Core.Urls.LocationSet set = new Hidistro.Core.Urls.LocationSet();
            foreach (XmlNode node in basePaths.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    XmlAttribute attribute = node.Attributes["name"];
                    XmlAttribute attribute2 = node.Attributes["path"];
                    XmlAttribute attribute3 = node.Attributes["physicalPath"];
                    XmlAttribute attribute4 = node.Attributes["exclude"];
                    XmlAttribute attribute5 = node.Attributes["type"];
                    if ((attribute != null) && (attribute2 != null))
                    {
                        string physicalPath = null;
                        if (attribute3 != null)
                        {
                            physicalPath = globalPath + attribute3.Value;
                        }
                        bool exclude = (attribute4 != null) && bool.Parse(attribute4.Value);
                        Location location = null;
                        string path = globalPath + attribute2.Value;
                        if (attribute5 == null)
                        {
                            location = new Location(path, physicalPath, exclude);
                        }
                        else
                        {
                            location = Activator.CreateInstance(Type.GetType(attribute5.Value), new object[] { path, physicalPath, exclude }) as Location;
                        }
                        set.Add(attribute.Value, location);
                    }
                }
            }
            return set;
        }

        private static ListDictionary CreateTransformers(XmlNode transformers)
        {
            ListDictionary dictionary = new ListDictionary();
            foreach (XmlNode node in transformers.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    string str = node.Attributes["key"].Value;
                    string str2 = node.Attributes["value"].Value;
                    if (!string.IsNullOrEmpty(str))
                    {
                        dictionary[str] = str2;
                    }
                }
            }
            return dictionary;
        }

        private void CreateUrls(XmlNode urls, ListDictionary transforms)
        {
            foreach (XmlNode node in urls.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    bool flag = this.enableHtmRewrite && ((node.Attributes["htmRewrite"] != null) && (string.Compare(node.Attributes["htmRewrite"].Value, "true", true) == 0));
                    string name = node.Attributes["name"].Value;
                    XmlAttribute attribute = node.Attributes["navigateUrl"];
                    if (attribute != null)
                    {
                        this._paths.Add(name, attribute.Value);
                    }
                    else
                    {
                        string str5;
                        string str2 = node.Attributes["location"].Value;
                        string str3 = null;
                        string str4 = null;
                        XmlAttribute attribute2 = node.Attributes["vanity"];
                        XmlAttribute attribute3 = node.Attributes["pattern"];
                        Location location = this._locationSet.FindLocationByName(str2);
                        if ((attribute2 != null) && (attribute3 != null))
                        {
                            str4 = location.Path + attribute3.Value;
                            str3 = location.PhysicalPath + attribute2.Value;
                        }
                        if (flag)
                        {
                            str5 = node.Attributes["path"].Value.Replace(".aspx", this.extension);
                            if (string.IsNullOrEmpty(str4) || string.IsNullOrEmpty(str3))
                            {
                                str4 = location.Path + node.Attributes["path"].Value;
                                str3 = location.PhysicalPath + node.Attributes["path"].Value;
                            }
                            str4 = str4.Replace(".aspx", this.extension);
                        }
                        else
                        {
                            str5 = node.Attributes["path"].Value;
                        }
                        foreach (string str6 in transforms.Keys)
                        {
                            str5 = str5.Replace(str6, transforms[str6].ToString());
                            if (!string.IsNullOrEmpty(str4))
                            {
                                str4 = str4.Replace(str6, transforms[str6].ToString());
                            }
                            if (!string.IsNullOrEmpty(str3))
                            {
                                str3 = str3.Replace(str6, transforms[str6].ToString());
                            }
                        }
                        this._paths.Add(name, location.Path + str5);
                        string str7 = location.Path + str5;
                        if (Globals.ApplicationPath.Length > 0)
                        {
                            str7 = str7.Replace(Globals.ApplicationPath, "").ToLower(CultureInfo.InvariantCulture);
                        }
                        this._reversePaths.Add(str7, name);
                        if (!string.IsNullOrEmpty(str4) && !string.IsNullOrEmpty(str3))
                        {
                            location.Add(new ReWrittenUrl(str2 + "." + name, str4, str3));
                        }
                    }
                }
            }
        }

        public string FormatUrl(string name)
        {
            return this.FormatUrl(name, null);
        }

        public virtual string FormatUrl(string name, params object[] parameters)
        {
            if (parameters == null)
            {
                return this.Paths[name].Trim();
            }
            return string.Format(CultureInfo.InvariantCulture, this.Paths[name].Trim(), parameters);
        }

        protected void Initialize(string siteUrlsXmlFile)
        {
            string applicationPath = Globals.ApplicationPath;
            if (applicationPath != null)
            {
                applicationPath = applicationPath.Trim();
            }
            XmlDocument document = this.CreateDoc(siteUrlsXmlFile);
            this.enableHtmRewrite = string.Compare(document.SelectSingleNode("SiteUrls").Attributes["enableHtmRewrite"].Value, "true", true) == 0;
            this.extension = document.SelectSingleNode("SiteUrls").Attributes["extension"].Value;
            XmlNode basePaths = document.SelectSingleNode("SiteUrls/locations");
            this._locationSet = this.CreateLocationSet(basePaths, applicationPath);
            this._locationFilter = this._locationSet.Filter;
            ListDictionary transforms = CreateTransformers(document.SelectSingleNode("SiteUrls/transformers"));
            XmlNode urls = document.SelectSingleNode("SiteUrls/urls");
            this.CreateUrls(urls, transforms);
        }

        private static string ResolveUrl(string path)
        {
            if (Globals.ApplicationPath.Length > 0)
            {
                return (Globals.ApplicationPath + path);
            }
            return path;
        }

        public string LocationFilter
        {
            get
            {
                return this._locationFilter;
            }
        }

        public Hidistro.Core.Urls.LocationSet LocationSet
        {
            get
            {
                return this._locationSet;
            }
        }

        public NameValueCollection Paths
        {
            get
            {
                return this._paths;
            }
        }

        public NameValueCollection ReversePaths
        {
            get
            {
                return this._reversePaths;
            }
        }
    }
}

