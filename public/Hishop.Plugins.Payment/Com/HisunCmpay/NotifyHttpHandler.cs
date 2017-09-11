namespace Com.HisunCmpay
{
    using System;
    using System.Collections.Specialized;
    using System.Web;

    public class NotifyHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            NameValueCollection form = context.Request.Form;
            try
            {
                HiOrderNotifyRes res = new HiOrderNotifyRes(form);
                if ("SUCCESS".Equals(res.Status))
                {
                    context.Response.Write("SUCCESS");
                }
            }
            catch (Exception)
            {
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}

