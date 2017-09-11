namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class RoleDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            IList<RoleInfo> roles = ManagerHelper.GetRoles();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            if ((roles != null) && (roles.Count > 0))
            {
                foreach (RoleInfo info in roles)
                {
                    base.Items.Add(new ListItem(info.RoleName, info.RoleId.ToString()));
                }
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

        public int SelectedValue
        {
            get
            {
                int result = 0;
                int.TryParse(base.SelectedValue, out result);
                return result;
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString()));
            }
        }
    }
}

