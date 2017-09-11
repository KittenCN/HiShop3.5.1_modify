namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Web;

    public class Hi_Ajax_DelFolder : IHttpHandler
    {
        public string DelFolder(HttpContext context)
        {
            if (GalleryHelper.DeletePhotoCategory(Convert.ToInt32(context.Request["id"])))
            {
                return "{\"status\":1,\"msg\":\"\"}";
            }
            return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.DelFolder(context));
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

