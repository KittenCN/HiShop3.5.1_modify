namespace Hidistro.UI.Web.GetStoreCard
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class Default : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            int currentDistributorId = Globals.GetCurrentDistributorId();
            base.Response.Redirect(string.Concat(new object[] { "/Vshop/StoreCard.aspx?userId=", currentDistributorId, "&ReferralId=", currentDistributorId }));
            base.Response.End();
        }
    }
}

