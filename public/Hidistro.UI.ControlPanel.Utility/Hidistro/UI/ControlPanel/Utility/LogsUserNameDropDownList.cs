namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class LogsUserNameDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            IList<string> operationUseNames = new List<string>();
            operationUseNames = EventLogs.GetOperationUseNames();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            foreach (string str in operationUseNames)
            {
                base.Items.Add(new ListItem(str, str));
            }
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }
    }
}

