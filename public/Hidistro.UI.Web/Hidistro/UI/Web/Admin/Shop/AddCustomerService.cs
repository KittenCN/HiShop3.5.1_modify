namespace Hidistro.UI.Web.Admin.Shop
{
   using  global:: ControlPanel.Settings;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Settings;
    using Hishop.MeiQia.Api.Api;
    using Hishop.MeiQia.Api.Util;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Security;

    public class AddCustomerService : IHttpHandler
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
            CustomerServiceSettings masterSettings = CustomerServiceManager.GetMasterSettings(false);
            string unit = masterSettings.unit;
            string str2 = context.Request["userver"].ToString();
            string password = context.Request["password"].ToString();
            string str4 = context.Request["nickname"].ToString();
            string str5 = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
            string tokenValue = TokenApi.GetTokenValue(masterSettings.AppId, masterSettings.AppSecret);
            if (!string.IsNullOrEmpty(tokenValue))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("unit", unit);
                parameters.Add("userver", str2);
                parameters.Add("password", str5);
                parameters.Add("nickname", str4);
                parameters.Add("realname", "");
                parameters.Add("level", "");
                parameters.Add("tel", "");
                string msg = string.Empty;
                CustomerServiceInfo customer = new CustomerServiceInfo();
                if (result != 0)
                {
                    string str8 = CustomerApi.UpdateCustomer(tokenValue, parameters);
                    if (!string.IsNullOrWhiteSpace(str8))
                    {
                        string jsonValue = Common.GetJsonValue(str8, "errcode");
                        string str10 = Common.GetJsonValue(str8, "errmsg");
                        if (jsonValue == "0")
                        {
                            customer = CustomerServiceHelper.GetCustomer(result);
                            customer.unit = unit;
                            customer.userver = str2;
                            customer.password = password;
                            customer.nickname = str4;
                            if (CustomerServiceHelper.UpdateCustomer(customer, ref msg))
                            {
                                context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
                            }
                            else
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"" + msg + "\"}");
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"type\":\"error\",\"data\":\"" + str10 + "\"}");
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"修改客服信息失败！\"}");
                    }
                }
                else
                {
                    string str11 = CustomerApi.CreateCustomer(tokenValue, parameters);
                    if (!string.IsNullOrWhiteSpace(str11))
                    {
                        string str12 = Common.GetJsonValue(str11, "errcode");
                        string str13 = Common.GetJsonValue(str11, "errmsg");
                        if (str12 == "0")
                        {
                            customer.unit = unit;
                            customer.userver = str2;
                            customer.password = password;
                            customer.nickname = str4;
                            if (CustomerServiceHelper.CreateCustomer(customer, ref msg) > 0)
                            {
                                context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
                            }
                            else
                            {
                                context.Response.Write("{\"type\":\"error\",\"data\":\"" + msg + "\"}");
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"type\":\"error\",\"data\":\"" + str13 + "\"}");
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"注册客服用户失败！\"}");
                    }
                }
            }
            else
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"获取access_token失败！\"}");
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

