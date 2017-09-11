namespace Hidistro.UI.Web.Admin.promotion
{
    using Entities.Promotions;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SendCouponByManager : AdminPage
    {
        protected DropDownList ddlCouponList;
        protected HtmlForm form;
        protected ucDateTimePicker ucDateBeginDate;
        protected ucDateTimePicker ucDateEndDate;

        protected SendCouponByManager() : base("m08", "yxp18")
        {
        }

        private void BindDdlCouponList()
        {
            DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(DateTime.Now, (CouponType)1);
            if (unFinishedCoupon != null)
            {
                foreach (DataRow row in unFinishedCoupon.Rows)
                {
                    ListItem item = new ListItem {
                        Text = row["CouponName"].ToString(),
                        Value = row["CouponId"].ToString()
                    };
                    this.ddlCouponList.Items.Add(item);
                }
            }
        }

        protected string GetMemberCustomGroup()
        {
            StringBuilder builder = new StringBuilder();
            IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
            if ((customGroupingList != null) && (customGroupingList.Count > 0))
            {
                foreach (CustomGroupingInfo info in customGroupingList)
                {
                    builder.Append(" <label class=\"middle mr20\">");
                    builder.AppendFormat("<input type=\"checkbox\" class=\"customGroup\" value=\"{0}\">{1}", info.Id, info.GroupName);
                    builder.Append("  </label>");
                }
            }
            return builder.ToString();
        }

        protected string GetMemberGrande()
        {
            StringBuilder builder = new StringBuilder();
            IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
            if ((memberGrades != null) && (memberGrades.Count > 0))
            {
                foreach (MemberGradeInfo info in memberGrades)
                {
                    builder.Append(" <label class=\"middle mr20\">");
                    builder.AppendFormat("<input type=\"checkbox\" class=\"memberGradeCheck\" value=\"{0}\">{1}", info.GradeId, info.Name);
                    builder.Append("  </label>");
                }
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindDdlCouponList();
            }
        }
    }
}

