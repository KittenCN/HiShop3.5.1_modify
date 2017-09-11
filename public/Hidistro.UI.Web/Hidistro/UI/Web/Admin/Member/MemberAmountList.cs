namespace Hidistro.UI.Web.Admin.Member
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MemberAmountList : AdminPage
    {
        protected decimal AvailableTotal;
        protected Button btnQueryLogs;
        protected Button Button1;
        protected Button Button4;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected decimal CurrentTotal;
        private string EndTime;
        protected HiddenField hidType;
        protected HiddenField hidWays;
        public int lastDay;
        protected Pager pager;
        private string PayId;
        protected Repeater reCommissions;
        private string StartTime;
        protected DropDownList TradeTypeList;
        private string TradeTypeValue;
        protected DropDownList TradeWaysList;
        private string TradeWaysValue;
        protected TextBox txtOrderId;
        protected TextBox txtStoreName;
        protected decimal UnliquidatedTotal;
        private string UserName;

        protected MemberAmountList() : base("m04", "hyp12")
        {
            this.PayId = "";
            this.UserName = "";
            this.TradeTypeValue = "";
            this.TradeWaysValue = "";
            this.StartTime = "";
            this.EndTime = "";
        }

        private void BindData()
        {
            MemberAmountQuery entity = new MemberAmountQuery {
                UserName = this.UserName,
                PayId = this.PayId,
                TradeType = this.TradeTypeValue,
                TradeWays = this.TradeWaysValue,
                EndTime = this.EndTime,
                StartTime = this.StartTime,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "Id"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult amountWithUserName = MemberAmountProcessor.GetAmountWithUserName(entity);
            this.reCommissions.DataSource = amountWithUserName.Data;
            this.reCommissions.DataBind();
            this.pager.TotalRecords = amountWithUserName.TotalRecords;
            Dictionary<string, decimal> amountDic = MemberAmountProcessor.GetAmountDic(entity);
            this.CurrentTotal = amountDic["CurrentTotal"];
            this.AvailableTotal = amountDic["AvailableTotal"];
            this.UnliquidatedTotal = amountDic["UnliquidatedTotal"];
        }

        protected void btnQueryLogs_Click(object sender, EventArgs e)
        {
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            this.PayId = this.txtOrderId.Text;
            this.UserName = this.txtStoreName.Text;
            this.TradeTypeValue = this.hidType.Value;
            this.TradeWaysValue = this.hidWays.Value;
            this.lastDay = 0;
            this.ReBind(true);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
            this.lastDay = 7;
            this.PayId = this.txtOrderId.Text;
            this.UserName = this.txtStoreName.Text;
            this.TradeTypeValue = this.hidType.Value;
            this.TradeWaysValue = this.hidWays.Value;
            this.ReBind(true);
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
            this.lastDay = 30;
            this.PayId = this.txtOrderId.Text;
            this.UserName = this.txtStoreName.Text;
            this.TradeTypeValue = this.hidType.Value;
            this.TradeWaysValue = this.hidWays.Value;
            this.ReBind(true);
        }

        private void LoadDropDownList()
        {
            Dictionary<int, string> enumValueAndDescription = MemberHelper.GetEnumValueAndDescription(typeof(TradeType));
            this.TradeTypeList.Items.Clear();
            foreach (KeyValuePair<int, string> pair in enumValueAndDescription)
            {
                ListItem item = new ListItem {
                    Text = pair.Value,
                    Value = pair.Key.ToString()
                };
                this.TradeTypeList.Items.Add(item);
            }
            this.TradeTypeList.Items.Insert(0, new ListItem("-全部-", ""));
            Dictionary<int, string> dictionary2 = MemberHelper.GetEnumValueAndDescription(typeof(TradeWays));
            this.TradeWaysList.Items.Clear();
            foreach (KeyValuePair<int, string> pair2 in dictionary2)
            {
                ListItem item2 = new ListItem {
                    Text = pair2.Value,
                    Value = pair2.Key.ToString()
                };
                this.TradeWaysList.Items.Add(item2);
            }
            this.TradeWaysList.Items.Insert(0, new ListItem("-全部-", ""));
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
                {
                    this.UserName = base.Server.UrlDecode(this.Page.Request.QueryString["UserName"]);
                }
                this.txtStoreName.Text = this.UserName;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PayId"]))
                {
                    this.PayId = base.Server.UrlDecode(this.Page.Request.QueryString["PayId"]);
                }
                this.txtOrderId.Text = this.PayId;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TradeType"]))
                {
                    this.TradeTypeValue = base.Server.UrlDecode(this.Page.Request.QueryString["TradeType"]);
                }
                this.hidType.Value = this.TradeTypeValue;
                this.TradeTypeList.SelectedValue = this.TradeTypeValue;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TradeWays"]))
                {
                    this.TradeWaysValue = base.Server.UrlDecode(this.Page.Request.QueryString["TradeWays"]);
                }
                this.hidWays.Value = this.TradeWaysValue;
                this.TradeWaysList.SelectedValue = this.TradeWaysValue;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
                {
                    this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
                    this.calendarStartDate.SelectedDate = new DateTime?(DateTime.Parse(this.StartTime));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
                {
                    this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
                    this.calendarEndDate.SelectedDate = new DateTime?(DateTime.Parse(this.EndTime));
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
            }
            else
            {
                if (this.calendarStartDate.SelectedDate.HasValue)
                {
                    this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (this.calendarEndDate.SelectedDate.HasValue)
                {
                    this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadDropDownList();
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("PayId", this.PayId);
            queryStrings.Add("UserName", this.UserName);
            queryStrings.Add("StartTime", this.StartTime);
            queryStrings.Add("EndTime", this.EndTime);
            queryStrings.Add("TradeType", this.TradeTypeValue);
            queryStrings.Add("TradeWays", this.TradeWaysValue);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("lastDay", this.lastDay.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

