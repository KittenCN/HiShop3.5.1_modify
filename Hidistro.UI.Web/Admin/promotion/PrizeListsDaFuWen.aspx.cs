using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.promotion
{
    public partial class PrizeListsDaFuWen : AdminPage
    {
        protected PrizeListsDaFuWen() : base("m08", "yxp10")
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