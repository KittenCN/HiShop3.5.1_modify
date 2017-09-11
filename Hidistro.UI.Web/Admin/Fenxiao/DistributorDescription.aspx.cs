using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
    public partial class DistributorDescription : AdminPage
    {
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