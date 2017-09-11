namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;

    [ParseChildren(false), PersistChildren(true)]
    public class HeadContainer : Control
    {
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<script language=\"javascript\" type=\"text/javascript\"> \r\n            var applicationPath = \"{0}\";\r\n        </script>", Globals.ApplicationPath);
            writer.WriteLine();
            this.RenderMetaLanguage(writer);
            this.RenderFavicon(writer);
            this.RenderMetaAuthor(writer);
            this.RenderMetaGenerator(writer);
        }

        private void RenderFavicon(HtmlTextWriter writer)
        {
            string str = Globals.FullPath(Globals.GetSiteUrls().Favicon);
            writer.WriteLine("<link rel=\"icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", str);
            writer.WriteLine("<link rel=\"shortcut icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", str);
        }

        private void RenderMetaAuthor(HtmlTextWriter writer)
        {
            writer.WriteLine("<meta name=\"author\" content=\"Hishop development team\" />");
        }

        private void RenderMetaGenerator(HtmlTextWriter writer)
        {
            writer.WriteLine("<meta name=\"GENERATOR\" content=\"销客多 3.5\" />");
        }

        private void RenderMetaLanguage(HtmlTextWriter writer)
        {
            writer.WriteLine("<meta http-equiv=\"content-language\" content=\"zh-CN\" />");
        }
    }
}

