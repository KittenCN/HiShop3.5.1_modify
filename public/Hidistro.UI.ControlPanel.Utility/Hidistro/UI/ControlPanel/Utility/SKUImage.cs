namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SKUImage : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(this.ImageUrl))
            {
                writer.Write(string.Format("<a href=\"javascript:void(0)\">{0}</a>", this.ValueStr));
            }
            else
            {
                writer.Write(string.Format("<a  class=\"{0}\" href=\"javascript:void(0)\"><img src=\"{1}\" width=\"23\" height=\"20\" alt=\"{2}\" /></a>", this.CssClass, Globals.ApplicationPath + this.ImageUrl, this.ValueStr));
            }
        }

        public string ImageUrl { get; set; }

        public string ValueStr { get; set; }
    }
}

