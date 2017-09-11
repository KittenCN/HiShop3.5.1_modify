namespace Hidistro.Core.Urls
{
    using Hidistro.Core;
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;

    public class SiteUrls
    {
        private SiteUrlsData urlData;

        public SiteUrls(SiteUrlsData data)
        {
            this.urlData = data;
        }

        public static void CloseHtmRewrite()
        {
            UpdateHtmRewrite("false");
        }

        public static void EnableHtmRewrite()
        {
            UpdateHtmRewrite("true");
        }

        protected static NameValueCollection ExtractQueryParams(string url)
        {
            if (url == null)
            {
                return null;
            }
            int index = url.IndexOf("?");
            NameValueCollection values = new NameValueCollection();
            if (index > 0)
            {
                foreach (string str in url.Substring(index + 1).Split(new char[] { '&' }))
                {
                    string[] strArray2 = str.Split(new char[] { '=' });
                    string name = strArray2[0];
                    string str3 = string.Empty;
                    if (strArray2.Length > 1)
                    {
                        str3 = strArray2[1];
                    }
                    values.Add(name, str3);
                }
            }
            return values;
        }

        public static void ForceRefresh()
        {
            HiCache.Remove("SiteUrls");
        }

        public static string FormatUrlWithParameters(string url, string parameters)
        {
            if (url == null)
            {
                return string.Empty;
            }
            if ((parameters != null) && (parameters.Length > 0))
            {
                url = url + "?" + parameters;
            }
            return url;
        }

        private static XmlDocument GetDoc()
        {
            XmlDocument document = new XmlDocument();
            document.Load(GetSiteUrlsFilename());
            return document;
        }

        public static bool GetEnableHtmRewrite()
        {
            XmlAttribute attribute = GetDoc().SelectSingleNode("SiteUrls").Attributes["enableHtmRewrite"];
            return (string.Compare(attribute.Value, "true", true) == 0);
        }

        private static string GetSiteUrlsFilename()
        {
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                return current.Request.MapPath("~/config/SiteUrls.config");
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\SiteUrls.config");
        }

        public static SiteUrls Instance()
        {
            SiteUrls urls = HiCache.Get("FileCache-SiteUrls") as SiteUrls;
            if (urls == null)
            {
                string siteUrlsFilename = GetSiteUrlsFilename();
                SiteUrlsData data = new SiteUrlsData(siteUrlsFilename);
                urls = new SiteUrls(data);
                CacheDependency dep = new CacheDependency(siteUrlsFilename);
                HiCache.Max("FileCache-SiteUrls", urls, dep);
            }
            return urls;
        }

        public virtual string RawPath(string rawpath)
        {
            return this.urlData.FormatUrl(rawpath);
        }

        public virtual string Redirect(string url)
        {
            return this.urlData.FormatUrl("redirect", new object[] { Globals.UrlEncode(url) });
        }

        public static string RemoveParameters(string url)
        {
            if (url == null)
            {
                return string.Empty;
            }
            int index = url.IndexOf("?");
            if (index > 0)
            {
                return url.Substring(0, index);
            }
            return url;
        }

        public virtual string SubBrandDetails(int brandId, object rewriteName)
        {
            if (((rewriteName == null) || (rewriteName == DBNull.Value)) || string.IsNullOrEmpty(rewriteName.ToString()))
            {
                return this.urlData.FormatUrl("branddetails", new object[] { brandId });
            }
            return this.urlData.FormatUrl("branddetails_Rewrite", new object[] { brandId, rewriteName });
        }

        public virtual string SubCategory(int categoryId, object rewriteName)
        {
            if (((rewriteName == null) || (rewriteName == DBNull.Value)) || string.IsNullOrEmpty(rewriteName.ToString()))
            {
                return this.urlData.FormatUrl("subCategory", new object[] { categoryId });
            }
            return this.urlData.FormatUrl("subCategory_Rewrite", new object[] { categoryId, rewriteName });
        }

        private static void UpdateHtmRewrite(string status)
        {
            XmlDocument doc = GetDoc();
            XmlAttribute attribute = doc.SelectSingleNode("SiteUrls").Attributes["enableHtmRewrite"];
            if (string.Compare(attribute.Value, status, true) != 0)
            {
                attribute.Value = status;
                doc.Save(GetSiteUrlsFilename());
            }
        }

        public virtual string Favicon
        {
            get
            {
                return this.urlData.FormatUrl("favicon");
            }
        }

        public virtual string Home
        {
            get
            {
                return this.urlData.FormatUrl("home");
            }
        }

        public virtual string LocationFilter
        {
            get
            {
                return this.urlData.LocationFilter;
            }
        }

        public Hidistro.Core.Urls.LocationSet Locations
        {
            get
            {
                return this.urlData.LocationSet;
            }
        }

        public Hidistro.Core.Urls.LocationSet LocationSet
        {
            get
            {
                return this.urlData.LocationSet;
            }
        }

        public virtual string Login
        {
            get
            {
                string pathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                string str2 = null;
                str2 = ExtractQueryParams(pathAndQuery)["ReturnUrl"];
                if (string.IsNullOrEmpty(str2))
                {
                    return this.urlData.FormatUrl("login", new object[] { HttpContext.Current.Request.RawUrl });
                }
                return this.urlData.FormatUrl("login", new object[] { str2 });
            }
        }

        public virtual string LoginReturnHome
        {
            get
            {
                return this.urlData.FormatUrl("login", new object[] { Globals.ApplicationPath });
            }
        }

        public virtual string Logout
        {
            get
            {
                return this.urlData.FormatUrl("logout");
            }
        }

        public NameValueCollection ReversePaths
        {
            get
            {
                return this.urlData.ReversePaths;
            }
        }

        public SiteUrlsData UrlData
        {
            get
            {
                return this.urlData;
            }
        }

        public virtual string UserChangePassword
        {
            get
            {
                return this.urlData.FormatUrl("user_ChangePassword");
            }
        }
    }
}

