namespace Hidistro.UI.Web.Admin.member
{
    using Ajax;
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Members)]
    public class MembersIntegralQuery : AdminPage
    {
        private bool? approved;
        protected Button btnSearchButton;
        public string clientType;
        protected Grid grdMemberList;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string phone;
        private int? rankId;
        protected MemberGradeDropDownList rankList;
        private string realName;
        private string searchKey;
        protected HtmlForm thisForm;
        protected TextBox txtPhone;
        protected TextBox txtRealName;
        protected TextBox txtSearchText;
        public string ValidSmsNum;
        private int? vipCard;

        public MembersIntegralQuery() : base("m04", "hyp10")
        {
            this.ValidSmsNum = "0";
        }

        protected void BindData()
        {
            MemberQuery query = new MemberQuery {
                Username = this.searchKey,
                Realname = this.realName,
                GradeId = this.rankId,
                PageIndex = this.pager.PageIndex,
                IsApproved = this.approved,
                SortBy = this.grdMemberList.SortOrderBy,
                PageSize = this.pager.PageSize,
                Stutas = (UserStatus)1,
                EndTime = new DateTime?(DateTime.Now),
                StartTime = new DateTime?(DateTime.Now.AddDays((double) -this.GetSiteSetting().ActiveDay)),
                CellPhone = (base.Request.QueryString["phone"] != null) ? base.Request.QueryString["phone"] : "",
                ClientType = (base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : ""
            };
            if (this.grdMemberList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            if (this.vipCard.HasValue && (this.vipCard.Value != 0))
            {
                query.HasVipCard = new bool?(this.vipCard.Value == 1);
            }
            DbQueryResult members = MemberHelper.GetMembers(query, false);
            this.grdMemberList.DataSource = members.Data;
            this.grdMemberList.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = members.TotalRecords;
        }

        public void BindDDL()
        {
            this.rankList.DataBind();
            this.rankList.SelectedValue = this.rankId;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void ddlApproved_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReBind(false);
        }

        private SiteSettings GetSiteSetting()
        {
            return SettingsManager.GetMasterSettings(false);
        }

        private void grdMemberList_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["rankId"], out result))
                {
                    this.rankId = new int?(result);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
                {
                    this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
                {
                    this.approved = new bool?(Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["phone"]))
                {
                    this.phone = this.Page.Request.QueryString["phone"];
                }
                this.rankList.SelectedValue = this.rankId;
                this.txtSearchText.Text = this.searchKey;
                this.txtRealName.Text = this.realName;
                this.txtPhone.Text = this.phone;
            }
            else
            {
                this.rankId = this.rankList.SelectedValue;
                this.searchKey = this.txtSearchText.Text;
                this.realName = this.txtRealName.Text.Trim();
                this.phone = this.txtPhone.Text.Trim();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(this.grdMemberList_ReBindData);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.RegisterTypeForAjax(typeof(ManageMembers));
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.ViewState["ClientType"] = (base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : null;
                this.BindDDL();
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (this.rankList.SelectedValue.HasValue)
            {
                queryStrings.Add("rankId", this.rankList.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("searchKey", this.txtSearchText.Text);
            queryStrings.Add("realName", this.txtRealName.Text);
            queryStrings.Add("clientType", (this.ViewState["ClientType"] != null) ? this.ViewState["ClientType"].ToString() : "");
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("phone", this.txtPhone.Text);
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

