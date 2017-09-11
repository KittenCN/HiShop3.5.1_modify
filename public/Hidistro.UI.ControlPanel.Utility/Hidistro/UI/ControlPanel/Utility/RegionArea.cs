namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class RegionArea : TemplatedWebControl
    {
        private HtmlGenericControl contentRegion;
        private HtmlGenericControl contents;

        protected override void AttachChildControls()
        {
            this.contents = (HtmlGenericControl) this.FindControl("contents");
            this.contentRegion = (HtmlGenericControl) this.FindControl("contentRegion");
            if (!this.Page.IsPostBack)
            {
                this.BindAreasHtml();
                this.BindRegionHtml();
            }
        }

        private void BindAreasHtml()
        {
            Dictionary<int, string> regions = RegionHelper.GetRegions();
            StringBuilder builder = new StringBuilder();
            builder.Append("<ul>");
            foreach (int num in regions.Keys)
            {
                StringBuilder builder2 = new StringBuilder();
                string str = string.Empty;
                foreach (int num2 in RegionHelper.GetProvinces(num).Keys)
                {
                    builder2.Append(num2.ToString() + ",");
                }
                if (!string.IsNullOrEmpty(builder2.ToString()))
                {
                    str = builder2.ToString().Substring(0, builder2.ToString().Length - 1);
                }
                builder.AppendFormat("<li> <input id=\"areas_{0}\" onclick=\"checkRansack(this.value,this.checked)\" type=\"checkbox\" value=\"{1}\" /><label for=\"areas_{0}\">{2}</label> </li>", num, str, regions[num]);
            }
            builder.Append("</ul>");
            this.contents.InnerHtml = builder.ToString();
        }

        private void BindRegionHtml()
        {
            Dictionary<int, string> regions = RegionHelper.GetRegions();
            StringBuilder builder = new StringBuilder();
            builder.Append("<ul>");
            foreach (int num in regions.Keys)
            {
                Dictionary<int, string> provinces = RegionHelper.GetProvinces(num);
                foreach (int num2 in provinces.Keys)
                {
                    builder.AppendFormat("<li> <input id=\"{0}\" type=\"checkbox\"  value=\"{1}\" /><label for=\"{0}\">{1}</label></li> ", num2, provinces[num2]);
                }
            }
            builder.Append("</ul>");
            this.contentRegion.InnerHtml = builder.ToString();
        }
    }
}

