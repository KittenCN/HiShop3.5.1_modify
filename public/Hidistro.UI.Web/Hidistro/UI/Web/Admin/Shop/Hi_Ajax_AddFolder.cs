namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Web;

    public class Hi_Ajax_AddFolder : IHttpHandler
    {
        public string InsertFolder()
        {
            int num = GalleryHelper.AddPhotoCategory2("新建文件夹");
            return ("{\"status\":1,\"data\":" + num + ",\"msg\":\"\"}");
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.InsertFolder());
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

