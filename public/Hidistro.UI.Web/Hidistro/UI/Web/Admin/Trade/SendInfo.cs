namespace Hidistro.UI.Web.Admin.Trade
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SendInfo : AdminPage
    {
        protected Button btnDeleteCheck;
        protected Button btnRemark;
        protected HtmlInputHidden hidOrderId;
        protected HyperLink hlinkAllOrder;
        protected HyperLink hlinkNotPay;
        protected HyperLink hlinkYetPay;
        protected PageSize hrefPageSize;
        protected FormatedMoneyLabel lblOrderTotalForRemark;
        protected Label lblStatus;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        protected Pager pager;
        protected string Reurl;
        protected Repeater rptList;
        protected HtmlInputText txtcategoryId;
        protected TextBox txtRemark;

        protected SendInfo() : base("m03", "ddp08")
        {
            this.Reurl = string.Empty;
        }

        private void BindOrders()
        {
            OrderQuery orderQuery = this.GetOrderQuery();
            DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
            this.rptList.DataSource = orders.Data;
            this.rptList.DataBind();
            this.pager.TotalRecords = orders.TotalRecords;
            this.lblStatus.Text = ((int) orderQuery.Status).ToString();
        }

        protected void btnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请先选择要删除的订单", false);
            }
            else
            {
                int num = OrderHelper.DeleteOrders("'" + str.Replace(",", "','") + "'");
                this.BindOrders();
                this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
            }
        }

        private void btnOrderGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
            }
            else
            {
                List<string> list = new List<string>();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    list.Add("'" + str2 + "'");
                }
                DataSet orderGoods = OrderHelper.GetOrderGoods(string.Join(",", list.ToArray()));
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                builder.AppendLine("<td>订单单号</td>");
                builder.AppendLine("<td>商品名称</td>");
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("<td>备注</td>");
                builder.AppendLine("</tr>");
                foreach (DataRow row in orderGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"] + "</td>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("<td>" + row["Remark"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }

        private void btnProductGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
            }
            else
            {
                List<string> list = new List<string>();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    list.Add("'" + str2 + "'");
                }
                DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                builder.AppendLine("<td>商品名称</td>");
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("</tr>");
                foreach (DataRow row in productGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (this.hidOrderId.Value == "0")
            {
                string str = Globals.RequestFormStr("CheckBoxGroup");
                if (str.Length <= 0)
                {
                    this.ShowMsg("请先选择要批量备注的订单", false);
                }
                else if (this.txtRemark.Text.Length > 300)
                {
                    this.ShowMsg("备注长度限制在300个字符以内", false);
                }
                else
                {
                    foreach (string str2 in str.Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            OrderInfo orderInfo = OrderHelper.GetOrderInfo(str2);
                            orderInfo.OrderId = str2;
                            if (this.orderRemarkImageForRemark.SelectedItem != null)
                            {
                                orderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                            }
                            orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                            OrderHelper.SaveRemark(orderInfo);
                        }
                    }
                    this.ShowMsg("批量保存备注成功", true);
                    this.BindOrders();
                }
            }
            else if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备注长度限制在300个字符以内", false);
            }
            else
            {
                OrderInfo order = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
                order.OrderId = this.hidOrderId.Value;
                if (this.orderRemarkImageForRemark.SelectedItem != null)
                {
                    order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                }
                order.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                if (OrderHelper.SaveRemark(order))
                {
                    this.BindOrders();
                    this.ShowMsg("保存备注成功", true);
                }
                else
                {
                    this.ShowMsg("保存失败", false);
                }
            }
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要发货的订单", false);
            }
            else
            {
                this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/BatchSendOrderGoods.aspx?OrderIds=" + str));
            }
        }

        private OrderQuery GetOrderQuery()
        {
            int num3;
            int num4;
            OrderQuery query = new OrderQuery {
                Status = OrderStatus.BuyerAlreadyPaid
            };
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
            {
                int num = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out num))
                {
                    query.IsPrinted = new int?(num);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
            {
                int num2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["ModeId"], out num2))
                {
                    query.ShippingModeId = new int?(num2);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out num3))
            {
                query.RegionId = new int?(num3);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && int.TryParse(this.Page.Request.QueryString["UserId"], out num4))
            {
                query.UserId = new int?(num4);
            }
            int result = 0;
            if (int.TryParse(base.Request.QueryString["orderType"], out result) && (result > 0))
            {
                query.Type = new OrderQuery.OrderType?((OrderQuery.OrderType) result);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
            {
                query.StoreName = Globals.UrlDecode(this.Page.Request.QueryString["StoreName"]);
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "OrderDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Globals.RequestFormStr("isCallback"))
            {
                case "true":
                {
                    int num;
                    string str2;
                    string str3;
                    if (string.IsNullOrEmpty(base.Request["ReturnsId"]))
                    {
                        base.Response.Write("{\"Status\":\"0\"}");
                        base.Response.End();
                        return;
                    }
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request["orderId"]);
                    StringBuilder builder = new StringBuilder();
                    if (base.Request["type"] == "refund")
                    {
                        RefundHelper.GetRefundType(base.Request["orderId"], out num, out str3);
                    }
                    else
                    {
                        num = 0;
                        str3 = "";
                    }
                    if (num == 1)
                    {
                        str2 = "退到预存款";
                    }
                    else
                    {
                        str2 = "银行转帐";
                    }
                    builder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(orderInfo.GetTotal()));
                    if (!(base.Request["type"] == "replace"))
                    {
                        builder.AppendFormat(",\"RefundType\":\"{0}\"", num);
                        builder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", str2);
                    }
                    builder.AppendFormat(",\"Contacts\":\"{0}\"", orderInfo.ShipTo);
                    builder.AppendFormat(",\"Email\":\"{0}\"", orderInfo.EmailAddress);
                    builder.AppendFormat(",\"Telephone\":\"{0}\"", (orderInfo.TelPhone + " " + orderInfo.CellPhone).Trim());
                    builder.AppendFormat(",\"Address\":\"{0}\"", orderInfo.Address);
                    builder.AppendFormat(",\"Remark\":\"{0}\"", str3.Replace("\r\n", ""));
                    builder.AppendFormat(",\"PostCode\":\"{0}\"", orderInfo.ZipCode);
                    builder.AppendFormat(",\"GroupBuyId\":\"{0}\"", (orderInfo.GroupBuyId > 0) ? orderInfo.GroupBuyId : 0);
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                    base.Response.End();
                    break;
                }
                case "GetOrdersStates":
                {
                    base.Response.ContentType = "application/json";
                    DataTable allOrderID = OrderHelper.GetAllOrderID();
                    int count = allOrderID.Rows.Count;
                    int length = allOrderID.Select("OrderStatus=" + 1).Length;
                    int num4 = allOrderID.Select("OrderStatus=" + 2).Length;
                    int num5 = allOrderID.Select("OrderStatus=" + 3).Length;
                    int num6 = allOrderID.Select("OrderStatus=" + 5).Length;
                    int num7 = allOrderID.Select("OrderStatus=" + 7).Length;
                    int num8 = allOrderID.Select("OrderStatus=" + 4).Length;
                    string s = string.Concat(new object[] { "{\"type\":\"1\",\"allcount\":", count, ",\"waibuyerpaycount\":", length, ",\"buyalreadypaidcount\":", num4, ",\"sellalreadysentcount\":", num5, ",\"finishedcount\":", num6, ",\"applyforreturnscount\":", num7, ",\"closedcount\":", num8, "}" });
                    base.Response.Write(s);
                    base.Response.End();
                    break;
                }
            }
            this.Reurl = base.Request.Url.ToString();
            if (!this.Reurl.Contains("?"))
            {
                this.Reurl = this.Reurl + "?pageindex=1";
            }
            this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
            this.btnDeleteCheck.Click += new EventHandler(this.btnDeleteCheck_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindOrders();
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(e.CommandArgument.ToString());
            if (orderInfo != null)
            {
                if ((e.CommandName == "CONFIRM_PAY") && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
                {
                    if (orderInfo.GroupBuyId <= 0)
                    {
                    }
                }
                else if (e.CommandName == "FINISH_TRADE")
                {
                    orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE);
                }
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater repeater = (Repeater) e.Item.FindControl("rptSubList");
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(DataBinder.Eval(e.Item.DataItem, "OrderID").ToString());
                if ((orderInfo != null) && (orderInfo.LineItems.Count > 0))
                {
                    repeater.DataSource = orderInfo.LineItems.Values;
                    repeater.DataBind();
                }
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Item.DataItem, "OrderStatus");
                if (!(DataBinder.Eval(e.Item.DataItem, "Gateway") is DBNull))
                {
                    string text1 = (string) DataBinder.Eval(e.Item.DataItem, "Gateway");
                }
                int num = (DataBinder.Eval(e.Item.DataItem, "GroupBuyId") != DBNull.Value) ? ((int) DataBinder.Eval(e.Item.DataItem, "GroupBuyId")) : 0;
                HyperLink link = (HyperLink) e.Item.FindControl("lkbtnEditPrice");
                if (status == OrderStatus.WaitBuyerPay)
                {
                    link.Visible = true;
                }
                if (num > 0)
                {
                    GroupBuyStatus status1 = (GroupBuyStatus) DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
                }
            }
        }
    }
}

