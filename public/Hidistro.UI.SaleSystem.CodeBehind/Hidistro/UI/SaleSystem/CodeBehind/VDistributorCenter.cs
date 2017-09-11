namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorCenter : VMemberTemplatedWebControl
    {
        private Literal commissionName1;
        private Literal commissionName2;
        private Literal firstShop;
        private Literal fxCenter;
        private Literal fxExplain;
        private Literal fxTeamName;
        private Image imglogo;
        private Literal litdistirbutors;
        private Literal litMysubFirst;
        private Literal litMysubMember;
        private Literal litMysubSecond;
        private Literal litOrders;
        private Literal litProtuctNum;
        private Literal litReferralBlance;
        private Literal litrGradeName;
        private Literal litStroeName;
        private Literal litTodayOrdersNum;
        private Literal litUserId;
        private Literal litUserId1;
        private Literal litUserId2;
        private Literal litUserId3;
        private Literal litUserId4;
        private Literal myCommission;
        private FormatedMoneyLabel refrraltotal;
        private FormatedMoneyLabel saletotal;
        private Literal secondShop;
        private Literal shopName;
        private HtmlContainerControl UpClassInfo;

        protected override void AttachChildControls()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string distributorCenterName = masterSettings.DistributorCenterName;
            string commissionName = masterSettings.CommissionName;
            this.fxCenter = (Literal) this.FindControl("fxCenter");
            this.fxCenter.Text = distributorCenterName;
            this.commissionName1 = (Literal) this.FindControl("commissionName1");
            this.commissionName2 = (Literal) this.FindControl("commissionName2");
            this.commissionName1.Text = commissionName;
            this.commissionName2.Text = commissionName;
            this.fxTeamName = (Literal) this.FindControl("fxTeamName");
            this.fxTeamName.Text = masterSettings.DistributionTeamName;
            this.shopName = (Literal) this.FindControl("shopName");
            this.shopName.Text = masterSettings.MyShopName;
            this.firstShop = (Literal) this.FindControl("firstShop");
            this.firstShop.Text = masterSettings.FirstShopName;
            this.secondShop = (Literal) this.FindControl("secondShop");
            this.secondShop.Text = masterSettings.SecondShopName;
            this.myCommission = (Literal) this.FindControl("myCommission");
            this.myCommission.Text = masterSettings.MyCommissionName;
            this.fxExplain = (Literal) this.FindControl("fxExplain");
            this.fxExplain.Text = masterSettings.DistributionDescriptionName;
            PageTitle.AddSiteNameTitle(distributorCenterName);
            int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
            if (userIdDistributors == null)
            {
                HttpContext.Current.Response.Redirect("DistributorRegCheck.aspx");
            }
            else if (userIdDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                this.imglogo = (Image) this.FindControl("image");
                if (!string.IsNullOrEmpty(userIdDistributors.Logo))
                {
                    this.imglogo.ImageUrl = userIdDistributors.Logo;
                }
                if (masterSettings.IsShowDistributorSelfStoreName)
                {
                    this.imglogo.Attributes.Add("onclick", "window.location.href = 'DistributorInfo.aspx'");
                }
                this.litStroeName = (Literal) this.FindControl("litStroeName");
                this.litStroeName.Text = userIdDistributors.StoreName;
                this.litrGradeName = (Literal) this.FindControl("litrGradeName");
                DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
                if (distributorGradeInfo != null)
                {
                    this.litrGradeName.Text = distributorGradeInfo.Name;
                }
                this.litReferralBlance = (Literal) this.FindControl("litReferralBlance");
                this.litReferralBlance.Text = userIdDistributors.ReferralBlance.ToString("F2");
                this.litUserId = (Literal) this.FindControl("litUserId");
                this.litUserId1 = (Literal) this.FindControl("litUserId1");
                this.litUserId2 = (Literal) this.FindControl("litUserId2");
                this.litUserId3 = (Literal) this.FindControl("litUserId3");
                this.litUserId4 = (Literal) this.FindControl("litUserId4");
                this.litUserId.Text = userIdDistributors.UserId.ToString();
                this.litUserId1.Text = userIdDistributors.UserId.ToString();
                this.litUserId2.Text = userIdDistributors.UserId.ToString();
                this.litUserId3.Text = userIdDistributors.UserId.ToString();
                this.litUserId4.Text = userIdDistributors.UserId.ToString();
                this.litTodayOrdersNum = (Literal) this.FindControl("litTodayOrdersNum");
                OrderQuery query = new OrderQuery {
                    UserId = new int?(currentMemberUserId),
                    Status = OrderStatus.Today
                };
                this.litTodayOrdersNum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                this.refrraltotal = (FormatedMoneyLabel) this.FindControl("refrraltotal");
                this.refrraltotal.Money = DistributorsBrower.GetUserCommissions(userIdDistributors.UserId, DateTime.Now, null, null, null, "");
                this.saletotal = (FormatedMoneyLabel) this.FindControl("saletotal");
                this.saletotal.Money = userIdDistributors.OrdersTotal;
                this.litMysubMember = (Literal) this.FindControl("litMysubMember");
                this.litMysubFirst = (Literal) this.FindControl("litMysubFirst");
                this.litMysubSecond = (Literal) this.FindControl("litMysubSecond");
                DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(userIdDistributors.UserId);
                if ((distributorsSubStoreNum != null) || (distributorsSubStoreNum.Rows.Count > 0))
                {
                    this.litMysubMember.Text = distributorsSubStoreNum.Rows[0]["memberCount"].ToString();
                    this.litMysubFirst.Text = distributorsSubStoreNum.Rows[0]["firstV"].ToString();
                    this.litMysubSecond.Text = distributorsSubStoreNum.Rows[0]["secondV"].ToString();
                }
                else
                {
                    this.litMysubMember.Text = "0";
                    this.litMysubFirst.Text = "0";
                    this.litMysubSecond.Text = "0";
                }
                this.litProtuctNum = (Literal) this.FindControl("litProtuctNum");
                this.litProtuctNum.Text = ProductBrowser.GetProductsNumber(true).ToString();
                query.Status = OrderStatus.All;
                this.litOrders = (Literal) this.FindControl("litOrders");
                this.litOrders.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                this.UpClassInfo = (HtmlContainerControl) this.FindControl("UpClassInfo");
                IList<DistributorGradeInfo> distributorGradeInfos = VShopHelper.GetDistributorGradeInfos();
                DistributorGradeInfo info3 = null;
                foreach (DistributorGradeInfo info4 in distributorGradeInfos)
                {
                    if (distributorGradeInfo.CommissionsLimit < info4.CommissionsLimit)
                    {
                        if (info3 == null)
                        {
                            info3 = info4;
                        }
                        else if (info3.CommissionsLimit > info4.CommissionsLimit)
                        {
                            info3 = info4;
                        }
                    }
                }
                if (info3 == null)
                {
                    this.UpClassInfo.Visible = false;
                }
                else
                {
                    decimal num2 = (info3.CommissionsLimit - userIdDistributors.ReferralBlance) - userIdDistributors.ReferralRequestBalance;
                    if (num2 < 0M)
                    {
                        num2 = 0.01M;
                    }
                    this.UpClassInfo.InnerHtml = "再获得<span> " + num2.ToString("F2") + " 元</span>" + commissionName + "升级为 <span>" + info3.Name + "</span>";
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorCenter.html";
            }
            base.OnInit(e);
        }
    }
}

