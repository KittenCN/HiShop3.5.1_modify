﻿namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class FormatedTimeLabel : Literal
    {
        private string dataField;
        private string formatDateTime = string.Empty;
        private string nullToDisplay = "-";
        private bool showTime = true;

        public override void DataBind()
        {
            if (this.DataField != null)
            {
                this.Time = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
            }
            else
            {
                base.DataBind();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (((this.Time == null) || (this.Time == DBNull.Value)) || (Convert.ToDateTime((DateTime) this.Time, CultureInfo.InvariantCulture) == DateTime.MinValue))
            {
                base.Text = this.NullToDisplay;
            }
            else
            {
                DateTime time = (DateTime) this.Time;
                if (!string.IsNullOrEmpty(this.FormatDateTime))
                {
                    base.Text = time.ToString(this.FormatDateTime, CultureInfo.InvariantCulture);
                }
                else if (this.ShopTime)
                {
                    base.Text = time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    base.Text = time.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                base.Render(writer);
            }
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

        public string FormatDateTime
        {
            get
            {
                return this.formatDateTime;
            }
            set
            {
                this.formatDateTime = value;
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

        public bool ShopTime
        {
            get
            {
                return this.showTime;
            }
            set
            {
                this.showTime = value;
            }
        }

        public object Time
        {
            get
            {
                if (this.ViewState["Time"] == null)
                {
                    return null;
                }
                return this.ViewState["Time"];
            }
            set
            {
                if ((value == null) || (value == DBNull.Value))
                {
                    this.ViewState["Time"] = null;
                }
                else
                {
                    this.ViewState["Time"] = value;
                }
            }
        }
    }
}

