namespace Hidistro.UI.Web.Handler
{
    using Hidistro.Core;
    using System;
    using System.IO;
    using System.Web;

    public class flashupload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Expires = -1;
            try
            {
                HttpPostedFile file = context.Request.Files["Filedata"];
                string path = "/Storage/temp/";
                string str2 = "/Storage/temp/";
                FileInfo info = new FileInfo(file.FileName);
                string str3 = DateTime.Now.ToString("yyyyMM");
                string str4 = context.Server.MapPath(path) + str3 + "/";
                if (!Directory.Exists(str4))
                {
                    Directory.CreateDirectory(str4);
                }
                switch (info.Extension.ToLower())
                {
                    case ".gif":
                    case ".jpg":
                    case ".png":
                    {
                        string str6 = string.Concat(new object[] { "product_", DateTime.Now.ToString("HHmmss"), DateTime.Now.Millisecond, info.Extension });
                        if (File.Exists(str4 + str6))
                        {
                            File.Delete(str4 + str6);
                        }
                        Globals.UploadFileAndCheck(file, str4 + str6);
                        context.Response.StatusCode = 200;
                        context.Response.Write(str2 + str3 + "/" + str6);
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = 200;
                context.Response.Write(exception.ToString());
            }
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

