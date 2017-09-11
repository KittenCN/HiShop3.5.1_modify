namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MonthDropDownList : DropDownList
    {
        public MonthDropDownList()
        {
            for (int i = 1; i <= 12; i++)
            {
                string text = i.ToString();
                this.Items.Add(new ListItem(text, i.ToString()));
            }
            this.SelectedValue = DateTime.Now.Month;
        }

        public int SelectedValue
        {
            get
            {
                int num;
                if (int.TryParse(base.SelectedValue, out num))
                {
                    return num;
                }
                return DateTime.Now.Month;
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

