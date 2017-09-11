namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using System;
    using System.Web;

    public class Hi_Ajax_RenameImg : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (Globals.RequestFormNum("type") != 3)
            {
                context.Response.Write(this.ReName(context));
            }
            else
            {
                context.Response.Write("{\"status\": 0,\"msg\":\"商品图片名称请到商品管理页面修改！\"}");
            }
        }

        public string ReName(HttpContext context)
        {
            GalleryHelper.RenamePhoto(Convert.ToInt32(context.Request.Form["file_id"]), context.Request.Form["file_name"]);
            return "{\"status\": 1,\"msg\":\"\"}";
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

