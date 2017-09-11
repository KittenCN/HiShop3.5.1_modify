namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;

    public class distributorcenter : AdminPage
    {
        protected string CommissionName;
        protected string DistributionDescriptionName;
        protected string DistributionTeamName;
        protected string DistributorCenterName;
        protected string FirstShopName;
        protected string MyCommissionName;
        protected string MyShopName;
        protected string SecondShopName;
        private SiteSettings siteSettings;

        protected distributorcenter() : base("m05", "fxp14")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                string str2;
                if (((str2 = Globals.RequestFormStr("action")) != null) && ((str2 == "Save") || (str2 == "Huifu")))
                {
                    try
                    {
                        base.Response.ContentType = "text/plain";
                        this.siteSettings.DistributorCenterName = Globals.RequestFormStr("fx0");
                        this.siteSettings.CommissionName = Globals.RequestFormStr("fx1");
                        this.siteSettings.DistributionTeamName = Globals.RequestFormStr("fx2");
                        this.siteSettings.MyShopName = Globals.RequestFormStr("fx3");
                        this.siteSettings.FirstShopName = Globals.RequestFormStr("fx4");
                        this.siteSettings.SecondShopName = Globals.RequestFormStr("fx5");
                        this.siteSettings.MyCommissionName = Globals.RequestFormStr("fx6");
                        this.siteSettings.DistributionDescriptionName = Globals.RequestFormStr("fx7");
                        SettingsManager.Save(this.siteSettings);
                        base.Response.Write("保存成功");
                    }
                    catch (Exception exception)
                    {
                        base.Response.Write("保存失败！（" + exception.ToString() + ")");
                    }
                    base.Response.End();
                }
                else
                {
                    this.DistributorCenterName = this.siteSettings.DistributorCenterName;
                    this.CommissionName = this.siteSettings.CommissionName;
                    this.DistributionTeamName = this.siteSettings.DistributionTeamName;
                    this.MyShopName = this.siteSettings.MyShopName;
                    this.FirstShopName = this.siteSettings.FirstShopName;
                    this.SecondShopName = this.siteSettings.SecondShopName;
                    this.MyCommissionName = this.siteSettings.MyCommissionName;
                    this.DistributionDescriptionName = this.siteSettings.DistributionDescriptionName;
                }
            }
        }
    }
}

