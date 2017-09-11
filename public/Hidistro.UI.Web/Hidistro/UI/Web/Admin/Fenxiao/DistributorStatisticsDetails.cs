namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class DistributorStatisticsDetails : AdminPage
    {
        protected Button btnQueryLogs;
        protected Button Button1;
        protected Button Button4;
        protected HtmlGenericControl BuyPrice;
        protected HtmlGenericControl BuyUsernums;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected string CurrentStoreName;
        protected decimal CurrentTotal;
        private string EndTime;
        protected LinkButton Frist;
        protected string FristDisplay;
        private int i;
        public int lastDay;
        protected HiImage ListImage1;
        protected HtmlGenericControl OrdersTotal;
        protected Pager pager;
        protected Repeater reDistributor;
        protected HtmlGenericControl ReferralOrders;
        private int rows;
        protected LinkButton Second;
        protected string SecondDisplay;
        private string StartTime;
        private string subLevel;
        protected HtmlGenericControl TotalReferral;
        protected HtmlGenericControl txtStoreName;
        private int userid;

        protected DistributorStatisticsDetails() : base("m05", "fxp05")
        {
            this.StartTime = "";
            this.subLevel = "1";
            this.EndTime = "";
            this.CurrentStoreName = "";
            this.FristDisplay = "active";
            this.SecondDisplay = "";
        }

        private void BindData(int UserId)
        {
            DbQueryResult result = VShopHelper.GetSubDistributorsRankingsN(this.StartTime, this.EndTime, this.pager.PageSize, this.pager.PageIndex, this.userid, int.Parse(this.subLevel));
            DataTable data = (DataTable) result.Data;
            this.reDistributor.DataSource = result.Data;
            this.reDistributor.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
            DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(this.userid);
            this.Frist.Text = "一级分店(" + distributorsSubStoreNum.Rows[0]["firstV"] + ")";
            this.Second.Text = "二级分店(" + distributorsSubStoreNum.Rows[0]["secondV"] + ")";
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
            this.subLevel = "1";
            this.ReBind(true);
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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["subLevel"]))
                {
                    this.subLevel = base.Server.UrlDecode(this.Page.Request.QueryString["subLevel"]);
                }
                else
                {
                    this.subLevel = "1";
                }
                if (this.subLevel == "1")
                {
                    this.FristDisplay = "active";
                    this.SecondDisplay = "";
                }
                else
                {
                    this.FristDisplay = "";
                    this.SecondDisplay = "active";
                    this.subLevel = "2";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
            {
                this.Page.Response.Redirect("DistributorStatistics.aspx");
            }
            this.ListImage1.ImageUrl = "/admin/images/90x90.png";
            this.LoadParameters();
            this.reDistributor.ItemDataBound += new RepeaterItemEventHandler(this.reDistributor_ItemDataBound);
            DataTable table = VShopHelper.GetDistributorSaleinfo(this.StartTime, this.EndTime, new int[] { this.userid });
            if ((table != null) && (table.Rows.Count > 0))
            {
                string path = (string) table.Rows[0]["Logo"];
                if (File.Exists(base.Server.MapPath(path)))
                {
                    this.ListImage1.ImageUrl = path;
                }
                this.txtStoreName.InnerText = (string) table.Rows[0]["StoreName"];
                this.OrdersTotal.InnerText = "￥" + Convert.ToDouble(table.Rows[0]["OrderTotalSum"]).ToString("0.00");
                this.ReferralOrders.InnerText = table.Rows[0]["Ordernums"].ToString();
                decimal num = decimal.Parse(table.Rows[0]["CommTotalSum"].ToString());
                this.TotalReferral.InnerText = "￥" + Convert.ToDouble(num.ToString()).ToString("0.00");
                this.BuyUsernums.InnerText = table.Rows[0]["BuyUserIds"].ToString();
                decimal num2 = decimal.Parse(table.Rows[0]["BuyUserIds"].ToString());
                this.BuyPrice.InnerText = (num2 > 0M) ? ((decimal.Parse(table.Rows[0]["OrderTotalSum"].ToString()) / num2)).ToString("F2") : "0.00";
            }
            this.BindData(this.userid);
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
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

        private void reDistributor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.FindControl("litph");
                DataRowView dataItem = (DataRowView) e.Item.DataItem;
                this.i++;
                this.rows = ((this.pager.PageIndex - 1) * this.pager.PageSize) + this.i;
                if (dataItem["Ordernums"].ToString() == "0")
                {
                    literal.Text = "　";
                }
                else if (this.rows == 1)
                {
                    literal.Text = "<img src=\"../images/0001.gif\"></img>";
                }
                else if (this.rows == 2)
                {
                    literal.Text = "<img src=\"../images/0002.gif\"></img>";
                }
                else if (this.rows == 3)
                {
                    literal.Text = "<img src=\"../images/0003.gif\"></img>";
                }
                else
                {
                    literal.Text = (int.Parse(literal.Text) + this.rows).ToString();
                }
            }
        }

        protected void Second_Click(object sender, EventArgs e)
        {
            this.FristDisplay = "";
            this.SecondDisplay = "active";
            this.subLevel = "2";
            this.ReBind(true);
        }
    }
}

