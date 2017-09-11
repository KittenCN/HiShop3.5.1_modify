namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SiteUrl : HyperLink
    {
        private string requstName;
        private string urlName;

        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(base.NavigateUrl) && !string.IsNullOrEmpty(this.UrlName))
            {
                if (!string.IsNullOrEmpty(this.RequstName))
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl(this.UrlName, new object[] { this.Page.Request.QueryString[this.RequstName] });
                }
                else
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl(this.UrlName);
                }
            }
            base.Render(writer);
        }

        public string RequstName
        {
            get
            {
                return this.requstName;
            }
            set
            {
                this.requstName = value;
            }
        }

        public string UrlName
        {
            get
            {
                return this.urlName;
            }
            set
            {
                this.urlName = value;
            }
        }
    }
}

