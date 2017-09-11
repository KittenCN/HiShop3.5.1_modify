namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class Hi_Ajax_RenameFolder : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.RenameFolder(context));
        }

        public string RenameFolder(HttpContext context)
        {
            Dictionary<int, string> photoCategorys = new Dictionary<int, string>();
            photoCategorys.Add(Convert.ToInt32(context.Request.Form["category_img_id"]), context.Request.Form["name"]);
            if (GalleryHelper.UpdatePhotoCategories(photoCategorys) > 0)
            {
                return "{\"status\":1,\"msg\":\"\"}";
            }
            return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
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

