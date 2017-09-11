using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;

namespace Hidistro.UI.Web.Admin.AliFuwu
{
    [PrivilegeCheck(Privilege.ProductCategory)]
    public partial class ConcernUrl : AdminPage
    {
        private string action;
        protected bool enableGuidePageSet;
        private SiteSettings siteSettings;
        protected ConcernUrl() : base("m11", "fwp04")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
            this.action = Globals.RequestFormStr("action");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.siteSettings.AliPayFuwuGuidePageSet = this.txtGuidePageSet.Text.Trim();
            SettingsManager.Save(this.siteSettings);
            this.ShowMsg("修改成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                if (this.action == "setenable")
                {
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                    this.siteSettings.EnableAliPayFuwuGuidePageSet = Globals.RequestFormNum("enable") == 1;
                    SettingsManager.Save(this.siteSettings);
                    base.Response.Write(s);
                    base.Response.End();
                }
                else if (this.siteSettings.AliPayFuwuGuidePageSet.Length > 10)
                {
                    this.txtGuidePageSet.Text = this.siteSettings.AliPayFuwuGuidePageSet;
                }
            }
            this.enableGuidePageSet = this.siteSettings.EnableAliPayFuwuGuidePageSet;
        }
    }
}