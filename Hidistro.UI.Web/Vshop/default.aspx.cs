using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Vshop
{
    public partial class _default : System.Web.UI.Page
    {
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