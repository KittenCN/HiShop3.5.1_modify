namespace Hidistro.Entities
{
    using Hidistro.Core;
    using System;
    using System.Web;
    using System.Xml;

    public class VTemplateHelper
    {
        public int GetCategoryMaxNum()
        {
            int result = 1;
            int.TryParse(new VTemplateHelper().GetXmlNode().SelectSingleNode("root/CategoryMaxNum").InnerText, out result);
            return result;
        }

        public int GetCategoryProductMaxNum()
        {
            int result = 1;
            int.TryParse(new VTemplateHelper().GetXmlNode().SelectSingleNode("root/CategoryProductMaxNum").InnerText, out result);
            return result;
        }

        public string GetDefaultBg()
        {
            return new VTemplateHelper().GetXmlNode().SelectSingleNode("root/DefaultBg").InnerText;
        }

        public int GetTopicMaxNum()
        {
            int result = 1;
            int.TryParse(new VTemplateHelper().GetXmlNode().SelectSingleNode("root/TopicMaxNum").InnerText, out result);
            return result;
        }

        public int GetTopicProductMaxNum()
        {
            int result = 1;
            int.TryParse(new VTemplateHelper().GetXmlNode().SelectSingleNode("root/TopicProductMaxNum").InnerText, out result);
            return result;
        }

        public XmlDocument GetXmlNode()
        {
            XmlDocument document = HiCache.Get("TemplateFileCache") as XmlDocument;
            if (document == null)
            {
                document = new XmlDocument();
                document.Load(HttpContext.Current.Request.MapPath(Globals.GetVshopSkinPath(null) + "/template.xml"));
            }
            return document;
        }
    }
}

