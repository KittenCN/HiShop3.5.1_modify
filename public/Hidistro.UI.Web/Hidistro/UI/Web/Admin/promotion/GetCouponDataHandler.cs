namespace Hidistro.UI.Web.Admin.promotion
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Web;

    public class GetCouponDataHandler : IHttpHandler
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
                CouponInfo coupon = CouponHelper.GetCoupon(int.Parse(context.Request["id"].ToString()));
                var type = new {
                    type = "success",
                    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    data = coupon
                };
                IsoDateTimeConverter converter = new IsoDateTimeConverter {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
                string s = JsonConvert.SerializeObject(type, Formatting.Indented, new JsonConverter[] { converter });
                context.Response.Write(s);
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

