namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Web.UI.WebControls;

    public class AllowLoginRadioButtonList : RadioButtonList
    {
        public AllowLoginRadioButtonList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("开放", "True"));
            this.Items.Add(new ListItem("暂时关闭", "False"));
            this.RepeatDirection = RepeatDirection.Horizontal;
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

