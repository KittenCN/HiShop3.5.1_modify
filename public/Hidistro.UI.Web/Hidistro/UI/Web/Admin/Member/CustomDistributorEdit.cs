namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class CustomDistributorEdit : AdminPage
    {
        protected HtmlForm thisForm;
        protected TextBox txtGroupName;
        protected TextBox txtShopIntroduction;

        protected CustomDistributorEdit() : base("m04", "hyp05")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

