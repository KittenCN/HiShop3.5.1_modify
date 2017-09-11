namespace Hidistro.Core
{
    using Hidistro.Core.Entities;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;

    public class CustomerServiceManager
    {
        private const string MasterSettingsCacheKey = "FileCache-CustomerServiceSettings";

        public static CustomerServiceSettings GetMasterSettings(bool cacheable)
        {
            if (!cacheable)
            {
                HiCache.Remove("FileCache-CustomerServiceSettings");
            }
            XmlDocument document = HiCache.Get("FileCache-CustomerServiceSettings") as XmlDocument;
            if (document == null)
            {
                string masterSettingsFilename = GetMasterSettingsFilename();
                if (!File.Exists(masterSettingsFilename))
                {
                    return null;
                }
                document = new XmlDocument();
                document.Load(masterSettingsFilename);
                if (cacheable)
                {
                    HiCache.Max("FileCache-CustomerServiceSettings", document, new CacheDependency(masterSettingsFilename));
                }
            }
            return CustomerServiceSettings.FromXml(document);
        }

        private static string GetMasterSettingsFilename()
        {
            HttpContext current = HttpContext.Current;
            return ((current != null) ? current.Request.MapPath("~/config/CustomerService.config") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\CustomerService.config"));
        }

        public static void Save(CustomerServiceSettings settings)
        {
            SaveMasterSettings(settings);
            HiCache.Remove("FileCache-CustomerServiceSettings");
        }

        private static void SaveMasterSettings(CustomerServiceSettings settings)
        {
            string masterSettingsFilename = GetMasterSettingsFilename();
            XmlDocument doc = new XmlDocument();
            if (File.Exists(masterSettingsFilename))
            {
                doc.Load(masterSettingsFilename);
            }
            settings.WriteToXml(doc);
            doc.Save(masterSettingsFilename);
        }
    }
}

