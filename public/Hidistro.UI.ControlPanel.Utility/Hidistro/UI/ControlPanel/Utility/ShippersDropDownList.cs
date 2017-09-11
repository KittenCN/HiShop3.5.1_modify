namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities.Sales;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ShippersDropDownList : DropDownList
    {
        private bool includeDistributor;

        public override void DataBind()
        {
            this.Items.Clear();
            foreach (ShippersInfo info in SalesHelper.GetShippers(this.IncludeDistributor))
            {
                this.Items.Add(new ListItem(info.ShipperName, info.ShipperId.ToString()));
            }
        }

        public bool IncludeDistributor
        {
            get
            {
                return this.includeDistributor;
            }
            set
            {
                this.includeDistributor = value;
            }
        }

        public int SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return 0;
                }
                return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

