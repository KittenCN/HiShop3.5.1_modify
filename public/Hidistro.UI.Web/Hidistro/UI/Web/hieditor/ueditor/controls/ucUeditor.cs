namespace Hidistro.UI.Web.hieditor.ueditor.controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ucUeditor : UserControl
    {
        private int _height = 200;
        private bool _isfristedit = true;
        private int _ShowType;
        private int _width = 600;
        protected TextBox txtMemo;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtMemo.Width = this._width;
            this.txtMemo.Height = this._height;
        }

        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        public bool IsFirstEdit
        {
            get
            {
                return this._isfristedit;
            }
            set
            {
                this._isfristedit = value;
            }
        }

        public int ShowType
        {
            get
            {
                return this._ShowType;
            }
            set
            {
                this._ShowType = value;
            }
        }

        public string Text
        {
            get
            {
                return base.Request.Form[this.txtMemo.ClientID.Replace("_", "$")];
            }
            set
            {
                this.txtMemo.Text = value;
            }
        }

        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }
    }
}

