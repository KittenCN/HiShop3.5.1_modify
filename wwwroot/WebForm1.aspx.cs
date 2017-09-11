using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Hidistro.UI.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        readonly Database database = DatabaseFactory.CreateDatabase();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(database.ConnectionString);
        }
    }
}