namespace Hidistro.UI.Web.Admin.Shop
{
   using  global:: ControlPanel.Settings;
    using Hidistro.Entities.Settings;
    using Newtonsoft.Json;
    using System;
    using System.Web;

    public class GetCustomerService : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int result = 0;
            int.TryParse(context.Request["id"].ToString(), out result);
            if (result > 0)
            {
                CustomerServiceInfo customer = CustomerServiceHelper.GetCustomer(result);
                var typea = new {
                    type = "success",
                    userver = customer.userver,
                    password = customer.password,
                    nickname = customer.nickname
                };
                string s = JsonConvert.SerializeObject(typea);
                context.Response.Write(s);
            }
            else
            {
                var typea2 = new {
                    type = "success",
                    userver = string.Empty,
                    password = string.Empty,
                    nickname = string.Empty
                };
                string str2 = JsonConvert.SerializeObject(typea2);
                context.Response.Write(str2);
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

