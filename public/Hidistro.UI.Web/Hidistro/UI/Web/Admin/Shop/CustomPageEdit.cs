namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class CustomPageEdit : AdminPage
    {
        protected string cssLinkSrc;
        public string cssSrc;
        public string id;
        public bool isModuleEdit;
        protected HtmlInputHidden j_pageID;
        protected Literal La_script;
        public CustomPage model;
        public int modelStatus;
        protected string scriptLinkSrc;
        public string scriptSrc;

        public CustomPageEdit() : base("m01", "dpp13")
        {
            this.scriptSrc = "/Templates/vshop/";
            this.cssSrc = "/Templates/vshop/";
            this.scriptLinkSrc = string.Empty;
            this.cssLinkSrc = string.Empty;
        }

        public string GetTemplatescript(string tempName)
        {
            if (string.IsNullOrEmpty(tempName))
            {
                return "";
            }
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
                this.id = base.Request.QueryString["id"];
                this.model = CustomPageHelp.GetCustomPageByID(Convert.ToInt32(this.id));
                this.j_pageID.Value = this.id;
                this.La_script.Text = this.GetTemplatescript(this.model.TempIndexName);
                this.isModuleEdit = true;
                this.modelStatus = this.model.Status;
                if (!string.IsNullOrEmpty(this.model.TempIndexName) && (this.model.TempIndexName != "none"))
                {
                    this.cssSrc = this.cssSrc + this.model.TempIndexName + "/css/head.css";
                    this.scriptSrc = this.scriptSrc + this.model.TempIndexName + "/script/head.js";
                    this.scriptLinkSrc = "<script src=\"" + this.scriptSrc + "\"></script>";
                    this.cssLinkSrc = "<link rel=\"stylesheet\" href=\"" + this.cssSrc + "\">";
                }
            }
        }
    }
}

