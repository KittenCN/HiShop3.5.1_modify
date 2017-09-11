namespace Hidistro.UI.Web
{
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class WebForm1 : Page
    {
        private readonly Database database = DatabaseFactory.CreateDatabase();
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.Response.Write(this.database.ConnectionString);
        }
    }
}

