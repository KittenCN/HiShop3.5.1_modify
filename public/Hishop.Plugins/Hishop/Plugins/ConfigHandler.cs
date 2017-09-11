namespace Hishop.Plugins
{
    using System;
    using System.Web;

    public class ConfigHandler : IHttpHandler
    {
        private static void ProcessEmailSender(HttpContext context)
        {
            if (context.Request["action"] == "getlist")
            {
                EmailPlugins plugins = EmailPlugins.Instance();
                context.Response.ContentType = "application/json";
                context.Response.Write(plugins.GetPlugins().ToJsonString());
            }
            else if (context.Request["action"] == "getmetadata")
            {
                context.Response.ContentType = "text/xml";
                EmailSender sender = EmailSender.CreateInstance(context.Request["name"]);
                if (sender == null)
                {
                    context.Response.Write("<xml></xml>");
                }
                else
                {
                    context.Response.Write(sender.GetMetaData().OuterXml);
                }
            }
        }

        private void ProcessOpenId(HttpContext context)
        {
            if (context.Request["action"] == "getlist")
            {
                OpenIdPlugins plugins = OpenIdPlugins.Instance();
                context.Response.ContentType = "application/json";
                context.Response.Write(plugins.GetPlugins().ToJsonString());
            }
            else if (context.Request["action"] == "getmetadata")
            {
                context.Response.ContentType = "text/xml";
                OpenIdService service = OpenIdService.CreateInstance(context.Request["name"]);
                if (service == null)
                {
                    context.Response.Write("<xml></xml>");
                }
                else
                {
                    context.Response.Write(service.GetMetaData().OuterXml);
                }
            }
        }

        private static void ProcessPaymentRequest(HttpContext context)
        {
            if (context.Request["action"] == "getlist")
            {
                PaymentPlugins plugins = PaymentPlugins.Instance();
                context.Response.ContentType = "application/json";
                context.Response.Write(plugins.GetPlugins().ToJsonString());
            }
            else if (context.Request["action"] == "getmetadata")
            {
                context.Response.ContentType = "text/xml";
                PaymentRequest request = PaymentRequest.CreateInstance(context.Request["name"]);
                if (request == null)
                {
                    context.Response.Write("<xml></xml>");
                }
                else
                {
                    context.Response.Write(request.GetMetaData().OuterXml);
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string str = context.Request["type"];
                if (str != null)
                {
                    if (!(str == "PaymentRequest"))
                    {
                        if (str == "OpenIdService")
                        {
                            goto Label_0062;
                        }
                        if (str == "EmailSender")
                        {
                            goto Label_006C;
                        }
                        if (str == "SMSSender")
                        {
                            goto Label_0075;
                        }
                        if (str == "Logistics")
                        {
                        }
                    }
                    else
                    {
                        ProcessPaymentRequest(context);
                    }
                }
                return;
            Label_0062:
                this.ProcessOpenId(context);
                return;
            Label_006C:
                ProcessEmailSender(context);
                return;
            Label_0075:
                ProcessSMSSender(context);
            }
            catch
            {
            }
        }

        private static void ProcessSMSSender(HttpContext context)
        {
            if (context.Request["action"] == "getlist")
            {
                SMSPlugins plugins = SMSPlugins.Instance();
                context.Response.ContentType = "application/json";
                context.Response.Write(plugins.GetPlugins().ToJsonString());
            }
            else if (context.Request["action"] == "getmetadata")
            {
                context.Response.ContentType = "text/xml";
                SMSSender sender = SMSSender.CreateInstance(context.Request["name"]);
                if (sender == null)
                {
                    context.Response.Write("<xml></xml>");
                }
                else
                {
                    context.Response.Write(sender.GetMetaData().OuterXml);
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

