using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web
{

    public partial class Custom : System.Web.UI.Page
    {
        protected string cssLinkStr = string.Empty;
        public string cssSrc = "/Templates/vshop/";          //样式路径
        public bool showMenu = false;
        public string siteName = string.Empty;
        public string imgUrl = string.Empty;
        public string Desc = string.Empty;//店铺简介
        public SiteSettings siteSettings;
        //public string memberID = "";
        protected string htmlTitleName = string.Empty;//店铺名
        protected string WeixinfollowUrl = "";
        protected string AlinfollowUrl = "";

        //public string ShowCopyRight = string.Empty;
        protected bool EnabeHomePageBottomLink = false;
        protected bool EnableHomePageBottomCopyright = false;
        protected bool EnableGuidePageSet = false;
        protected bool IsAutoGuide = false;
        protected bool IsMustConcern = false;
        protected string DistributionLinkName = "";
        protected string DistributionLink = "";
        protected string CopyrightLinkName = "";
        protected string CopyrightLink = "";
        protected bool IsShowMenu = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            string custPath = Page.RouteData.Values["custpath"] != null ? Page.RouteData.Values["custpath"].ToString() : "notfound";

            H_Page.CustomPagePath = custPath;
            var model = CustomPageHelp.GetCustomPageByPath(custPath);
            if (model == null)
                Response.Redirect("/default.aspx");

            IsShowMenu = model.IsShowMenu;
            string reId = Request.QueryString["ReferralId"];
            if (!string.IsNullOrEmpty(reId)) 
            {
                model.PV += 1;
                CustomPageHelp.Update(model);
            }
            if (!string.IsNullOrEmpty(model.TempIndexName))
            {
                cssSrc += model.TempIndexName + "/css/head.css";
                cssLinkStr = "<link rel=\"stylesheet\" href=\"" + cssSrc + "\">";
            }
            siteSettings = SettingsManager.GetMasterSettings(true);
            //ShowCopyRight = siteSettings.ShowCopyRight;
            htmlTitleName = model.Name;
            Desc = model.Details;
            string userAgent = Page.Request.UserAgent;//获取浏览器信息
            //EnabeHomePageBottomLink = siteSettings.EnabeHomePageBottomLink;//控制底部菜单是否显示
            //EnableHomePageBottomCopyright = siteSettings.EnableHomePageBottomCopyright;//控制是否显示版权信息
            //EnableGuidePageSet = siteSettings.EnableGuidePageSet;
            //IsAutoGuide = siteSettings.IsAutoGuide;
            //IsMustConcern = siteSettings.IsMustConcern;
            //DistributionLinkName = siteSettings.DistributionLinkName;//分销申请栏目名称
            //DistributionLink = string.IsNullOrEmpty(siteSettings.DistributionLink) ? "javascript:void(0);" : siteSettings.DistributionLink; //分销申请链接
            //CopyrightLinkName = siteSettings.CopyrightLinkName;//版本信息文字
            //CopyrightLink = string.IsNullOrEmpty(siteSettings.CopyrightLink) ? "javascript:void(0);" : siteSettings.CopyrightLink; //文字对应跳转链接

            //if (siteSettings.EnableAliPayFuwuGuidePageSet)
            //{
            //    AlinfollowUrl = siteSettings.AliPayFuwuGuidePageSet;//服务窗口关注连接
            //}
            //if (siteSettings.EnableGuidePageSet)
            //{
            //    WeixinfollowUrl = siteSettings.GuidePageSet; //微信关注连接
            //}

            if (!IsPostBack)
            {
                HiAffiliation.LoadPage();//放在前面，防止参数丢失

                string weiXinOpenID = Globals.GetCurrentWXOpenId;
                int goId = Globals.RequestQueryNum("go");
                if (userAgent.ToLower().Contains("micromessenger") && string.IsNullOrEmpty(weiXinOpenID) && siteSettings.IsValidationService && goId != 1)   //判断是否是在微信中打开
                {
                    Page.Response.Redirect("Follow.aspx?ReferralId=" + Globals.GetCurrentDistributorId());
                    Page.Response.End();
                }
                int memberID = Globals.GetCurrentMemberUserId();
                if (memberID == 0 && siteSettings.IsAutoToLogin && userAgent.ToLower().Contains("micromessenger"))//判断是否是在微信中打开
                {
                    Uri linkUri = HttpContext.Current.Request.Url;
                    string toUrl = Globals.GetWebUrlStart() + "/default.aspx?ReferralId=" + Globals.RequestQueryNum("ReferralId").ToString();
                    Response.Redirect("/UserLogining.aspx?returnUrl=" + Globals.UrlEncode(toUrl));
                    Response.End();
                }


                showMenu = siteSettings.EnableShopMenu;
               
                BindWXInfo();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter html = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(html);
            base.Render(tw);
            tw.Flush();
            tw.Close();
            string outhtml = html.ToString();
            //outhtml = Regex.Replace(outhtml, "\\s+", " ");
            //outhtml = Regex.Replace(outhtml, ">\\s+<", "><");
            outhtml = outhtml.Trim();
            string localUrl = Globals.GetWebUrlStart();
            outhtml = outhtml.Replace("<img src=\"" + localUrl + "/Storage/master/product/", "<img class='imgLazyLoading' src=\"/Utility/pics/lazy-ico.gif\" data-original=\"" + localUrl + "/Storage/master/product/");
            outhtml = outhtml.Replace("<img src=\"/Storage/master/product/", "<img class='imgLazyLoading' src=\"/Utility/pics/lazy-ico.gif\" data-original=\"/Storage/master/product/");

            //outhtml = Regex.Replace(outhtml, @"<img[^>]*\bsrc=('|"")([^'"">]*)\1[^>]*>", "<img alt='' src='$2' />", RegexOptions.IgnoreCase);
            writer.Write(outhtml);

        }
        public void BindWXInfo()
        {
            int distributorId = Globals.GetCurrentDistributorId();//获取当前所属分销商的ID
            if (distributorId > 0)
            {
                DistributorsInfo model = DistributorsBrower.GetDistributorInfo(distributorId);
                if (model != null)
                {
                    siteName = model.StoreName;
                    imgUrl = "http://" + HttpContext.Current.Request.Url.Host + model.Logo;
                    //Desc = model.StoreDescription;
                }
            }
            //如果未读取到站点信息，则读取配置
            if (string.IsNullOrEmpty(siteName))
            {
                siteName = siteSettings.SiteName;
                imgUrl = "http://" + HttpContext.Current.Request.Url.Host + siteSettings.DistributorLogoPic;
                //Desc = siteSettings.ShopIntroduction;
            }
            //htmlTitleName = siteName;// string.Format(CultureInfo.InvariantCulture, "{0} - {1}", "店铺主页", siteName);
        }

    }
}