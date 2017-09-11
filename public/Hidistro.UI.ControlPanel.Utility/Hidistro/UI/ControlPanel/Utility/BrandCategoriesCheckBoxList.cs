namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Commodities;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class BrandCategoriesCheckBoxList : CheckBoxList
    {
        private int repeatColumns = 7;
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public override void DataBind()
        {
            this.Items.Clear();
            foreach (DataRow row in CatalogHelper.GetBrandCategories().Rows)
            {
                int num = (int) row["BrandId"];
                this.Items.Add(new ListItem((string) row["BrandName"], num.ToString(CultureInfo.InvariantCulture)));
            }
        }

        public override int RepeatColumns
        {
            get
            {
                return this.repeatColumns;
            }
            set
            {
                this.repeatColumns = value;
            }
        }

        public override System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return this.repeatDirection;
            }
            set
            {
                this.repeatDirection = value;
            }
        }
    }
}

