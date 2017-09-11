namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ValidationService : AdminPage
    {
        private string action;
        protected bool enableIsAutoToLogin;
        protected bool enableValidationService;
        private SiteSettings siteSettings;

        protected ValidationService() : base("m06", "wxp07")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
            this.action = Globals.RequestFormStr("action");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str2;
            if (!base.IsPostBack && ((str2 = this.action) != null))
            {
                string str;
                if (!(str2 == "setenable"))
                {
                    if (str2 == "setautologin")
                    {
                        base.Response.Clear();
                        base.Response.ContentType = "application/json";
                        str = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                        try
                        {
                            this.siteSettings.IsAutoToLogin = Globals.RequestFormNum("enable") == 1;
                            if (this.siteSettings.IsAutoToLogin && !this.siteSettings.IsValidationService)
                            {
                                str = "{\"type\":\"0\",\"tips\":\"操作失败，需要先开启微信授权登录！\"}";
                            }
                            else
                            {
                                SettingsManager.Save(this.siteSettings);
                            }
                        }
                        catch
                        {
                            str = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                        }
                        base.Response.Write(str);
                        base.Response.End();
                    }
                }
                else
                {
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    str = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                    try
                    {
                        this.siteSettings.IsValidationService = Globals.RequestFormNum("enable") == 1;
                        if (!this.siteSettings.IsValidationService)
                        {
                            this.siteSettings.IsAutoToLogin = false;
                        }
                        SettingsManager.Save(this.siteSettings);
                    }
                    catch
                    {
                        str = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                    }
                    base.Response.Write(str);
                    base.Response.End();
                }
            }
            this.enableValidationService = this.siteSettings.IsValidationService;
            this.enableIsAutoToLogin = this.siteSettings.IsAutoToLogin;
        }
    }
}

