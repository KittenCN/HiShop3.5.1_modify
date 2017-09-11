namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AdminNew : MasterPage
    {
        protected ContentPlaceHolder ContentPlaceHolder1;
        protected int CurrentUserId;
        protected ContentPlaceHolder head;
        protected HtmlInputHidden hiddTelReg;
        protected string htmlWebTitle = string.Empty;
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
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            this.litSitename.Text = masterSettings.SiteName;
            this.htmlWebTitle = masterSettings.SiteName;
            this.hiddTelReg.Value = masterSettings.TelReg;
            if (!this.Page.IsPostBack)
            {
                AdminPage page = this.Page as AdminPage;
                Navigation navigation = Navigation.GetNavigation(true);
                this.topMenu.Text = navigation.RenderTopMenu(page.ModuleId);
                this.leftMenu.Text = navigation.RenderLeftMenu(page.ModuleId, page.PageId);
                this.litUsername.Text = currentManager.UserName;
            }
        }
    }
}

