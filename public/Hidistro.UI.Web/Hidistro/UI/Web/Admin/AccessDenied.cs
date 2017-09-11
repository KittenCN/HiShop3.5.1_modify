namespace Hidistro.UI.Web.Admin
{
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class AccessDenied : AdminPage
    {
        protected Literal litMessage;

        protected AccessDenied() : base("m01", "00000")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.litMessage.Text = "您登录的管理员帐号没有权限访问当前页面或进行当前操作";
        }
    }
}

