using System;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
    /// <summary>
    /// Hi_Ajax_GoodsGourp 的摘要说明
    /// </summary>
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