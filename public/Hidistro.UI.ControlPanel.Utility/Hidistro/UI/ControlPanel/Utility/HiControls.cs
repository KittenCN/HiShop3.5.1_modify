namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class HiControls : WebControl
    {
        private int height;
        private string linkURL = "";

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.LinkURL))
            {
                try
                {
                    WebRequest request = WebRequest.Create(this.LinkURL);
                    request.Timeout = 0x186a0;
                    using (Stream stream = request.GetResponse().GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("GB2312")))
                        {
                            writer.Write(reader.ReadToEnd());
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public string LinkURL
        {
            get
            {
                return this.linkURL;
            }
            set
            {
                this.linkURL = value;
            }
        }
    }
}

