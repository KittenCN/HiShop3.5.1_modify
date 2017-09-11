namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ConcernUrl : AdminPage
    {
        private string action;
        protected Button btnSave;
        protected bool enableGuidePageSet;
        protected HtmlForm form1;
        private SiteSettings siteSettings;
        protected TextBox txtGuidePageSet;

        protected ConcernUrl() : base("m06", "wxp02")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
            this.action = Globals.RequestFormStr("action");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.siteSettings.GuidePageSet = this.txtGuidePageSet.Text.Trim();
            SettingsManager.Save(this.siteSettings);
            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                if (this.action == "setenable")
                {
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                    this.siteSettings.EnableGuidePageSet = Globals.RequestFormNum("enable") == 1;
                    SettingsManager.Save(this.siteSettings);
                    base.Response.Write(s);
                    base.Response.End();
                }
                else if (this.siteSettings.GuidePageSet.Length > 10)
                {
                    this.txtGuidePageSet.Text = this.siteSettings.GuidePageSet;
                }
            }
            this.enableGuidePageSet = this.siteSettings.EnableGuidePageSet;
        }
    }
}

