namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using System;
    using System.Web;

    public class Hi_Ajax_GetCustomPageByID : IHttpHandler
    {
        public string GetTemplateJson(HttpContext context, int id)
        {
            try
            {
                CustomPage customPageByID = CustomPageHelp.GetCustomPageByID(id);
                if (customPageByID == null)
                {
                    return "";
                }
                if (customPageByID.Status == 0)
                {
                    return customPageByID.FormalJson.Replace("\r\n", "").Replace("\n", "");
                }
                return customPageByID.DraftJson.Replace("\r\n", "").Replace("\n", "");
            }
            catch
            {
                return "";
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string str = context.Request.QueryString["id"];
            context.Response.Write(this.GetTemplateJson(context, Convert.ToInt32(str)));
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

