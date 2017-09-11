namespace Hidistro.UI.Web
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.CodeBehind;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;

    public class Custom : Page
    {
        protected string AlinfollowUrl = "";
        protected string CopyrightLink = "";
        protected string CopyrightLinkName = "";
        protected string cssLinkStr = string.Empty;
        public string cssSrc = "/Templates/vshop/";
        public string Desc = string.Empty;
        protected string DistributionLink = "";
        protected string DistributionLinkName = "";
        protected bool EnabeHomePageBottomLink;
        protected bool EnableGuidePageSet;
        protected bool EnableHomePageBottomCopyright;
        protected CustomHomePage H_Page;
        protected string htmlTitleName = string.Empty;
        public string imgUrl = string.Empty;
        protected bool IsAutoGuide;
        protected bool IsMustConcern;
        protected bool IsShowMenu;
        protected Hidistro.UI.Common.Controls.MeiQiaSet MeiQiaSet;
        public bool showMenu;
        public string siteName = string.Empty;
        public SiteSettings siteSettings;
        protected WeixinSet weixin;
        protected string WeixinfollowUrl = "";

        public void BindWXInfo()
        {
            int currentDistributorId = Globals.GetCurrentDistributorId();
            if (currentDistributorId > 0)
            {
                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(currentDistributorId);
                if (distributorInfo != null)
                {
                    this.siteName = distributorInfo.StoreName;
                    this.imgUrl = "http://" + HttpContext.Current.Request.Url.Host + distributorInfo.Logo;
                }
            }
            if (string.IsNullOrEmpty(this.siteName))
            {
                this.siteName = this.siteSettings.SiteName;
                this.imgUrl = "http://" + HttpContext.Current.Request.Url.Host + this.siteSettings.DistributorLogoPic;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string path = (this.Page.RouteData.Values["custpath"] != null) ? this.Page.RouteData.Values["custpath"].ToString() : "notfound";
            this.H_Page.CustomPagePath = path;
            CustomPage customPageByPath = CustomPageHelp.GetCustomPageByPath(path);
            if (customPageByPath == null)
            {
                base.Response.Redirect("/default.aspx");
            }
            this.IsShowMenu = customPageByPath.IsShowMenu;
            string str2 = base.Request.QueryString["ReferralId"];
            if (!string.IsNullOrEmpty(str2))
            {
                customPageByPath.PV++;
                CustomPageHelp.Update(customPageByPath);
            }
            if (!string.IsNullOrEmpty(customPageByPath.TempIndexName))
            {
                this.cssSrc = this.cssSrc + customPageByPath.TempIndexName + "/css/head.css";
                this.cssLinkStr = "<link rel=\"stylesheet\" href=\"" + this.cssSrc + "\">";
            }
            this.siteSettings = SettingsManager.GetMasterSettings(true);
            this.htmlTitleName = customPageByPath.Name;
            this.Desc = customPageByPath.Details;
            string userAgent = this.Page.Request.UserAgent;
            if (!base.IsPostBack)
            {
                HiAffiliation.LoadPage();
                string getCurrentWXOpenId = Globals.GetCurrentWXOpenId;
                int num = Globals.RequestQueryNum("go");
                if ((userAgent.ToLower().Contains("micromessenger") && string.IsNullOrEmpty(getCurrentWXOpenId)) && (this.siteSettings.IsValidationService && (num != 1)))
                {
                    this.Page.Response.Redirect("Follow.aspx?ReferralId=" + Globals.GetCurrentDistributorId());
                    this.Page.Response.End();
                }
                if (((Globals.GetCurrentMemberUserId(false) == 0) && this.siteSettings.IsAutoToLogin) && userAgent.ToLower().Contains("micromessenger"))
                {
                    Uri url = HttpContext.Current.Request.Url;
                    string urlToEncode = Globals.GetWebUrlStart() + "/default.aspx?ReferralId=" + Globals.RequestQueryNum("ReferralId").ToString();
                    base.Response.Redirect("/UserLogining.aspx?returnUrl=" + Globals.UrlEncode(urlToEncode));
                    base.Response.End();
                }
                this.showMenu = this.siteSettings.EnableShopMenu;
                this.BindWXInfo();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter writer2 = new StringWriter();
            HtmlTextWriter writer3 = new HtmlTextWriter(writer2);
            base.Render(writer3);
            writer3.Flush();
            writer3.Close();
            string str = writer2.ToString().Trim();
            string webUrlStart = Globals.GetWebUrlStart();
            str = str.Replace("<img src=\"" + webUrlStart + "/Storage/master/product/", "<img class='imgLazyLoading' src=\"/Utility/pics/lazy-ico.gif\" data-original=\"" + webUrlStart + "/Storage/master/product/").Replace("<img src=\"/Storage/master/product/", "<img class='imgLazyLoading' src=\"/Utility/pics/lazy-ico.gif\" data-original=\"/Storage/master/product/");
            writer.Write(str);
        }
    }
}

