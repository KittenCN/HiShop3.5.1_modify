namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web;

    public class HtmlDecodeTextBox : TrimTextBox
    {
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = !string.IsNullOrEmpty(value) ? HttpUtility.HtmlDecode(value) : value;
            }
        }
    }
}

