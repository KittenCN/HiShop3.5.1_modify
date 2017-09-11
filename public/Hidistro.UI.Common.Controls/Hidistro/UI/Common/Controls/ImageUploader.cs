namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ImageUploader : WebControl
    {
        private string thumbnailUrl100;
        private string thumbnailUrl160;
        private string thumbnailUrl180;
        private string thumbnailUrl220;
        private string thumbnailUrl310;
        private string thumbnailUrl40;
        private string thumbnailUrl410;
        private string thumbnailUrl60;
        private string uploadedImageUrl = string.Empty;

        public ImageUploader()
        {
            this.UploadType = Hidistro.UI.Common.Controls.UploadType.Product;
        }

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

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            string webResourceUrl = this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.images.upload.png");
            WebControl child = new WebControl(HtmlTextWriterTag.Div);
            string str2 = "background:url(" + webResourceUrl + ");background-repeat:no-repeat; background-position:20px -80px";
            child.Attributes.Add("id", this.ID + "_preview");
            child.Attributes.Add("style", str2);
            child.Attributes.Add("class", "preview");
            WebControl control2 = new WebControl(HtmlTextWriterTag.Div);
            control2.Attributes.Add("id", this.ID + "_upload");
            control2.Attributes.Add("class", "actionBox");
            if ((Globals.GetCurrentManagerUserId() == 0) || this.IsUploaded)
            {
                control2.Attributes.Add("style", "display:none;");
            }
            WebControl control3 = new WebControl(HtmlTextWriterTag.A);
            control3.Attributes.Add("href", "javascript:void(0);");
            control3.Attributes.Add("style", "background-image: url(" + webResourceUrl + ");");
            control3.Attributes.Add("class", "files");
            control3.Attributes.Add("id", this.ID + "_content");
            control2.Controls.Add(control3);
            WebControl control4 = new WebControl(HtmlTextWriterTag.Div);
            WebControl control5 = new WebControl(HtmlTextWriterTag.A);
            control4.Attributes.Add("id", this.ID + "_delete");
            control4.Attributes.Add("class", "actionBox");
            if ((Globals.GetCurrentManagerUserId() == 0) || !this.IsUploaded)
            {
                control4.Attributes.Add("style", "display:none;");
            }
            control5.Attributes.Add("href", "javascript:DeleteImage('" + this.ID + "','" + this.UploadType.ToString().ToLower() + "');");
            control5.Attributes.Add("style", "background-image: url(" + webResourceUrl + ");");
            control5.Attributes.Add("class", "actions");
            control4.Controls.Add(control5);
            this.Controls.Add(child);
            this.Controls.Add(control2);
            this.Controls.Add(control4);
            if (this.Page.Header.FindControl("uploaderStyle") == null)
            {
                WebControl control6 = new WebControl(HtmlTextWriterTag.Link);
                control6.Attributes.Add("rel", "stylesheet");
                control6.Attributes.Add("href", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.css.style.css"));
                control6.Attributes.Add("type", "text/css");
                control6.Attributes.Add("media", "screen");
                control6.ID = "uploaderStyle";
                this.Page.Header.Controls.Add(control6);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.UploadedImageUrl = this.Context.Request.Form[this.ID + "_uploadedImageUrl"];
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            foreach (Control control in this.Controls)
            {
                control.RenderControl(writer);
                writer.WriteLine();
            }
            if (!this.Page.ClientScript.IsStartupScriptRegistered(base.GetType(), "UploadScript"))
            {
                string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.ImageUploader.script.upload.js"));
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "UploadScript", script, false);
            }
            if (!this.Page.ClientScript.IsStartupScriptRegistered(base.GetType(), this.ID + "_InitScript"))
            {
                string str2 = "$(document).ready(function() { InitUploader(\"" + this.ID + "\", \"" + this.UploadType.ToString().ToLower() + "\");";
                if (this.IsUploaded)
                {
                    string str3 = str2;
                    str2 = str3 + "UpdatePreview('" + this.ID + "', '" + this.ThumbnailUrl100 + "');";
                }
                str2 = str2 + "});" + Environment.NewLine;
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), this.ID + "_InitScript", str2, true);
            }
            writer.WriteLine();
            writer.AddAttribute("id", this.ID + "_uploadedImageUrl");
            writer.AddAttribute("name", this.ID + "_uploadedImageUrl");
            writer.AddAttribute("value", this.UploadedImageUrl);
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        public bool IsUploaded
        {
            get
            {
                return !string.IsNullOrEmpty(this.UploadedImageUrl);
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl100
        {
            get
            {
                return this.thumbnailUrl100;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl160
        {
            get
            {
                return this.thumbnailUrl160;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl180
        {
            get
            {
                return this.thumbnailUrl180;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl220
        {
            get
            {
                return this.thumbnailUrl220;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl310
        {
            get
            {
                return this.thumbnailUrl310;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl40
        {
            get
            {
                return this.thumbnailUrl40;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl410
        {
            get
            {
                return this.thumbnailUrl410;
            }
        }

        [Browsable(false)]
        public string ThumbnailUrl60
        {
            get
            {
                return this.thumbnailUrl60;
            }
        }

        [Browsable(false)]
        public string UploadedImageUrl
        {
            get
            {
                return this.uploadedImageUrl;
            }
            set
            {
                if (this.CheckFileExists(value))
                {
                    this.uploadedImageUrl = value;
                    this.thumbnailUrl40 = value.Replace("/images/", "/thumbs40/40_");
                    this.thumbnailUrl60 = value.Replace("/images/", "/thumbs60/60_");
                    this.thumbnailUrl100 = value.Replace("/images/", "/thumbs100/100_");
                    this.thumbnailUrl160 = value.Replace("/images/", "/thumbs160/160_");
                    this.thumbnailUrl180 = value.Replace("/images/", "/thumbs180/180_");
                    this.thumbnailUrl220 = value.Replace("/images/", "/thumbs220/220_");
                    this.thumbnailUrl310 = value.Replace("/images/", "/thumbs310/310_");
                    this.thumbnailUrl410 = value.Replace("/images/", "/thumbs410/410_");
                }
            }
        }

        public Hidistro.UI.Common.Controls.UploadType UploadType { get; set; }
    }
}

