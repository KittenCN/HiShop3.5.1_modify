namespace Hidistro.UI.Web.Admin.member
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Members)]
    public class MembersIntegralDetail : AdminPage
    {
        protected Button btnSearchButton;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        public string clientType;
        protected DropDownList drIntegralStatus;
        private DateTime? endDate;
        protected Grid grdMemberList;
        protected PageSize hrefPageSize;
        public int IntegralStatus;
        protected Pager pager;
        protected Pager pager1;
        protected Script Script4;
        protected Script Script5;
        protected Script Script6;
        private DateTime? startDate;
        protected HtmlForm thisForm;
        private int userId;
        public string ValidSmsNum;

        public MembersIntegralDetail() : base("m04", "hyp10")
        {
            this.IntegralStatus = -1;
            this.ValidSmsNum = "0";
        }

        private void BindConsultation()
        {
            IntegralDetailQuery query = new IntegralDetailQuery {
                PageIndex = this.pager.PageIndex,
                UserId = this.userId,
                SortBy = this.grdMemberList.SortOrderBy,
                PageSize = this.pager.PageSize,
                IntegralStatus = this.IntegralStatus,
                StartTime = this.startDate,
                EndTime = this.endDate
            };
            if (this.grdMemberList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult integralDetail = MemberHelper.GetIntegralDetail(query);
            this.grdMemberList.DataSource = integralDetail.Data;
            this.grdMemberList.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = integralDetail.TotalRecords;
        }

        protected void BindData()
        {
            if (!this.Page.IsPostBack)
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["userId"], out num))
                {
                    this.userId = num;
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
                {
                    this.startDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["startDate"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
                {
                    this.endDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["endDate"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IntegralStatus"]))
                {
                    this.IntegralStatus = int.Parse(this.Page.Request.QueryString["IntegralStatus"]);
                }
                this.calendarStartDate.SelectedDate = this.startDate;
                this.calendarEndDate.SelectedDate = this.endDate;
                this.drIntegralStatus.SelectedValue = this.IntegralStatus.ToString();
                IntegralDetailQuery query = new IntegralDetailQuery {
                    PageIndex = this.pager.PageIndex,
                    UserId = this.userId,
                    SortBy = this.grdMemberList.SortOrderBy,
                    PageSize = this.pager.PageSize,
                    IntegralStatus = this.IntegralStatus,
                    StartTime = this.startDate,
                    EndTime = this.endDate
                };
                if (this.grdMemberList.SortOrder.ToLower() == "desc")
                {
                    query.SortOrder = SortAction.Desc;
                }
                DbQueryResult integralDetail = MemberHelper.GetIntegralDetail(query);
                this.grdMemberList.DataSource = integralDetail.Data;
                this.grdMemberList.DataBind();
                this.pager1.TotalRecords = this.pager.TotalRecords = integralDetail.TotalRecords;
            }
            else
            {
                this.startDate = this.calendarStartDate.SelectedDate;
                this.endDate = this.calendarEndDate.SelectedDate;
                this.IntegralStatus = int.Parse(this.drIntegralStatus.SelectedValue);
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void grdMemberList_ReBindData(object sender)
        {
            this.BindConsultation();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.BindData();
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
            {
                queryStrings.Add("userId", this.Page.Request.QueryString["userId"].ToString());
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            queryStrings.Add("IntegralStatus", this.drIntegralStatus.SelectedValue.ToString());
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

