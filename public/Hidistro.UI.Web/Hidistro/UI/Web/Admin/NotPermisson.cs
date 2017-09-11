namespace Hidistro.UI.Web.Admin
{
    using System;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class NotPermisson : Page
    {
        protected HtmlForm form1;
        protected Literal litMsg;

        private void BindDate()
        {
            StringBuilder builder = new StringBuilder();
            string str = base.Request.QueryString["type"];
            if (string.IsNullOrEmpty(str))
            {
                str = "1";
            }
            string str3 = str;
            if (str3 != null)
            {
                if (!(str3 == "1"))
                {
                    if (str3 == "2")
                    {
                        builder.Append("<li>您没有访问该页面的权限！</li>");
                    }
                    else if (str3 == "3")
                    {
                        builder.Append("<li>系统出错了！</li>");
                    }
                }
                else
                {
                    builder.Append("<li>操作出错了！</li>");
                }
            }
            string str2 = base.Request.QueryString["msg"];
            if (!string.IsNullOrEmpty(str2))
            {
                builder.AppendFormat("<li>{0}</li>", str2);
            }
            this.litMsg.Text = builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindDate();
            }
        }
    }
}

