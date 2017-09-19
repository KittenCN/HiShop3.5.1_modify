using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace Hidistro.UI.Web.GetStoreCard
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int currentDistributorId = Globals.GetCurrentDistributorId();
            base.Response.Redirect(string.Concat(new object[] { "/Vshop/StoreCard.aspx?userId=", currentDistributorId, "&ReferralId=", currentDistributorId }));
            base.Response.End();
        }
    }
}