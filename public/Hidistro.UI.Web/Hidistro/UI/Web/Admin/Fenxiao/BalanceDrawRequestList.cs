namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class BalanceDrawRequestList : AdminPage
    {
        protected Button btnSearchButton;
        protected Button Button1;
        protected Button Button4;
        private int lastDay;
        protected Pager pager;
        protected Repeater reBalanceDrawRequest;
        private string RequestEndTime;
        private string RequestStartTime;
        private string StoreName;
        protected ucDateTimePicker txtRequestEndTime;
        protected ucDateTimePicker txtRequestStartTime;
        protected TextBox txtStoreName;

        protected BalanceDrawRequestList() : base("m05", "fxp10")
        {
            this.RequestStartTime = "";
            this.StoreName = "";
            this.RequestEndTime = "";
        }

        private void BindData()
        {
            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                RequestTime = "",
                CheckTime = "CheckTime",
                RequestStartTime = this.RequestStartTime,
                RequestEndTime = this.RequestEndTime,
                StoreName = this.StoreName,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "RequestTime",
                IsCheck = "2"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = VShopHelper.GetBalanceDrawRequest(entity);
            this.reBalanceDrawRequest.DataSource = balanceDrawRequest.Data;
            this.reBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            if (this.txtRequestStartTime.SelectedDate.HasValue)
            {
                this.RequestStartTime = this.txtRequestStartTime.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (this.txtRequestEndTime.SelectedDate.HasValue)
            {
                this.RequestEndTime = this.txtRequestEndTime.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            this.lastDay = 0;
            this.ReBind(true);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.RequestEndTime = now.ToString("yyyy-MM-dd");
            this.RequestStartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
            this.lastDay = 7;
            this.ReBind(true);
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.RequestEndTime = now.ToString("yyyy-MM-dd");
            this.RequestStartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
            this.lastDay = 30;
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestStartTime"]))
                {
                    this.RequestStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestStartTime"]);
                    this.txtRequestStartTime.SelectedDate = new DateTime?(DateTime.Parse(this.RequestStartTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RequestEndTime"]))
                {
                    this.RequestEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["RequestEndTime"]);
                    this.txtRequestEndTime.SelectedDate = new DateTime?(DateTime.Parse(this.RequestEndTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
                {
                    int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
                    if (this.lastDay == 30)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("#1CA47D");
                    }
                    else if (this.lastDay == 7)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("#1CA47D");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                    else
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("");
                    }
                }
                this.txtStoreName.Text = this.StoreName;
            }
            else
            {
                this.StoreName = this.txtStoreName.Text;
                if (this.txtRequestStartTime.SelectedDate.HasValue)
                {
                    this.RequestStartTime = this.txtRequestStartTime.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (this.txtRequestEndTime.SelectedDate.HasValue)
                {
                    this.RequestEndTime = this.txtRequestEndTime.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
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
            queryStrings.Add("lastDay", this.lastDay.ToString());
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

