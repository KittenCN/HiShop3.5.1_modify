namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class NoPermissionShow : Page
    {
        protected int CurrentUserId;
        protected Literal leftMenu;
        protected Literal litSitename;
        protected Literal litUsername;
        protected Literal topMenu;

        protected void Page_Load(object sender, EventArgs e)
        {
            ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
            if (currentManager != null)
            {
                this.CurrentUserId = currentManager.UserId;
            }
            else
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/admin/Login.aspx", true);
            }
            if (!this.Page.IsPostBack)
            {
                string currentModuleId = Globals.RequestQueryStr("m");
                string currentPageId = Globals.RequestQueryStr("p");
                Navigation navigation = Navigation.GetNavigation(true);
                this.topMenu.Text = navigation.RenderTopMenu(currentModuleId);
                this.leftMenu.Text = navigation.RenderLeftMenu(currentModuleId, currentPageId);
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                this.litSitename.Text = masterSettings.SiteName;
                this.litUsername.Text = currentManager.UserName;
            }
        }
    }
}

