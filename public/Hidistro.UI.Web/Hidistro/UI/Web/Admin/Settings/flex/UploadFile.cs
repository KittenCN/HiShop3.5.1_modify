namespace Hidistro.UI.Web.Admin.Settings.flex
{
    using Hidistro.Core;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;

    public class UploadFile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpFileCollection files = base.Request.Files;
            if (files.Count > 0)
            {
                string str = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/master/flex");
                HttpPostedFile postedFile = files[0];
                string str2 = Path.GetExtension(postedFile.FileName).ToLower();
                if ((((str2 != ".jpg") && (str2 != ".gif")) && ((str2 != ".jpeg") && (str2 != ".png"))) && (str2 != ".bmp"))
                {
                    base.Response.Write("1");
                }
                else
                {
                    string s = DateTime.Now.ToString("yyyyMMdd") + new Random().Next(0x2710, 0x1869f).ToString(CultureInfo.InvariantCulture) + str2;
                    string filename = str + "/" + s;
                    try
                    {
                        if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
                        {
                            base.Response.Write("0");
                        }
                        else
                        {
                            postedFile.SaveAs(filename);
                            base.Response.Write(s);
                        }
                    }
                    catch
                    {
                        base.Response.Write("0");
                    }
                }
            }
            else
            {
                base.Response.Write("2");
            }
        }
    }
}

