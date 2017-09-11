namespace Hidistro.UI.Web.Admin.Shop
{
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class Default : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.Response.Write(";");
            base.Response.End();
        }
    }
}

