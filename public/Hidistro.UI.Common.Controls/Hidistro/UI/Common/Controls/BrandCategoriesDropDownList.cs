namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Commodities;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class BrandCategoriesDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            DataTable table = new DataTable();
            foreach (DataRow row in CatalogHelper.GetBrandCategories().Rows)
            {
                int num = (int) row["BrandId"];
                this.Items.Add(new ListItem((string) row["BrandName"], num.ToString(CultureInfo.InvariantCulture)));
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
                if (!string.IsNullOrEmpty(base.SelectedValue))
                {
                    return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }
    }
}

