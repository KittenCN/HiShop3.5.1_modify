namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class ReplyProductConsultation : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int num = 0;
            int.TryParse(context.Request["id"].ToString(), out num);
            string str = context.Request["content"].ToString();
            if ((num > 0) && !string.IsNullOrEmpty(str))
            {
                ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(num);
                productConsultation.ReplyText = str;
                productConsultation.ReplyUserId = new int?(Globals.GetCurrentManagerUserId());
                productConsultation.ReplyDate = new DateTime?(DateTime.Now);
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductConsultationInfo>(productConsultation, new string[] { "Reply" });
                string str2 = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        str2 = str2 + Formatter.FormatErrorMessage(result.Message);
                    }
                    context.Response.Write("{\"type\":\"error\",\"data\":\"" + str2 + "\"}");
                }
                if (ProductCommentHelper.ReplyProductConsultation(productConsultation))
                {
                    context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
                }
                else
                {
                    context.Response.Write("{\"type\":\"success\",\"data\":\"回复商品咨询失败\"}");
                }
            }
            else
            {
                context.Response.Write("{\"type\":\"success\",\"data\":\"回复商品咨询失败\"}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

