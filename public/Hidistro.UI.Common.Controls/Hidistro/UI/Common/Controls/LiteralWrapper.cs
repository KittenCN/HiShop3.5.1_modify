namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class LiteralWrapper : IText
    {
        private Literal _literal;

        internal LiteralWrapper(Literal literal)
        {
            this._literal = literal;
        }

        public System.Web.UI.Control Control
        {
            get
            {
                return this._literal;
            }
        }

        public string Text
        {
            get
            {
                return this._literal.Text;
            }
            set
            {
                this._literal.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this._literal.Visible;
            }
            set
            {
                this._literal.Visible = value;
            }
        }
    }
}

