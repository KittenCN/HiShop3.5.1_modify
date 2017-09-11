namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class CommissionsAllList : AdminPage
    {
        protected Button btnQueryLogs;
        protected Button Button1;
        protected Button Button4;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected string CurrentStoreName;
        protected decimal CurrentTotal;
        private string EndTime;
        public int lastDay;
        private string OrderId;
        protected Pager pager;
        protected Repeater reCommissions;
        private string StartTime;
        private string StoreName;
        protected TextBox txtOrderId;
        protected TextBox txtStoreName;

        protected CommissionsAllList() : base("m05", "fxp08")
        {
            this.OrderId = "";
            this.StoreName = "";
            this.StartTime = "";
            this.EndTime = "";
            this.CurrentStoreName = "";
        }

        private void BindData()
        {
            DateTime time;
            CommissionsQuery entity = new CommissionsQuery {
                StoreName = this.StoreName,
                OrderNum = this.OrderId,
                EndTime = this.EndTime,
                StartTime = this.StartTime,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "CommId"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(entity, "0");
            this.reCommissions.DataSource = commissionsWithStoreName.Data;
            this.reCommissions.DataBind();
            this.pager.TotalRecords = commissionsWithStoreName.TotalRecords;
            if (!DateTime.TryParse(this.StartTime, out time))
            {
                time = DateTime.Parse("2015-01-01");
            }
            this.CurrentTotal = DistributorsBrower.GetUserCommissions(0, time, this.EndTime, this.StoreName, entity.OrderNum, "");
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
            this.OrderId = this.txtOrderId.Text;
            this.StoreName = this.txtStoreName.Text;
            this.lastDay = 0;
            this.ReBind(true);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
            this.lastDay = 7;
            this.OrderId = this.txtOrderId.Text;
            this.StoreName = this.txtStoreName.Text;
            this.ReBind(true);
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
            this.lastDay = 30;
            this.OrderId = this.txtOrderId.Text;
            this.StoreName = this.txtStoreName.Text;
            this.ReBind(true);
        }

        protected string getNextName(string uid, string rid, string rpath)
        {
            string str = "原上级店铺";
            if ((uid == rid) || string.IsNullOrEmpty(rpath))
            {
                return "成交店铺";
            }
            if (uid == rpath)
            {
                return "上一级分销商";
            }
            if (rpath.Contains("|"))
            {
                string[] strArray = rpath.Split(new char[] { '|' });
                if (strArray[0] == uid)
                {
                    str = "上二级分销商";
                }
                if (strArray[1] == uid)
                {
                    str = "上一级分销商";
                }
            }
            return str;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                this.txtStoreName.Text = this.StoreName;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
                {
                    this.OrderId = base.Server.UrlDecode(this.Page.Request.QueryString["OrderId"]);
                }
                this.txtOrderId.Text = this.OrderId;
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
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OrderId", this.OrderId);
            queryStrings.Add("StoreName", this.StoreName);
            queryStrings.Add("StartTime", this.StartTime);
            queryStrings.Add("EndTime", this.EndTime);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("lastDay", this.lastDay.ToString());
            base.ReloadPage(queryStrings);
        }

        protected void rptypelist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (string.IsNullOrEmpty(this.StoreName) && ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem)))
            {
                Repeater repeater = e.Item.FindControl("reCommissionsChild") as Repeater;
                DataRowView dataItem = (DataRowView) e.Item.DataItem;
                int num = Convert.ToInt32(dataItem.Row["ReferralUserId"]);
                string str = dataItem.Row["OrderId"].ToString();
                if ((num > 0) && (str != ""))
                {
                    CommissionsQuery entity = new CommissionsQuery {
                        ReferralUserId = num,
                        StoreName = "",
                        OrderNum = str,
                        EndTime = "",
                        StartTime = "",
                        PageIndex = 1,
                        PageSize = 0x3e8,
                        SortOrder = SortAction.Desc,
                        SortBy = "CommId"
                    };
                    Globals.EntityCoding(entity, true);
                    DbQueryResult commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(entity, "3");
                    repeater.DataSource = commissionsWithStoreName.Data;
                    repeater.DataBind();
                }
            }
        }
    }
}

