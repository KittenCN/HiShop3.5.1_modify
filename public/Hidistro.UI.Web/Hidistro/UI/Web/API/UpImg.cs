namespace Hidistro.UI.Web.API
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;

    public class UpImg : IHttpHandler
    {
        private string action;
        private string snailtype;
        private string uploaderId;
        private string uploadSize;
        private string uploadType;

        private void DoDelete(HttpContext context, string snailtype)
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
                    string str4 = path.Replace(@"\images\", @"\thumbs60\60_");
                    string str5 = path.Replace(@"\images\", @"\thumbs100\100_");
                    string str6 = path.Replace(@"\images\", @"\thumbs160\160_");
                    string str7 = path.Replace(@"\images\", @"\thumbs180\180_");
                    string str8 = path.Replace(@"\images\", @"\thumbs220\220_");
                    string str9 = path.Replace(@"\images\", @"\thumbs310\310_");
                    string str10 = path.Replace(@"\images\", @"\thumbs410\410_");
                    Globals.DelImgByFilePath(path);
                    if (snailtype == "1")
                    {
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
                        if (File.Exists(str10))
                        {
                            File.Delete(str10);
                        }
                    }
                    break;
                }
            }
            context.Response.Write("<script type=\"text/javascript\">window.parent.DeleteCallback('" + this.uploaderId + "');</script>");
        }

        private void DoUpload(HttpContext context, string snailtype)
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
                    this.UploadImage(context, file, snailtype);
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            this.uploaderId = context.Request.QueryString["uploaderId"];
            this.uploadType = context.Request.QueryString["uploadType"];
            this.action = context.Request.QueryString["action"];
            this.snailtype = context.Request.QueryString["snailtype"];
            this.uploadSize = context.Request.QueryString["uploadSize"];
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Expires = -1;
            try
            {
                if (this.action.Equals("upload"))
                {
                    this.DoUpload(context, this.snailtype);
                }
                else if (this.action.Equals("delete"))
                {
                    this.DoDelete(context, this.snailtype);
                }
            }
            catch (Exception exception)
            {
                this.WriteBackError(context, exception.Message);
            }
        }

        private void UploadImage(HttpContext context, HttpPostedFile file, string snailtype)
        {
            string str = "/storage/data/grade";
            string str2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(file.FileName);
            string str3 = str + "/images/" + str2;
            if (this.uploadType.ToLower() == UploadType.SharpPic.ToString().ToLower())
            {
                str = "/Storage/data/Sharp/";
                str2 = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                str3 = str + str2;
            }
            else if (this.uploadType.ToLower() == UploadType.Vote.ToString().ToLower())
            {
                str = "/Storage/master/vote/";
                str3 = str + str2;
            }
            else if (this.uploadType.ToLower() == UploadType.Topic.ToString().ToLower())
            {
                str = "/Storage/master/topic/";
                str3 = str + str2;
            }
            else if (this.uploadType.ToLower() == UploadType.Weibo.ToString().ToLower())
            {
                str = "/Storage/master/Weibo/";
                str3 = str + str2;
            }
            if (this.uploadType.ToLower() == UploadType.Brand.ToString().ToLower())
            {
                str = "/Storage/master/brand/";
                str3 = str + str2;
            }
            else if (this.uploadType.ToLower() == UploadType.ShopMenu.ToString().ToLower())
            {
                str = "/Storage/master/ShopMenu/";
                str3 = str + str2;
                int result = 0;
                if (!string.IsNullOrEmpty(this.uploadSize))
                {
                    if (!int.TryParse(this.uploadSize, out result))
                    {
                        this.WriteBackError(context, "UploadSize属性值只能是数字！");
                        return;
                    }
                    if (file.ContentLength > result)
                    {
                        this.WriteBackError(context, "文件大小不超过10KB!");
                        return;
                    }
                }
                string str4 = Path.GetExtension(file.FileName).ToLower();
                if (((!str4.Equals(".gif") && !str4.Equals(".jpg")) && (!str4.Equals(".jpeg") && !str4.Equals(".png"))) && !str4.Equals(".bmp"))
                {
                    this.WriteBackError(context, "请上传正确的图片文件。");
                    return;
                }
            }
            string str5 = str + "/thumbs40/40_" + str2;
            string str6 = str + "/thumbs60/60_" + str2;
            string str7 = str + "/thumbs100/100_" + str2;
            string str8 = str + "/thumbs160/160_" + str2;
            string str9 = str + "/thumbs180/180_" + str2;
            string str10 = str + "/thumbs220/220_" + str2;
            string str11 = str + "/thumbs310/310_" + str2;
            string str12 = str + "/thumbs410/410_" + str2;
            string imageFilePath = context.Request.MapPath(Globals.ApplicationPath + str3);
            if (Globals.UploadFileAndCheck(file, imageFilePath) == "1")
            {
                string sourceFilename = context.Request.MapPath(Globals.ApplicationPath + str3);
                if (snailtype == "1")
                {
                    ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str5), 40, 40);
                    ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str6), 60, 60);
                    ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str7), 100, 100);
                    ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str8), 160, 160);
                    ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str9), 180, 180);
                    ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str10), 220, 220);
                    ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str11), 310, 310);
                    ResourcesHelper.CreateThumbnail(sourceFilename, context.Request.MapPath(Globals.ApplicationPath + str12), 410, 410);
                }
            }
            string[] strArray = new string[] { "'" + this.uploadType + "'", "'" + this.uploaderId + "'", "'" + str3 + "'", "'" + snailtype + "','" + this.uploadSize + "'" };
            context.Response.Write("<script type=\"text/javascript\">window.parent.UploadCallback(" + string.Join(",", strArray) + ");</script>");
        }

        private void WriteBackError(HttpContext context, string error)
        {
            string[] strArray = new string[] { "'" + this.uploadType + "'", "'" + this.uploaderId + "'", "'" + error + "'", "'" + this.snailtype + "'", "'" + this.uploadSize + "'" };
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

