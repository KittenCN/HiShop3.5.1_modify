namespace Hidistro.UI.Web.Admin.Shop
{
    using Ajax;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class SelectTemplate : AdminPage
    {
        protected Repeater Repeater1;
        public string showUrl;
        public const string tempFileDic = "/admin/shop/ShopEdit.aspx";
        public const string tempImgDic = "/Templates/vshop/";
        public string templateCuName;
        public string tempLatePath;
        protected HtmlForm thisForm;

        public SelectTemplate() : base("m01", "dpp13")
        {
            this.tempLatePath = "";
            this.templateCuName = "";
            this.showUrl = "";
        }

        [AjaxMethod]
        public int CreateCustomTemplate(string tempName)
        {
            CustomPage page=new CustomPage ();
            if (string.IsNullOrEmpty(tempName))
            {
                return 0;
            }
            page = new CustomPage {
                Name = "页面名称",
                CreateTime = DateTime.Now,
                Status = 1,
                TempIndexName = (tempName == "none") ? "" : tempName,
                PageUrl = page.CreateTime.ToString("yyyyMMddHHmmss"),
                Details = "自定义页面",
                IsShowMenu = true,
                DraftDetails = "自定义页面",
                DraftName = "页面名称",
                DraftPageUrl = page.PageUrl,
                PV = 0,
                DraftJson = (tempName == "none") ? this.GetCustomTempDefaultJson() : this.GetIndexTempJsonByName(tempName),
                FormalJson = (tempName == "none") ? this.GetCustomTempDefaultJson() : this.GetIndexTempJsonByName(tempName)
            };
            return CustomPageHelp.Create(page);
        }

        public override void DataBind()
        {
            this.Repeater1.DataSource = this.LoadThemes();
            this.Repeater1.DataBind();
        }

        public string GetCustomTempDefaultJson()
        {
            StreamReader reader = new StreamReader(base.Server.MapPath("/Templates/vshop/custom/default/data/default.json"), Encoding.UTF8);
            try
            {
                string str = reader.ReadToEnd();
                reader.Close();
                return str.Replace("\r\n", "").Replace("\n", "");
            }
            catch
            {
                return "";
            }
        }

        public string GetImgName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && !(fileName == "none"))
            {
                return ("/Templates/vshop/" + fileName + "/default.png");
            }
            return "/Templates/vshop/custom/default/empty.jpg";
        }

        public string GetIndexTempJsonByName(string tempName)
        {
            StreamReader reader = new StreamReader(base.Server.MapPath("/Templates/vshop/" + tempName + "/data/default.json"), Encoding.UTF8);
            try
            {
                string str = reader.ReadToEnd();
                reader.Close();
                return str.Replace("\r\n", "").Replace("\n", "");
            }
            catch
            {
                return "";
            }
        }

        protected IList<ShopIndex.ManageThemeInfo> LoadThemes()
        {
            XmlDocument document = new XmlDocument();
            IList<ShopIndex.ManageThemeInfo> list = new List<ShopIndex.ManageThemeInfo>();
            string[] strArray = Directory.Exists(base.Server.MapPath("/Templates/vshop/")) ? Directory.GetDirectories(base.Server.MapPath("/Templates/vshop/")) : null;
            foreach (string str in strArray)
            {
                DirectoryInfo info = new DirectoryInfo(str);
                string str2 = info.Name.ToLower(CultureInfo.InvariantCulture);
                if ((str2.Length > 0) && !str2.StartsWith("_"))
                {
                    foreach (FileInfo info2 in info.GetFiles("template.xml"))
                    {
                        ShopIndex.ManageThemeInfo info3 = new ShopIndex.ManageThemeInfo();
                        FileStream inStream = info2.OpenRead();
                        document.Load(inStream);
                        inStream.Close();
                        info3.Name = document.SelectSingleNode("root/Name").InnerText;
                        info3.ThemeName = str2;
                        if (str2 == this.tempLatePath)
                        {
                            this.templateCuName = document.SelectSingleNode("root/Name").InnerText;
                        }
                        list.Add(info3);
                    }
                }
            }
            ShopIndex.ManageThemeInfo item = new ShopIndex.ManageThemeInfo {
                Name = "空白模板",
                ThemeImgUrl = "/admin/shop/Public/images/empty.jpg",
                ThemeName = "none"
            };
            list.Insert(0, item);
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.RegisterTypeForAjax(typeof(SelectTemplate));
            if (!base.IsPostBack)
            {
                int port = base.Request.Url.Port;
                string str = (port == 80) ? "" : (":" + port.ToString());
                this.showUrl = "http://" + base.Request.Url.Host + str + Globals.ApplicationPath + "/default.aspx";
                SettingsManager.GetMasterSettings(true);
                this.DataBind();
            }
        }
    }
}

