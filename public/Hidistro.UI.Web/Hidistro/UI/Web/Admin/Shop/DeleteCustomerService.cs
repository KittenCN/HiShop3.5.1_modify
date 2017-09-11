namespace Hidistro.UI.Web.Admin.Shop
{
   using  global:: ControlPanel.Settings;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Settings;
    using Hishop.MeiQia.Api.Api;
    using Hishop.MeiQia.Api.Util;
    using System;
    using System.Web;

    public class DeleteCustomerService : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (Globals.GetCurrentManagerUserId() <= 0)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"请先登录\"}");
                context.Response.End();
            }
            int result = 0;
            int.TryParse(context.Request["id"].ToString(), out result);
            if (result > 0)
            {
                CustomerServiceInfo customer = CustomerServiceHelper.GetCustomer(result);
                CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
                string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
                if (!string.IsNullOrEmpty(tokenValue))
                {
                    string str2 = CustomerApi.DeleteCustomer(tokenValue, customer.unit, customer.userver);
                    if (!string.IsNullOrWhiteSpace(str2))
                    {
                        string jsonValue = Common.GetJsonValue(str2, "errcode");
                        string str4 = Common.GetJsonValue(str2, "errmsg");
                        if (jsonValue == "0")
                        {
                            if (CustomerServiceHelper.DeletCustomer(result))
                            {
                                context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
                            }
                            else
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"删除客服失败！\"}");
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"type\":\"error\",\"data\":\"" + str4 + "\"}");
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"删除客服失败！\"}");
                    }
                }
                else
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"获取access_token失败！\"}");
                }
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

