namespace Hidistro.UI.Common.Controls
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ListImage : Image
    {
        protected override void OnDataBinding(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.DataField))
            {
                object obj2 = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
                if (((obj2 != null) && (obj2 != DBNull.Value)) && !string.IsNullOrEmpty(obj2.ToString()))
                {
                    base.ImageUrl = (string) obj2;
                }
                else
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                    if (this.DataField.Equals("ThumbnailUrl40"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail1;
                    }
                    else if (this.DataField.Equals("ThumbnailUrl60"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail2;
                    }
                    else if (this.DataField.Equals("ThumbnailUrl100"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail3;
                    }
                    else if (this.DataField.Equals("ThumbnailUrl160"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail4;
                    }
                    else if (this.DataField.Equals("ThumbnailUrl180"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail5;
                    }
                    else if (this.DataField.Equals("ThumbnailUrl220"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail6;
                    }
                    else if (this.DataField.Equals("ThumbnailUrl310"))
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail7;
                    }
                    else
                    {
                        base.ImageUrl = masterSettings.DefaultProductThumbnail8;
                    }
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.ImageUrl))
            {
                if ((!string.IsNullOrEmpty(base.ImageUrl) && !Utils.IsUrlAbsolute(base.ImageUrl.ToLower())) && ((Utils.ApplicationPath.Length > 0) && !base.ImageUrl.StartsWith(Utils.ApplicationPath)))
                {
                    base.ImageUrl = Utils.ApplicationPath + base.ImageUrl;
                }
                base.Render(writer);
            }
        }

        public string DataField { get; set; }
    }
}

