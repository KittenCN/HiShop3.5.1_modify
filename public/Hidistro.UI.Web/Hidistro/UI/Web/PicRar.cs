namespace Hidistro.UI.Web
{
    using Hidistro.Core;
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class PicRar : Page
    {
        protected HtmlForm form1;
        public string label_html = string.Empty;

        public static bool IsNumeric(string strNumeric)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.Match(strNumeric).Success;
        }

        public static bool IsQueryString(string strQuery)
        {
            return IsQueryString(strQuery, "N");
        }

        public static bool IsQueryString(string strQuery, string Q)
        {
            bool flag = false;
            string str = Q;
            if (str != null)
            {
                if (!(str == "N"))
                {
                    if ((str == "S") && ((HttpContext.Current.Request.QueryString[strQuery] != null) && (HttpContext.Current.Request.QueryString[strQuery].ToString() != string.Empty)))
                    {
                        flag = true;
                    }
                    return flag;
                }
                if ((HttpContext.Current.Request.QueryString[strQuery] != null) && IsNumeric(HttpContext.Current.Request.QueryString[strQuery].ToString()))
                {
                    flag = true;
                }
            }
            return flag;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string virtualPath = string.Empty;
                int maxWidth = 0;
                int maxHeight = 0;
                if (IsQueryString("P", "S"))
                {
                    virtualPath = base.Request.QueryString["P"];
                }
                if (IsQueryString("W"))
                {
                    maxWidth = int.Parse(base.Request.QueryString["W"]);
                }
                if (IsQueryString("H"))
                {
                    maxHeight = int.Parse(base.Request.QueryString["H"]);
                }
                if (virtualPath != string.Empty)
                {
                    PIC pic = new PIC();
                    if (!virtualPath.StartsWith("/"))
                    {
                        virtualPath = "/" + virtualPath;
                    }
                    virtualPath = Globals.ApplicationPath + virtualPath;
                    pic.SendSmallImage(base.Request.MapPath(virtualPath), maxHeight, maxWidth);
                    string watermarkFilename = base.Request.MapPath(Globals.ApplicationPath + "/Admin/images/watermark.gif");
                    MemoryStream stream = pic.AddImageSignPic(pic.OutBMP, watermarkFilename, 9, 60, 4);
                    pic.Dispose();
                    base.Response.ClearContent();
                    base.Response.ContentType = "image/gif";
                    base.Response.BinaryWrite(stream.ToArray());
                    base.Response.End();
                    stream.Dispose();
                }
            }
            catch (Exception exception)
            {
                this.label_html = exception.Message;
            }
        }
    }
}

