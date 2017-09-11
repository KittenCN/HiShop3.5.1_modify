namespace Hidistro.UI.Web.Admin.Sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ProductSaleRanking : AdminPage
    {
        private DateTime? BeginDate;
        protected Button btnExport;
        protected Button btnMonthView;
        protected Button btnSearch;
        protected Button btnWeekView;
        private DateTime? EndDate;
        protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
        protected ExportFormatRadioButtonList exportFormatRadioButtonList;
        protected HtmlForm form1;
        private int lastDay;
        protected Pager pager;
        protected Repeater rptList;
        protected ucDateTimePicker txtBeginDate;
        protected ucDateTimePicker txtEndDate;

        protected ProductSaleRanking() : base("m10", "tjp04")
        {
        }

        private void BindData()
        {
            OrderStatisticsQuery entity = new OrderStatisticsQuery {
                BeginDate = this.BeginDate,
                EndDate = this.EndDate,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "SaleAmountFee"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = ShopStatisticHelper.Product_GetStatisticReport(entity);
            this.rptList.DataSource = result.Data;
            this.rptList.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.exportFieldsCheckBoxList.SelectedItem == null)
            {
                this.ShowMsg("请选择需要导出的记录", false);
            }
            else
            {
                IList<string> fields = new List<string>();
                IList<string> list2 = new List<string>();
                foreach (ListItem item in this.exportFieldsCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        fields.Add(item.Value);
                        list2.Add(item.Text);
                    }
                }
                OrderStatisticsQuery query = new OrderStatisticsQuery {
                    BeginDate = this.BeginDate,
                    EndDate = this.EndDate
                };
                DataTable table = ShopStatisticHelper.Product_GetStatisticReport_NoPage(query, fields);
                StringBuilder builder = new StringBuilder();
                foreach (string str in list2)
                {
                    builder.Append(str + ",");
                    if (str == list2[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
                foreach (DataRow row in table.Rows)
                {
                    foreach (string str2 in fields)
                    {
                        builder.Append(row[str2]).Append(",");
                        if (str2 == fields[list2.Count - 1])
                        {
                            builder = builder.Remove(builder.Length - 1, 1);
                            builder.Append("\r\n");
                        }
                    }
                }
                this.Page.Response.Clear();
                this.Page.Response.Buffer = false;
                this.Page.Response.Charset = "GB2312";
                if (this.exportFormatRadioButtonList.SelectedValue == "csv")
                {
                    this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DataExport.csv");
                    this.Page.Response.ContentType = "application/octet-stream";
                }
                else
                {
                    this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DataExport.txt");
                    this.Page.Response.ContentType = "application/vnd.ms-word";
                }
                this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                this.Page.EnableViewState = false;
                this.Page.Response.Write(builder.ToString());
                this.Page.Response.End();
            }
        }

        protected void btnMonthView_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.BeginDate = new DateTime?(now.AddDays(-29.0));
            this.EndDate = new DateTime?(now);
            this.lastDay = 30;
            this.ReBind_Url(true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.txtBeginDate.TextToDate.HasValue)
            {
                this.BeginDate = new DateTime?(this.txtBeginDate.TextToDate.Value);
            }
            if (this.txtEndDate.TextToDate.HasValue)
            {
                this.EndDate = new DateTime?(this.txtEndDate.TextToDate.Value);
            }
            this.lastDay = 0;
            this.ReBind_Url(true);
        }

        protected void btnWeekView_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            this.BeginDate = new DateTime?(now.AddDays(-6.0));
            this.EndDate = new DateTime?(now);
            this.lastDay = 7;
            this.ReBind_Url(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                this.BeginDate = new DateTime?(DateTime.Today.AddDays(-6.0));
                this.EndDate = new DateTime?(DateTime.Today);
                if (base.GetUrlParam("BeginDate") != "")
                {
                    this.BeginDate = new DateTime?(DateTime.Parse(base.GetUrlParam("BeginDate")));
                }
                if (base.GetUrlParam("EndDate") != "")
                {
                    this.EndDate = new DateTime?(DateTime.Parse(base.GetUrlParam("EndDate")));
                }
                this.txtBeginDate.TextToDate = this.BeginDate;
                this.txtEndDate.TextToDate = this.EndDate;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
                {
                    int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
                    if (this.lastDay >= 7)
                    {
                        this.btnWeekView.BorderColor = (this.lastDay == 7) ? ColorTranslator.FromHtml("#1CA47D") : ColorTranslator.FromHtml("");
                        this.btnMonthView.BorderColor = (this.lastDay == 30) ? ColorTranslator.FromHtml("#1CA47D") : ColorTranslator.FromHtml("");
                    }
                    else
                    {
                        this.btnWeekView.BorderColor = ColorTranslator.FromHtml("");
                        this.btnMonthView.BorderColor = ColorTranslator.FromHtml("");
                    }
                }
            }
            else
            {
                if (this.txtBeginDate.TextToDate.HasValue)
                {
                    this.BeginDate = new DateTime?(this.txtBeginDate.TextToDate.Value);
                }
                if (this.txtEndDate.TextToDate.HasValue)
                {
                    this.EndDate = new DateTime?(this.txtEndDate.TextToDate.Value);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (base.GetUrlParam("BeginDate") == "")
            {
                string retInfo = "";
                ShopStatisticHelper.StatisticsOrdersByRecDate(DateTime.Today, UpdateAction.AllUpdate, 0, out retInfo);
            }
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
                this.exportFieldsCheckBoxList.Items.Clear();
                this.exportFieldsCheckBoxList.Items.Add(new ListItem("排名", "RankIndex"));
                this.exportFieldsCheckBoxList.Items.Add(new ListItem("商品名称", "ProductName"));
                this.exportFieldsCheckBoxList.Items.Add(new ListItem("销售量", "SaleQty"));
                this.exportFieldsCheckBoxList.Items.Add(new ListItem("销售额", "SaleAmountFee"));
                this.exportFieldsCheckBoxList.Items.Add(new ListItem("购买人数", "BuyerNumber"));
                this.exportFieldsCheckBoxList.Items.Add(new ListItem("转化率", "ConversionRate"));
            }
        }

        private void ReBind_Url(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("BeginDate", this.BeginDate.ToString());
            queryStrings.Add("EndDate", this.EndDate.ToString());
            queryStrings.Add("lastDay", this.lastDay.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

