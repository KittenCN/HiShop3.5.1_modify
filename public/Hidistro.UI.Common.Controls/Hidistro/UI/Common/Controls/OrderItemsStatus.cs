namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Orders;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderItemsStatus : Literal
    {
        public string GetOrderItemStatus(OrderStatus orderitem)
        {
            string str = "";
            switch (orderitem)
            {
                case OrderStatus.BuyerAlreadyPaid:
                    return string.Concat(new object[] { "<a class=\"btn-have\" href=\"RequestReturn.aspx?orderId=", this.OrderId, "&ProductId=", this.ProductId, "\">申请退款</a>" });

                case OrderStatus.SellerAlreadySent:
                    return string.Concat(new object[] { "<a class=\"btn-have\" href=\"RequestReturn.aspx?orderId=", this.OrderId, "&ProductId=", this.ProductId, "\">申请退货</a>" });

                case OrderStatus.Closed:
                case OrderStatus.Finished:
                case OrderStatus.ApplyForReplacement:
                    return str;

                case OrderStatus.ApplyForRefund:
                    return "<a class=\"btn-have\">退款审核中</a>";

                case OrderStatus.ApplyForReturns:
                    return "<a class=\"btn-have\">退货中</a>";

                case OrderStatus.Refunded:
                    return "<a class=\"btn-have\">退款完成</a>";

                case OrderStatus.Returned:
                    return "<a class=\"btn-have\">退货完成</a>";
            }
            return str;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = this.GetOrderItemStatus((OrderStatus) this.OrderStatusCode);
            base.Render(writer);
        }

        public object OrderId
        {
            get
            {
                return this.ViewState["OrderId"];
            }
            set
            {
                this.ViewState["OrderId"] = value;
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

        public object ProductId
        {
            get
            {
                return this.ViewState["ProductId"];
            }
            set
            {
                this.ViewState["ProductId"] = value;
            }
        }
    }
}

