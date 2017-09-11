namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class HourDropDownList : DropDownList
    {
        public override void DataBind()
        {
            this.Items.Clear();
            for (int i = 0; i <= 0x17; i++)
            {
                string text = i + "时";
                this.Items.Add(new ListItem(text, i.ToString()));
            }
        }

        public int? SelectedValue
        {
            get
            {
                int result = 0;
                int.TryParse(base.SelectedValue, out result);
                return new int?(result);
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedValue = value.Value.ToString();
                }
            }
        }
    }
}

