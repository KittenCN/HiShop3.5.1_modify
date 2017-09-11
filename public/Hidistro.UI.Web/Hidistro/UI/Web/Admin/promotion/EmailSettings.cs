namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using System;
    using System.Net.Mail;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.SiteSettings)]
    public class EmailSettings : AdminPage
    {
        protected Button btnChangeEmailSettings;
        protected Button btnTestEmailSettings;
        protected Script Script1;
        protected HtmlForm thisForm;
        protected HiddenField txtConfigData;
        protected HiddenField txtSelectedName;
        protected TextBox txtTestEmail;

        protected EmailSettings() : base("m08", "yxp18")
        {
        }

        private void btnChangeEmailSettings_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = this.LoadConfig(out str);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                masterSettings.EmailSender = string.Empty;
                masterSettings.EmailSettings = string.Empty;
            }
            else
            {
                if (!data.IsValid)
                {
                    string msg = "";
                    foreach (string str3 in data.ErrorMsgs)
                    {
                        msg = msg + Formatter.FormatErrorMessage(str3);
                    }
                    this.ShowMsg(msg, false);
                    return;
                }
                masterSettings.EmailSender = str;
                masterSettings.EmailSettings = HiCryptographer.Encrypt(data.SettingsXml);
            }
            SettingsManager.Save(masterSettings);
            this.ShowMsg("配置成功", true);
        }

        private void btnTestEmailSettings_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = this.LoadConfig(out str);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                this.ShowMsg("请先选择发送方式并填写配置信息", false);
            }
            else if (!data.IsValid)
            {
                string msg = "";
                foreach (string str3 in data.ErrorMsgs)
                {
                    msg = msg + Formatter.FormatErrorMessage(str3);
                }
                this.ShowMsg(msg, false);
            }
            else if (string.IsNullOrEmpty(this.txtTestEmail.Text) || (this.txtTestEmail.Text.Trim().Length == 0))
            {
                this.ShowMsg("请填写接收测试邮件的邮箱地址", false);
            }
            else if (!Regex.IsMatch(this.txtTestEmail.Text.Trim(), @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                this.ShowMsg("请填写正确的邮箱地址", false);
            }
            else
            {
                MailMessage mail = new MailMessage {
                    IsBodyHtml = true,
                    Priority = MailPriority.High,
                    Body = "Success",
                    Subject = "This is a test mail"
                };
                mail.To.Add(this.txtTestEmail.Text.Trim());
                EmailSender sender2 = EmailSender.CreateInstance(str, data.SettingsXml);
                try
                {
                    if (sender2.Send(mail, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
                    {
                        this.ShowMsg("发送测试邮件成功", true);
                    }
                    else
                    {
                        this.ShowMsg("发送测试邮件失败", false);
                    }
                }
                catch
                {
                    this.ShowMsg("邮件配置错误", false);
                }
            }
        }

        private ConfigData LoadConfig(out string selectedName)
        {
            selectedName = base.Request.Form["ddlEmails"];
            this.txtSelectedName.Value = selectedName;
            this.txtConfigData.Value = "";
            if (string.IsNullOrEmpty(selectedName) || (selectedName.Length == 0))
            {
                return null;
            }
            ConfigablePlugin plugin = EmailSender.CreateInstance(selectedName);
            if (plugin == null)
            {
                return null;
            }
            ConfigData configData = plugin.GetConfigData(base.Request.Form);
            if (configData != null)
            {
                this.txtConfigData.Value = configData.SettingsXml;
            }
            return configData;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnChangeEmailSettings.Click += new EventHandler(this.btnChangeEmailSettings_Click);
            this.btnTestEmailSettings.Click += new EventHandler(this.btnTestEmailSettings_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (masterSettings.EmailEnabled)
                {
                    this.txtSelectedName.Value = masterSettings.EmailSender.ToLower();
                    ConfigData data = new ConfigData(HiCryptographer.Decrypt(masterSettings.EmailSettings));
                    this.txtConfigData.Value = data.SettingsXml;
                }
            }
        }
    }
}

