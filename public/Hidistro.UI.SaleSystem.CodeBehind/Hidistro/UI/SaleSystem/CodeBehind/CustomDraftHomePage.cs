namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(true)]
    public class CustomDraftHomePage : CustomDraftTemplatedWebControl
    {
        protected override void OnInit(EventArgs e)
        {
            this.TempFilePath = "Skin-HomePage.html";
            if (this.SkinName == null)
            {
                this.SkinName = this.TempFilePath;
            }
            base.OnInit(e);
        }

        [Bindable(true)]
        public string CustomPagePath { get; set; }

        protected override string SkinPath
        {
            get
            {
                string path = "/Templates/vshop/custom/draft/" + this.CustomPagePath + "/" + this.SkinName;
                if (!File.Exists(HttpContext.Current.Server.MapPath(path)))
                {
                    HttpContext.Current.Response.Redirect("/Default.aspx");
                }
                return path;
            }
        }

        [Bindable(true)]
        public string TempFilePath { get; set; }
    }
}

