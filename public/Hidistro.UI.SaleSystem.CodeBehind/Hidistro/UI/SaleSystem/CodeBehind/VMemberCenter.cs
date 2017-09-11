namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberCenter : VMemberTemplatedWebControl
    {
        private Literal fxCenter;
        private Image image;
        private HtmlInputHidden IsSign;
        private Literal litAmount;
        private Literal litBindUser;
        private Literal litCoupon;
        private Literal litExpenditure;
        private Literal litPoints;
        private Literal litrGradeName;
        private Literal litUserName;
        private HtmlInputHidden txtShowDis;
        private HtmlInputHidden txtWaitForstr;
        private HtmlContainerControl UpClassInfo;
        private Literal usePoints;
        private HtmlInputHidden UserBindName;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("会员中心");
            MemberInfo currentMemberInfo = base.CurrentMemberInfo;
            if (currentMemberInfo == null)
            {
                this.Page.Response.Redirect("/logout.aspx");
            }
            else
            {
                int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
                this.UserBindName = (HtmlInputHidden) this.FindControl("UserBindName");
                this.UserBindName.Value = currentMemberInfo.UserBindName;
                this.UpClassInfo = (HtmlContainerControl) this.FindControl("UpClassInfo");
                this.litUserName = (Literal) this.FindControl("litUserName");
                this.litPoints = (Literal) this.FindControl("litPoints");
                this.litPoints.Text = currentMemberInfo.Points.ToString();
                this.image = (Image) this.FindControl("image");
                this.usePoints = (Literal) this.FindControl("usePoints");
                this.usePoints.Text = currentMemberInfo.Points.ToString();
                this.litAmount = (Literal) this.FindControl("litAmount");
                this.litAmount.Text = Math.Round(currentMemberInfo.AvailableAmount, 2).ToString();
                MemberCouponsSearch search = new MemberCouponsSearch {
                    CouponName = "",
                    Status = "0",
                    MemberId = currentMemberUserId,
                    IsCount = true,
                    PageIndex = 1,
                    PageSize = 10,
                    SortBy = "CouponId",
                    SortOrder = SortAction.Desc
                };
                int total = 0;
                CouponHelper.GetMemberCoupons(search, ref total);
                this.litCoupon = (Literal) this.FindControl("litCoupon");
                this.litCoupon.Text = total.ToString();
                this.litBindUser = (Literal) this.FindControl("litBindUser");
                this.litExpenditure = (Literal) this.FindControl("litExpenditure");
                this.litExpenditure.SetWhenIsNotNull("￥" + currentMemberInfo.Expenditure.ToString("F2"));
                if (!string.IsNullOrEmpty(currentMemberInfo.UserBindName))
                {
                    this.litBindUser.Text = " style=\"display:none\"";
                }
                MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMemberInfo.GradeId);
                this.litrGradeName = (Literal) this.FindControl("litrGradeName");
                if (memberGrade != null)
                {
                    this.litrGradeName.Text = memberGrade.Name;
                }
                else
                {
                    this.litrGradeName.Text = "普通会员";
                }
                this.litUserName.Text = string.IsNullOrEmpty(currentMemberInfo.OpenId) ? (string.IsNullOrEmpty(currentMemberInfo.RealName) ? currentMemberInfo.UserName : currentMemberInfo.RealName) : currentMemberInfo.UserName;
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                this.fxCenter = (Literal) this.FindControl("fxCenter");
                this.fxCenter.Text = masterSettings.DistributorCenterName;
                this.IsSign = (HtmlInputHidden) this.FindControl("IsSign");
                if (!masterSettings.sign_score_Enable)
                {
                    this.IsSign.Value = "-1";
                }
                else if (!UserSignHelper.IsSign(currentMemberInfo.UserId))
                {
                    this.IsSign.Value = "1";
                }
                if (!string.IsNullOrEmpty(currentMemberInfo.UserHead))
                {
                    this.image.ImageUrl = currentMemberInfo.UserHead;
                }
                this.txtWaitForstr = (HtmlInputHidden) this.FindControl("txtWaitForstr");
                OrderQuery query = new OrderQuery {
                    Status = OrderStatus.WaitBuyerPay
                };
                int userOrderCount = MemberProcessor.GetUserOrderCount(currentMemberUserId, query);
                query.Status = OrderStatus.SellerAlreadySent;
                int num4 = MemberProcessor.GetUserOrderCount(currentMemberUserId, query);
                query.Status = OrderStatus.BuyerAlreadyPaid;
                int num5 = MemberProcessor.GetUserOrderCount(currentMemberUserId, query);
                int waitCommentByUserID = ProductBrowser.GetWaitCommentByUserID(currentMemberUserId);
                int userOrderReturnCount = MemberProcessor.GetUserOrderReturnCount(currentMemberUserId);
                this.txtWaitForstr.Value = userOrderCount.ToString() + "|" + num5.ToString() + "|" + num4.ToString() + "|" + waitCommentByUserID.ToString() + "|" + userOrderReturnCount.ToString();
                DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
                this.txtShowDis = (HtmlInputHidden) this.FindControl("txtShowDis");
                if ((userIdDistributors == null) || (userIdDistributors.ReferralStatus != 0))
                {
                    this.txtShowDis.Value = "false";
                }
                else
                {
                    this.txtShowDis.Value = "true";
                }
                IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
                MemberGradeInfo info4 = null;
                foreach (MemberGradeInfo info5 in memberGrades)
                {
                    double? tranVol = memberGrade.TranVol;
                    double? nullable2 = info5.TranVol;
                    if ((tranVol.GetValueOrDefault() >= nullable2.GetValueOrDefault()) && (tranVol.HasValue & nullable2.HasValue))
                    {
                        int? tranTimes = memberGrade.TranTimes;
                        int? nullable4 = info5.TranTimes;
                        if ((tranTimes.GetValueOrDefault() >= nullable4.GetValueOrDefault()) && (tranTimes.HasValue & nullable4.HasValue))
                        {
                            continue;
                        }
                    }
                    double? nullable5 = memberGrade.TranVol;
                    double? nullable6 = info5.TranVol;
                    if ((nullable5.GetValueOrDefault() >= nullable6.GetValueOrDefault()) || !(nullable5.HasValue & nullable6.HasValue))
                    {
                        int? nullable7 = memberGrade.TranTimes;
                        int? nullable8 = info5.TranTimes;
                        if ((nullable7.GetValueOrDefault() >= nullable8.GetValueOrDefault()) || !(nullable7.HasValue & nullable8.HasValue))
                        {
                            continue;
                        }
                    }
                    if (info4 == null)
                    {
                        info4 = info5;
                    }
                    else
                    {
                        double? nullable9 = info4.TranVol;
                        double? nullable10 = info5.TranVol;
                        if ((nullable9.GetValueOrDefault() <= nullable10.GetValueOrDefault()) || !(nullable9.HasValue & nullable10.HasValue))
                        {
                            int? nullable11 = info4.TranTimes;
                            int? nullable12 = info5.TranTimes;
                            if ((nullable11.GetValueOrDefault() <= nullable12.GetValueOrDefault()) || !(nullable11.HasValue & nullable12.HasValue))
                            {
                                continue;
                            }
                        }
                        info4 = info5;
                    }
                }
                if (info4 == null)
                {
                    this.UpClassInfo.Visible = false;
                }
                else
                {
                    int num8 = 0;
                    if (info4.TranTimes.HasValue)
                    {
                        num8 = info4.TranTimes.Value - currentMemberInfo.OrderNumber;
                    }
                    if (num8 <= 0)
                    {
                        num8 = 1;
                    }
                    decimal num9 = 0M;
                    if (info4.TranVol.HasValue)
                    {
                        num9 = ((decimal) info4.TranVol.Value) - currentMemberInfo.Expenditure;
                    }
                    if (num9 <= 0M)
                    {
                        num9 = 0.01M;
                    }
                    this.UpClassInfo.InnerHtml = "再交易<span>" + num8.ToString() + "次 </span>或消费<span> " + Math.Round((decimal) (num9 + 0.49M), 0).ToString() + "元 </span>升级";
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberCenter.html";
            }
            base.OnInit(e);
        }
    }
}

