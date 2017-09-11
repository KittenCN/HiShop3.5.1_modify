namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Text;
    using System.Web.UI;

    public class LeaveListTime : Control
    {
        private string auto = "productId";
        private string bindData = "EndDate";
        private string outStr = string.Empty;
        private string startData = "StartDate";

        public override void DataBind()
        {
            int num = 1;
            int num2 = (int) DataBinder.Eval(this.Page.GetDataItem(), this.Auto);
            DateTime time = (DateTime) DataBinder.Eval(this.Page.GetDataItem(), this.BindData);
            DateTime time2 = (DateTime) DataBinder.Eval(this.Page.GetDataItem(), this.StartData);
            if (time < DateTime.Now)
            {
                num = 0;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(" <script type=\"text/javascript\"> ");
            builder.AppendFormat(" function LimitTimeBuyTimeShow_{0}()", num2.ToString());
            builder.Append(" { ");
            builder.AppendFormat(" showTimeList(\"{0}\",\"htmlspan{1}\",\"{2}\",\"{3}\");", new object[] { time.ToString("yyyy-MM-dd HH:mm:ss"), num2.ToString(), num, time2.ToString("yyyy-MM-dd HH:mm:ss") });
            builder.AppendFormat(" setTimeout(\"LimitTimeBuyTimeShow_{0}()\", 1000);", num2.ToString());
            builder.Append(" }");
            builder.AppendFormat(" LimitTimeBuyTimeShow_{0}(); ", num2.ToString());
            builder.Append(" </script>");
            this.outStr = builder.ToString();
            base.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.outStr.ToString());
        }

        public string Auto
        {
            get
            {
                return this.auto;
            }
            set
            {
                this.auto = value;
            }
        }

        public string BindData
        {
            get
            {
                return this.bindData;
            }
            set
            {
                this.bindData = value;
            }
        }

        public string StartData
        {
            get
            {
                return this.startData;
            }
            set
            {
                this.startData = value;
            }
        }
    }
}

