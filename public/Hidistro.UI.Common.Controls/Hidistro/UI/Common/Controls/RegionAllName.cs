namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class RegionAllName : Literal
    {
        protected override void Render(HtmlTextWriter writer)
        {
            int currentRegionId = int.Parse(this.RegionId);
            string fullRegion = string.Empty;
            if (currentRegionId > 0)
            {
                fullRegion = RegionHelper.GetFullRegion(currentRegionId, "  ");
            }
            base.Text = fullRegion;
            base.Render(writer);
        }

        public string RegionId
        {
            get
            {
                if (this.ViewState["RegionId"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["RegionId"];
            }
            set
            {
                this.ViewState["RegionId"] = value;
            }
        }
    }
}

