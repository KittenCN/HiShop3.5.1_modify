namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class FormatedSiteUrl : HyperLink
    {
        private object data;
        private string dataField;
        private string urlName;

        protected override void OnDataBinding(EventArgs e)
        {
            this.data = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((string.IsNullOrEmpty(base.NavigateUrl) && !string.IsNullOrEmpty(this.UrlName)) && (this.data != null))
            {
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl(this.UrlName, new object[] { this.data });
            }
            base.Render(writer);
        }

        public string DataField
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
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

