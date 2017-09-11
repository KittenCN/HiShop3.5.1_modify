namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;

    public class UploadHandler : IHttpHandler
    {
        private string action;
        private string uploaderId;
        private string uploadType;

        private void DoDelete(HttpContext context)
        {
            string path = context.Request.MapPath(Globals.ApplicationPath + context.Request.Form[this.uploaderId + "_uploadedImageUrl"]);
            switch (Path.GetExtension(path).ToLower())
            {
                case ".gif":
                case ".jpg":
                case ".png":
                case ".jpeg":
                {
                    string str3 = path.Replace(@"\images\", @"\thumbs40\40_");
                    path.Replace(@"\images\", @"\thumbs60\60_");
                    string str4 = path.Replace(@"\images\", @"\thumbs100\100_");
                    string str5 = path.Replace(@"\images\", @"\thumbs160\160_");
                    string str6 = path.Replace(@"\images\", @"\thumbs180\180_");
                    string str7 = path.Replace(@"\images\", @"\thumbs220\220_");
                    string str8 = path.Replace(@"\images\", @"\thumbs310\310_");
                    string str9 = path.Replace(@"\images\", @"\thumbs410\410_");
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    if (File.Exists(str3))
                    {
                        File.Delete(str3);
                    }
                    if (File.Exists(str4))
                    {
                        File.Delete(str4);
                    }
                    if (File.Exists(str5))
                    {
                        File.Delete(str5);
                    }
                    if (File.Exists(str6))
                    {
                        File.Delete(str6);
                    }
                    if (File.Exists(str7))
                    {
                        File.Delete(str7);
                    }
                    if (File.Exists(str8))
                    {
                        File.Delete(str8);
                    }
                    if (File.Exists(str9))
                    {
                        File.Delete(str9);
                    }
                    break;
                }
            }
            context.Response.Write("<script type=\"text/javascript\">window.parent.DeleteCallback('" + this.uploaderId + "');</script>");
        }

        private void DoUpload(HttpContext context)
        {
            if (context.Request.Files.Count == 0)
            {
                this.WriteBackError(context, "没有检测到任何文件");
            }
            else
            {
                HttpPostedFile file = context.Request.Files[0];
                for (int i = 1; (file.ContentLength == 0) && (i < context.Request.Files.Count); i++)
                {
                    file = context.Request.Files[i];
                }
                if (file.ContentLength == 0)
                {
                    this.WriteBackError(context, "当前文件没有任何内容");
                }
                else if (!file.ContentType.ToLower().StartsWith("image/") || !Regex.IsMatch(Path.GetExtension(file.FileName.ToLower()), @"\.(jpg|gif|png|bmp|jpeg)$", RegexOptions.Compiled))
                {
                    this.WriteBackError(context, "文件类型错误，请选择有效的图片文件");
                }
                else
                {
                    this.UploadImage(context, file);
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            this.uploaderId = context.Request.QueryString["uploaderId"];
            this.uploadType = context.Request.QueryString["uploadType"];
            this.action = context.Request.QueryString["action"];
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Expires = -1;
            try
            {
                if (this.action.Equals("upload"))
                {
                    this.DoUpload(context);
                }
                else if (this.action.Equals("delete"))
                {
                    this.DoDelete(context);
                }
            }
            catch (Exception exception)
            {
                this.WriteBackError(context, exception.Message);
            }
        }

        private void UploadImage(HttpContext context, HttpPostedFile file)
        {
            string str = Globals.GetStoragePath() + "/" + this.uploadType;
            string str2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(file.FileName);
            string str3 = str + "/images/" + str2;
            string str4 = str + "/thumbs40/40_" + str2;
            string str5 = str + "/thumbs60/60_" + str2;
            string str6 = str + "/thumbs100/100_" + str2;
            string str7 = str + "/thumbs160/160_" + str2;
            string str8 = str + "/thumbs180/180_" + str2;
            string str9 = str + "/thumbs220/220_" + str2;
            string str10 = str + "/thumbs310/310_" + str2;
            string str11 = str + "/thumbs410/410_" + str2;
            if (ResourcesHelper.CheckPostedFile(file, "image"))
            {
                file.SaveAs(context.Request.MapPath(Globals.ApplicationPath + str3));
                string sourceFilename = context.Request.MapPath(Globals.ApplicationPath + str3);
                ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str4), 40, 40);
                ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str5), 60, 60);
                ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str6), 100, 100);
                ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str7), 160, 160);
                ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str8), 180, 180);
                ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str9), 220, 220);
                ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str10), 310, 310);
                ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str11), 410, 410);
                string[] strArray = new string[] { "'" + this.uploadType + "'", "'" + this.uploaderId + "'", "'" + str3 + "'", "'" + str4 + "'", "'" + str5 + "'", "'" + str6 + "'", "'" + str7 + "'", "'" + str8 + "'", "'" + str9 + "'", "'" + str10 + "'", "'" + str11 + "'" };
                context.Response.Write("<script type=\"text/javascript\">window.parent.UploadCallback(" + string.Join(",", strArray) + ");</script>");
            }
        }

        private void WriteBackError(HttpContext context, string error)
        {
            string[] strArray = new string[] { "'" + this.uploadType + "'", "'" + this.uploaderId + "'", "'" + error + "'" };
            context.Response.Write("<script type=\"text/javascript\">window.parent.ErrorCallback(" + string.Join(",", strArray) + ");</script>");
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

