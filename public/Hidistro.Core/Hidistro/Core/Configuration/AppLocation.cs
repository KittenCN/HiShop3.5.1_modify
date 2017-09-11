namespace Hidistro.Core.Configuration
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;

    public class AppLocation
    {
        private string defaultName;
        private Hashtable ht = new Hashtable();
        private const string HttpContextAppLocation = "AppLocation";
        private IList<string> keys = new List<string>();
        private string pattern;
        private Regex regex;

        internal void Add(HiApplication app)
        {
            if (this.ht.Contains(app.Name))
            {
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "The HiApplication.Name ({0}) was not unique", new object[] { app.Name }));
            }
            this.ht.Add(app.Name, app);
            this.keys.Add(app.Name);
        }

        public static AppLocation Create(XmlNode node)
        {
            if (node == null)
            {
                return null;
            }
            AppLocation location = new AppLocation();
            XmlAttributeCollection attributes = node.Attributes;
            if (attributes != null)
            {
                foreach (XmlAttribute attribute in attributes)
                {
                    if (attribute.Name == "pattern")
                    {
                        location.Pattern = Globals.ApplicationPath + attribute.Value;
                    }
                    else if (attribute.Name == "defaultName")
                    {
                        location.DefaultName = attribute.Value;
                    }
                }
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode node2 = node.ChildNodes[i];
                    if (node2.Name == "add")
                    {
                        XmlAttributeCollection attributes2 = node2.Attributes;
                        if (attributes2 != null)
                        {
                            string pattern = Globals.ApplicationPath + attributes2["pattern"].Value;
                            string name = attributes2["name"].Value;
                            ApplicationType appType = (ApplicationType) Enum.Parse(typeof(ApplicationType), attributes2["type"].Value, true);
                            location.Add(new HiApplication(pattern, name, appType));
                        }
                    }
                }
            }
            return location;
        }

        internal HiApplication CurrentHiApplication()
        {
            HttpContext current = HttpContext.Current;
            if (current == null)
            {
                return null;
            }
            HiApplication application = current.Items["AppLocation"] as HiApplication;
            if (application == null)
            {
                application = this.LookUp(current.Request.Path);
                if (application != null)
                {
                    current.Items.Add("AppLocation", application);
                }
            }
            return application;
        }

        public static AppLocation Default()
        {
            AppLocation location = new AppLocation();
            location.Add(new HiApplication("/", "Common", ApplicationType.Common));
            return location;
        }

        public bool IsApplicationType(ApplicationType applicationType)
        {
            return (this.CurrentApplicationType == applicationType);
        }

        public bool IsName(string name)
        {
            return (string.Compare(name, this.CurrentName, true, CultureInfo.InvariantCulture) == 0);
        }

        internal HiApplication LookUp(string url)
        {
            if ((this.Pattern == null) || this.regex.IsMatch(url))
            {
                for (int i = 0; i < this.keys.Count; i++)
                {
                    HiApplication application = this.ht[this.keys[i]] as HiApplication;
                    if (application.IsMatch(url))
                    {
                        return application;
                    }
                }
                if (this.DefaultName != null)
                {
                    return (this.ht[this.DefaultName] as HiApplication);
                }
            }
            return null;
        }

        public ApplicationType CurrentApplicationType
        {
            get
            {
                HiApplication application = this.CurrentHiApplication();
                if (application != null)
                {
                    return application.ApplicationType;
                }
                return ApplicationType.Unknown;
            }
        }

        public string CurrentName
        {
            get
            {
                HiApplication application = this.CurrentHiApplication();
                if (application != null)
                {
                    return application.Name;
                }
                return null;
            }
        }

        public string DefaultName
        {
            get
            {
                return this.defaultName;
            }
            set
            {
                this.defaultName = value;
            }
        }

        public bool IsKnownApplication
        {
            get
            {
                return (this.CurrentApplicationType != ApplicationType.Unknown);
            }
        }

        public string Pattern
        {
            get
            {
                return this.pattern;
            }
            set
            {
                this.pattern = value;
                if (this.pattern != null)
                {
                    this.regex = new Regex(this.pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
            }
        }
    }
}

