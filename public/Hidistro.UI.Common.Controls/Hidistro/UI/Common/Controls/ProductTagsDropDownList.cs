namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class ProductTagsDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "全部";

        public override void DataBind()
        {
            base.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            foreach (DataRow row in CatalogHelper.GetTags().Rows)
            {
                ListItem item = new ListItem(Globals.HtmlDecode(row["TagName"].ToString()), row["TagID"].ToString());
                base.Items.Add(item);
            }
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue));
            }
            set
            {
                if (!value.HasValue)
                {
                    base.SelectedValue = string.Empty;
                }
                else
                {
                    base.SelectedValue = value.ToString();
                }
            }
        }
    }
}

