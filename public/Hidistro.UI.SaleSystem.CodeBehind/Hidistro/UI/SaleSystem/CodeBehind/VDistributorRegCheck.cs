namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VDistributorRegCheck : VshopTemplatedWebControl
    {
        protected string IsEnable = "0";
        protected bool IsMemberAmountPass;
        private Literal litAddTips;
        private HtmlInputHidden litExpenditure;
        private HtmlInputHidden litIsEnable;
        private HtmlInputHidden litIsMember;
        private HtmlInputHidden litMemberAmountPass;
        private HtmlInputHidden litminMoney;
        private HtmlInputHidden litProdOK;
        private HtmlInputHidden litProds;
        private HtmlInputHidden UserBindName;

        protected override void AttachChildControls()
        {
            this.litAddTips = (Literal) this.FindControl("litAddTips");
            this.litIsEnable = (HtmlInputHidden) this.FindControl("litIsEnable");
            this.litIsMember = (HtmlInputHidden) this.FindControl("litIsMember");
            this.litExpenditure = (HtmlInputHidden) this.FindControl("litExpenditure");
            this.litMemberAmountPass = (HtmlInputHidden) this.FindControl("litMemberAmountPass");
            this.litminMoney = (HtmlInputHidden) this.FindControl("litminMoney");
            this.litProds = (HtmlInputHidden) this.FindControl("litProds");
            this.litProdOK = (HtmlInputHidden) this.FindControl("litProdOK");
            int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings.DistributorApplicationCondition && (masterSettings.RechargeMoneyToDistributor > 0M))
            {
                if (MemberAmountProcessor.GetUserMaxAmountDetailed(currentMemberUserId) >= masterSettings.RechargeMoneyToDistributor)
                {
                    this.IsMemberAmountPass = true;
                    this.litAddTips.Text = "<li class=\"pl50\"><a href=\"/Vshop/MemberRecharge.aspx\" style=\"color:red\"><i class=\"iconfont icon-tubiaoweb09 mr5 pull-left\"></i>一次性预存" + masterSettings.RechargeMoneyToDistributor.ToString("F2").Replace(".00", "") + "元，即可成为分销商！</a><p class=\"success\">已达成</p></li>";
                }
                else
                {
                    this.litAddTips.Text = "<li class=\"pl50\"><a href=\"/Vshop/MemberRecharge.aspx\" style=\"color:red\"><i class=\"iconfont icon-tubiaoweb09 mr5 pull-left\"></i>一次性预存" + masterSettings.RechargeMoneyToDistributor.ToString("F2").Replace(".00", "") + "元，即可成为分销商！</a></li>";
                }
            }
            if (currentMemberUserId > 0)
            {
                this.litIsMember.Value = "1";
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                if (currentMember == null)
                {
                    this.Page.Response.Redirect("/Vshop/DistributorCenter.aspx");
                    return;
                }
                this.UserBindName = (HtmlInputHidden) this.FindControl("UserBindName");
                this.UserBindName.Value = currentMember.UserBindName;
                decimal expenditure = currentMember.Expenditure;
                if (userIdDistributors != null)
                {
                    if (userIdDistributors.ReferralStatus == 0)
                    {
                        this.IsEnable = "1";
                        this.Context.Response.Redirect("/Vshop/DistributorCenter.aspx");
                        this.Context.Response.End();
                    }
                    else if (userIdDistributors.ReferralStatus == 1)
                    {
                        this.IsEnable = "3";
                    }
                    else if (userIdDistributors.ReferralStatus == 9)
                    {
                        this.IsEnable = "9";
                    }
                }
                else
                {
                    if (VshopBrowser.IsPassAutoToDistributor(currentMember, true) && !SystemAuthorizationHelper.CheckDistributorIsCanAuthorization())
                    {
                        DistributorsBrower.MemberAutoToDistributor(currentMember);
                        this.Page.Response.Redirect("/Vshop/DistributorCenter.aspx", true);
                        this.Page.Response.End();
                        return;
                    }
                    decimal num4 = 0M;
                    DataTable userOrderPaidWaitFinish = OrderHelper.GetUserOrderPaidWaitFinish(currentMemberUserId);
                    decimal total = 0M;
                    OrderInfo orderInfo = null;
                    for (int i = 0; i < userOrderPaidWaitFinish.Rows.Count; i++)
                    {
                        orderInfo = OrderHelper.GetOrderInfo(userOrderPaidWaitFinish.Rows[i]["orderid"].ToString());
                        if (orderInfo != null)
                        {
                            total = orderInfo.GetTotal();
                            if (total > 0M)
                            {
                                num4 += total;
                            }
                        }
                    }
                    expenditure += num4;
                    if (!masterSettings.DistributorApplicationCondition)
                    {
                        if (SystemAuthorizationHelper.CheckDistributorIsCanAuthorization())
                        {
                            this.IsEnable = "2";
                        }
                        else
                        {
                            this.IsEnable = "4";
                        }
                    }
                    else
                    {
                        int finishedOrderMoney = masterSettings.FinishedOrderMoney;
                        this.litminMoney.Value = finishedOrderMoney.ToString();
                        if ((finishedOrderMoney > 0) && (expenditure >= finishedOrderMoney))
                        {
                            if (SystemAuthorizationHelper.CheckDistributorIsCanAuthorization())
                            {
                                this.IsEnable = "2";
                            }
                            else
                            {
                                this.IsEnable = "4";
                            }
                        }
                        if (masterSettings.EnableDistributorApplicationCondition && !string.IsNullOrEmpty(masterSettings.DistributorProductsDate))
                        {
                            if (!string.IsNullOrEmpty(masterSettings.DistributorProducts))
                            {
                                this.litProds.Value = masterSettings.DistributorProducts;
                                if (masterSettings.DistributorProductsDate.Contains("|"))
                                {
                                    DateTime result = new DateTime();
                                    DateTime time2 = new DateTime();
                                    DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[0].ToString(), out result);
                                    DateTime.TryParse(masterSettings.DistributorProductsDate.Split(new char[] { '|' })[1].ToString(), out time2);
                                    if ((result.CompareTo(DateTime.Now) > 0) || (time2.CompareTo(DateTime.Now) < 0))
                                    {
                                        this.litProds.Value = "";
                                        this.litIsEnable.Value = "0";
                                    }
                                    else if (MemberProcessor.CheckMemberIsBuyProds(currentMemberUserId, this.litProds.Value, new DateTime?(result), new DateTime?(time2)))
                                    {
                                        if (SystemAuthorizationHelper.CheckDistributorIsCanAuthorization())
                                        {
                                            this.IsEnable = "2";
                                            this.litProdOK.Value = "(已购买指定商品,在" + time2.ToString("yyyy-MM-dd") + "之前申请有效)";
                                        }
                                        else
                                        {
                                            this.IsEnable = "4";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                this.IsEnable = "6";
                            }
                        }
                    }
                }
                this.litExpenditure.Value = expenditure.ToString("F2");
                if (this.IsMemberAmountPass)
                {
                    this.litMemberAmountPass.Value = "1";
                }
            }
            else
            {
                this.litIsMember.Value = "0";
            }
            this.litIsEnable.Value = this.IsEnable;
            PageTitle.AddSiteNameTitle("申请分销商");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDistributorRegCheck.html";
            }
            base.OnInit(e);
        }
    }
}

