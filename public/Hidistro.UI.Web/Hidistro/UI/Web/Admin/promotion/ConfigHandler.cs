namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;

    public class ConfigHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (Globals.GetCurrentManagerUserId() <= 0)
                {
                    context.Response.Write("请先登录");
                    context.Response.End();
                }
                string str = context.Request["type"].ToString();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                switch (str)
                {
                    case "0":
                    {
                        bool flag = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.PonitToCash_Enable = flag;
                        break;
                    }
                    case "1":
                    {
                        bool flag2 = bool.Parse(context.Request["enable"].ToString());
                        masterSettings.ShareAct_Enable = flag2;
                        break;
                    }
                }
                SettingsManager.Save(masterSettings);
                context.Response.Write("保存成功");
            }
            catch (Exception exception)
            {
                context.Response.Write("保存失败！（" + exception.Message + ")");
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

