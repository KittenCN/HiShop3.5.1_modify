namespace Hidistro.UI.Web.Vshop
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.CodeBehind;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;

    public class Default : Page
    {
        protected string AlinfollowUrl = "";
        protected string concernMsg = "";
        protected string CopyrightLink = "";
        protected string CopyrightLinkName = "";
        public string cssSrc = "/Templates/vshop/";
        public string Desc = string.Empty;
        protected string DistributionLink = "";
        protected string DistributionLinkName = "";
        protected bool EnabeHomePageBottomLink;
        protected bool EnableAliPayFuwuGuidePageSet;
        protected bool EnableGuidePageSet;
        protected bool EnableHomePageBottomCopyright;
        protected int guideConcernType;
        protected string guidePageSet = "";
        protected HomePage H_Page;
        protected string htmlTitleName = string.Empty;
        public string imgUrl = string.Empty;
        protected bool IsAutoGuide;
        protected int IsFollowAliFuwu;
        protected int IsFollowWeiXin;
        protected bool IsHomeShowFloatMenu;
        protected bool IsMustConcern;
        protected Hidistro.UI.Common.Controls.MeiQiaSet MeiQiaSet;
        public bool showMenu;
        public string siteName = string.Empty;
        public SiteSettings siteSettings;
        protected WeixinSet weixin;
        protected string WeixinfollowUrl = "";

        public void BindInfo()
        {
            int currentDistributorId = Globals.GetCurrentDistributorId();
            if ((currentDistributorId > 0) && this.siteSettings.IsShowDistributorSelfStoreName)
            {
                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(currentDistributorId);
                if (distributorInfo != null)
                {
                    this.siteName = distributorInfo.StoreName;
                    this.imgUrl = Globals.GetWebUrlStart() + distributorInfo.Logo;
                    this.Desc = distributorInfo.StoreDescription;
                }
            }
            if (string.IsNullOrEmpty(this.siteName))
            {
                this.siteName = this.siteSettings.SiteName;
                this.imgUrl = Globals.GetWebUrlStart() + this.siteSettings.DistributorLogoPic;
                this.Desc = this.siteSettings.ShopIntroduction;
            }
            this.htmlTitleName = this.siteName;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.siteSettings = SettingsManager.GetMasterSettings(true);
            this.htmlTitleName = this.siteSettings.SiteName;
            string userAgent = this.Page.Request.UserAgent;
            this.EnabeHomePageBottomLink = this.siteSettings.EnabeHomePageBottomLink;
            this.EnableHomePageBottomCopyright = this.siteSettings.EnableHomePageBottomCopyright;
            this.EnableGuidePageSet = this.siteSettings.EnableGuidePageSet;
            this.IsAutoGuide = this.siteSettings.IsAutoGuide;
            this.IsMustConcern = this.siteSettings.IsMustConcern;
            this.DistributionLinkName = this.siteSettings.DistributionLinkName;
            this.DistributionLink = string.IsNullOrEmpty(this.siteSettings.DistributionLink) ? "javascript:void(0);" : this.siteSettings.DistributionLink;
            this.CopyrightLinkName = this.siteSettings.CopyrightLinkName;
            this.CopyrightLink = string.IsNullOrEmpty(this.siteSettings.CopyrightLink) ? "javascript:void(0);" : this.siteSettings.CopyrightLink;
            this.guideConcernType = this.siteSettings.GuideConcernType;
            this.guidePageSet = this.siteSettings.GuidePageSet;
            this.concernMsg = this.siteSettings.ConcernMsg;
            this.EnableAliPayFuwuGuidePageSet = this.siteSettings.EnableAliPayFuwuGuidePageSet;
            if (this.siteSettings.EnableAliPayFuwuGuidePageSet)
            {
                this.AlinfollowUrl = this.siteSettings.AliPayFuwuGuidePageSet;
            }
            if (this.siteSettings.EnableGuidePageSet)
            {
                this.WeixinfollowUrl = this.siteSettings.GuidePageSet;
            }
            this.IsHomeShowFloatMenu = this.siteSettings.IsHomeShowFloatMenu;
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
                int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
                if (((currentMemberUserId == 0) && this.siteSettings.IsAutoToLogin) && userAgent.ToLower().Contains("micromessenger"))
                {
                    Uri url = HttpContext.Current.Request.Url;
                    string urlToEncode = Globals.GetWebUrlStart() + "/default.aspx?ReferralId=" + Globals.RequestQueryNum("ReferralId").ToString();
                    base.Response.Redirect("/UserLogining.aspx?returnUrl=" + Globals.UrlEncode(urlToEncode));
                    base.Response.End();
                }
                MemberInfo member = MemberHelper.GetMember(currentMemberUserId);
                if (member != null)
                {
                    this.IsFollowWeiXin = member.IsFollowWeixin;
                    this.IsFollowAliFuwu = member.IsFollowAlipay;
                }
                this.showMenu = this.siteSettings.EnableShopMenu;
                this.cssSrc = this.cssSrc + this.siteSettings.VTheme + "/css/head.css";
                this.BindInfo();
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

