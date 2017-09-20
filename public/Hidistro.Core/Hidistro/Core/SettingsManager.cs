﻿namespace Hidistro.Core
{
    using Hidistro.Core.Entities;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;

    public static class SettingsManager
    {
        private const string MasterSettingsCacheKey = "FileCache-MasterSettings";
        private static object xmlLock = new object();

        public static SiteSettings GetMasterSettings(bool cacheable)
        {
            if (!cacheable)
            {
                HiCache.Remove("FileCache-MasterSettings");
            }
            XmlDocument document = HiCache.Get("FileCache-MasterSettings") as XmlDocument;
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
                    HiCache.Max("FileCache-MasterSettings", document, new CacheDependency(masterSettingsFilename));
                }
            }
            return SiteSettings.FromXml(document);
        }

        /// <summary>
        /// edit by  lipu  20170920
        /// case:异常请求在此上下文中不可用
        /// </summary>
        /// <returns></returns>
        private static string GetMasterSettingsFilename()
        {
            // HttpContext current = HttpContext.Current;
            //  return ((current != null) ? current.Request.MapPath("~/config/SiteSettings.config") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\SiteSettings.config"));

            return  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\SiteSettings.config");

        }

        public static void Save(SiteSettings settings)
        {
            SaveMasterSettings(settings);
            HiCache.Remove("FileCache-MasterSettings");
        }

        private static void SaveMasterSettings(SiteSettings settings)
        {
            lock (xmlLock)
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
}

