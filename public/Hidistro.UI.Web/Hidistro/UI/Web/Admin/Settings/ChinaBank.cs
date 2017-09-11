namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ChinaBank : AdminPage
    {
        protected bool _enable;
        protected Script Script4;
        protected HtmlForm thisForm;
        protected TextBox txt_des;
        protected TextBox txt_md5;
        protected TextBox txt_mid;

        protected ChinaBank() : base("m09", "szp02")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txt_mid.Text = masterSettings.ChinaBank_mid;
                this.txt_md5.Text = masterSettings.ChinaBank_MD5;
                this.txt_des.Text = masterSettings.ChinaBank_DES;
                this._enable = masterSettings.ChinaBank_Enable;
            }
        }

        private void saveData()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(this.txt_mid.Text))
            {
                this.ShowMsg("请输入支付宝帐号！", false);
            }
            masterSettings.ChinaBank_mid = this.txt_mid.Text;
            if (string.IsNullOrEmpty(this.txt_md5.Text))
            {
                this.ShowMsg("请输入支付宝帐号姓名！", false);
            }
            masterSettings.ChinaBank_MD5 = this.txt_md5.Text;
            if (string.IsNullOrEmpty(this.txt_des.Text))
            {
                this.ShowMsg("请输入合作者身份（PID）！", false);
            }
            masterSettings.ChinaBank_DES = this.txt_des.Text;
            SettingsManager.Save(masterSettings);
            this.ShowMsg("保存成功！", true);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            this.saveData();
        }
    }
}

