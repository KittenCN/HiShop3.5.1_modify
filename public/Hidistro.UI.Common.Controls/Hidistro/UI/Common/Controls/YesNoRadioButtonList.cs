namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class YesNoRadioButtonList : RadioButtonList
    {
        public YesNoRadioButtonList()
        {
            this.NoText = "否";
            this.YesText = "是";
            this.Items.Clear();
            this.Items.Add(new ListItem(this.YesText, "True"));
            this.Items.Add(new ListItem(this.NoText, "False"));
            this.RepeatDirection = RepeatDirection.Horizontal;
            this.SelectedValue = true;
        }

        public string NoText { get; set; }

        public bool SelectedValue
        {
            get
            {
                return bool.Parse(base.SelectedValue);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value ? "True" : "False"));
            }
        }

        public string YesText { get; set; }
    }
}

