namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;

    [PrivilegeCheck(Privilege.Members)]
    public class MembershipDetails : AdminPage
    {
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected HiImage ListImage1;
        protected HtmlGenericControl OrdersTotal;
        protected HtmlGenericControl ReferralBlance;
        protected HtmlGenericControl ReferralOrders;
        protected HtmlGenericControl TotalReferral;
        protected HtmlGenericControl txtAddress;
        protected HtmlGenericControl txtCellPhone;
        protected HtmlGenericControl txtCreateTime;
        protected HtmlGenericControl txtGrade;
        protected HtmlGenericControl txtMicroName;
        protected HtmlGenericControl txtOpenId;
        protected HtmlGenericControl txtQQ;
        protected HtmlGenericControl txtRealName;
        protected HtmlGenericControl txtRefStoreName;
        protected HtmlGenericControl txtUserName;
        protected int userid;

        protected MembershipDetails() : base("m04", "hyp02")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.ListImage1.ImageUrl = "/Templates/common/images/user.png";
                MemberInfo member = MemberHelper.GetMember(this.userid);
                if (member == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(member.UserHead))
                    {
                        this.ListImage1.ImageUrl = member.UserHead;
                    }
                    this.txtUserName.InnerText = member.UserName;
                    MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(member.GradeId);
                    if (memberGrade != null)
                    {
                        this.txtGrade.InnerText = memberGrade.Name;
                    }
                    this.txtCellPhone.InnerText = member.CellPhone;
                    this.txtCreateTime.InnerText = member.CreateDate.ToString();
                    this.txtMicroName.InnerText = member.UserName;
                    this.txtRealName.InnerText = member.RealName;
                    if (member.ReferralUserId <= 0)
                    {
                        this.txtRefStoreName.InnerText = "主站";
                    }
                    else
                    {
                        DistributorsInfo userIdDistributors = VShopHelper.GetUserIdDistributors(member.ReferralUserId);
                        if (userIdDistributors != null)
                        {
                            this.txtRefStoreName.InnerText = userIdDistributors.StoreName;
                        }
                    }
                    this.txtAddress.InnerText = RegionHelper.GetFullRegion(member.RegionId, "") + member.Address;
                    this.txtQQ.InnerText = member.QQ;
                    this.txtOpenId.InnerText = member.OpenId;
                    this.TotalReferral.InnerText = member.AvailableAmount.ToString("F2");
                    Dictionary<string, decimal> dataByUserId = MemberAmountProcessor.GetDataByUserId(this.userid);
                    this.ReferralOrders.InnerText = dataByUserId["OrderCount"].ToString();
                    this.OrdersTotal.InnerText = dataByUserId["OrderTotal"].ToString("F2");
                    this.ReferralBlance.InnerText = dataByUserId["RequestAmount"].ToString("F2");
                }
            }
        }
    }
}

