namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Style : Literal
    {
        private string href;
        private const string linkFormat = "<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />";

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.Href))
            {
                writer.Write("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />", this.Href, this.Media);
            }
        }

        public virtual string Href
        {
            get
            {
                if (string.IsNullOrEmpty(this.href))
                {
                    return null;
                }
                if (this.href.StartsWith("/"))
                {
                    return (Globals.ApplicationPath + this.href);
                }
                return (Globals.ApplicationPath + "/" + this.href);
            }
            set
            {
                this.href = value;
            }
        }

        [DefaultValue("screen")]
        public string Media { get; set; }
    }
}

