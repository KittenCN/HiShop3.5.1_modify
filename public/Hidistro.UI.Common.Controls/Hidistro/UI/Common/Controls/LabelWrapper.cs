namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class LabelWrapper : IText
    {
        private Label _label;

        internal LabelWrapper(Label label)
        {
            this._label = label;
        }

        public System.Web.UI.Control Control
        {
            get
            {
                return this._label;
            }
        }

        public string Text
        {
            get
            {
                return this._label.Text;
            }
            set
            {
                this._label.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this._label.Visible;
            }
            set
            {
                this._label.Visible = value;
            }
        }
    }
}

