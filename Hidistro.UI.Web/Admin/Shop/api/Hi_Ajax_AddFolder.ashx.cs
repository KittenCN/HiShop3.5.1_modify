using Hidistro.ControlPanel.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
    /// <summary>
    /// Hi_Ajax_AddFolder 的摘要说明
    /// </summary>
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