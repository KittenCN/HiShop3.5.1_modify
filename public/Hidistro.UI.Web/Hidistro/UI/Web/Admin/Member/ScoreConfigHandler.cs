namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Web;

    public class ScoreConfigHandler : IHttpHandler
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
                bool flag = bool.Parse(context.Request["enable"].ToString());
                switch (str)
                {
                    case "0":
                        masterSettings.sign_score_Enable = flag;
                        break;

                    case "1":
                        masterSettings.shopping_score_Enable = flag;
                        break;

                    case "2":
                        masterSettings.share_score_Enable = flag;
                        break;
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

