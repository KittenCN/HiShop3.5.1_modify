namespace Hidistro.UI.Web.API
{
    using Entities.Settings;
    using global::ControlPanel.Settings;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class Hi_Ajax_NavMenu : IHttpHandler
    {
        public IList<MenuInfo> GetAllMenu(string shopMenuStyle)
        {
            IList<MenuInfo> list = new List<MenuInfo>();
            return MenuHelper.GetMenus(shopMenuStyle);
        }

        public string GetPhone()
        {
            int currentDistributorId = Globals.GetCurrentDistributorId();
            if (currentDistributorId != 0)
            {
                MemberInfo member = MemberProcessor.GetMember(currentDistributorId, true);
                if (member != null)
                {
                    return member.CellPhone;
                }
            }
            return SettingsManager.GetMasterSettings(true).ShopTel;
        }

        public void ProcessRequest(HttpContext context)
        {
            string userAgent = context.Request.UserAgent;
            context.Response.ContentType = "text/plain";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            IList<MenuInfo> allMenu = this.GetAllMenu(masterSettings.ShopMenuStyle);
            string guidePageSet = masterSettings.GuidePageSet;
            if (userAgent.ToLower().Contains("alipay"))
            {
                guidePageSet = masterSettings.AliPayFuwuGuidePageSet;
            }
            string s = JsonConvert.SerializeObject(new { status = 1, msg = "", Phone = this.GetPhone(), GuidePage = guidePageSet, ShopDefault = masterSettings.ShopDefault, MemberDefault = masterSettings.MemberDefault, GoodsType = masterSettings.GoodsType, GoodsCheck = masterSettings.GoodsCheck, ActivityMenu = masterSettings.ActivityMenu, DistributorsMenu = masterSettings.DistributorsMenu, GoodsListMenu = masterSettings.GoodsListMenu, BrandMenu = masterSettings.BrandMenu, ShopMenuStyle = masterSettings.ShopMenuStyle, menuList = allMenu });
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

