namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Web.UI.WebControls;

    public class UseAttributeImageRadioButtonList : RadioButtonList
    {
        public UseAttributeImageRadioButtonList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("文字", "False"));
            this.RepeatDirection = RepeatDirection.Horizontal;
            this.SelectedValue = false;
        }

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
    }
}

