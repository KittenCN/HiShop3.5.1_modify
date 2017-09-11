﻿namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class PageSizeDropDownList : DropDownList
    {
        public PageSizeDropDownList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("10", "10"));
            this.Items.Add(new ListItem("20", "20"));
            this.Items.Add(new ListItem("30", "30"));
            this.Items.Add(new ListItem("40", "40"));
            this.Items.Add(new ListItem("50", "50"));
        }

        public int SelectedValue
        {
            get
            {
                return int.Parse(base.SelectedValue, CultureInfo.CurrentCulture);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

