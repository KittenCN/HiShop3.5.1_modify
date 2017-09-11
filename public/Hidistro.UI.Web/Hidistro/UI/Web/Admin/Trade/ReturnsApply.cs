namespace Hidistro.UI.Web.Admin.Trade
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Collections.Specialized;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ReturnsApply : AdminPage
    {
        protected Button btnAcceptRefund;
        protected Button btnAcceptReplace;
        protected Button btnAcceptReturn;
        protected Button btnCloseOrder;
        protected Button btnDeleteCheck;
        protected Button btnOrderGoods;
        protected Button btnProductGoods;
        protected Button btnRefuseRefund;
        protected Button btnRefuseReplace;
        protected Button btnRefuseReturn;
        protected Button btnRemark;
        protected Button btnSearchButton;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected CloseTranReasonDropDownList ddlCloseReason;
        protected RegionSelector dropRegion;
        protected HtmlInputHidden hidAdminRemark;
        protected HtmlInputHidden hidOrderId;
        protected HtmlInputHidden hidOrderTotal;
        protected HtmlInputHidden hidRefundMoney;
        protected HtmlInputHidden hidRefundType;
        protected PageSize hrefPageSize;
        protected Label lblAddress;
        protected Label lblContacts;
        protected Label lblEmail;
        protected Label lblOrderId;
        protected Label lblOrderTotal;
        protected FormatedMoneyLabel lblOrderTotalForRemark;
        protected Label lblRefundRemark;
        protected Label lblRefundType;
        protected Label lblStatus;
        protected Label lblTelephone;
        protected DropDownList OrderFromList;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        protected Pager pager;
        protected Label replace_lblAddress;
        protected Label replace_lblComments;
        protected Label replace_lblContacts;
        protected Label replace_lblEmail;
        protected Label replace_lblOrderId;
        protected Label replace_lblOrderTotal;
        protected Label replace_lblPostCode;
        protected Label replace_lblTelephone;
        protected TextBox replace_txtAdminRemark;
        protected Label return_lblAddress;
        protected Label return_lblContacts;
        protected Label return_lblEmail;
        protected Label return_lblOrderId;
        protected Label return_lblOrderTotal;
        protected Label return_lblRefundType;
        protected Label return_lblReturnRemark;
        protected Label return_lblTelephone;
        protected TextBox return_txtAdminRemark;
        protected TextBox return_txtRefundMoney;
        protected string Reurl;
        protected Repeater rptList;
        protected TextBox txtAdminRemark;
        protected HtmlInputText txtcategoryId;
        protected TextBox txtOrderId;
        protected TextBox txtProductName;
        protected TextBox txtRemark;
        protected TextBox txtShopName;
        protected TextBox txtShopTo;
        protected TextBox txtUserName;

        protected ReturnsApply() : base("m03", "ddp12")
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
            this.txtUserName.Text = orderQuery.UserName;
            this.txtOrderId.Text = orderQuery.OrderId;
            this.txtProductName.Text = orderQuery.ProductName;
            this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.lblStatus.Text = ((int) orderQuery.Status).ToString();
            if (orderQuery.RegionId.HasValue)
            {
                this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
            }
            this.txtShopName.Text = orderQuery.StoreName;
        }

        private void bindOrderType()
        {
            int result = 0;
            int.TryParse(base.Request.QueryString["orderType"], out result);
            this.OrderFromList.SelectedIndex = result;
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

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadOrders(true);
        }

        private OrderQuery GetOrderQuery()
        {
            int num3;
            int num4;
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
            {
                query.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
            {
                query.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                query.StartDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                query.EndDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["EndDate"]).AddMilliseconds(86399.0));
            }
            query.OrderItemsStatus = (OrderStatus)6;
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
            if (Globals.RequestFormStr("isCallback") == "true")
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
            }
            this.Reurl = base.Request.Url.ToString();
            if (!this.Reurl.Contains("?"))
            {
                this.Reurl = this.Reurl + "?pageindex=1";
            }
            this.Reurl = Regex.Replace(this.Reurl, @"&t=(\d+)", "");
            this.Reurl = Regex.Replace(this.Reurl, @"(\?)t=(\d+)", "?");
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            if (!this.Page.IsPostBack)
            {
                this.bindOrderType();
                this.BindOrders();
            }
        }

        private void ReloadOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("UserName", this.txtUserName.Text);
            queryStrings.Add("OrderId", this.txtOrderId.Text);
            queryStrings.Add("ProductName", this.txtProductName.Text);
            queryStrings.Add("ShipTo", this.txtShopTo.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            queryStrings.Add("OrderType", this.OrderFromList.SelectedValue);
            queryStrings.Add("OrderStatus", this.lblStatus.Text);
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                queryStrings.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            if (this.dropRegion.GetSelectedRegionId().HasValue)
            {
                queryStrings.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
            }
            queryStrings.Add("StoreName", this.txtShopName.Text.Trim());
            base.ReloadPage(queryStrings);
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
                string str = "";
                if (!(DataBinder.Eval(e.Item.DataItem, "Gateway") is DBNull))
                {
                    str = (string) DataBinder.Eval(e.Item.DataItem, "Gateway");
                }
                int num = (DataBinder.Eval(e.Item.DataItem, "GroupBuyId") != DBNull.Value) ? ((int) DataBinder.Eval(e.Item.DataItem, "GroupBuyId")) : 0;
                HtmlInputButton button = (HtmlInputButton) e.Item.FindControl("btnModifyPrice");
                HtmlInputButton button2 = (HtmlInputButton) e.Item.FindControl("btnSendGoods");
                Button button3 = (Button) e.Item.FindControl("btnPayOrder");
                Button button4 = (Button) e.Item.FindControl("btnConfirmOrder");
                HtmlInputButton button5 = (HtmlInputButton) e.Item.FindControl("btnCloseOrderClient");
                HtmlAnchor anchor1 = (HtmlAnchor) e.Item.FindControl("lkbtnCheckRefund");
                HtmlAnchor anchor2 = (HtmlAnchor) e.Item.FindControl("lkbtnCheckReturn");
                HtmlAnchor anchor3 = (HtmlAnchor) e.Item.FindControl("lkbtnCheckReplace");
                Literal literal = (Literal) e.Item.FindControl("WeiXinNickName");
                Literal literal2 = (Literal) e.Item.FindControl("litOtherInfo");
                int totalPointNumber = orderInfo.GetTotalPointNumber();
                MemberInfo member = MemberProcessor.GetMember(orderInfo.UserId, true);
                if (member != null)
                {
                    literal.Text = "买家：" + member.UserName;
                }
                StringBuilder builder = new StringBuilder();
                decimal total = orderInfo.GetTotal();
                if (total > 0M)
                {
                    builder.Append("<strong>￥" + total.ToString("F2") + "</strong>");
                    builder.Append("<small>(含运费￥" + orderInfo.AdjustedFreight.ToString("F2") + ")</small>");
                }
                if (totalPointNumber > 0)
                {
                    builder.Append("<small>" + totalPointNumber.ToString() + "积分</small>");
                }
                if (orderInfo.PaymentType == "货到付款")
                {
                    builder.Append("<span class=\"setColor bl\"><strong>货到付款</strong></span>");
                }
                if (string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append("<strong>￥" + total.ToString("F2") + "</strong>");
                }
                literal2.Text = builder.ToString();
                if (status == OrderStatus.WaitBuyerPay)
                {
                    button.Visible = true;
                    button.Attributes.Add("onclick", "DialogFrame('../trade/EditOrder.aspx?OrderId=" + DataBinder.Eval(e.Item.DataItem, "OrderID") + "&reurl='+ encodeURIComponent(goUrl),'修改订单价格',900,450)");
                    button5.Attributes.Add("onclick", "CloseOrderFun('" + DataBinder.Eval(e.Item.DataItem, "OrderID") + "')");
                    button5.Visible = true;
                    if (str != "hishop.plugins.payment.podrequest")
                    {
                        button3.Visible = true;
                    }
                }
                if (num > 0)
                {
                    GroupBuyStatus status2 = (GroupBuyStatus) DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
                    button2.Visible = (status == OrderStatus.BuyerAlreadyPaid) && (status2 == GroupBuyStatus.Success);
                }
                else
                {
                    button2.Visible = (status == OrderStatus.BuyerAlreadyPaid) || ((status == OrderStatus.WaitBuyerPay) && (str == "hishop.plugins.payment.podrequest"));
                }
                button2.Attributes.Add("onclick", "DialogFrame('../trade/SendOrderGoods.aspx?OrderId=" + DataBinder.Eval(e.Item.DataItem, "OrderID") + "&reurl='+ encodeURIComponent(goUrl),'订单发货',750,220)");
                button4.Visible = status == OrderStatus.SellerAlreadySent;
            }
        }

        protected void rptSubList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                OrderCombineStatus status = (OrderCombineStatus) e.Item.FindControl("lbOrderCombineStatus");
                status.OrderItemID = Globals.ToNum(DataBinder.Eval(e.Item.DataItem, "ID").ToString());
                status.OrderID = DataBinder.Eval(e.Item.DataItem, "OrderID").ToString();
                status.SkuID = DataBinder.Eval(e.Item.DataItem, "SkuID").ToString();
                status.Type = Globals.ToNum(DataBinder.Eval(e.Item.DataItem, "Type").ToString());
                status.DetailUrl = "OrderDetails.aspx?OrderId=" + DataBinder.Eval(e.Item.DataItem, "OrderID").ToString() + "#returnInfo";
            }
        }
    }
}

