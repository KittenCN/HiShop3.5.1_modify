namespace Hidistro.UI.Web.Admin.CashBack
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MemberSelect : AdminPage
    {
        protected Button btnSearchButton;
        protected Panel divEmpty;
        protected HtmlForm form1;
        protected Pager pager;
        private string phone;
        protected Repeater rptList;
        protected TextBox txtPhone;
        protected TextBox txtUserName;
        private string userName;

        protected MemberSelect() : base("m99", "00000")
        {
        }

        protected void BindData()
        {
            MemberQuery query = new MemberQuery {
                Username = this.userName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                EndTime = new DateTime?(DateTime.Now),
                CellPhone = this.phone
            };
            DbQueryResult members = MemberHelper.GetMembers(query, false);
            this.rptList.DataSource = members.Data;
            this.rptList.DataBind();
            this.pager.TotalRecords = members.TotalRecords;
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["username"]))
                {
                    this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["username"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["phone"]))
                {
                    this.phone = this.Page.Request.QueryString["phone"];
                }
                this.txtUserName.Text = this.userName;
                this.txtPhone.Text = this.phone;
            }
            else
            {
                this.userName = this.txtUserName.Text;
                this.phone = this.txtPhone.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("username", this.txtUserName.Text.Trim());
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

