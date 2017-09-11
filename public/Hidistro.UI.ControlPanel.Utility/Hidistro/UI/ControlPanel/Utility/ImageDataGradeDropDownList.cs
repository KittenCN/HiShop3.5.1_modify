namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ImageDataGradeDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            base.Items.Add(new ListItem("默认分组", "0"));
            foreach (DataRow row in GalleryHelper.GetPhotoCategories(0).Rows)
            {
                base.Items.Add(new ListItem(row["CategoryName"].ToString(), row["CategoryId"].ToString()));
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

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }
    }
}

