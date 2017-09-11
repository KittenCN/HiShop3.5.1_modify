namespace Hidistro.UI.Web.Admin.promotion
{
    using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using System;
    using System.Web;

    public class EditProductHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (Globals.GetCurrentManagerUserId() <= 0)
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请先登录\"}");
                    context.Response.End();
                }
                int num = int.Parse(context.Request["actType"].ToString());
                int couponId = int.Parse(context.Request["id"].ToString());
                string productIds = context.Request["products"];
                int num3 = int.Parse(context.Request["type"].ToString());
                bool flag = false;
                if (num == 0)
                {
                    switch (num3)
                    {
                        case 0:
                            flag = CouponHelper.SetProductsStatus(couponId, 1, productIds);
                            break;

                        case 1:
                            flag = CouponHelper.SetProductsStatus(couponId, 0, productIds);
                            break;

                        case 2:
                            flag = CouponHelper.DeleteProducts(couponId, productIds);
                            break;
                    }
                }
                else if (num == 1)
                {
                    switch (num3)
                    {
                        case 0:
                            flag = ActivityHelper.SetProductsStatus(couponId, 1, productIds);
                            break;

                        case 1:
                            flag = ActivityHelper.SetProductsStatus(couponId, 0, productIds);
                            break;

                        case 2:
                            flag = ActivityHelper.DeleteProducts(couponId, productIds);
                            break;
                    }
                }
                else if (num == 2)
                {
                    switch (num3)
                    {
                        case 0:
                            flag = PointExChangeHelper.SetProductsStatus(couponId, 1, productIds);
                            break;

                        case 1:
                            flag = PointExChangeHelper.SetProductsStatus(couponId, 0, productIds);
                            break;

                        case 2:
                            flag = PointExChangeHelper.DeleteProducts(couponId, productIds);
                            break;
                    }
                }
                if (flag)
                {
                    context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
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

