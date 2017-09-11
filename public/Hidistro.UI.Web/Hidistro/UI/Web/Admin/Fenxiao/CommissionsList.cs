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
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class CommissionsList : AdminPage
    {
        protected Button btnQueryLogs;
        protected Button Button1;
        protected Button Button4;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected string CurrentStoreName;
        protected decimal CurrentTotal;
        protected string EndTime;
        protected LinkButton Frist;
        protected string FristDisplay;
        protected Literal fristSum;
        public int lastDay;
        protected LinkButton Other;
        protected Repeater otherCommissions;
        protected string OtherDisplay;
        protected Literal OtherSum;
        protected Pager pager;
        protected Repeater reCommissions;
        protected LinkButton Second;
        protected string SecondDisplay;
        protected Literal secondSum;
        protected string StartTime;
        protected LinkButton Store;
        protected string StoreDisplay;
        protected Literal storeSum;
        protected Repeater SubCommissions;
        private string subLevel;
        protected int userid;

        protected CommissionsList() : base("m05", "fxp03")
        {
            this.StartTime = "";
            this.EndTime = "";
            this.CurrentStoreName = "";
            this.StoreDisplay = "active";
            this.FristDisplay = "";
            this.SecondDisplay = "";
            this.OtherDisplay = "";
            this.subLevel = "0";
        }

        private void BindData()
        {
            DbQueryResult commissionsWithStoreName;
            DateTime time;
            CommissionsQuery entity = new CommissionsQuery {
                UserId = int.Parse(this.Page.Request.QueryString["UserId"]),
                EndTime = this.EndTime,
                StartTime = this.StartTime,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "CommId"
            };
            Globals.EntityCoding(entity, true);
            if (this.subLevel == "0")
            {
                commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(entity, "5");
                this.reCommissions.DataSource = commissionsWithStoreName.Data;
                this.reCommissions.DataBind();
            }
            else if (this.subLevel == "45")
            {
                commissionsWithStoreName = VShopHelper.GetCommissionsWithStoreName(entity, "4");
                this.otherCommissions.DataSource = commissionsWithStoreName.Data;
                this.otherCommissions.DataBind();
            }
            else
            {
                commissionsWithStoreName = VShopHelper.GetSubDistributorsContribute(this.StartTime, this.EndTime, this.pager.PageSize, this.pager.PageIndex, this.userid, int.Parse(this.subLevel));
                this.SubCommissions.DataSource = commissionsWithStoreName.Data;
                this.SubCommissions.DataBind();
            }
            this.pager.TotalRecords = commissionsWithStoreName.TotalRecords;
            if (!DateTime.TryParse(this.StartTime, out time))
            {
                time = DateTime.Parse("2015-01-01");
            }
            this.CurrentTotal = DistributorsBrower.GetUserCommissions(entity.UserId, time, this.EndTime, null, null, "");
            decimal num = DistributorsBrower.GetUserCommissions(entity.UserId, time, this.EndTime, null, null, "0");
            decimal num2 = DistributorsBrower.GetUserCommissions(entity.UserId, time, this.EndTime, null, null, "1");
            decimal num3 = DistributorsBrower.GetUserCommissions(entity.UserId, time, this.EndTime, null, null, "2");
            decimal num4 = ((this.CurrentTotal - num) - num2) - num3;
            this.storeSum.Text = num.ToString("f2");
            this.fristSum.Text = num2.ToString("f2");
            this.secondSum.Text = num3.ToString("f2");
            this.OtherSum.Text = num4.ToString("f2");
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(entity.UserId);
            this.CurrentStoreName = userIdDistributors.StoreName;
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
            this.subLevel = base.Request.QueryString["subLevel"];
            this.lastDay = 0;
            this.ReBind(true);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
            this.lastDay = 7;
            this.ReBind(true);
        }

        protected void Button4_Click1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.EndTime = now.ToString("yyyy-MM-dd");
            this.StartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
            this.lastDay = 30;
            this.ReBind(true);
        }

        protected void Frist_Click(object sender, EventArgs e)
        {
            this.FristDisplay = "active";
            this.SecondDisplay = "";
            this.StoreDisplay = "";
            this.OtherDisplay = "";
            this.subLevel = "1";
            this.ReBind(true);
        }

        protected string getNextName(string StoreName, string uid, string rid, string rpath)
        {
            string str = "店铺销售";
            if ((uid == rid) || string.IsNullOrEmpty(rpath))
            {
                return "店铺销售";
            }
            if (uid == rpath)
            {
                return (StoreName + "（下一级）");
            }
            if (rpath.Contains("|"))
            {
                string[] strArray = rpath.Split(new char[] { '|' });
                if (strArray[0] == uid)
                {
                    str = StoreName + "（下二级）";
                }
                if (strArray[1] == uid)
                {
                    str = StoreName + "（下一级）";
                }
                return str;
            }
            return rpath;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["subLevel"]))
                {
                    this.subLevel = base.Server.UrlDecode(this.Page.Request.QueryString["subLevel"]);
                }
                else
                {
                    this.subLevel = "0";
                }
                if (this.subLevel == "1")
                {
                    this.FristDisplay = "active";
                    this.SecondDisplay = "";
                    this.OtherDisplay = "";
                    this.StoreDisplay = "";
                }
                else if (this.subLevel == "2")
                {
                    this.FristDisplay = "";
                    this.SecondDisplay = "active";
                    this.StoreDisplay = "";
                    this.OtherDisplay = "";
                    this.subLevel = "2";
                }
                else if (this.subLevel == "45")
                {
                    this.FristDisplay = "";
                    this.SecondDisplay = "";
                    this.StoreDisplay = "";
                    this.OtherDisplay = "active";
                }
                else
                {
                    this.FristDisplay = "";
                    this.SecondDisplay = "";
                    this.OtherDisplay = "";
                    this.StoreDisplay = "active";
                    this.subLevel = "0";
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
                {
                    int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
                    if (this.lastDay == 30)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("");
                        this.Button4.BorderColor = ColorTranslator.FromHtml("#FF00CC");
                    }
                    else if (this.lastDay == 7)
                    {
                        this.Button1.BorderColor = ColorTranslator.FromHtml("#FF00CC");
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

        protected void Other_Click(object sender, EventArgs e)
        {
            this.StoreDisplay = "";
            this.FristDisplay = "";
            this.SecondDisplay = "";
            this.OtherDisplay = "active";
            this.subLevel = "45";
            this.ReBind(true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
            {
                this.BindData();
            }
            else
            {
                this.Page.Response.Redirect("DistributorList.aspx");
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            string text1 = this.Page.Request.QueryString["subLevel"];
            queryStrings.Add("UserId", this.Page.Request.QueryString["UserId"]);
            queryStrings.Add("StartTime", this.StartTime);
            queryStrings.Add("EndTime", this.EndTime);
            queryStrings.Add("subLevel", this.subLevel);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("lastDay", this.lastDay.ToString());
            base.ReloadPage(queryStrings);
        }

        protected void Second_Click(object sender, EventArgs e)
        {
            this.StoreDisplay = "";
            this.FristDisplay = "";
            this.SecondDisplay = "active";
            this.OtherDisplay = "";
            this.subLevel = "2";
            this.ReBind(true);
        }

        protected void Store_Click(object sender, EventArgs e)
        {
            this.StoreDisplay = "active";
            this.FristDisplay = "";
            this.SecondDisplay = "";
            this.OtherDisplay = "";
            this.subLevel = "0";
            this.ReBind(true);
        }
    }
}

