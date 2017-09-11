namespace Hidistro.UI.Web.Admin.Shop
{
    using Ajax;
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class CustomPageManage : AdminPage
    {
        protected Pager pager;
        protected Repeater Repeater1;
        public string showUrl;
        public int status;
        public const string tempFileDic = "/admin/shop/ShopEdit.aspx";
        public const string tempImgDic = "/Templates/vshop/";
        public string templateCuName;
        public string tempLatePath;
        protected HtmlForm thisForm;

        public CustomPageManage() : base("m01", "dpp13")
        {
            this.status = 1;
            this.tempLatePath = "";
            this.templateCuName = "";
            this.showUrl = "";
        }

        public override void DataBind()
        {
            CustomPageQuery query = new CustomPageQuery {
                Name = this.Page.Request.QueryString["Name"],
                Status = new int?(this.status),
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize
            };
            DbQueryResult pages = CustomPageHelp.GetPages(query);
            this.Repeater1.DataSource = pages.Data;
            this.Repeater1.DataBind();
            this.pager.TotalRecords = pages.TotalRecords;
        }

        [AjaxMethod]
        public bool DeleteCustomPage(int id)
        {
            if (id < 1)
            {
                return false;
            }
            return CustomPageHelp.DeletePage(id);
        }

        public string GetDraftPageUrl(string url)
        {
            int port = base.Request.Url.Port;
            string str = (port == 80) ? "" : (":" + port.ToString());
            return ("http://" + base.Request.Url.Host + str + "/draftcustom/" + url);
        }

        public string GetImgName(string fileName)
        {
            return ("/Templates/vshop/" + fileName + "/default.png");
        }

        public void GetIndexName(string tempName)
        {
            XmlDocument document = new XmlDocument();
            DirectoryInfo info = new DirectoryInfo(base.Server.MapPath("/Templates/vshop/" + tempName));
            string str = info.Name.ToLower(CultureInfo.InvariantCulture);
            if ((str.Length > 0) && !str.StartsWith("_"))
            {
                foreach (FileInfo info2 in info.GetFiles("template.xml"))
                {
                    FileStream inStream = info2.OpenRead();
                    document.Load(inStream);
                    inStream.Close();
                    this.templateCuName = document.SelectSingleNode("root/Name").InnerText;
                }
            }
        }

        public void GetIndexUrl()
        {
            int port = base.Request.Url.Port;
            string str = (port == 80) ? "" : (":" + port.ToString());
            this.showUrl = "http://" + base.Request.Url.Host + str + Globals.ApplicationPath + "/default.aspx";
        }

        public string GetPageUrl(string url)
        {
            int port = base.Request.Url.Port;
            string str = (port == 80) ? "" : (":" + port.ToString());
            return ("http://" + base.Request.Url.Host + str + "/custom/" + url);
        }

        public string GetTempLateLogicName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return fileName.Substring(0, base.Eval("Name").ToString().LastIndexOf("."));
            }
            return "ti";
        }

        public string GetTempUrl(string tempLateLogicName)
        {
            if (!string.IsNullOrEmpty(tempLateLogicName))
            {
                return ("/admin/shop/ShopEdit.aspx?tempName=" + tempLateLogicName);
            }
            return "/admin/shop/ShopEdit.aspx?tempName=ti";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.RegisterTypeForAjax(typeof(CustomPageManage));
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                this.tempLatePath = masterSettings.VTheme;
                this.GetIndexName(this.tempLatePath);
                this.status = (this.Page.Request.QueryString["Status"] != null) ? Convert.ToInt32(this.Page.Request.QueryString["Status"]) : 0;
                this.DataBind();
                this.GetIndexUrl();
            }
        }
    }
}

