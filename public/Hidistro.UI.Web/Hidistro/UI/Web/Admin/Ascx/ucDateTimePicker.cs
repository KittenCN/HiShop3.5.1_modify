namespace Hidistro.UI.Web.Admin.Ascx
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ucDateTimePicker : UserControl
    {
        private string _CssClass = "";
        private string _DateFormat = "yyyy-MM-dd";
        private bool _Enabled = true;
        private bool _IsAdmin = true;
        private bool _isEnd;
        private int _minView = 2;
        private string _PlaceHolder = "";
        private string _Style = "";
        private int _Width;
        protected Literal ltScript;
        protected TextBox txtDateTimePicker;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._CssClass))
            {
                this.txtDateTimePicker.CssClass = this._CssClass;
            }
            if (this._Width > 0)
            {
                this.txtDateTimePicker.Width = this._Width;
            }
            if (!this._Enabled)
            {
                this.txtDateTimePicker.Enabled = false;
            }
            if (!string.IsNullOrEmpty(this._PlaceHolder))
            {
                this.txtDateTimePicker.Attributes.Add("placeholder", this._PlaceHolder);
            }
            if (!string.IsNullOrEmpty(this._Style))
            {
                this.txtDateTimePicker.Attributes.Add("style", this._Style);
            }
            this.ltScript.Text = string.Concat(new object[] { "<script>var ", this.txtDateTimePicker.ClientID, "_obj=$(\"#", this.txtDateTimePicker.ClientID, "\").datetimepicker({ format: '", this._DateFormat.ToString().Replace("mm", "ii").Replace("MM", "mm").Replace("HH", "hh"), "', minView: ", this._minView, " ,isadmin:", this._IsAdmin.ToString().ToLower(), ",isEnd:", this.IsEnd ? "1" : "0", "});</script>" });
        }

        public string CssClass
        {
            get
            {
                return this._CssClass;
            }
            set
            {
                this._CssClass = value;
            }
        }

        public string DateFormat
        {
            set
            {
                switch (value)
                {
                    case "yyyy-MM-dd":
                        this._DateFormat = value;
                        return;

                    case "yyyy-MM-dd HH":
                    case "yyyy-MM-dd HH:mm":
                    case "yyyy-MM-dd HH:mm:ss":
                        this._minView = 0;
                        this._DateFormat = value;
                        return;
                }
                this._DateFormat = "yyyy-MM-dd";
            }
        }

        public bool Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                this._Enabled = value;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return this._IsAdmin;
            }
            set
            {
                this._IsAdmin = value;
            }
        }

        public bool IsEnd
        {
            get
            {
                return this._isEnd;
            }
            set
            {
                this._isEnd = value;
            }
        }

        public string PlaceHolder
        {
            get
            {
                return this._PlaceHolder;
            }
            set
            {
                this._PlaceHolder = value;
            }
        }

        public DateTime? SelectedDate
        {
            get
            {
                DateTime? nullable = null;
                string str = Globals.RequestFormStr(this.txtDateTimePicker.ClientID.Replace("_", "$"));
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        nullable = new DateTime?(DateTime.Parse(str));
                    }
                    catch (Exception)
                    {
                        return new DateTime?(DateTime.Now);
                    }
                }
                return nullable;
            }
            set
            {
                if (!value.HasValue)
                {
                    this.txtDateTimePicker.Text = "";
                }
                else
                {
                    this.txtDateTimePicker.Text = DateTime.Parse(value.ToString()).ToString(this._DateFormat);
                }
            }
        }

        public string Style
        {
            get
            {
                return this._Style;
            }
            set
            {
                this._Style = value;
            }
        }

        public string Text
        {
            get
            {
                return this.txtDateTimePicker.Text;
            }
            set
            {
                this.txtDateTimePicker.Text = value;
            }
        }

        public DateTime? TextToDate
        {
            get
            {
                DateTime? nullable = null;
                string str = this.txtDateTimePicker.Text.Trim();
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        nullable = new DateTime?(DateTime.Parse(str));
                    }
                    catch (Exception)
                    {
                        return new DateTime?(DateTime.Now);
                    }
                }
                return nullable;
            }
            set
            {
                if (!value.HasValue)
                {
                    this.txtDateTimePicker.Text = "";
                }
                else
                {
                    this.txtDateTimePicker.Text = DateTime.Parse(value.ToString()).ToString(this._DateFormat);
                }
            }
        }

        public int Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                this._Width = value;
            }
        }
    }
}

