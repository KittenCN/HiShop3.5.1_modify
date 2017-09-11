namespace Hidistro.UI.Web.Vshop
{
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class _default : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.Response.StatusCode = 0x12d;
            base.Response.Status = "301 Moved Permanently";
            base.Response.AppendHeader("Location", "/default.aspx");
            base.Response.AppendHeader("Cache-Control", "no-cache");
            base.Response.End();
        }
    }
}

