namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Orders;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderRemarkImage : Literal
    {
        private string dataField;
        private string imageFormat = "<span class=\"glyphicon {0} help\" style=\"color:{1}\"></span>";
        private int managerMarkValue;

        protected string GetImageSrc(object managerMark)
        {
            string str = "glyphicon-flag";
            string str2 = "#ababab";
            switch (((OrderMark) managerMark))
            {
                case OrderMark.Draw:
                    str = "glyphicon-ok";
                    str2 = "#309930";
                    break;

                case OrderMark.ExclamationMark:
                    str = "glyphicon-exclamation-sign";
                    str2 = "#CB1E02";
                    break;

                case OrderMark.Red:
                    str2 = "#CB1E02";
                    break;

                case OrderMark.Green:
                    str2 = "#4E994E";
                    break;

                case OrderMark.Yellow:
                    str2 = "#FFC500";
                    break;

                case OrderMark.Gray:
                    str2 = "#ABABAB";
                    break;
            }
            return string.Format(this.imageFormat, str, str2);
        }

        protected override void OnDataBinding(EventArgs e)
        {
            if (this.managerMarkValue <= 0)
            {
                object managerMark = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
                if ((managerMark != null) && (managerMark != DBNull.Value))
                {
                    base.Text = this.GetImageSrc(managerMark);
                }
                else
                {
                    base.Text = string.Format(this.imageFormat, "glyphicon-flag", "#ababab");
                }
            }
            base.OnDataBinding(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.managerMarkValue > 0)
            {
                base.Text = this.GetImageSrc(this.managerMarkValue);
            }
            base.Render(writer);
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

        public int ManagerMarkValue
        {
            get
            {
                return this.managerMarkValue;
            }
            set
            {
                this.managerMarkValue = value;
            }
        }
    }
}

