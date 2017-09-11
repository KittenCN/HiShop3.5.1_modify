namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ButtonWrapper : IButton, IText
    {
        private Button _button;

        public event EventHandler Click
        {
            add
            {
                this._button.Click += value;
            }
            remove
            {
                this._button.Click -= value;
            }
        }

        public event CommandEventHandler Command
        {
            add
            {
                this._button.Command += value;
            }
            remove
            {
                this._button.Command -= value;
            }
        }

        internal ButtonWrapper(Button button)
        {
            this._button = button;
        }

        public AttributeCollection Attributes
        {
            get
            {
                return this._button.Attributes;
            }
        }

        public bool CausesValidation
        {
            get
            {
                return this._button.CausesValidation;
            }
            set
            {
                this._button.CausesValidation = value;
            }
        }

        public string CommandArgument
        {
            get
            {
                return this._button.CommandArgument;
            }
            set
            {
                this._button.CommandArgument = value;
            }
        }

        public string CommandName
        {
            get
            {
                return this._button.CommandName;
            }
            set
            {
                this._button.CommandName = value;
            }
        }

        public System.Web.UI.Control Control
        {
            get
            {
                return this._button;
            }
        }

        public string Text
        {
            get
            {
                return this._button.Text;
            }
            set
            {
                this._button.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this._button.Visible;
            }
            set
            {
                this._button.Visible = value;
            }
        }
    }
}

