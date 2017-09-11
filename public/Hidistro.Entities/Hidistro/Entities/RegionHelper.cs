namespace Hidistro.Entities
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;

    public static class RegionHelper
    {
        private static XmlNode FindNode(int id)
        {
            string str = id.ToString(CultureInfo.InvariantCulture);
            string xpath = string.Format("//county[@id='{0}']", str);
            XmlDocument regionDocument = GetRegionDocument();
            XmlNode node = regionDocument.SelectSingleNode(xpath);
            if (node != null)
            {
                return node;
            }
            xpath = string.Format("//city[@id='{0}']", str);
            node = regionDocument.SelectSingleNode(xpath);
            if (node != null)
            {
                return node;
            }
            xpath = string.Format("//province[@id='{0}']", str);
            node = regionDocument.SelectSingleNode(xpath);
            if (node != null)
            {
                return node;
            }
            return null;
        }

        public static string GetAllChild(int currentRegionId)
        {
            string str = currentRegionId.ToString();
            XmlNode node = FindNode(currentRegionId);
            if (node != null)
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    str = str + "," + node2.Attributes["id"].Value;
                }
            }
            return str;
        }

        public static Dictionary<int, string> GetAllCitys()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            XmlNode node = GetRegionDocument().SelectSingleNode("root");
            string str = "";
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.ChildNodes.Count > 0)
                {
                    foreach (XmlNode node3 in node2.ChildNodes)
                    {
                        str = node3.Attributes["name"].Value;
                        if (node3.ChildNodes.Count > 0)
                        {
                            foreach (XmlNode node4 in node3.ChildNodes)
                            {
                                dictionary.Add(int.Parse(node4.Attributes["id"].Value), str + "-" + node4.Attributes["name"].Value);
                            }
                        }
                    }
                }
            }
            return dictionary;
        }

        public static Dictionary<int, string> GetAllProvinces()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (XmlNode node in GetRegionDocument().SelectNodes("//province"))
            {
                dictionary.Add(int.Parse(node.Attributes["id"].Value), node.Attributes["name"].Value);
            }
            return dictionary;
        }

        public static Dictionary<int, string> GetAllRegionIds(bool readRegion, bool readprovince, bool readcity, bool readcounty)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (XmlNode node2 in GetRegionDocument().SelectSingleNode("root").ChildNodes)
            {
                if (readRegion)
                {
                    dictionary.Add(int.Parse(node2.Attributes["id"].Value), node2.Attributes["name"].Value);
                }
                if (node2.ChildNodes.Count > 0)
                {
                    foreach (XmlNode node3 in node2.ChildNodes)
                    {
                        if (readprovince)
                        {
                            dictionary.Add(int.Parse(node3.Attributes["id"].Value), node3.Attributes["name"].Value);
                        }
                        if (node3.ChildNodes.Count > 0)
                        {
                            foreach (XmlNode node4 in node3.ChildNodes)
                            {
                                if (readcity)
                                {
                                    dictionary.Add(int.Parse(node4.Attributes["id"].Value), node4.Attributes["name"].Value);
                                }
                                if (node4.ChildNodes.Count > 0)
                                {
                                    foreach (XmlNode node5 in node4.ChildNodes)
                                    {
                                        if (readcounty)
                                        {
                                            dictionary.Add(int.Parse(node5.Attributes["id"].Value), node5.Attributes["name"].Value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dictionary;
        }

        private static Dictionary<int, string> GetChildList(string xpath)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (XmlNode node2 in GetRegionDocument().SelectSingleNode(xpath).ChildNodes)
            {
                dictionary.Add(int.Parse(node2.Attributes["id"].Value), node2.Attributes["name"].Value);
            }
            return dictionary;
        }

        public static string GetCity(int currentRegionId)
        {
            XmlNode node = FindNode(currentRegionId);
            if (node == null)
            {
                return currentRegionId.ToString();
            }
            string str = currentRegionId.ToString();
            for (XmlNode node2 = node.ParentNode; node2.Name != "region"; node2 = node2.ParentNode)
            {
                if (node2.Name == "city")
                {
                    str = node2.Attributes["id"].Value;
                }
            }
            if (str == "0")
            {
                str = currentRegionId.ToString();
            }
            return str;
        }

        public static Dictionary<int, string> GetCitys(int provinceId)
        {
            return GetChildList(string.Format("root/region/province[@id='{0}']", provinceId.ToString(CultureInfo.InvariantCulture)));
        }

        public static Dictionary<int, string> GetCountys(int cityId)
        {
            return GetChildList(string.Format("root/region/province/city[@id='{0}']", cityId.ToString(CultureInfo.InvariantCulture)));
        }

        public static string GetFullPath(int currentRegionId)
        {
            XmlNode node = FindNode(currentRegionId);
            if (node == null)
            {
                return string.Empty;
            }
            string str = node.Attributes["id"].Value;
            for (XmlNode node2 = node.ParentNode; node2.Name != "region"; node2 = node2.ParentNode)
            {
                str = node2.Attributes["id"].Value + "," + str;
            }
            return str;
        }

        public static string GetFullRegion(int currentRegionId, string separator)
        {
            XmlNode node = FindNode(currentRegionId);
            if (node == null)
            {
                return currentRegionId.ToString();
            }
            string str = node.Attributes["name"].Value;
            for (XmlNode node2 = node.ParentNode; node2.Name != "region"; node2 = node2.ParentNode)
            {
                str = node2.Attributes["name"].Value + separator + str;
                if (node2.Name == "city")
                {
                    string text1 = node2.Attributes["id"].Value;
                }
            }
            return str;
        }

        public static Dictionary<int, string> GetProvinces(int regionId)
        {
            return GetChildList(string.Format("root/region[@id='{0}']", regionId.ToString(CultureInfo.InvariantCulture)));
        }

        public static XmlNode GetRegion(int regionId)
        {
            return FindNode(regionId);
        }

        private static XmlDocument GetRegionDocument()
        {
            XmlDocument document = HiCache.Get("FileCache-Regions") as XmlDocument;
            if (document == null)
            {
                string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/config/region.config");
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max("FileCache-Regions", document, new CacheDependency(filename));
            }
            return document;
        }

        public static int GetRegionId(string county, string city, string province)
        {
            string xpath = string.Format("//province[@name='{0}']", province);
            XmlNode node = GetRegionDocument().SelectSingleNode(xpath);
            if (node == null)
            {
                return 0;
            }
            int num = int.Parse(node.Attributes["id"].Value);
            xpath = string.Format("//province[@id='{0}']/city[@name='{1}']", num, city);
            node = node.SelectSingleNode(xpath);
            if (node != null)
            {
                num = int.Parse(node.Attributes["id"].Value);
                xpath = string.Format("//city[@id='{0}']/county[@name='{1}']", num, county);
                node = node.SelectSingleNode(xpath);
                if (node != null)
                {
                    num = int.Parse(node.Attributes["id"].Value);
                }
            }
            return num;
        }

        public static Dictionary<int, string> GetRegions()
        {
            return GetChildList("root");
        }

        public static int GetTopRegionId(int currentRegionId)
        {
            XmlNode node = FindNode(currentRegionId);
            if (node == null)
            {
                return 0;
            }
            int num = currentRegionId;
            for (XmlNode node2 = node.ParentNode; node2.Name != "region"; node2 = node2.ParentNode)
            {
                num = int.Parse(node2.Attributes["id"].Value);
            }
            return num;
        }
    }
}

