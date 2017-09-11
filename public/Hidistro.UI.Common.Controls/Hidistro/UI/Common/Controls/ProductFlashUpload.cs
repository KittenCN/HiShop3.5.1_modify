namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ProductFlashUpload : WebControl
    {
        private string _FlashUploadType = "product";
        private int _MaxNum = 5;
        private string _OldValue = string.Empty;
        private string _Value = string.Empty;

        private bool CheckFileExists(string imageUrl)
        {
            if (!this.CheckFileFormat(imageUrl))
            {
                return false;
            }
            if (imageUrl.ToLower().IndexOf("http://") < 0)
            {
                return File.Exists(this.Page.Request.MapPath(Globals.ApplicationPath + imageUrl));
            }
            return true;
        }

        private bool CheckFileFormat(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                string str = imageUrl.ToUpper();
                if ((str.Contains(".JPG") || str.Contains(".GIF")) || ((str.Contains(".PNG") || str.Contains(".BMP")) || str.Contains(".JPEG")))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckFileFormatOrPath(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                string str = imageUrl.ToUpper();
                if (((str.Contains(".JPG") || str.Contains(".GIF")) || ((str.Contains(".PNG") || str.Contains(".BMP")) || str.Contains(".JPEG"))) && (imageUrl.Contains(this.Context.Server.MapPath(Globals.GetStoragePath() + "/" + this._FlashUploadType)) || imageUrl.Contains(this.Context.Server.MapPath("/utility/pics/none.gif"))))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            WebControl child = new WebControl(HtmlTextWriterTag.Input);
            child.Attributes.Add("id", this.ID.ToString() + "_hdPhotoListOriginal");
            child.Attributes.Add("name", this.ID.ToString() + "_hdPhotoListOriginal");
            child.Attributes.Add("type", "hidden");
            child.Attributes.Add("value", this._Value);
            WebControl control2 = new WebControl(HtmlTextWriterTag.Input);
            control2.Attributes.Add("id", this.ID + "_hdPhotoList");
            control2.Attributes.Add("name", this.ID + "_hdPhotoList");
            control2.Attributes.Add("type", "hidden");
            control2.Attributes.Add("value", this._Value);
            WebControl control3 = new WebControl(HtmlTextWriterTag.Div);
            control3.Attributes.Add("id", this.ID + "_divFileProgressContainer");
            control3.Attributes.Add("style", "height: 75px;display:none;");
            Literal literal = new Literal();
            StringBuilder builder = new StringBuilder();
            string[] strArray = this._Value.Split(new char[] { ',' });
            foreach (string str in strArray)
            {
                builder.Append(this.GetOneImgHtml(str));
            }
            literal.Text = builder.ToString();
            Literal literal2 = new Literal {
                Text = "<div id=\"" + this.ID + "_divImgList\"><div class=\"picfirst\"></div>" + builder.ToString() + "</div>"
            };
            WebControl control4 = new WebControl(HtmlTextWriterTag.Div);
            control4.Attributes.Add("id", this.ID + "_divFlashUploadHolder");
            control4.Attributes.Add("style", "width: 91px; margin: 0px 10px;float: left;");
            this.Controls.Add(child);
            this.Controls.Add(control2);
            this.Controls.Add(control3);
            this.Controls.Add(literal2);
            this.Controls.Add(control4);
            int length = strArray.Length;
            if (string.IsNullOrEmpty(this._Value))
            {
                length = 0;
            }
            else if (this._MaxNum <= length)
            {
                control4.Style.Add("display", "none");
            }
            Literal literal3 = new Literal {
                Text = string.Concat(new object[] { 
                    "<script type=\"text/javascript\">var obj", this.ID, "_hdPhotoList = new FlashUploadObject(\"", this.ID, "_hdPhotoList\", \"", this.ID, "_divImgList\", \"", this.ID, "_divFlashUploadHolder\", \"picfirst\", \"", this.ID, "_divFileProgressContainer\", ", this.MaxNum, ",", length, ");obj", this.ID, 
                    "_hdPhotoList.upfilebuttonload();obj", this.ID, "_hdPhotoList.GetPhotoValue();</script>"
                 })
            };
            this.Controls.Add(literal3);
        }

        private void DeleteNoUsePhotos(string originalphotolist, string nowphotolist)
        {
            if (!string.IsNullOrEmpty(originalphotolist))
            {
                string[] strArray = originalphotolist.Split(new char[] { ',' });
                string str = "," + nowphotolist + ",";
                foreach (string str2 in strArray)
                {
                    if (!str.Contains("," + str2 + ",") && str2.StartsWith("/Storage/"))
                    {
                        this.DoDelete(this.Context.Server.MapPath(str2));
                    }
                }
            }
        }

        private void DoDelete(string originalPath)
        {
            if (((this._FlashUploadType == "product") || this._FlashUploadType.Equals("gift")) && this.CheckFileFormatOrPath(originalPath))
            {
                switch (Path.GetExtension(originalPath).ToLower())
                {
                    case ".gif":
                    case ".jpg":
                    case ".png":
                    case ".jpeg":
                    {
                        string path = originalPath.Replace(@"\images\", @"\thumbs40\40_");
                        originalPath.Replace(@"\images\", @"\thumbs60\60_");
                        string str3 = originalPath.Replace(@"\images\", @"\thumbs100\100_");
                        string str4 = originalPath.Replace(@"\images\", @"\thumbs160\160_");
                        string str5 = originalPath.Replace(@"\images\", @"\thumbs180\180_");
                        string str6 = originalPath.Replace(@"\images\", @"\thumbs220\220_");
                        string str7 = originalPath.Replace(@"\images\", @"\thumbs310\310_");
                        string str8 = originalPath.Replace(@"\images\", @"\thumbs410\410_");
                        if (File.Exists(originalPath))
                        {
                            File.Delete(originalPath);
                        }
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
                        break;
                    }
                }
            }
        }

        private string FilterUrlStart(string photolist)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(photolist))
            {
                return str;
            }
            string[] strArray = photolist.Trim().Trim(new char[] { ',' }).Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            foreach (string str2 in strArray)
            {
                if (str2.StartsWith("http://") && !str2.StartsWith("http://images.net.92hidc.com"))
                {
                    string[] strArray2 = str2.Split(new char[] { '/' });
                    builder.Append(str2.Replace("http://" + strArray2[2], "").Trim() + ",");
                }
                else
                {
                    builder.Append(str2 + ",");
                }
            }
            return builder.ToString().Trim(new char[] { ',' });
        }

        private string GetOneImgHtml(string img)
        {
            string str = string.Empty;
            if (img.Length > 10)
            {
                str = "<div class=\"uploadimages\"><div class=\"preview\"><div class=\"divoperator\"><a href=\"javascript:;\" class=\"leftmove\" title=\"左移\">&lt;</a><a href=\"javascript:;\" class=\"rightmove\" title=\"右移\">&gt;</a><a href=\"javascript:;\" class=\"photodel\" title=\"删除\">X</a></div><img style=\"width: 85px; height: 85px;\" src=\"" + img + "\"></div><div class=\"actionBox\"><a href=\"javascript:;\" class=\"actions\">设为默认</a></div></div>";
            }
            return str;
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            foreach (Control control in this.Controls)
            {
                control.RenderControl(writer);
                writer.WriteLine();
            }
            writer.WriteLine();
        }

        private string UpdateOnePhoto(string photo)
        {
            string str = photo;
            if (str.Contains("/temp/") && (str.Length > 10))
            {
                string[] strArray = photo.Split(new char[] { '.' });
                if (strArray.Length <= 1)
                {
                    return "";
                }
                string str2 = strArray[strArray.Length - 1].Trim().ToLower();
                string str3 = Globals.GetStoragePath() + "/" + this._FlashUploadType;
                string str4 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + "." + str2;
                string path = str;
                string[] strArray2 = photo.Split(new char[] { '/' });
                if (path.StartsWith("http://"))
                {
                    path = path.Replace("http://" + strArray2[2], "");
                }
                path = this.Context.Server.MapPath(path);
                string str6 = str3 + "/images/" + str4;
                File.Copy(path, this.Context.Server.MapPath(str6), true);
                string sourceFilename = this.Context.Request.MapPath(Globals.ApplicationPath + str6);
                string str8 = str3 + "/thumbs40/40_" + str4;
                string str9 = str3 + "/thumbs60/60_" + str4;
                string str10 = str3 + "/thumbs100/100_" + str4;
                string str11 = str3 + "/thumbs160/160_" + str4;
                string str12 = str3 + "/thumbs180/180_" + str4;
                string str13 = str3 + "/thumbs220/220_" + str4;
                string str14 = str3 + "/thumbs310/310_" + str4;
                string str15 = str3 + "/thumbs410/410_" + str4;
                str = str6;
                ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str8), 40, 40);
                ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str9), 60, 60);
                ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str10), 100, 100);
                ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str11), 160, 160);
                ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str12), 180, 180);
                ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str13), 220, 220);
                ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str14), 310, 310);
                ResourcesHelper.CreateThumbnail(sourceFilename, this.Context.Request.MapPath(Globals.ApplicationPath + str15), 410, 410);
            }
            if (str.Length > 10)
            {
                str = "," + str;
            }
            return str;
        }

        private string UpdatePhotoList(string photolist)
        {
            string[] strArray = this.FilterUrlStart(photolist).Trim().Trim(new char[] { ',' }).Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            foreach (string str in strArray)
            {
                builder.Append(this.UpdateOnePhoto(str.Trim()));
            }
            string nowphotolist = this.Context.Server.UrlDecode(builder.ToString()).Trim(new char[] { ',' });
            string oldValue = this.OldValue;
            if (!string.IsNullOrEmpty(oldValue))
            {
                oldValue = this.Context.Server.UrlDecode(oldValue);
            }
            this.DeleteNoUsePhotos(this.OldValue, nowphotolist);
            return nowphotolist;
        }

        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        public string FlashUploadType
        {
            get
            {
                return this._FlashUploadType;
            }
            set
            {
                this._FlashUploadType = value;
            }
        }

        public int MaxNum
        {
            get
            {
                return this._MaxNum;
            }
            set
            {
                this._MaxNum = value;
            }
        }

        [Browsable(false)]
        public string OldValue
        {
            get
            {
                return this.Context.Request.Form[this.ID + "_hdPhotoListOriginal"];
            }
            set
            {
                this._OldValue = value;
            }
        }

        public string Value
        {
            get
            {
                return this.UpdatePhotoList(this.Context.Request.Form[this.ID + "_hdPhotoList"]);
            }
            set
            {
                string str = this.FilterUrlStart(value);
                this._Value = str;
                this._OldValue = str;
            }
        }
    }
}

