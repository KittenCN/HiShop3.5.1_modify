namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.Core;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class ImgUpload : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.QueryString["delimg"] != null)
            {
                base.Response.Write("0");
                base.Response.End();
            }
            int num = int.Parse(base.Request.QueryString["imgurl"]);
            base.Request.QueryString["oldurl"].ToString();
            try
            {
                if (num < 1)
                {
                    HttpPostedFile file = base.Request.Files["Filedata"];
                    string str = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
                    string path = "/Storage/master/topic/";
                    if (!Directory.Exists(base.Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(base.Server.MapPath(path));
                    }
                    string str3 = Path.GetExtension(file.FileName).ToLower();
                    switch (str3)
                    {
                        case ".gif":
                        case ".jpg":
                        case ".png":
                        case ".jpeg":
                        {
                            string str4 = str + str3;
                            Globals.UploadFileAndCheck(file, Globals.MapPath(path + str4));
                            base.Response.StatusCode = 200;
                            base.Response.Write(str + "|/Storage/master/topic/" + str4);
                            return;
                        }
                    }
                    base.Response.StatusCode = 500;
                    base.Response.Write("服务器错误");
                    base.Response.End();
                }
                else
                {
                    base.Response.Write("0");
                }
            }
            catch (Exception)
            {
                base.Response.StatusCode = 500;
                base.Response.Write("服务器错误");
                base.Response.End();
            }
            finally
            {
                base.Response.End();
            }
        }
    }
}

