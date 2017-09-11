namespace Hidistro.UI.Web.Admin.Shop
{
    using Ajax;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ShopIndex : AdminPage
    {
        protected Repeater Repeater1;
        public string showUrl;
        public const string tempFileDic = "/admin/shop/ShopEdit.aspx";
        public const string tempImgDic = "/Templates/vshop/";
        public string templateCuName;
        public string tempLatePath;
        protected HtmlForm thisForm;

        public ShopIndex() : base("m01", "dpp03")
        {
            this.tempLatePath = "";
            this.templateCuName = "";
            this.showUrl = "";
        }

        public override void DataBind()
        {
            this.Repeater1.DataSource = this.LoadThemes();
            this.Repeater1.DataBind();
        }

        [AjaxMethod]
        public bool EnableTemp(string TempName)
        {
            if (string.IsNullOrEmpty(TempName))
            {
                return false;
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            masterSettings.VTheme = TempName;
            SettingsManager.Save(masterSettings);
            HiCache.Remove("TemplateFileCache");
            return true;
        }

        public string GetImgName(string fileName)
        {
            return ("/Templates/vshop/" + fileName + "/default.png");
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

        protected IList<ManageThemeInfo> LoadThemes()
        {
            XmlDocument document = new XmlDocument();
            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
            string[] strArray = Directory.Exists(base.Server.MapPath("/Templates/vshop/")) ? Directory.GetDirectories(base.Server.MapPath("/Templates/vshop/")) : null;
            foreach (string str in strArray)
            {
                DirectoryInfo info = new DirectoryInfo(str);
                string str2 = info.Name.ToLower(CultureInfo.InvariantCulture);
                if ((str2.Length > 0) && !str2.StartsWith("_"))
                {
                    foreach (FileInfo info2 in info.GetFiles("template.xml"))
                    {
                        ManageThemeInfo item = new ManageThemeInfo();
                        FileStream inStream = info2.OpenRead();
                        document.Load(inStream);
                        inStream.Close();
                        item.Name = document.SelectSingleNode("root/Name").InnerText;
                        item.ThemeName = str2;
                        if (str2 == this.tempLatePath)
                        {
                            this.templateCuName = document.SelectSingleNode("root/Name").InnerText;
                        }
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.RegisterTypeForAjax(typeof(ShopIndex));
            if (!base.IsPostBack)
            {
                int port = base.Request.Url.Port;
                string str = (port == 80) ? "" : (":" + port.ToString());
                this.showUrl = "http://" + base.Request.Url.Host + str + Globals.ApplicationPath + "/default.aspx";
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                this.tempLatePath = masterSettings.VTheme;
                this.DataBind();
            }
        }

        public class ManageThemeInfo
        {
            public string Name { get; set; }

            public string ThemeImgUrl { get; set; }

            public string ThemeName { get; set; }
        }
    }
}

