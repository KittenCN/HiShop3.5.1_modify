namespace Hidistro.UI.Web.Admin.Shop
{
    using System;
    using System.IO;
    using System.Web;

    public class Hi_Ajax_GoodsGourp : IHttpHandler
    {
        public string GetGoodsGroupJson(HttpContext context)
        {
            return File.ReadAllText(context.Server.MapPath("/Data/GoodsGroupJson.json"));
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.GetGoodsGroupJson(context));
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

