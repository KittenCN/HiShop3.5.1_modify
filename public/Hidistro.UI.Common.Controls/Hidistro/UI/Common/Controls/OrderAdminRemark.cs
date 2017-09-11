namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderAdminRemark : Label
    {
        private string dataField = "adminremark";

        protected override void OnDataBinding(EventArgs e)
        {
            object obj2 = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
            if (((obj2 != null) && (obj2 != DBNull.Value)) && !string.IsNullOrEmpty(obj2.ToString()))
            {
                base.Text = "<img src=\"../images/xi.gif\" />";
                base.ToolTip = obj2.ToString();
            }
            else
            {
                base.Text = "-";
            }
            base.OnDataBinding(e);
        }

        public string DataField
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }
    }
}

