namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ControlPanel.Promotions;
    using global::ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VGetRedShare : VshopTemplatedWebControl
    {
        private Literal litItemParams;

        protected override void AttachChildControls()
        {
            this.litItemParams = (Literal) this.FindControl("litItemParams");
            string str = HttpContext.Current.Request.QueryString.Get("orderid");
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (!string.IsNullOrEmpty(str) && (currentMember != null))
            {
                DataTable orderRedPager = ShareActHelper.GetOrderRedPager(str, currentMember.UserId);
                if ((orderRedPager != null) && (orderRedPager.Rows.Count > 0))
                {
                    DataView defaultView = orderRedPager.DefaultView;
                    if (defaultView.Count > 0)
                    {
                        ShareActivityInfo act = ShareActHelper.GetAct(Convert.ToInt32(defaultView[0]["RedPagerActivityId"]));
                        if (act != null)
                        {
                            string shareTitle = act.ShareTitle;
                            string description = act.Description;
                            if (shareTitle.Contains("{{店铺名称}}") || description.Contains("{{店铺名称}}"))
                            {
                                HttpCookie cookie = new HttpCookie("Vshop-ReferralId");
                                if ((cookie != null) && (cookie.Value != null))
                                {
                                    DistributorsInfo userIdDistributors = new DistributorsInfo();
                                    userIdDistributors = DistributorsBrower.GetUserIdDistributors(int.Parse(cookie.Value));
                                    description = description.Replace("{{店铺名称}}", userIdDistributors.StoreName);
                                    shareTitle = shareTitle.Replace("{{店铺名称}}", userIdDistributors.StoreName);
                                }
                                else
                                {
                                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                                    description = description.Replace("{{店铺名称}}", masterSettings.SiteName);
                                    shareTitle = shareTitle.Replace("{{店铺名称}}", masterSettings.SiteName);
                                }
                            }
                            if (shareTitle.Contains("{{微信昵称}}"))
                            {
                                shareTitle = shareTitle.Replace("{{微信昵称}}", currentMember.UserName);
                            }
                            if (description.Contains("{{微信昵称}}"))
                            {
                                description = description.Replace("{{微信昵称}}", currentMember.UserName);
                            }
                            string webUrlStart = Globals.GetWebUrlStart();
                            this.litItemParams.Text = string.Concat(new object[] { webUrlStart, act.ImgUrl, "|", shareTitle.Replace("|", "｜"), "|", Globals.GetWebUrlStart(), "/Vshop/getredpager.aspx?id=", defaultView[0]["RedPagerActivityId"], "&userid=", currentMember.UserId, "&ReferralId=", Globals.GetCurrentDistributorId(), "|", description.Replace("|", "｜") });
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("/vshop/MemberCenter.aspx?t=1");
                    }
                }
                else
                {
                    orderRedPager = ShareActHelper.GetOrderRedPager(str, -100);
                    if (orderRedPager.Rows.Count > 0)
                    {
                        HttpContext.Current.Response.Redirect(string.Concat(new object[] { "/Vshop/getredpager.aspx?id=", orderRedPager.Rows[0]["RedPagerActivityId"].ToString(), "&userid=", currentMember.UserId, "&ReferralId=", Globals.GetCurrentDistributorId() }));
                        HttpContext.Current.Response.End();
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("/vshop/MemberCenter.aspx?t=2");
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/default.aspx");
                HttpContext.Current.Response.End();
            }
            PageTitle.AddSiteNameTitle("分享助力");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VGetRedShare.html";
            }
            base.OnInit(e);
        }
    }
}

