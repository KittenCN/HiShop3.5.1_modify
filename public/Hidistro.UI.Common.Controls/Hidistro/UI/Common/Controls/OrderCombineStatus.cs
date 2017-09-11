namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderCombineStatus : Literal
    {
        private string _DetailUrl = string.Empty;
        private bool _IsShowToUser;
        private string _OrderID = string.Empty;
        private int _OrderItemID;
        private string _SkuID = string.Empty;
        private int _Type;

        public string GetOrderCombineStatus(OrderStatus orderStatusCode, OrderStatus OrderItemsStatusCode)
        {
            string orderStatusName = "";
            if (((this.Gateway != null) && (this.Gateway.ToString() == "hishop.plugins.payment.podrequest")) && (orderStatusCode == OrderStatus.WaitBuyerPay))
            {
                orderStatusName = "等待发货";
            }
            else if (this._IsShowToUser && (orderStatusCode == OrderStatus.SellerAlreadySent))
            {
                orderStatusName = "等待收货";
            }
            else
            {
                orderStatusName = OrderInfo.GetOrderStatusName(orderStatusCode);
            }
            if ((this._OrderItemID == 0) && (string.IsNullOrEmpty(this._OrderID) || string.IsNullOrEmpty(this._SkuID)))
            {
                return (orderStatusName ?? "");
            }
            string str2 = string.Empty;
            if (this._Type != 1)
            {
                RefundInfo info = RefundHelper.GetByOrderIdAndProductID(this._OrderID, 0, this._SkuID, this._OrderItemID);
                if (info != null)
                {
                    switch (info.HandleStatus)
                    {
                        case RefundInfo.Handlestatus.Applied:
                            str2 = "退款中";
                            break;

                        case RefundInfo.Handlestatus.Refunded:
                            str2 = "已退款";
                            if (orderStatusCode == OrderStatus.Closed)
                            {
                                str2 = "已关闭";
                            }
                            break;

                        case RefundInfo.Handlestatus.Refused:
                            str2 = "拒绝退款";
                            break;

                        case RefundInfo.Handlestatus.NoneAudit:
                            str2 = "退款待审核";
                            break;

                        case RefundInfo.Handlestatus.HasTheAudit:
                            str2 = "退款已审核";
                            break;

                        case RefundInfo.Handlestatus.NoRefund:
                            str2 = "待退款";
                            break;

                        case RefundInfo.Handlestatus.AuditNotThrough:
                            str2 = "拒绝退款";
                            break;

                        case RefundInfo.Handlestatus.RefuseRefunded:
                            str2 = "拒绝退款";
                            break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(this._DetailUrl) && !string.IsNullOrEmpty(str2))
            {
                switch (str2)
                {
                    case "已退款":
                    case "已关闭":
                        return ("<a style=\"margin-top:10px;background-color:#FFBB66; color:#fff\" href='" + this._DetailUrl + "' target='_blank'>" + str2 + "</a>");
                }
                return (orderStatusName + "<br/><a style=\"margin-top:10px;background-color:#FFBB66; color:#fff\" href='" + this._DetailUrl + "' target='_blank'>" + str2 + "</a>");
            }
            return (orderStatusName + "<br/>" + str2);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = this.GetOrderCombineStatus((OrderStatus) this.OrderStatusCode, (OrderStatus) this.OrderItemsStatusCode);
            base.Render(writer);
        }

        public string DetailUrl
        {
            get
            {
                return this._DetailUrl;
            }
            set
            {
                this._DetailUrl = value;
            }
        }

        public object Gateway { get; set; }

        public bool IsShowToUser
        {
            get
            {
                return this._IsShowToUser;
            }
            set
            {
                this._IsShowToUser = value;
            }
        }

        public string OrderID
        {
            get
            {
                return this._OrderID;
            }
            set
            {
                this._OrderID = value;
            }
        }

        public int OrderItemID
        {
            get
            {
                return this._OrderItemID;
            }
            set
            {
                this._OrderItemID = value;
            }
        }

        public object OrderItemsStatusCode
        {
            get
            {
                return this.ViewState["OrderItemsStatusCode"];
            }
            set
            {
                this.ViewState["OrderItemsStatusCode"] = value;
            }
        }

        public object OrderStatusCode
        {
            get
            {
                return this.ViewState["OrderStatusCode"];
            }
            set
            {
                this.ViewState["OrderStatusCode"] = value;
            }
        }

        public string SkuID
        {
            get
            {
                return this._SkuID;
            }
            set
            {
                this._SkuID = value;
            }
        }

        public int Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
            }
        }
    }
}

