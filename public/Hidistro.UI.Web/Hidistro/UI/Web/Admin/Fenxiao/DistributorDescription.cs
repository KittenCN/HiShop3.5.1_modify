namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class DistributorDescription : AdminPage
    {
        protected Button Button2;
        protected ucUeditor htmlfkContent;
        protected HtmlForm thisForm;

        protected DistributorDescription() : base("m05", "fxp02")
        {
        }

        protected void btnSave_fkContent(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.DistributorDescription = this.htmlfkContent.Text.Trim();
            SettingsManager.Save(masterSettings);
            this.ShowMsg("分销说明修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.htmlfkContent.Text = masterSettings.DistributorDescription;
            }
        }
    }
}

