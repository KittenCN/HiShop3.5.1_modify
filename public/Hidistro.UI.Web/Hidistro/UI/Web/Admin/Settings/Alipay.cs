namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Alipay : AdminPage
    {
        protected bool _enable;
        protected Script Script4;
        private SiteSettings siteSettings;
        protected HtmlForm thisForm;
        protected TextBox txt_key;
        protected TextBox txt_mid;
        protected TextBox txt_mName;
        protected TextBox txt_pid;

        protected Alipay() : base("m09", "szp01")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.txt_mid.Text = this.siteSettings.Alipay_mid;
                this.txt_mName.Text = this.siteSettings.Alipay_mName;
                this.txt_pid.Text = this.siteSettings.Alipay_Pid;
                this.txt_key.Text = this.siteSettings.Alipay_Key;
            }
            this._enable = this.siteSettings.EnableAlipayRequest;
        }

        private void saveData()
        {
            if (string.IsNullOrEmpty(this.txt_mid.Text))
            {
                this.ShowMsg("请输入支付宝帐号！", false);
            }
            this.siteSettings.Alipay_mid = this.txt_mid.Text;
            if (string.IsNullOrEmpty(this.txt_mName.Text))
            {
                this.ShowMsg("请输入支付宝帐号姓名！", false);
            }
            this.siteSettings.Alipay_mName = this.txt_mName.Text;
            if (string.IsNullOrEmpty(this.txt_pid.Text))
            {
                this.ShowMsg("请输入合作者身份（PID）！", false);
            }
            this.siteSettings.Alipay_Pid = this.txt_pid.Text;
            if (string.IsNullOrEmpty(this.txt_key.Text))
            {
                this.ShowMsg("请输入安全校验码（Key）！", false);
            }
            string text = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><Seller_account_name>{2}</Seller_account_name></xml>", this.txt_pid.Text, this.txt_key.Text, this.txt_mid.Text);
            if (!string.IsNullOrWhiteSpace(this.txt_pid.Text) && !string.IsNullOrWhiteSpace(this.txt_key.Text))
            {
                string.IsNullOrWhiteSpace(this.txt_mid.Text);
            }
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
            if (paymentMode == null)
            {
                PaymentModeInfo info2 = new PaymentModeInfo {
                    Name = "支付宝手机支付",
                    Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest",
                    Description = string.Empty,
                    IsUseInpour = true,
                    Charge = 0M,
                    IsPercent = false,
                    Settings = HiCryptographer.Encrypt(text)
                };
                if (SalesHelper.CreatePaymentMode(info2) == PaymentModeActionStatus.Success)
                {
                    this.siteSettings.Alipay_Key = this.txt_key.Text;
                    SettingsManager.Save(this.siteSettings);
                    this.ShowMsg("设置成功", true);
                }
                else
                {
                    this.ShowMsg("设置失败", false);
                }
            }
            else
            {
                paymentMode.Settings = HiCryptographer.Encrypt(text);
                if (SalesHelper.UpdatePaymentMode(paymentMode) == PaymentModeActionStatus.Success)
                {
                    this.siteSettings.Alipay_Key = this.txt_key.Text;
                    SettingsManager.Save(this.siteSettings);
                    this.ShowMsg("设置成功", true);
                }
                else
                {
                    this.ShowMsg("设置失败", false);
                }
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            this.saveData();
        }
    }
}

