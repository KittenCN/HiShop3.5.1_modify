namespace Hidistro.UI.Web.API
{
    using Hidistro.Core;
    using Hishop.Weixin.MP.Util;
    using System;
    using System.Web;

    public class wx : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            string weixinToken = SettingsManager.GetMasterSettings(false).WeixinToken;
            string signature = request["signature"];
            string nonce = request["nonce"];
            string timestamp = request["timestamp"];
            string s = request["echostr"];
            if (request.HttpMethod == "GET")
            {
                if (CheckSignature.Check(signature, timestamp, nonce, weixinToken))
                {
                    context.Response.Write(s);
                }
                else
                {
                    context.Response.Write("");
                }
                context.Response.End();
            }
            else
            {
                try
                {
                    CustomMsgHandler handler = new CustomMsgHandler(request.InputStream);
                    handler.Execute();
                    Globals.Debuglog(handler.RequestDocument.ToString(), "_Debuglog.txt");
                    context.Response.Write(handler.ResponseDocument);
                }
                catch (Exception exception)
                {
                    Globals.Debuglog(exception.Message, "_Debuglog.txt");
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

