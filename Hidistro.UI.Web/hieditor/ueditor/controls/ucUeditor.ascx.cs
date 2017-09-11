using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
           
namespace Hidistro.UI.Web.hieditor.ueditor.controls
{
    public partial class ucUeditor : UserControl
    {
        public int _height = 200;
        public bool _isfristedit = true;
        public int _ShowType;
        public int _width = 600;


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