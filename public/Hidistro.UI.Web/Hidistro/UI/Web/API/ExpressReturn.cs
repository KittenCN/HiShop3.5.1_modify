namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel;
    using Hidistro.Entities.Sales;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Web;

    public class ExpressReturn : IHttpHandler
    {
        private HttpContext context;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string str = context.Request["action"];
                if (string.IsNullOrEmpty(str))
                {
                    context.Response.Write("参数错误");
                }
                else
                {
                    string str2;
                    this.context = context;
                    if (((str2 = str) != null) && (str2 == "SaveExpressData"))
                    {
                        this.SaveExpressData();
                    }
                }
            }
            catch (Exception exception)
            {
                context.Response.Write(exception.Message.ToString());
            }
        }

        private void SaveExpressData()
        {
            string str = this.context.Request["param"];
            if (string.IsNullOrEmpty(str))
            {
                this.context.Response.Write("{\"result\":\"false\",\"returnCode\":\"500\",\"message\":\"服务器错误\"}");
            }
            else
            {
                try
                {
                    JObject obj2 = JObject.Parse(str);
                    ExpressDataInfo model = new ExpressDataInfo {
                        ExpressNumber = obj2["lastResult"]["nu"].ToString(),
                        CompanyCode = obj2["lastResult"]["com"].ToString(),
                        DataContent = str
                    };
                    if (new ExpressDataHelper().AddExpressData(model))
                    {
                        this.context.Response.Write("{\"result\":\"true\",\"returnCode\":\"200\",\"message\":\"成功\"}");
                    }
                    else
                    {
                        this.context.Response.Write("{\"result\":\"false\",\"returnCode\":\"500\",\"message\":\"服务器错误\"}");
                    }
                }
                catch (Exception)
                {
                    this.context.Response.Write("{\"result\":\"false\",\"returnCode\":\"500\",\"message\":\"服务器错误\"}");
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

