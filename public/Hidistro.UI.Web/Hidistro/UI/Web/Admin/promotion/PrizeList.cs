namespace Hidistro.UI.Web.Admin.promotion
{
    using ASPNET.WebControls;
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using Hidistro.Vshop;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.WebControls;

    public class PrizeList : AdminPage
    {
        private string ActitivyTitle;
        protected Button AddrBtn;
        private string AddrReggion;
        protected Button batchDel;
        protected Button btnExportButton;
        protected Button btnSearchButton;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected Button ConfirmSend;
        private string EndDate;
        protected HiddenField HideRegion;
        protected HiddenField hidLogId;
        protected HiddenField hidPid;
        protected PageSize hrefPageSize;
        protected LinkButton ListAll;
        protected LinkButton ListHasReceive;
        protected LinkButton ListHasSend;
        protected LinkButton ListWaitAddr;
        protected LinkButton ListWaitSend;
        protected Pager pager;
        private string ProductName;
        private string Receiver;
        protected Repeater reDistributor;
        protected RegionSelector SelReggion;
        protected Button SendBtn;
        protected string ShowTabNum;
        private string StartDate;
        protected HiddenField txtDeliverID;
        protected HiddenField txtDeliverStaus;
        protected HiddenField txtDeliveryTime;
        protected HiddenField txtLogID;
        protected HiddenField txtLogPid;
        protected TextBox txtPrizeAddress;
        protected TextBox txtPrizeCourierNumber;
        protected DropDownList txtPrizeExpressName;
        protected TextBox txtPrizeReceiver;
        protected TextBox txtPrizeTel;
        protected TextBox txtProductName;
        protected TextBox txtReceiver;
        protected TextBox txtTitle;

        protected PrizeList() : base("m08", "yxp16")
        {
            this.Receiver = "";
            this.StartDate = "";
            this.EndDate = "";
            this.ProductName = "";
            this.AddrReggion = "";
            this.ActitivyTitle = "";
            this.ShowTabNum = "0";
        }

        protected void AddrBtn_Click(object sender, EventArgs e)
        {
            if (this.txtLogID.Value == "")
            {
                this.ShowMsg("LogID为空，请检查！", false);
            }
            else
            {
                string str = this.txtPrizeReceiver.Text.Trim();
                string str2 = this.txtPrizeTel.Text.Trim();
                string str3 = this.txtPrizeAddress.Text.Trim();
                string s = this.HideRegion.Value.Trim();
                if (str.Length < 2)
                {
                    this.ShowMsg("收货人不能为空", false);
                }
                else if (str2.Length < 8)
                {
                    this.ShowMsg("联系电话不正确", false);
                }
                else if (str3.Length < 6)
                {
                    this.ShowMsg("地址不够详细", false);
                }
                else
                {
                    int result = 0;
                    if (!int.TryParse(s, out result))
                    {
                        this.ShowMsg("省市区未选择", false);
                    }
                    else
                    {
                        s = RegionHelper.GetFullPath(result);
                        PrizesDeliveQuery query = new PrizesDeliveQuery {
                            Status = 1,
                            ReggionPath = s,
                            Address = str3,
                            Tel = str2,
                            Receiver = str,
                            LogId = this.txtLogID.Value,
                            Pid = this.txtLogPid.Value
                        };
                        int num2 = 0;
                        int.TryParse(this.txtDeliverID.Value.Trim(), out num2);
                        query.Id = num2;
                        if (query.LogId == "0")
                        {
                            if (GameHelper.UpdateOneyuanDelivery(query))
                            {
                                this.ShowMsg("保存收货人信息成功！", true);
                                this.BindData();
                            }
                            else
                            {
                                this.ShowMsg("保存信息失败", false);
                            }
                        }
                        else if (GameHelper.UpdatePrizesDelivery(query))
                        {
                            this.ShowMsg("保存收货人信息成功！", true);
                            this.BindData();
                        }
                        else
                        {
                            this.ShowMsg("保存信息失败", false);
                        }
                    }
                }
            }
        }

        protected void batchDel_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选择要删除的数据项", false);
            }
            else if (GameHelper.DeletePrizesDelivery(Array.ConvertAll<string, int>(str.Split(new char[] { ',' }), s => int.Parse(s))))
            {
                this.ShowMsg("删除成功！", true);
                this.BindData();
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void bidExpress()
        {
            DataTable expressTable = ExpressHelper.GetExpressTable();
            this.txtPrizeExpressName.Items.Clear();
            this.txtPrizeExpressName.Items.Add(new ListItem("----请选择快递公司----", ""));
            foreach (DataRow row in expressTable.Rows)
            {
                this.txtPrizeExpressName.Items.Add(new ListItem((string) row["Name"], (string) row["Name"]));
            }
        }

        private void BindData()
        {
            PrizesDeliveQuery entity = new PrizesDeliveQuery {
                Status = int.Parse(this.ShowTabNum) - 1,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                SortBy = "WinTime",
                PrizeType = 2,
                SortOrder = SortAction.Desc,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                ProductName = this.ProductName,
                Receiver = this.Receiver,
                ReggionId = this.AddrReggion,
                ActivityTitle = this.ActitivyTitle
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = GameHelper.GetAllPrizesDeliveryList(entity, "", "*");
            this.reDistributor.DataSource = result.Data;
            this.reDistributor.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
            this.bidExpress();
            DataTable prizesDeliveryNum = GameHelper.GetPrizesDeliveryNum();
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            if ((prizesDeliveryNum != null) && (prizesDeliveryNum.Rows.Count > 0))
            {
                num = int.Parse(prizesDeliveryNum.Rows[0]["st0"].ToString());
                num2 = int.Parse(prizesDeliveryNum.Rows[0]["st1"].ToString());
                num3 = int.Parse(prizesDeliveryNum.Rows[0]["st2"].ToString());
                num4 = int.Parse(prizesDeliveryNum.Rows[0]["st3"].ToString());
                num5 = ((num + num2) + num3) + num4;
            }
            this.ListAll.Text = "所有奖品(" + num5 + ")";
            this.ListWaitAddr.Text = "待填写收货地址(" + num + ")";
            this.ListWaitSend.Text = "待发货(" + num2 + ")";
            this.ListHasSend.Text = "已发货(" + num3 + ")";
            this.ListHasReceive.Text = "已收货(" + num4 + ")";
        }

        protected void btnExportButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ActitivyTitle"]))
            {
                this.ActitivyTitle = this.Page.Request.QueryString["ActitivyTitle"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AddrReggion"]))
            {
                this.AddrReggion = base.Server.UrlDecode(this.Page.Request.QueryString["AddrReggion"]);
                this.SelReggion.SetSelectedRegionId(new int?(int.Parse(this.AddrReggion)));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShowTabNum"]))
            {
                this.ShowTabNum = base.Server.UrlDecode(this.Page.Request.QueryString["ShowTabNum"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Receiver"]))
            {
                this.Receiver = base.Server.UrlDecode(this.Page.Request.QueryString["Receiver"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
            {
                this.ProductName = base.Server.UrlDecode(this.Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                this.StartDate = base.Server.UrlDecode(this.Page.Request.QueryString["StartDate"]);
                this.calendarStartDate.SelectedDate = new DateTime?(DateTime.Parse(this.StartDate));
            }
            else
            {
                this.StartDate = "";
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                this.EndDate = base.Server.UrlDecode(this.Page.Request.QueryString["EndDate"]);
                this.calendarEndDate.SelectedDate = new DateTime?(DateTime.Parse(this.EndDate));
            }
            else
            {
                this.EndDate = "";
            }
            PrizesDeliveQuery entity = new PrizesDeliveQuery {
                Status = int.Parse(this.ShowTabNum) - 1,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                SortBy = "WinTime",
                PrizeType = 2,
                SortOrder = SortAction.Desc,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                ProductName = this.ProductName,
                Receiver = this.Receiver,
                ReggionId = this.AddrReggion,
                ActivityTitle = this.ActitivyTitle
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = GameHelper.GetAllPrizesDeliveryList(entity, "", "*");
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >活动名称</td>");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >商品名称</td>");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >奖品等级</td>");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >中奖时间</td>");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >收货人</td>");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >联系电话</td>");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >详细地址</td>");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >快递公司</td>");
            builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >快递编号</td>");
            builder.AppendLine("</tr>");
            DataTable data = (DataTable) result.Data;
            string str = string.Empty;
            string str2 = string.Empty;
            new List<int>();
            if (!string.IsNullOrEmpty(this.hidLogId.Value) || !string.IsNullOrEmpty(this.hidPid.Value))
            {
                str = this.hidLogId.Value;
                str2 = this.hidPid.Value;
                string[] collection = str.Split(new char[] { ',' });
                string[] strArray2 = str2.Split(new char[] { ',' });
                List<string> list = new List<string>(collection);
                List<string> list2 = new List<string>(strArray2);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string item = data.Rows[i]["LogId"].ToString();
                    string str4 = data.Rows[i]["Pid"].ToString();
                    if (list.Contains(item) || list2.Contains(str4))
                    {
                        builder.AppendLine("<tr>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[i]["Title"] + "</td>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[i]["ProductName"] + "</td>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + GameHelper.GetPrizeGradeName(data.Rows[i]["PrizeGrade"].ToString()) + "</td>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[i]["WinTime"] + "</td>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[i]["Receiver"] + "</td>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[i]["Tel"] + "</td>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[i]["Address"] + "</td>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[i]["ExpressName"] + "</td>");
                        builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[i]["CourierNumber"] + "</td>");
                        builder.AppendLine("</tr>");
                    }
                }
            }
            else
            {
                for (int j = 0; j < data.Rows.Count; j++)
                {
                    data.Rows[j]["LogId"].ToString();
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[j]["Title"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[j]["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + GameHelper.GetPrizeGradeName(data.Rows[j]["PrizeGrade"].ToString()) + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[j]["WinTime"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[j]["Receiver"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[j]["Tel"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[j]["Address"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[j]["ExpressName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat: @;\" >" + data.Rows[j]["CourierNumber"] + "</td>");
                    builder.AppendLine("</tr>");
                }
            }
            if (data.Rows.Count == 0)
            {
                builder.AppendLine("<tr><td></td></tr>");
            }
            builder.AppendLine("</table>");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "UTF-8";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=PrizeLists_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            this.Page.Response.ContentType = "application/ms-excel";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                this.EndDate = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                this.StartDate = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            if (this.SelReggion.GetSelectedRegionId().HasValue)
            {
                this.AddrReggion = this.SelReggion.GetSelectedRegionId().Value.ToString();
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShowTabNum"]))
            {
                this.ShowTabNum = base.Server.UrlDecode(this.Page.Request.QueryString["ShowTabNum"]);
            }
            this.ReBind(true);
        }

        protected void ConfirmSend_Click(object sender, EventArgs e)
        {
            if (this.txtLogID.Value == "")
            {
                this.ShowMsg("LogID为空，请检查！", false);
            }
            else
            {
                int result = 0;
                if (!int.TryParse(this.txtDeliverID.Value.Trim(), out result) || (this.txtDeliverStaus.Value != "2"))
                {
                    this.ShowMsg("当前状态下不允许操作！", false);
                }
                else
                {
                    PrizesDeliveQuery query = new PrizesDeliveQuery {
                        Status = 3,
                        ReceiveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        LogId = this.txtLogID.Value,
                        Id = result
                    };
                    if (GameHelper.UpdatePrizesDelivery(query))
                    {
                        this.ShowMsg("收货在确认成功！", true);
                        this.BindData();
                    }
                    else
                    {
                        this.ShowMsg("收货在确认成功失败", false);
                    }
                }
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                this.AddrReggion = "";
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ActitivyTitle"]))
                {
                    this.ActitivyTitle = this.Page.Request.QueryString["ActitivyTitle"];
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["AddrReggion"]))
                {
                    this.AddrReggion = base.Server.UrlDecode(this.Page.Request.QueryString["AddrReggion"]);
                    this.SelReggion.SetSelectedRegionId(new int?(int.Parse(this.AddrReggion)));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShowTabNum"]))
                {
                    this.ShowTabNum = base.Server.UrlDecode(this.Page.Request.QueryString["ShowTabNum"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Receiver"]))
                {
                    this.Receiver = base.Server.UrlDecode(this.Page.Request.QueryString["Receiver"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
                {
                    this.ProductName = base.Server.UrlDecode(this.Page.Request.QueryString["ProductName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
                {
                    this.StartDate = base.Server.UrlDecode(this.Page.Request.QueryString["StartDate"]);
                    this.calendarStartDate.SelectedDate = new DateTime?(DateTime.Parse(this.StartDate));
                }
                else
                {
                    this.StartDate = "";
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
                {
                    this.EndDate = base.Server.UrlDecode(this.Page.Request.QueryString["EndDate"]);
                    this.calendarEndDate.SelectedDate = new DateTime?(DateTime.Parse(this.EndDate));
                }
                else
                {
                    this.EndDate = "";
                }
                this.txtReceiver.Text = this.Receiver;
                this.txtProductName.Text = this.ProductName;
                this.txtTitle.Text = this.ActitivyTitle;
            }
            else
            {
                if (this.calendarStartDate.SelectedDate.HasValue)
                {
                    this.StartDate = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                if (this.calendarEndDate.SelectedDate.HasValue)
                {
                    this.EndDate = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                }
                this.Receiver = this.txtReceiver.Text;
                this.ProductName = this.txtProductName.Text;
                this.AddrReggion = "";
                this.ActitivyTitle = this.txtTitle.Text;
                if (this.SelReggion.GetSelectedRegionId().HasValue)
                {
                    this.AddrReggion = this.SelReggion.GetSelectedRegionId().Value.ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.AddrBtn.Click += new EventHandler(this.AddrBtn_Click);
            this.SendBtn.Click += new EventHandler(this.SendBtn_Click);
            this.ConfirmSend.Click += new EventHandler(this.ConfirmSend_Click);
            this.batchDel.Click += new EventHandler(this.batchDel_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Receiver", this.txtReceiver.Text);
            queryStrings.Add("ProductName", this.txtProductName.Text);
            queryStrings.Add("StartDate", this.StartDate);
            queryStrings.Add("EndDate", this.EndDate);
            queryStrings.Add("AddrReggion", this.AddrReggion);
            queryStrings.Add("ShowTabNum", this.ShowTabNum);
            queryStrings.Add("ActitivyTitle", this.ActitivyTitle);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        protected void SendBtn_Click(object sender, EventArgs e)
        {
            if (this.txtLogID.Value == "")
            {
                this.ShowMsg("LogID为空，请检查！", false);
            }
            else
            {
                int result = 0;
                if ((!int.TryParse(this.txtDeliverID.Value.Trim(), out result) || (this.txtDeliverStaus.Value == "")) || ((this.txtDeliverStaus.Value == "3") || (this.txtDeliverStaus.Value == "0")))
                {
                    this.ShowMsg("当前状态下不允许操作！", false);
                }
                else
                {
                    string str = this.txtDeliveryTime.Value.Trim();
                    string str2 = this.txtPrizeExpressName.Text.Trim();
                    string str3 = this.txtPrizeCourierNumber.Text.Trim();
                    if (str2.Length < 2)
                    {
                        this.ShowMsg("快递公司名称有误！", false);
                    }
                    else if (str3.Length < 6)
                    {
                        this.ShowMsg("快递单号不正确！", false);
                    }
                    else
                    {
                        PrizesDeliveQuery query = new PrizesDeliveQuery {
                            Status = 2,
                            ExpressName = str2,
                            CourierNumber = str3
                        };
                        if (str == "")
                        {
                            query.DeliveryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        query.LogId = this.txtLogID.Value;
                        query.Id = result;
                        if (GameHelper.UpdatePrizesDelivery(query))
                        {
                            this.ShowMsg("奖品发货信息保存成功！", true);
                            this.BindData();
                        }
                        else
                        {
                            this.ShowMsg("奖品发货信息保存失败", false);
                        }
                    }
                }
            }
        }

        protected void tabClick(object sender, EventArgs e)
        {
            string commandName = ((LinkButton) sender).CommandName;
            base.Response.Write(commandName);
            this.txtReceiver.Text = "";
            this.txtProductName.Text = "";
            this.ShowTabNum = commandName;
            this.AddrReggion = "";
            this.StartDate = "";
            this.EndDate = "";
            this.ReBind(true);
        }
    }
}

