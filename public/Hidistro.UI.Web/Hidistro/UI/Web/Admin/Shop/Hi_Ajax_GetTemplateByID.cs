namespace Hidistro.UI.Web.Admin.Shop
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;

    public class Hi_Ajax_GetTemplateByID : IHttpHandler
    {
        public string GetTemplateJson(HttpContext context, string dataName)
        {
            StreamReader reader = new StreamReader(context.Server.MapPath("/Templates/vshop/" + dataName + "/data/default.json"), Encoding.UTF8);
            try
            {
                string str = reader.ReadToEnd();
                reader.Close();
                return str.Replace("\r\n", "").Replace("\n", "");
            }
            catch
            {
                return "";
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string dataName = context.Request.QueryString["id"];
            context.Response.Write(this.GetTemplateJson(context, dataName));
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

