namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web.UI;

    public class HiPage : Page
    {
        private static readonly Regex viewStateRegex = new Regex("<div>(\\s+)<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<data>.*?)\" />(\\s+)</div>(\\r\\n)+", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Multiline);

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.EnableViewState)
            {
                base.Render(writer);
            }
            else
            {
                using (StringWriter writer2 = new StringWriter())
                {
                    using (HtmlTextWriter writer3 = new HtmlTextWriter(writer2))
                    {
                        base.Render(writer3);
                        string input = writer2.ToString();
                        Match match = viewStateRegex.Match(input);
                        if (match.Success)
                        {
                            input = input.Remove(match.Index, match.Length);
                        }
                        writer.Write(input);
                    }
                }
            }
        }
    }
}

