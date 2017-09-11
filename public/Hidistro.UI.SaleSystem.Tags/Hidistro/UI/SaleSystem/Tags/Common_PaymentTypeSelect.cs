namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_PaymentTypeSelect : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            IList<PaymentModeInfo> paymentModes = ShoppingProcessor.GetPaymentModes();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            StringBuilder builder = new StringBuilder();
            builder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
            builder.AppendLine("<ul id=\"ulselectPaymentType\" class=\"dropdown-menu\" role=\"menu\">");
            string userAgent = this.Page.Request.UserAgent;
            if ((masterSettings.EnableWeiXinRequest && userAgent.ToLower().Contains("micromessenger")) && masterSettings.IsValidationService)
            {
                builder.AppendLine("<li><a href=\"javascript:void(0)\" name=\"88\">微信支付</a></li>");
            }
            if (Globals.RequestQueryNum("bargainDetialId") <= 0)
            {
                if (masterSettings.EnableOffLineRequest)
                {
                    builder.AppendLine("<li><a href=\"javascript:void(0)\" name=\"99\">线下付款</a></li>");
                }
                if (masterSettings.EnablePodRequest)
                {
                    builder.AppendLine("<li><a href=\"javascript:void(0)\" name=\"0\">货到付款</a></li>");
                }
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
                            builder.AppendFormat("<li><a href=\"javascript:void(0)\" name=\"{0}\">{1}</a></li>", info.ModeId, info.Name).AppendLine();
                        }
                    }
                    else if ((masterSettings.EnableWapShengPay && !string.IsNullOrEmpty(document.GetElementsByTagName("SenderId")[0].InnerText)) && !string.IsNullOrEmpty(document.GetElementsByTagName("SellerKey")[0].InnerText))
                    {
                        builder.AppendFormat("<li><a href=\"javascript:void(0)\" name=\"{0}\">{1}</a></li>", info.ModeId, info.Name).AppendLine();
                    }
                }
            }
            builder.AppendLine("</ul>");
            writer.Write(builder.ToString());
        }
    }
}

