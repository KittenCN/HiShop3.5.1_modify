namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class GuideConcern : AdminPage
    {
        private string action;
        protected int concernradio;
        protected bool EnableGuidePageSet;
        protected bool IsAutoGuide;
        protected bool isMustcheckbox;
        private SiteSettings siteSettings;
        protected HtmlForm thisForm;
        protected TextBox txtConcernMsg;
        protected TextBox txtGuidePageSet;

        protected GuideConcern() : base("m06", "wxp02")
        {
            this.concernradio = 1;
            this.siteSettings = SettingsManager.GetMasterSettings(false);
            this.action = Globals.RequestFormStr("action");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                switch (this.action)
                {
                    case "MustGuideConcern":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            bool flag = bool.Parse(Globals.RequestFormStr("enable"));
                            this.siteSettings.IsMustConcern = flag;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception)
                        {
                            base.Response.Write("保存失败！（" + exception.ToString() + ")");
                        }
                        base.Response.End();
                        return;

                    case "EnabeGuidePage":
                        try
                        {
                            base.Response.Clear();
                            base.Response.ContentType = "text/plain";
                            bool flag2 = bool.Parse(Globals.RequestFormStr("enable"));
                            if (!flag2)
                            {
                                this.siteSettings.IsAutoGuide = false;
                            }
                            this.siteSettings.EnableGuidePageSet = flag2;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception2)
                        {
                            base.Response.Write("保存失败！（" + exception2.ToString() + ")");
                        }
                        base.Response.End();
                        return;

                    case "EnableAutoGuide":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            bool flag3 = bool.Parse(Globals.RequestFormStr("enable"));
                            this.siteSettings.IsAutoGuide = flag3;
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception3)
                        {
                            base.Response.Write("保存失败！（" + exception3.ToString() + ")");
                        }
                        base.Response.End();
                        return;

                    case "ConcernType":
                        try
                        {
                            base.Response.ContentType = "text/plain";
                            string str = Globals.RequestFormStr("txt1");
                            string str2 = Globals.RequestFormStr("txt2");
                            int num = Globals.RequestFormNum("concernType");
                            this.siteSettings.GuideConcernType = num;
                            if (num == 0)
                            {
                                this.siteSettings.ConcernMsg = str;
                            }
                            else
                            {
                                this.siteSettings.GuidePageSet = str2;
                            }
                            SettingsManager.Save(this.siteSettings);
                            base.Response.Write("保存成功");
                        }
                        catch (Exception exception4)
                        {
                            base.Response.Write("保存失败！（" + exception4.ToString() + ")");
                        }
                        base.Response.End();
                        return;
                }
                this.EnableGuidePageSet = this.siteSettings.EnableGuidePageSet;
                this.IsAutoGuide = this.siteSettings.IsAutoGuide;
                this.txtConcernMsg.Text = this.siteSettings.ConcernMsg;
                this.txtGuidePageSet.Text = this.siteSettings.GuidePageSet;
                this.concernradio = this.siteSettings.GuideConcernType;
                this.isMustcheckbox = this.siteSettings.IsMustConcern;
            }
        }
    }
}

