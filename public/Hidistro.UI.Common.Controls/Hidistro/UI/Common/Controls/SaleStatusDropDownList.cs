namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Commodities;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class SaleStatusDropDownList : DropDownList
    {
        private bool allowNull = true;

        public SaleStatusDropDownList()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(string.Empty, string.Empty));
            }
            this.Items.Add(new ListItem("出售中", "1"));
            this.Items.Add(new ListItem("仓库中", "3"));
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

        public ProductSaleStatus SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return ProductSaleStatus.All;
                }
                return (ProductSaleStatus) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

