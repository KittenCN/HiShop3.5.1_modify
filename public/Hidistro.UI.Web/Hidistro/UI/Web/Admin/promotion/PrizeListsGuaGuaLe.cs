namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;

    public class PrizeListsGuaGuaLe : AdminPage
    {
        protected HtmlForm thisForm;
        protected UCPrizeLists UCPrizeLists1;

        protected PrizeListsGuaGuaLe() : base("m08", "yxp11")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int.Parse(base.Request.QueryString["gameId"]);
            }
            catch (Exception)
            {
                base.GotoResourceNotFound();
            }
        }
    }
}

