namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Net.Mail;
    using System.Web.UI.WebControls;

    public class MailPriorityRadioButtonList : RadioButtonList
    {
        public MailPriorityRadioButtonList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("中", MailPriority.Normal.ToString()));
            this.Items.Add(new ListItem("低", MailPriority.Low.ToString()));
            this.Items.Add(new ListItem("高", MailPriority.High.ToString()));
            this.RepeatDirection = RepeatDirection.Horizontal;
            this.SelectedIndex = 0;
        }

        public MailPriority SelectedValue
        {
            get
            {
                return (MailPriority) Enum.Parse(typeof(MailPriority), base.SelectedValue);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString()));
            }
        }
    }
}

