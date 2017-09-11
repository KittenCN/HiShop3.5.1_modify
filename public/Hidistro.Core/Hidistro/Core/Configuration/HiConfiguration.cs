namespace Hidistro.Core.Configuration
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Security;
    using System.Xml;

    public class HiConfiguration
    {
        private string adminFolder = "admin";
        private Hidistro.Core.Configuration.AppLocation app;
        private const string ConfigCacheKey = "FileCache-Configuragion";
        private string emailEncoding = "utf-8";
        private string emailRegex = @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3,4}){1,2})";
        private const string filesPath = "/";
        private readonly Dictionary<string, string> integratedApplications = new Dictionary<string, string>();
        private int passwordMaxLength = 0x10;
        private Hidistro.Core.Configuration.RolesConfiguration roleConfiguration;
        private int shippingAddressQuantity = 5;
        private short smtpServerConnectionLimit = -1;
        private SSLSettings ssl;
        private readonly Dictionary<string, string> supportedLanguages = new Dictionary<string, string>();
        public static readonly int[,] ThumbnailSizes = new int[,] { { 10, 10 }, { 0x16, 0x16 }, { 40, 40 }, { 100, 100 }, { 160, 160 }, { 310, 310 } };
        private int usernameMaxLength = 20;
        private int usernameMinLength = 3;
        private string usernameRegex = "[一-龥a-zA-Z0-9]+[一-龥_a-zA-Z0-9]*";
        private bool useUniversalCode;
        private readonly XmlDocument xmlDoc;

        public HiConfiguration(XmlDocument doc)
        {
            this.xmlDoc = doc;
            this.LoadValuesFromConfigurationXml();
        }

        internal void GetAppLocation(XmlNode node)
        {
            this.app = Hidistro.Core.Configuration.AppLocation.Create(node);
        }

        internal void GetAttributes(XmlAttributeCollection attributeCollection)
        {
            XmlAttribute attribute = attributeCollection["smtpServerConnectionLimit"];
            if (attribute != null)
            {
                this.smtpServerConnectionLimit = short.Parse(attribute.Value, CultureInfo.InvariantCulture);
            }
            else
            {
                this.smtpServerConnectionLimit = -1;
            }
            attribute = attributeCollection["ssl"];
            if (attribute != null)
            {
                this.ssl = (SSLSettings) Enum.Parse(typeof(SSLSettings), attribute.Value, true);
            }
            attribute = attributeCollection["usernameMinLength"];
            if (attribute != null)
            {
                this.usernameMinLength = int.Parse(attribute.Value);
            }
            attribute = attributeCollection["usernameMaxLength"];
            if (attribute != null)
            {
                this.usernameMaxLength = int.Parse(attribute.Value);
            }
            attribute = attributeCollection["usernameRegex"];
            if (attribute != null)
            {
                this.usernameRegex = attribute.Value;
            }
            attribute = attributeCollection["emailEncoding"];
            if (attribute != null)
            {
                this.emailEncoding = attribute.Value;
            }
            attribute = attributeCollection["shippingAddressQuantity"];
            if (attribute != null)
            {
                this.shippingAddressQuantity = int.Parse(attribute.Value);
            }
            attribute = attributeCollection["passwordMaxLength"];
            if (attribute != null)
            {
                this.passwordMaxLength = int.Parse(attribute.Value);
            }
            if (this.passwordMaxLength < Membership.Provider.MinRequiredPasswordLength)
            {
                this.passwordMaxLength = 0x10;
            }
            attribute = attributeCollection["emailRegex"];
            if (attribute != null)
            {
                this.emailRegex = attribute.Value;
            }
            attribute = attributeCollection["adminFolder"];
            if (attribute != null)
            {
                this.adminFolder = attribute.Value;
            }
            attribute = attributeCollection["useUniversalCode"];
            if ((attribute != null) && attribute.Value.Equals("true"))
            {
                this.useUniversalCode = true;
            }
        }

        public static HiConfiguration GetConfig()
        {
            HiConfiguration configuration = HiCache.Get("FileCache-Configuragion") as HiConfiguration;
            if (configuration == null)
            {
                HttpContext current = HttpContext.Current;
                string filename = null;
                if (current != null)
                {
                    try
                    {
                        filename = current.Request.MapPath("~/config/Hishop.config");
                    }
                    catch
                    {
                        filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\Hishop.config");
                    }
                }
                else
                {
                    filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\Hishop.config");
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                configuration = new HiConfiguration(doc);
                HiCache.Max("FileCache-Configuragion", configuration, new CacheDependency(filename));
            }
            return configuration;
        }

        public XmlNode GetConfigSection(string nodePath)
        {
            return this.xmlDoc.SelectSingleNode(nodePath);
        }

        internal void GetIntegratedApplications(XmlNode node)
        {
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (!this.integratedApplications.ContainsKey(node2.Attributes["applicationName"].Value))
                {
                    this.integratedApplications.Add(node2.Attributes["applicationName"].Value, node2.Attributes["implement"].Value);
                }
            }
        }

        internal void GetLanguages(XmlNode node)
        {
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if ((string.Compare(node2.Attributes["enabled"].Value, "true", false, CultureInfo.InvariantCulture) == 0) && !this.supportedLanguages.ContainsKey(node2.Attributes["key"].Value))
                {
                    this.supportedLanguages.Add(node2.Attributes["key"].Value, node2.Attributes["name"].Value);
                }
            }
        }

        internal void LoadValuesFromConfigurationXml()
        {
            XmlNode configSection = this.GetConfigSection("Hishop/Core");
            XmlAttributeCollection attributeCollection = configSection.Attributes;
            this.GetAttributes(attributeCollection);
            foreach (XmlNode node2 in configSection.ChildNodes)
            {
                if (node2.Name == "Languages")
                {
                    this.GetLanguages(node2);
                }
                if (node2.Name == "appLocation")
                {
                    this.GetAppLocation(node2);
                }
                if (node2.Name == "IntegratedApplications")
                {
                    this.GetIntegratedApplications(node2);
                }
            }
            if (this.app == null)
            {
                this.app = Hidistro.Core.Configuration.AppLocation.Default();
            }
            if (this.roleConfiguration == null)
            {
                this.roleConfiguration = new Hidistro.Core.Configuration.RolesConfiguration();
            }
        }

        public string AdminFolder
        {
            get
            {
                return this.adminFolder;
            }
        }

        public Hidistro.Core.Configuration.AppLocation AppLocation
        {
            get
            {
                return this.app;
            }
        }

        public string EmailEncoding
        {
            get
            {
                return this.emailEncoding;
            }
        }

        public string EmailRegex
        {
            get
            {
                return this.emailRegex;
            }
        }

        public string FilesPath
        {
            get
            {
                return "/";
            }
        }

        public Dictionary<string, string> IntegratedApplications
        {
            get
            {
                return this.integratedApplications;
            }
        }

        public int PasswordMaxLength
        {
            get
            {
                return this.passwordMaxLength;
            }
        }

        public int QueuedThreads
        {
            get
            {
                return 2;
            }
        }

        public Hidistro.Core.Configuration.RolesConfiguration RolesConfiguration
        {
            get
            {
                return this.roleConfiguration;
            }
        }

        public int ShippingAddressQuantity
        {
            get
            {
                return this.shippingAddressQuantity;
            }
        }

        public short SmtpServerConnectionLimit
        {
            get
            {
                return this.smtpServerConnectionLimit;
            }
        }

        public SSLSettings SSL
        {
            get
            {
                return this.ssl;
            }
        }

        public Dictionary<string, string> SupportedLanguages
        {
            get
            {
                return this.supportedLanguages;
            }
        }

        public int UsernameMaxLength
        {
            get
            {
                return this.usernameMaxLength;
            }
        }

        public int UsernameMinLength
        {
            get
            {
                return this.usernameMinLength;
            }
        }

        public string UsernameRegex
        {
            get
            {
                return this.usernameRegex;
            }
        }

        public bool UseUniversalCode
        {
            get
            {
                return this.useUniversalCode;
            }
        }
    }
}

