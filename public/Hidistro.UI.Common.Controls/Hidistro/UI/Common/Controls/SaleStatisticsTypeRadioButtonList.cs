namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class SaleStatisticsTypeRadioButtonList : RadioButtonList
    {
        public SaleStatisticsTypeRadioButtonList()
        {
            int num = 1;
            this.Items.Add(new ListItem("交易量", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 2;
            this.Items.Add(new ListItem("交易额", num2.ToString(CultureInfo.InvariantCulture)));
            int num3 = 3;
            this.Items.Add(new ListItem("利润", num3.ToString(CultureInfo.InvariantCulture)));
            this.RepeatDirection = RepeatDirection.Horizontal;
            this.SelectedIndex = 0;
        }

        public SaleStatisticsType SelectedValue
        {
            get
            {
                return (SaleStatisticsType) Enum.Parse(typeof(SaleStatisticsType), base.SelectedValue);
            }
            set
            {
                int num2 = (int) value;
                int index = base.Items.IndexOf(base.Items.FindByValue(num2.ToString()));
                base.SelectedIndex = index;
            }
        }
    }
}

