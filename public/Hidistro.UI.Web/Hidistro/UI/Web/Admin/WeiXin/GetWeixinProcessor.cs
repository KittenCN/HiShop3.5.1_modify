namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Members;
    using Newtonsoft.Json;
    using System;
    using System.Web;

    public class GetWeixinProcessor : IHttpHandler
    {
        private void GetCanChangeBind(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                int num = MemberHelper.CanChangeBindWeixin();
                context.Response.Write(JsonConvert.SerializeObject(new { status = num }));
            }
            catch (Exception)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { status = 4, msg = "程序出错了" }));
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string str2;
            string str = context.Request["action"];
            if (((str2 = str.ToLower()) != null) && (str2 == "getcanchangebind"))
            {
                this.GetCanChangeBind(context);
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

