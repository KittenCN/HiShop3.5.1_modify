namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Web;

    public class Hi_Ajax_DelImg : IHttpHandler
    {
        public string DelImg(HttpContext context)
        {
            string str = context.Request.Form["file_id[]"];
            if (string.IsNullOrEmpty(str))
            {
                return "{\"status\": 0,\"msg\":\"请勾选图片\"}";
            }
            if (ManagerHelper.GetCurrentManager() == null)
            {
                return "{\"status\": 0,\"msg\":\"请先登录\"}";
            }
            foreach (string str2 in str.Split(new char[] { ',' }))
            {
                GalleryHelper.DeletePhoto(Convert.ToInt32(str2));
            }
            return "{\"status\": 1,\"msg\":\"\"}";
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.DelImg(context));
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

