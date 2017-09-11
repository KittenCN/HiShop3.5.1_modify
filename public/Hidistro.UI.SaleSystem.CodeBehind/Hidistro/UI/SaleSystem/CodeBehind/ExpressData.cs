namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using System;
    using System.Web;

    public class ExpressData : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                this.SearchExpressData(context);
            }
            catch
            {
            }
        }

        private void SearchExpressData(HttpContext context)
        {
            string s = "{";
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (((orderInfo != null) && ((orderInfo.OrderStatus == OrderStatus.SellerAlreadySent) || (orderInfo.OrderStatus == OrderStatus.Finished))) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    string str3 = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber, 2);
                    s = s + "\"Express\":\"" + str3 + "\"";
                }
            }
            s = s + "}";
            context.Response.ContentType = "text/plain";
            context.Response.Write(s);
            context.Response.End();
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

