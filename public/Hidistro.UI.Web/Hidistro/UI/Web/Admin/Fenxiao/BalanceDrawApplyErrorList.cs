namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class BalanceDrawApplyErrorList : AdminPage
    {
        protected Button btnSearchButton;
        protected Button Button3;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected LinkButton Frist;
        protected HiddenField hSerialID;
        protected Pager pager;
        protected Repeater reCommissions;
        private string RequestEndTime;
        private string RequestStartTime;
        protected LinkButton Second;
        protected TextBox SignalrefuseMks;
        private string StoreName;
        protected TextBox txtStoreName;

        protected BalanceDrawApplyErrorList() : base("m05", "fxp09")
        {
            this.RequestStartTime = "";
            this.RequestEndTime = "";
            this.StoreName = "";
        }

        private void BindData()
        {
            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                RequestTime = "",
                CheckTime = "",
                StoreName = this.StoreName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "SerialID",
                RequestEndTime = this.RequestEndTime,
                RequestStartTime = this.RequestStartTime,
                IsCheck = "",
                UserId = ""
            };
            string[] extendChecks = new string[] { "3" };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(entity, extendChecks);
            this.reCommissions.DataSource = balanceDrawRequest.Data;
            this.reCommissions.DataBind();
            this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
            int[] checkValues = new int[2];
            checkValues[1] = 1;
            DataTable drawRequestNum = DistributorsBrower.GetDrawRequestNum(checkValues);
            int num = 0;
            if (drawRequestNum.Rows.Count > 0)
            {
                for (int i = 0; i < drawRequestNum.Rows.Count; i++)
                {
                    num += Globals.ToNum(drawRequestNum.Rows[i]["num"]);
                }
                this.Frist.Text = "待发放(" + num.ToString() + ")";
            }
            this.Second.Text = "发放异常(" + this.pager.TotalRecords + ")";
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.RequestStartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                this.RequestEndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            this.ReBind(true);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            int[] serialids = new int[] { int.Parse(this.hSerialID.Value) };
            if (serialids[0] != 0)
            {
                switch (DistributorsBrower.GetBalanceDrawRequestIsCheckStatus(serialids[0]))
                {
                    case -1:
                    case 2:
                        this.ShowMsg("当前项数据不可以驳回，操作终止！", false);
                        return;
                }
                if (DistributorsBrower.SetBalanceDrawRequestIsCheckStatus(serialids, -1, this.SignalrefuseMks.Text, null))
                {
                    this.ShowMsg("申请已驳回！", true);
                    this.LoadParameters();
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("申请驳回失败，请再次尝试", false);
                }
            }
        }

        protected void Frist_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("BalanceDrawApplyList.aspx");
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                    this.calendarStartDate.SelectedDate = new DateTime?(DateTime.Parse(this.RequestStartTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                    this.calendarEndDate.SelectedDate = new DateTime?(DateTime.Parse(this.RequestEndTime));
                }
                this.txtStoreName.Text = this.StoreName;
            }
            else
            {
                this.StoreName = this.txtStoreName.Text;
                if (this.calendarStartDate.SelectedDate.HasValue)
                {
                    this.RequestStartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (this.calendarEndDate.SelectedDate.HasValue)
                {
                    this.RequestEndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.Button3.Click += new EventHandler(this.Button3_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("RequestStartTime", this.RequestStartTime);
            queryStrings.Add("RequestEndTime", this.RequestEndTime);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

