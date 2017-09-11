namespace Hidistro.UI.Web.Admin.Insurance
{
    using ASPNET.WebControls;
    using Entities.Insurance;
    using Hidistro.ControlPanel.CashBack;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.CashBack;
    using SqlDal.Insurance;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Ajax;
    using Hidistro.ControlPanel.Store;
    using Entities.Store;
   
    public class ManageInsuranceOrder : AdminPage
    {
        protected Button btnSearchButton;
        private string cellphone;
        protected InsuranceOrderTypesDropDownList dropCashBackTypes;
        protected Grid grdMemberList;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlForm thisForm;
        protected TextBox txtPhone;
        protected TextBox txtUserName;
        private string type;
        private string Username;
        InsuranceDao dao = new InsuranceDao();
        public string ifsuc = "0";
        public ManageInsuranceOrder() : base("m108", "bxp01")
        {
            this.type = "";
        }

        private void BindCashBackData()
        {
            InsuranceOrderQuery query = new InsuranceOrderQuery
            {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                Cellphone = this.cellphone,
                UserName = this.Username
            };
            if (!string.IsNullOrWhiteSpace(this.type))
            {
                query.CashBackTypes = new InsuranceOrderTypes?((InsuranceOrderTypes) int.Parse(this.type));
            }
           
            DbQueryResult cashBackByPager = dao.GetInsuranceOrdeByPager(query);
            this.grdMemberList.DataSource = cashBackByPager.Data;
            this.grdMemberList.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = cashBackByPager.TotalRecords;
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
                {
                    this.Username = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["phone"]))
                {
                    this.cellphone = this.Page.Request.QueryString["phone"];
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["type"]))
                {
                    this.type = this.Page.Request.QueryString["type"];
                }
                this.txtUserName.Text = this.Username;
                this.txtPhone.Text = this.cellphone;
                if (!string.IsNullOrWhiteSpace(this.type))
                {
                    this.dropCashBackTypes.SelectedValue = new int?(int.Parse(this.type));
                }
            }
            else
            {
                this.Username = this.txtUserName.Text.Trim();
                this.cellphone = this.txtPhone.Text.Trim();
                this.type = this.dropCashBackTypes.SelectedValue.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();

            Utility.RegisterTypeForAjax(typeof(ManageInsuranceOrder));
            if (!this.Page.IsPostBack)
            {
                this.dropCashBackTypes.DataBind();
                int result = 0;
                if (int.TryParse(this.type, out result))
                {
                    this.dropCashBackTypes.SelectedValue = new int?(result);
                }
                this.BindCashBackData();
            }
        }

        [AjaxMethod]
        public string DelInsuranceOrder(int InsuranceOrderId)
        {
         
            if (dao.Delete(InsuranceOrderId))
            {
                return "success";
            }
            return "fail";
        }

    

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Username", this.txtUserName.Text.Trim());
            queryStrings.Add("phone", this.txtPhone.Text.Trim());
            if (this.dropCashBackTypes.SelectedValue.HasValue)
            {
                queryStrings.Add("type", this.dropCashBackTypes.SelectedValue.Value.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

