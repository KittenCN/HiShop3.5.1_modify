namespace Hidistro.Core.Entities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class CustomerServiceSettings
    {
        public static CustomerServiceSettings FromXml(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("Settings");
            return new CustomerServiceSettings { AppId = node.SelectSingleNode("AppId").InnerText, AppSecret = node.SelectSingleNode("AppSecret").InnerText, unitid = node.SelectSingleNode("unitid").InnerText, unit = node.SelectSingleNode("unit").InnerText, password = node.SelectSingleNode("password").InnerText, unitname = node.SelectSingleNode("unitname").InnerText, activated = node.SelectSingleNode("activated").InnerText, logo = node.SelectSingleNode("logo").InnerText, url = node.SelectSingleNode("url").InnerText, tel = node.SelectSingleNode("tel").InnerText, contact = node.SelectSingleNode("contact").InnerText, location = node.SelectSingleNode("location").InnerText };
        }

        private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
        {
            XmlNode newChild = root.SelectSingleNode(nodeName);
            if (newChild == null)
            {
                newChild = doc.CreateElement(nodeName);
                root.AppendChild(newChild);
            }
            newChild.InnerText = nodeValue;
        }

        public void WriteToXml(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("Settings");
            SetNodeValue(doc, root, "AppId", this.AppId);
            SetNodeValue(doc, root, "AppSecret", this.AppSecret);
            SetNodeValue(doc, root, "unitid", this.unitid);
            SetNodeValue(doc, root, "unit", this.unit);
            SetNodeValue(doc, root, "password", this.password);
            SetNodeValue(doc, root, "unitname", this.unitname);
            SetNodeValue(doc, root, "activated", this.activated);
            SetNodeValue(doc, root, "logo", this.logo);
            SetNodeValue(doc, root, "url", this.url);
            SetNodeValue(doc, root, "tel", this.tel);
            SetNodeValue(doc, root, "contact", this.contact);
            SetNodeValue(doc, root, "location", this.location);
        }

        public string activated { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string contact { get; set; }

        public string location { get; set; }

        public string logo { get; set; }

        public string password { get; set; }

        public string tel { get; set; }

        public string unit { get; set; }

        public string unitid { get; set; }

        public string unitname { get; set; }

        public string url { get; set; }
    }
}

