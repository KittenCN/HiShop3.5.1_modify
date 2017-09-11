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

    public class ShengPay : AdminPage
    {
        protected bool _enable;
        protected Script Script4;
        private SiteSettings siteSettings;
        protected HtmlForm thisForm;
        protected TextBox txt_key;
        protected TextBox txt_mid;

        protected ShengPay() : base("m09", "szp03")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.txt_mid.Text = this.siteSettings.ShenPay_mid;
                this.txt_key.Text = this.siteSettings.ShenPay_key;
            }
            this._enable = this.siteSettings.EnableWapShengPay;
        }

        private void saveData()
        {
            if (string.IsNullOrEmpty(this.txt_mid.Text))
            {
                this.ShowMsg("请输入商户号！", false);
            }
            this.siteSettings.ShenPay_mid = this.txt_mid.Text;
            if (string.IsNullOrEmpty(this.txt_key.Text))
            {
                this.ShowMsg("请输入商家密钥（Key）！", false);
            }
            this.siteSettings.ShenPay_key = this.txt_key.Text;
            SettingsManager.Save(this.siteSettings);
            string text = string.Format("<xml><SenderId>{0}</SenderId><SellerKey>{1}</SellerKey><Seller_account_name></Seller_account_name></xml>", this.txt_mid.Text, this.txt_key.Text);
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
            if (paymentMode == null)
            {
                PaymentModeInfo info2 = new PaymentModeInfo {
                    Name = "盛付通手机网页支付",
                    Gateway = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest",
                    Description = string.Empty,
                    IsUseInpour = false,
                    Charge = 0M,
                    IsPercent = false,
                    ApplicationType = PayApplicationType.payOnWAP,
                    Settings = HiCryptographer.Encrypt(text)
                };
                SalesHelper.CreatePaymentMode(info2);
            }
            else
            {
                PaymentModeInfo info4 = paymentMode;
                info4.Settings = HiCryptographer.Encrypt(text);
                info4.ApplicationType = PayApplicationType.payOnWAP;
                SalesHelper.UpdatePaymentMode(info4);
            }
            this.ShowMsg("保存成功！", true);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            this.saveData();
        }
    }
}

