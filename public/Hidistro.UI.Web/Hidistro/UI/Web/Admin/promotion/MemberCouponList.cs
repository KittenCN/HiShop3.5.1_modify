namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MemberCouponList : AdminPage
    {
        protected Button btnSeach;
        protected Grid grdCoupondsList;
        protected PageSize hrefPageSize;
        protected Pager pager1;
        protected HtmlForm thisForm;
        protected TextBox txt_name;
        protected TextBox txt_orderNo;

        protected MemberCouponList() : base("m08", "yxp01")
        {
        }

        private void BindData()
        {
            string str = Globals.RequestQueryStr("cname").Trim();
            string str2 = Globals.RequestQueryStr("cno").Trim();
            if (!string.IsNullOrEmpty(str))
            {
                this.txt_name.Text = str;
            }
            if (!string.IsNullOrEmpty(str2))
            {
                this.txt_orderNo.Text = str2;
            }
            string text = this.txt_name.Text;
            string text2 = this.txt_orderNo.Text;
            int total = 0;
            MemberCouponsSearch search = new MemberCouponsSearch {
                CouponName = str,
                OrderNo = str2,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "CouponId",
                SortOrder = SortAction.Desc
            };
            DataTable memberCoupons = CouponHelper.GetMemberCoupons(search, ref total);
            if ((memberCoupons != null) && (memberCoupons.Rows.Count > 0))
            {
                memberCoupons.Columns.Add("useConditon");
                memberCoupons.Columns.Add("sStatus");
                for (int i = 0; i < memberCoupons.Rows.Count; i++)
                {
                    decimal num3 = decimal.Parse(memberCoupons.Rows[i]["ConditionValue"].ToString());
                    if (num3 == 0M)
                    {
                        memberCoupons.Rows[i]["useConditon"] = "不限制";
                    }
                    else
                    {
                        memberCoupons.Rows[i]["useConditon"] = "满" + num3.ToString("F2") + "可使用";
                    }
                    memberCoupons.Rows[i]["sStatus"] = (int.Parse(memberCoupons.Rows[i]["Status"].ToString()) == 0) ? "已领取" : "已使用";
                }
            }
            this.grdCoupondsList.DataSource = memberCoupons;
            this.grdCoupondsList.DataBind();
            this.pager1.TotalRecords = total;
        }

        protected void btnImagetSearch_Click(object sender, EventArgs e)
        {
            string str = this.txt_name.Text.Trim();
            string str2 = this.txt_orderNo.Text.Trim();
            string str3 = "MemberCouponList.aspx?";
            if (!string.IsNullOrEmpty(str))
            {
                str3 = str3 + "&cname=" + base.Server.UrlEncode(str);
            }
            if (!string.IsNullOrEmpty(str2))
            {
                str3 = str3 + "&cno=" + base.Server.UrlEncode(str2);
            }
            base.Response.Redirect(str3.Trim(new char[] { '?' }));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSeach.Click += new EventHandler(this.btnImagetSearch_Click);
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

