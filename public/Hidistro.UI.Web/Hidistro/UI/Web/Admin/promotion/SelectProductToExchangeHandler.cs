namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using System;
    using System.Web;

    public class SelectProductToExchangeHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                int exchangeId = int.Parse(context.Request["id"].ToString());
                string productIds = context.Request["products"].ToString();
                string pNumbers = context.Request["pNumbers"];
                string points = context.Request["points"];
                string eachNumbers = context.Request["eachNumbers"];
                if (PointExChangeHelper.AddProducts(exchangeId, productIds, pNumbers, points, eachNumbers))
                {
                    context.Response.Write("{\"type\":\"success\",\"data\":\"添加商品成功\"}");
                }
                else
                {
                    context.Response.Write("{\"type\":\"success\",\"data\":\"写数据库失败\"}");
                }
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"" + exception.Message + "\"}");
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

