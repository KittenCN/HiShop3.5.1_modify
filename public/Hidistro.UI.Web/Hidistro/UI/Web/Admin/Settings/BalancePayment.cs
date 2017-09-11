namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;

    public class BalancePayment : AdminPage
    {
        protected bool _EnabelBalanceWithdrawal;
        protected bool _EnableBalancePayment;
        private SiteSettings siteSettings;
        protected HtmlForm thisForm;

        protected BalancePayment() : base("m09", "szp15")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                switch (Globals.RequestFormStr("type"))
                {
                    case "EnableBalancePayment":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            bool flag = bool.Parse(Globals.RequestFormStr("enable"));
                            this.siteSettings.EnableBalancePayment = flag;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception)
                        {
                            base.Response.Write("保存失败！（" + exception.ToString() + ")");
                        }
                        base.Response.End();
                        return;

                    case "EnabelBalanceWithdrawal":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            bool flag2 = bool.Parse(Globals.RequestFormStr("enable"));
                            this.siteSettings.EnabelBalanceWithdrawal = flag2;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception2)
                        {
                            base.Response.Write("保存失败！（" + exception2.ToString() + ")");
                        }
                        base.Response.End();
                        return;
                }
                this._EnableBalancePayment = this.siteSettings.EnableBalancePayment;
                this._EnabelBalanceWithdrawal = this.siteSettings.EnabelBalanceWithdrawal;
            }
        }
    }
}

