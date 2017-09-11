namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ShopEdit : AdminPage
    {
        public string cssSrc;
        public bool isModuleEdit;
        protected HtmlInputHidden j_pageID;
        protected Literal La_script;
        public string scriptSrc;
        public string tempName;

        public ShopEdit() : base("m01", "dpp03")
        {
            this.scriptSrc = "/Templates/vshop/";
            this.cssSrc = "/Templates/vshop/";
        }

        public bool GetIsModuleEdit(string tempName)
        {
            string filename = base.Server.MapPath(Globals.ApplicationPath + "/Templates/vshop/" + tempName + "/template.xml");
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            return Convert.ToBoolean(document.SelectSingleNode("root/IsModuleEdit").InnerText);
        }

        public string GetTemplatescript(string tempName)
        {
            string path = base.Server.MapPath("/Templates/vshop/ti/data/default.json");
            if (!string.IsNullOrEmpty(tempName))
            {
                path = base.Server.MapPath("/Templates/vshop/" + tempName + "/script/default.json");
            }
            StreamReader reader = new StreamReader(path, Encoding.UTF8);
            try
            {
                string str2 = reader.ReadToEnd();
                reader.Close();
                return str2;
            }
            catch
            {
                return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.tempName = base.Request.QueryString["tempName"];
                if (string.IsNullOrEmpty(this.tempName))
                {
                    this.tempName = "t1";
                }
                this.j_pageID.Value = this.tempName;
                this.La_script.Text = this.GetTemplatescript(this.tempName);
                this.isModuleEdit = this.GetIsModuleEdit(this.tempName);
                this.cssSrc = this.cssSrc + this.tempName + "/css/head.css";
                this.scriptSrc = this.scriptSrc + this.tempName + "/script/head.js";
            }
        }
    }
}

