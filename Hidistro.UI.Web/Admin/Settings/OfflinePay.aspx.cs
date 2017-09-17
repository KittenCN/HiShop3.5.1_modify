using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Settings
{
    public partial class OfflinePay : AdminPage
    {
        protected string _content;
        protected bool _enable;
        protected bool _podenable;
       
        private SiteSettings siteSettings;
       

        protected OfflinePay() : base("m09", "szp04")
        {
            this._content = "";
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.fkContent.Text = this.siteSettings.OffLinePayContent;
                this._podenable = this.siteSettings.EnablePodRequest;
            }
            this._enable = this.siteSettings.EnableOffLineRequest;
        }

        private void SaveData()
        {
            if (string.IsNullOrEmpty(this.fkContent.Text))
            {
                this.ShowMsg("请输入内容！", false);
            }
            this.siteSettings.OffLinePayContent = this.fkContent.Text;
            SettingsManager.Save(this.siteSettings);
            this.ShowMsgAndReUrl("保存成功", true, "OfflinePay.aspx");
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            this.SaveData();
        }
    }
}