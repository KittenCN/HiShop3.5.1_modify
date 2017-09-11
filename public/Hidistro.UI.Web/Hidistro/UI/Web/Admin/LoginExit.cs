namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using System;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;

    public class LoginExit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(string.Format("{0}{1}", Globals.DomainName, FormsAuthentication.FormsCookieName));
            cookie.Expires = DateTime.Now;
            HttpContext.Current.Response.Cookies.Add(cookie);
            base.Response.Redirect("Login.aspx", true);
        }
    }
}

