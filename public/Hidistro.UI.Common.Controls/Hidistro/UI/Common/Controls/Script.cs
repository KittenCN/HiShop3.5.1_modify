namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Script : Literal
    {
        private string src;
        private const string srcFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.Src))
            {
                writer.Write("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Src);
            }
        }

        public virtual string Src
        {
            get
            {
                if (string.IsNullOrEmpty(this.src))
                {
                    return null;
                }
                if (this.src.StartsWith("/"))
                {
                    return (Globals.ApplicationPath + this.src);
                }
                return (Globals.ApplicationPath + "/" + this.src);
            }
            set
            {
                this.src = value;
            }
        }
    }
}

