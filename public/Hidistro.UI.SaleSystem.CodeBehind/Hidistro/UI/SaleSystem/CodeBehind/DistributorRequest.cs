namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class DistributorRequest : VMemberTemplatedWebControl
    {
        private HtmlImage idImg;
        private HtmlInputHidden litIsEnable;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("申请分销");
            this.Page.Session["stylestatus"] = "2";
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMember.UserId);
            if ((userIdDistributors != null) && (userIdDistributors.ReferralStatus == 0))
            {
                this.Page.Response.Redirect("/Vshop/DistributorCenter.aspx", true);
                this.Page.Response.End();
            }
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (VshopBrowser.IsPassAutoToDistributor(currentMember, true))
            {
                DistributorsBrower.MemberAutoToDistributor(currentMember);
                this.Page.Response.Redirect("/Vshop/DistributorCenter.aspx", true);
                this.Page.Response.End();
            }
            else
            {
                if (!VshopBrowser.IsPassAutoToDistributor(currentMember, false))
                {
                    this.Page.Response.Redirect("/Vshop/DistributorRegCheck.aspx", true);
                    this.Page.Response.End();
                }
                if (!masterSettings.IsShowDistributorSelfStoreName)
                {
                    DistributorsBrower.MemberAutoToDistributor(currentMember);
                    this.Page.Response.Redirect("/Vshop/DistributorCenter.aspx", true);
                    this.Page.Response.End();
                }
                else
                {
                    int result = 0;
                    this.idImg = (HtmlImage) this.FindControl("idImg");
                    string logo = string.Empty;
                    if (int.TryParse(this.Page.Request.QueryString["ReferralId"], out result) && (result > 0))
                    {
                        DistributorsInfo info3 = DistributorsBrower.GetUserIdDistributors(result);
                        if ((info3 != null) && !string.IsNullOrEmpty(info3.Logo))
                        {
                            logo = info3.Logo;
                        }
                    }
                    if (string.IsNullOrEmpty(logo))
                    {
                        logo = masterSettings.DistributorLogoPic;
                    }
                    this.idImg.Src = logo;
                    if ((userIdDistributors != null) && (userIdDistributors.ReferralStatus != 0))
                    {
                        this.litIsEnable = (HtmlInputHidden) this.FindControl("litIsEnable");
                        this.litIsEnable.Value = userIdDistributors.ReferralStatus.ToString();
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDistributorRequest.html";
            }
            base.OnInit(e);
        }
    }
}

