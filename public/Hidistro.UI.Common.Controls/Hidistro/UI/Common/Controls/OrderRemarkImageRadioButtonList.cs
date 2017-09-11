namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Orders;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class OrderRemarkImageRadioButtonList : RadioButtonList
    {
        public OrderRemarkImageRadioButtonList()
        {
            this.Items.Clear();
            int num = 1;
            this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-ok help\" style=\"color:#309930\"></span>", num.ToString()));
            int num2 = 2;
            this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-exclamation-sign help\" style=\"color:#CB1E02\"></span>", num2.ToString()));
            int num3 = 3;
            this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-flag help\" style=\"color:#CB1E02\"></span>", num3.ToString()));
            int num4 = 4;
            this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-flag help\" style=\"color:#4E994E\"></span>", num4.ToString()));
            int num5 = 5;
            this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-flag help\" style=\"color:#FFC500\"></span>", num5.ToString()));
            int num6 = 6;
            this.Items.Add(new ListItem("<span class=\"glyphicon glyphicon-flag help\" style=\"color:#ABABAB\"></span>", num6.ToString()));
            this.RepeatDirection = RepeatDirection.Horizontal;
        }

        public OrderMark? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new OrderMark?((OrderMark) Enum.Parse(typeof(OrderMark), base.SelectedValue));
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(((int) value.Value).ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }
    }
}

