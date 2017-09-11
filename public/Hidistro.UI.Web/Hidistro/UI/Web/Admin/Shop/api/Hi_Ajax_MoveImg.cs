namespace Hidistro.UI.Web.Admin.Shop.api
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Linq;
    using System.Web;

    public class Hi_Ajax_MoveImg : IHttpHandler
    {
        public string ModelFolder(HttpContext context)
        {
            string str = context.Request.Form["file_id[]"];
            if (GalleryHelper.MovePhotoType((from x in str.Split(new char[] { ',' }).ToList<string>() select int.Parse(x)).ToList<int>(), Convert.ToInt32(context.Request.Form["cate_id"])) > 0)
            {
                return "{\"status\":1,\"msg\":\"\"}";
            }
            return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.ModelFolder(context));
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

