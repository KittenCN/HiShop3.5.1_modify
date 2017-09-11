namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class VMemberRecharge : VMemberTemplatedWebControl
    {
        private Literal litPaymentType;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("余额充值");
            this.litPaymentType = (Literal) this.FindControl("litPaymentType");
            IList<PaymentModeInfo> paymentModes = ShoppingProcessor.GetPaymentModes();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            StringBuilder builder = new StringBuilder();
            string userAgent = this.Page.Request.UserAgent;
            if ((masterSettings.EnableWeiXinRequest && userAgent.ToLower().Contains("micromessenger")) && masterSettings.IsValidationService)
            {
                builder.AppendLine("<div class=\"payway\" name=\"88\">微信支付</div>");
            }
            if ((paymentModes != null) && (paymentModes.Count > 0))
            {
                foreach (PaymentModeInfo info in paymentModes)
                {
                    string xml = HiCryptographer.Decrypt(info.Settings);
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(xml);
                    if (document.GetElementsByTagName("Partner").Count != 0)
                    {
                        if ((masterSettings.EnableAlipayRequest && !string.IsNullOrEmpty(document.GetElementsByTagName("Partner")[0].InnerText)) && (!string.IsNullOrEmpty(document.GetElementsByTagName("Key")[0].InnerText) && !string.IsNullOrEmpty(document.GetElementsByTagName("Seller_account_name")[0].InnerText)))
                        {
                            builder.AppendFormat("<div class=\"payway\" name=\"{0}\">{1}</div>", info.ModeId, info.Name).AppendLine();
                        }
                    }
                    else if ((masterSettings.EnableWapShengPay && !string.IsNullOrEmpty(document.GetElementsByTagName("SenderId")[0].InnerText)) && !string.IsNullOrEmpty(document.GetElementsByTagName("SellerKey")[0].InnerText))
                    {
                        builder.AppendFormat("<div class=\"payway\" name=\"{0}\">{1}</div>", info.ModeId, info.Name).AppendLine();
                    }
                }
            }
            this.litPaymentType.Text = builder.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberRecharge.html";
            }
            base.OnInit(e);
        }
    }
}

