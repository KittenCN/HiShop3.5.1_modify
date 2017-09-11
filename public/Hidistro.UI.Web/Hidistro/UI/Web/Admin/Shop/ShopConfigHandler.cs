namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;

    public class ShopConfigHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (Globals.GetCurrentManagerUserId() <= 0)
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请先登录\"}");
                    context.Response.End();
                }
                string str = context.Request["type"].ToString();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (str == "0")
                {
                    bool flag = bool.Parse(context.Request["enable"].ToString());
                    masterSettings.EnableSaleService = flag;
                }
                SettingsManager.Save(masterSettings);
                context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"" + exception.Message + "\"}");
            }
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

