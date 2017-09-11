namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SubStringLabel : Literal
    {
        private int _strLength;
        private string _strReplace = "...";
        private string field;

        protected override void OnDataBinding(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Field))
            {
                object obj2 = DataBinder.Eval(this.Page.GetDataItem(), this.Field);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    base.Text = (string) obj2;
                }
            }
            base.OnDataBinding(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((this.StrLength > 0) && (this.StrLength < base.Text.Length))
            {
                base.Text = base.Text.Substring(0, this.StrLength) + this.StrReplace;
            }
            base.Render(writer);
        }

        public string Field
        {
            get
            {
                return this.field;
            }
            set
            {
                this.field = value;
            }
        }

        public int StrLength
        {
            get
            {
                return this._strLength;
            }
            set
            {
                this._strLength = value;
            }
        }

        public string StrReplace
        {
            get
            {
                return this._strReplace;
            }
            set
            {
                this._strReplace = value;
            }
        }
    }
}

