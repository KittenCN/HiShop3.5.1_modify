namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFormatRadioButtonList : RadioButtonList
    {
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFormatRadioButtonList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("CSV格式", "csv"));
            this.Items.Add(new ListItem("TXT格式", "txt"));
            base.SelectedIndex = 0;
        }

        public override System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return this.repeatDirection;
            }
            set
            {
                this.repeatDirection = value;
            }
        }
    }
}

