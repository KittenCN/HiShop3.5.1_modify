namespace Hidistro.UI.Web.Admin.Settings.flex
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using System;
    using System.Text;
    using System.Web.UI;

    public class GetPrintData : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.Form["shipperId"];
            string str2 = base.Request.Form["orderId"];
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str2))
            {
                int result = 0;
                if (int.TryParse(str, out result))
                {
                    ShippersInfo shipper = SalesHelper.GetShipper(result);
                    if (shipper != null)
                    {
                        OrderInfo orderInfo = OrderHelper.GetOrderInfo(str2);
                        if (orderInfo != null)
                        {
                            this.WriteOrderInfo(orderInfo, shipper);
                        }
                    }
                }
            }
        }

        private void WriteOrderInfo(OrderInfo order, ShippersInfo shipper)
        {
            string[] strArray = RegionHelper.GetFullRegion(order.RegionId, ",").Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<nodes>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.ShipTo);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.TelPhone + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.CellPhone + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.ZipCode + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.Address);
            builder.AppendLine("</item>");
            if (strArray.Length > 0)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区1级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[0]);
                builder.AppendLine("</item>");
            }
            if (strArray.Length > 1)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区2级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[1]);
                builder.AppendLine("</item>");
            }
            if (strArray.Length > 2)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区3级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[2]);
                builder.AppendLine("</item>");
            }
            if (shipper != null)
            {
                string[] strArray2 = RegionHelper.GetFullRegion(shipper.RegionId, ",").Split(new char[] { ',' });
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-姓名</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.ShipperName);
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-手机</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.CellPhone + "_");
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-电话</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.TelPhone + "_");
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-地址</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.Address);
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-邮编</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.Zipcode + "_");
                builder.AppendLine("</item>");
                if (strArray2.Length > 0)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区1级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[0]);
                    builder.AppendLine("</item>");
                }
                if (strArray2.Length > 1)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区2级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[1]);
                    builder.AppendLine("</item>");
                }
                if (strArray2.Length > 2)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区3级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[2]);
                    builder.AppendLine("</item>");
                }
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-订单号</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.OrderId);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-总金额</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.GetTotal() + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-物品总重量</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.Weight);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-备注</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.ManagerRemark);
            builder.AppendLine("</item>");
            string str3 = "";
            if ((order.LineItems != null) && (order.LineItems.Count > 0))
            {
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    object obj2 = str3;
                    str3 = string.Concat(new object[] { obj2, "货号 ", info.SKU, " ", info.SKUContent, " \x00d7", info.ShipmentQuantity, "\n" });
                }
                str3 = str3.Replace("；", "").Replace(";", "").Replace("：", ":");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-详情</name>");
            builder.AppendFormat("<rename>{0}</rename>", str3);
            builder.AppendLine("</item>");
            if (order.ShippingDate == DateTime.Parse("0001-1-1"))
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>订单-送货时间</name>");
                builder.AppendFormat("<rename>{0}</rename>", "null");
                builder.AppendLine("</item>");
            }
            else
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>订单-送货时间</name>");
                builder.AppendFormat("<rename>{0}</rename>", order.ShippingDate);
                builder.AppendLine("</item>");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>网店名称</name>");
            builder.AppendFormat("<rename>{0}</rename>", SettingsManager.GetMasterSettings(true).SiteName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>自定义内容</name>");
            builder.AppendFormat("<rename>{0}</rename>", "null");
            builder.AppendLine("</item>");
            builder.AppendLine("</nodes>");
            base.Response.Write(builder.ToString());
        }
    }
}

