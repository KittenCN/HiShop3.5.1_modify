namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ImageLinkButton : LinkButton
    {
        private string alt;
        private string deleteMsg = "确定要执行该删除操作吗？删除后将不可以恢复！";
        private string imageFormat = "<img border=\"0\" src=\"{0}\" alt=\"{1}\" />";
        private bool isShow;
        private Hidistro.UI.Common.Controls.ImagePosition position;
        private bool showText = true;

        private string GetImageTag()
        {
            if (string.IsNullOrEmpty(this.ImageUrl))
            {
                return string.Empty;
            }
            return string.Format(CultureInfo.InvariantCulture, this.imageFormat, new object[] { this.ImageUrl, this.Alt });
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.IsShow)
            {
                string str = string.Format("return   confirm('{0}');", this.DeleteMsg);
                base.Attributes.Add("OnClick", str);
            }
            base.Attributes.Add("name", this.NamingContainer.UniqueID + "$" + this.ID);
            string imageTag = this.GetImageTag();
            if (!this.ShowText)
            {
                base.Text = "";
            }
            if (this.ImagePosition == Hidistro.UI.Common.Controls.ImagePosition.Right)
            {
                base.Text = base.Text + imageTag;
            }
            else
            {
                base.Text = imageTag + base.Text;
            }
            base.Render(writer);
        }

        public string Alt
        {
            get
            {
                return this.alt;
            }
            set
            {
                this.alt = value;
            }
        }

        public string DeleteMsg
        {
            get
            {
                return this.deleteMsg;
            }
            set
            {
                this.deleteMsg = value;
            }
        }

        public Hidistro.UI.Common.Controls.ImagePosition ImagePosition
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                if (this.ViewState["Src"] != null)
                {
                    return (string) this.ViewState["Src"];
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string relativeUrl = value;
                    if (relativeUrl.StartsWith("~"))
                    {
                        relativeUrl = base.ResolveUrl(relativeUrl);
                    }
                    this.ViewState["Src"] = relativeUrl;
                }
                else
                {
                    this.ViewState["Src"] = null;
                }
            }
        }

        public bool IsShow
        {
            get
            {
                return this.isShow;
            }
            set
            {
                this.isShow = value;
            }
        }

        public bool ShowText
        {
            get
            {
                return this.showText;
            }
            set
            {
                this.showText = value;
            }
        }
    }
}

