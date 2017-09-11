namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class DecimalLengthDropDownList : DropDownList
    {
        public DecimalLengthDropDownList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("2位", "2"));
            this.Items.Add(new ListItem("1位", "1"));
            this.Items.Add(new ListItem("0位", "0"));
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
                return 2;
            }
            set
            {
                base.SelectedValue = value.ToString();
            }
        }
    }
}

