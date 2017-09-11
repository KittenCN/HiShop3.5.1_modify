namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VMyCouponLists : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptActivity;
        private HtmlInputHidden txtShowTabNum;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            PageTitle.AddSiteNameTitle("我的优惠券");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            this.txtShowTabNum = (HtmlInputHidden) this.FindControl("txtShowTabNum");
            this.rptActivity = (VshopTemplatedRepeater) this.FindControl("rptActivity");
            this.txtShowTabNum.Value = "0";
            new PrizesDeliveQuery();
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            MemberCouponsSearch search = new MemberCouponsSearch {
                CouponName = "",
                Status = "0",
                MemberId = Globals.GetCurrentMemberUserId(false),
                IsCount = true,
                PageIndex = num,
                PageSize = num2,
                SortBy = "CouponId",
                SortOrder = SortAction.Desc
            };
            int total = 0;
            DataTable memberCoupons = CouponHelper.GetMemberCoupons(search, ref total);
            if ((memberCoupons != null) && (memberCoupons.Rows.Count > 0))
            {
                memberCoupons.Columns.Add("useConditon");
                memberCoupons.Columns.Add("sStatus");
                for (int i = 0; i < memberCoupons.Rows.Count; i++)
                {
                    decimal num5 = decimal.Parse(memberCoupons.Rows[i]["ConditionValue"].ToString());
                    if (num5 == 0M)
                    {
                        memberCoupons.Rows[i]["useConditon"] = "无消费额限制";
                    }
                    else
                    {
                        memberCoupons.Rows[i]["useConditon"] = "满" + num5.ToString("F2") + "可使用";
                    }
                    memberCoupons.Rows[i]["sStatus"] = (int.Parse(memberCoupons.Rows[i]["Status"].ToString()) == 0) ? "已领用" : "已使用";
                }
            }
            MemberProcessor.GetCurrentMember();
            this.rptActivity.DataSource = memberCoupons;
            this.rptActivity.DataBind();
            this.txtTotal.SetWhenIsNotNull(total.ToString());
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VMyCouponLists.html";
            }
            base.OnInit(e);
        }
    }
}

