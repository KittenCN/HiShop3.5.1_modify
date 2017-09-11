namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class WifiSet : AdminPage
    {
        protected Button btnSave;
        protected HiddenField hd_id;
        protected Script Script5;
        protected Script Script6;
        private SiteSettings siteSettings;
        protected TextBox txt_wifiDescribe;
        protected TextBox txt_wifiName;
        protected TextBox txt_wifiPwd;

        protected WifiSet() : base("m06", "wxp11")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(this.hd_id.Value);
            string str = this.txt_wifiName.Text.Trim();
            string str2 = this.txt_wifiPwd.Text.Trim();
            string wifiDescribe = this.txt_wifiDescribe.Text.Trim();
            string text1 = "wifi_" + str + "|" + str2;
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str2))
            {
                if (id > 0)
                {
                    UpdateXML(id, str, str2, wifiDescribe);
                }
                else
                {
                    InsertXMl(str, str2, wifiDescribe);
                }
                base.Response.Redirect("WifiSetList.aspx");
            }
        }

        protected void DeleteData()
        {
            string id = base.Request.QueryString["id"];
            DeleteXML(id);
            base.Response.Redirect("WifiSetList.aspx");
        }

        internal static void DeleteXML(string id)
        {
            XmlDocument document = new XmlDocument();
            string filename = HttpContext.Current.Server.MapPath("/config/WifiConfig.xml");
            document.Load(filename);
            XmlNode oldChild = document.DocumentElement.SelectSingleNode("/WifiConfig/WifiConfigs[@id=" + id + "]");
            if (oldChild != null)
            {
                oldChild.ParentNode.RemoveChild(oldChild);
            }
            document.Save(filename);
        }

        protected void InitData()
        {
            string str = base.Request.QueryString["id"];
            DataSet set = new DataSet();
            string path = HttpContext.Current.Server.MapPath("/config/WifiConfig.xml");
            if (File.Exists(path))
            {
                set.ReadXml(path);
                if (set != null)
                {
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        if (row["id"].ToString() == str)
                        {
                            this.hd_id.Value = str;
                            this.txt_wifiName.Text = row["wifiName"].ToString();
                            this.txt_wifiPwd.Text = row["wifiPwd"].ToString();
                            this.txt_wifiDescribe.Text = row["wifiDescribe"].ToString();
                        }
                    }
                }
            }
        }

        internal static void InsertXMl(string wifiName, string wifiPwd, string wifiDescribe)
        {
            string path = HttpContext.Current.Server.MapPath("/config/WifiConfig.xml");
            int num = 100;
            XmlDocument document = new XmlDocument();
            if (!File.Exists(path))
            {
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "utf-8", null);
                XmlElement documentElement = document.DocumentElement;
                document.InsertBefore(declaration, documentElement);
                XmlElement element2 = document.CreateElement("WifiConfig");
                document.AppendChild(element2);
                document.Save(path);
            }
            document.Load(path);
            if (document.SelectSingleNode("WifiConfig").ChildNodes.Count > 0)
            {
                num = Convert.ToInt32(document.DocumentElement.SelectSingleNode("/WifiConfig/WifiConfigs[last()]").Attributes["id"].Value) + 1;
            }
            XmlElement newChild = document.CreateElement("WifiConfigs");
            XmlAttribute node = document.CreateAttribute("id");
            node.Value = num.ToString();
            newChild.Attributes.Append(node);
            XmlElement element4 = document.CreateElement("WifiName");
            element4.InnerText = wifiName;
            newChild.AppendChild(element4);
            XmlElement element5 = document.CreateElement("WifiPwd");
            element5.InnerText = wifiPwd;
            newChild.AppendChild(element5);
            XmlElement element6 = document.CreateElement("WifiDescribe");
            element6.InnerText = wifiDescribe;
            newChild.AppendChild(element6);
            document.DocumentElement.AppendChild(newChild);
            document.Save(path);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (base.Request.QueryString["action"] == "edit")
                {
                    this.InitData();
                }
                else if (base.Request.QueryString["action"] == "delete")
                {
                    this.DeleteData();
                }
            }
        }

        internal static void UpdateXML(int id, string wifiName, string wifiPwd, string wifiDescribe)
        {
            XmlDocument document = new XmlDocument();
            string filename = HttpContext.Current.Server.MapPath("/config/WifiConfig.xml");
            document.Load(filename);
            foreach (XmlNode node in document.SelectSingleNode("WifiConfig").ChildNodes)
            {
                XmlElement element = (XmlElement) node;
                if (element.GetAttribute("id") == id.ToString())
                {
                    foreach (XmlNode node2 in element.ChildNodes)
                    {
                        XmlElement element2 = (XmlElement) node2;
                        if (element2.Name == "WifiName")
                        {
                            element2.InnerText = wifiName;
                        }
                        else if (element2.Name == "WifiPwd")
                        {
                            element2.InnerText = wifiPwd;
                        }
                        else if (element2.Name == "WifiDescribe")
                        {
                            element2.InnerText = wifiDescribe;
                        }
                    }
                }
            }
            document.Save(filename);
        }
    }
}

