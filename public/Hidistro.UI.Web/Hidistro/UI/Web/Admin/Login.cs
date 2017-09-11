namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Login : Page
    {
        protected HtmlForm aspnetForm;
        protected Button btnAdminLogin;
        protected string htmlWebTitle = string.Empty;
        protected SmallStatusMessage lblStatus;
        protected TextBox txtAdminName;
        protected TextBox txtAdminPassWord;
        protected TextBox txtCode;
        private string verifyCodeKey = "VerifyCode";

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            if (!Globals.CheckVerifyCode(this.txtCode.Text.Trim()))
            {
                this.ShowMessage("验证码不正确");
            }
            else
            {
                ManagerInfo manager = ManagerHelper.GetManager(this.txtAdminName.Text);
                if (manager == null)
                {
                    this.ShowMessage("无效的用户信息");
                }
                else if (manager.Password != HiCryptographer.Md5Encrypt(this.txtAdminPassWord.Text))
                {
                    this.ShowMessage("密码不正确");
                }
                else
                {
                    this.WriteCookie(manager);
                    this.Page.Response.Redirect("Default.aspx", true);
                }
            }
        }

        private bool CheckVerifyCode(string verifyCode)
        {
            if (base.Request.Cookies[this.verifyCodeKey] == null)
            {
                return false;
            }
            return (string.Compare(HiCryptographer.Decrypt(base.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0);
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdminLogin.Click += new EventHandler(this.btnAdminLogin_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                string verifyCode = base.Request["code"];
                string str2 = "";
                if (!this.CheckVerifyCode(verifyCode))
                {
                    str2 = "0";
                }
                else
                {
                    str2 = "1";
                }
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{ ");
                base.Response.Write(string.Format("\"flag\":\"{0}\"", str2));
                base.Response.Write("}");
                base.Response.End();
            }
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                this.htmlWebTitle = masterSettings.SiteName;
                Uri urlReferrer = this.Context.Request.UrlReferrer;
                if (urlReferrer != null)
                {
                    this.ReferralLink = urlReferrer.ToString();
                }
                this.txtAdminName.Focus();
            }
        }

        protected void Render1(HtmlTextWriter writer)
        {
            SystemAuthorizationInfo systemAuthorization = SystemAuthorizationHelper.GetSystemAuthorization(true);
            if (systemAuthorization != null)
            {
                switch (systemAuthorization.state)
                {
                    case SystemAuthorizationState.已过授权有效期:
                        writer.Write(SystemAuthorizationHelper.noticeMsg);
                        return;

                    case SystemAuthorizationState.未经官方授权:
                        writer.Write(SystemAuthorizationHelper.licenseMsg);
                        return;
                }
                base.Render(writer);
            }
        }

        private void ShowMessage(string msg)
        {
            this.lblStatus.Text = msg;
            this.lblStatus.Success = false;
            this.lblStatus.Visible = true;
        }

        private void WriteCookie(ManagerInfo userToLogin)
        {
            RoleInfo role = ManagerHelper.GetRole(userToLogin.RoleId);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userToLogin.UserId.ToString(), DateTime.Now, DateTime.Now.AddDays(1.0), true, string.Format("{0}_{1}", role.RoleId, role.IsDefault));
            string str = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(string.Format("{0}{1}", Globals.DomainName, FormsAuthentication.FormsCookieName), str);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private string ReferralLink
        {
            get
            {
                return (this.ViewState["ReferralLink"] as string);
            }
            set
            {
                this.ViewState["ReferralLink"] = value;
            }
        }
    }
}

