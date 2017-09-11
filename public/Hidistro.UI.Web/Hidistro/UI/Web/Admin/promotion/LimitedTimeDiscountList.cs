namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class LimitedTimeDiscountList : AdminPage
    {
        protected Button btnSeach;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected Grid grdLimitedTimeDiscountList;
        protected PageSize hrefPageSize;
        protected Label lblAll;
        protected Label lblEnd;
        protected Label lblIn;
        protected Label lblUnBegin;
        protected Pager pager1;
        protected int status;
        protected HtmlForm thisForm;
        protected TextBox txtActivityName;

        protected LimitedTimeDiscountList() : base("m08", "yxp24")
        {
        }

        private void BindData()
        {
            string text = this.txtActivityName.Text;
            DateTime? selectedDate = this.calendarStartDate.SelectedDate;
            DateTime? nullable2 = this.calendarEndDate.SelectedDate;
            ActivitySearch query = new ActivitySearch {
                status = (ActivityStatus) this.status,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "LimitedTimeDiscountId",
                SortOrder = SortAction.Desc,
                Name = text,
                begin = selectedDate,
                end = nullable2
            };
            DbQueryResult discountQuery = LimitedTimeDiscountHelper.GetDiscountQuery(query);
            DataTable data = (DataTable) discountQuery.Data;
            this.grdLimitedTimeDiscountList.DataSource = data;
            this.grdLimitedTimeDiscountList.DataBind();
            this.pager1.TotalRecords = discountQuery.TotalRecords;
            this.CountTotal();
        }

        protected void btnSeach_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void CountTotal()
        {
            ActivitySearch query = new ActivitySearch {
                status = ActivityStatus.All,
                IsCount = true,
                PageIndex = this.pager1.PageIndex,
                PageSize = this.pager1.PageSize,
                SortBy = "LimitedTimeDiscountId",
                SortOrder = SortAction.Desc
            };
            DataTable data = (DataTable) LimitedTimeDiscountHelper.GetDiscountQuery(query).Data;
            this.lblAll.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ActivityStatus.In;
            data = (DataTable) LimitedTimeDiscountHelper.GetDiscountQuery(query).Data;
            this.lblIn.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ActivityStatus.End;
            data = (DataTable) LimitedTimeDiscountHelper.GetDiscountQuery(query).Data;
            this.lblEnd.Text = (data != null) ? data.Rows.Count.ToString() : "0";
            query.status = ActivityStatus.unBegin;
            data = (DataTable) LimitedTimeDiscountHelper.GetDiscountQuery(query).Data;
            this.lblUnBegin.Text = (data != null) ? data.Rows.Count.ToString() : "0";
        }

        protected string GetDescription(string description)
        {
            if (description.Length > 8)
            {
                return (description.Substring(0, 8) + "..");
            }
            return description;
        }

        public string GetMemberGarde(string applyMembers, string defualtGroup, string customGroup)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(applyMembers))
            {
                IList<MemberGradeInfo> memberGrades = null;
                if (applyMembers == "0")
                {
                    memberGrades = MemberHelper.GetMemberGrades();
                }
                else
                {
                    memberGrades = MemberHelper.GetMemberGrades(applyMembers);
                }
                if ((memberGrades != null) && (memberGrades.Count > 0))
                {
                    foreach (MemberGradeInfo info in memberGrades)
                    {
                        builder.Append(info.Name + ",");
                    }
                }
            }
            if (!string.IsNullOrEmpty(defualtGroup) && (defualtGroup != "-1"))
            {
                if (defualtGroup == "0")
                {
                    builder.Append("新会员,活跃会员,沉睡会员,");
                }
                else
                {
                    if (defualtGroup.IndexOf('1') > -1)
                    {
                        builder.Append("新会员,");
                    }
                    if (defualtGroup.IndexOf('2') > -1)
                    {
                        builder.Append("活跃会员,");
                    }
                    if (defualtGroup.IndexOf('3') > -1)
                    {
                        builder.Append("沉睡会员,");
                    }
                }
            }
            if (!string.IsNullOrEmpty(customGroup))
            {
                IList<CustomGroupingInfo> customGroupingList = null;
                if (customGroup == "0")
                {
                    customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
                }
                else if (customGroup != "-1")
                {
                    customGroupingList = CustomGroupingHelper.GetCustomGroupingList(customGroup);
                }
                if ((customGroupingList != null) && (customGroupingList.Count > 0))
                {
                    foreach (CustomGroupingInfo info2 in customGroupingList)
                    {
                        builder.Append(info2.GroupName + ",");
                    }
                }
            }
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                return Globals.SubStr(builder.ToString().Substring(0, builder.Length - 1), 60, "...");
            }
            return "";
        }

        protected string GetStatusHtml(string status)
        {
            if (status == "3")
            {
                return "恢复活动";
            }
            return "暂停活动";
        }

        private void grdLimitedTimeDiscountList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int) this.grdLimitedTimeDiscountList.DataKeys[e.RowIndex].Value;
            if (!LimitedTimeDiscountHelper.UpdateDiscountStatus(id, DiscountStatus.Delete))
            {
                this.ShowMsg("未知错误", false);
            }
            else
            {
                this.BindData();
                this.ShowMsg("成功删除了选择的活动", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdLimitedTimeDiscountList.RowDeleting += new GridViewDeleteEventHandler(this.grdLimitedTimeDiscountList_RowDeleting);
            if (base.Request.Params.AllKeys.Contains<string>("status"))
            {
                if (!base.Request["status"].ToString().bInt(ref this.status))
                {
                    this.status = 0;
                }
            }
            else
            {
                this.status = 0;
            }
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

