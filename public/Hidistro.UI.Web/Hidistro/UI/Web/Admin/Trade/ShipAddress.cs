namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class ShipAddress : AdminPage
    {
        private string action;
        protected Button btnMondifyAddress;
        protected RegionSelector dropRegions;
        protected Literal lblOriAddress;
        private string orderId;
        protected TextBox txtAddress;
        protected TextBox txtCellPhone;
        protected TextBox txtShipTo;
        protected TextBox txtTelPhone;
        protected TextBox txtZipcode;

        protected ShipAddress() : base("m01", "00000")
        {
            this.action = "update";
        }

        private void BindUpdateSippingAddress(OrderInfo order)
        {
            this.txtShipTo.Text = order.ShipTo;
            this.dropRegions.SetSelectedRegionId(new int?(order.RegionId));
            this.txtAddress.Text = order.Address;
            this.txtZipcode.Text = order.ZipCode;
            this.txtTelPhone.Text = order.TelPhone;
            this.txtCellPhone.Text = order.CellPhone;
            string oldAddress = order.OldAddress;
            if (string.IsNullOrEmpty(oldAddress))
            {
                if (!string.IsNullOrEmpty(order.ShippingRegion))
                {
                    oldAddress = order.ShippingRegion.Replace(',', ' ');
                }
                if (!string.IsNullOrEmpty(order.Address))
                {
                    oldAddress = oldAddress + order.Address;
                }
                if (!string.IsNullOrEmpty(order.ShipTo))
                {
                    oldAddress = oldAddress + "，" + order.ShipTo;
                }
                if (!string.IsNullOrEmpty(order.TelPhone))
                {
                    oldAddress = oldAddress + "，" + order.TelPhone;
                }
                if (!string.IsNullOrEmpty(order.CellPhone))
                {
                    oldAddress = oldAddress + "，" + order.CellPhone;
                }
            }
            this.lblOriAddress.Text = oldAddress;
        }

        private void btnMondifyAddress_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
            orderInfo.ShipTo = this.txtShipTo.Text.Trim();
            orderInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
            orderInfo.Address = this.txtAddress.Text.Trim();
            orderInfo.TelPhone = this.txtTelPhone.Text.Trim();
            orderInfo.CellPhone = this.txtCellPhone.Text.Trim();
            orderInfo.ZipCode = this.txtZipcode.Text.Trim();
            orderInfo.ShippingRegion = this.dropRegions.SelectedRegions;
            if (string.IsNullOrEmpty(orderInfo.OldAddress))
            {
                orderInfo.OldAddress = this.lblOriAddress.Text;
            }
            if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
            {
                this.ShowMsgToTarget("电话号码和手机号码必填其一", false, "parent");
            }
            else if (this.action == "update")
            {
                orderInfo.OrderId = this.orderId;
                if (OrderHelper.MondifyAddress(orderInfo))
                {
                    OrderHelper.GetOrderInfo(this.orderId);
                    this.ShowMsgAndReUrl("收货地址修改成功", true, "OrderDetails.aspx?OrderId=" + this.orderId, "parent");
                }
                else
                {
                    this.ShowMsgToTarget("收货地址修改失败", false, "parent");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.orderId = Globals.RequestQueryStr("OrderId");
            if (string.IsNullOrEmpty(this.orderId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                if (!base.IsPostBack)
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
                    this.BindUpdateSippingAddress(orderInfo);
                }
                this.btnMondifyAddress.Click += new EventHandler(this.btnMondifyAddress_Click);
            }
        }
    }
}

