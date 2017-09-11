namespace Hidistro.UI.Web
{
    using Hidistro.Core;
    using Hidistro.Jobs;
    using Hishop.AlipayFuwu.Api.Model;
    using System;
    using System.Configuration;
    using System.Net;
    using System.Threading;
    using System.Web;
    using System.Web.Routing;

    public class Global : HttpApplication
    {
        private static string strUrl = "";

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                string str = "ASPSESSID";
                string str2 = "ASP.NET_SESSIONID";
                if (HttpContext.Current.Request.Form[str] != null)
                {
                    this.UpdateCookie(str2, HttpContext.Current.Request.Form[str]);
                }
                else if (HttpContext.Current.Request.QueryString[str] != null)
                {
                    this.UpdateCookie(str2, HttpContext.Current.Request.QueryString[str]);
                }
            }
            catch (Exception)
            {
                base.Response.StatusCode = 500;
                base.Response.Write("Error Initializing Session");
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["Installer"] == null)
            {
                try
                {
                    JobsHelp.stop();
                    if (string.IsNullOrEmpty(strUrl))
                    {
                        Thread.Sleep(0x3e8);
                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(strUrl);
                        using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                        {
                            using (response.GetResponseStream())
                            {
                            }
                        }
                    }
                }
                catch
                {
                    Globals.Debuglog("重启动Application_start失败！", "_Debuglog.txt");
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            string AppPath;
            bool result;
            this.RegisterRouters(RouteTable.Routes);
            if (ConfigurationManager.AppSettings["Installer"] != null)
            {
                Globals.Debuglog("到这里了，！网站未安装！", "_Debuglog.txt");
            }
            else
            {
                AlipayFuwuConfig.CommSetConfig(SettingsManager.GetMasterSettings(false).AlipayAppid, base.Server.MapPath("~/"), "GBK");
                AlipayFuwuConfig.SetWriteLog(true);
                if (string.IsNullOrEmpty(strUrl))
                {
                    string str = HttpContext.Current.Request.Url.Port.ToString();
                    strUrl = string.Format("http://{0}/UserLogin.aspx", HttpContext.Current.Request.Url.Host + ((str == "80") ? "" : (":" + str)));
                }
                JobsHelp.start(base.Server.MapPath("/config/JobConfig.xml"));
                AppPath = base.Server.MapPath("/");
                new Thread(() => new AsyncWorkDelegate_TongJi().CalData(AppPath, out result)).Start();
            }
        }

        private void RegisterRouters(RouteCollection routes)
        {
            routes.MapPageRoute("custom", "custom/{custpath}", "~/custom.aspx");
            routes.MapPageRoute("draftcustom", "draftcustom/{custpath}", "~/DraftCustom.aspx");
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        private void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookie_name);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
    }
}

