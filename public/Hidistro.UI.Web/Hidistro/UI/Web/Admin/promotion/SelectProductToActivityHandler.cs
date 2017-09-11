namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.ControlPanel.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class SelectProductToActivityHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                int couponId = int.Parse(context.Request["id"].ToString());
                string s = context.Request["products"].ToString();
                bool flag = bool.Parse(context.Request["bsingle"].ToString());
                bool flag2 = bool.Parse(context.Request["setSale"].ToString());
                bool flag3 = false;
                if (!flag)
                {
                    bool isAllProduct = bool.Parse(context.Request["all"].ToString());
                    IList<string> productIDs = s.Split(new char[] { '|' });
                    flag3 = ActivityHelper.AddProducts(couponId, isAllProduct, productIDs);
                    if (flag2)
                    {
                        ProductHelper.UpShelf(s.Replace('|', ','));
                    }
                }
                else
                {
                    int productID = int.Parse(s);
                    flag3 = ActivityHelper.AddProducts(couponId, productID);
                    if (flag2)
                    {
                        ProductHelper.UpShelf(productID.ToString());
                    }
                }
                if (flag3)
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

