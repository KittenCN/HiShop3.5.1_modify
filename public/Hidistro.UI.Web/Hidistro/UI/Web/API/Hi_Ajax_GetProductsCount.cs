namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Web;

    public class Hi_Ajax_GetProductsCount : IHttpHandler
    {
        public string GetCountJson()
        {
            string siteName = "";
            string distributorLogoPic = "";
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
            if ((cookie == null) || (cookie.Value == "0"))
            {
                siteName = SettingsManager.GetMasterSettings(true).SiteName;
                distributorLogoPic = SettingsManager.GetMasterSettings(true).DistributorLogoPic;
                return string.Concat(new object[] { "{\"count\":", ProductHelper.GetProductsCount(), ",\"storeName\":\"", siteName, "\",\"logoUrl\":\"\"}" });
            }
            DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(Convert.ToInt32(cookie.Value), true);
            if (currentDistributors != null)
            {
                siteName = currentDistributors.StoreName;
                distributorLogoPic = currentDistributors.Logo;
            }
            return string.Concat(new object[] { "{\"count\":", ProductHelper.GetProductsCount(), ",\"storeName\":\"", siteName, "\",\"logoUrl\":\"", distributorLogoPic, "\"}" });
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GetCountJson());
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

