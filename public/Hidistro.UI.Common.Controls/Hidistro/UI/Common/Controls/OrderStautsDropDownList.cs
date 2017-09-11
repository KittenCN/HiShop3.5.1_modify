namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Orders;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class OrderStautsDropDownList : DropDownList
    {
        private bool allowNull = true;

        public OrderStautsDropDownList()
        {
            base.Items.Clear();
            int num = 0;
            base.Items.Add(new ListItem("所有订单", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 1;
            base.Items.Add(new ListItem("等待买家付款", num2.ToString(CultureInfo.InvariantCulture)));
            int num3 = 2;
            base.Items.Add(new ListItem("等待发货", num3.ToString(CultureInfo.InvariantCulture)));
            int num4 = 3;
            base.Items.Add(new ListItem("已发货", num4.ToString(CultureInfo.InvariantCulture)));
            int num5 = 4;
            base.Items.Add(new ListItem("已关闭", num5.ToString(CultureInfo.InvariantCulture)));
            int num6 = 5;
            base.Items.Add(new ListItem("成功订单", num6.ToString(CultureInfo.InvariantCulture)));
            int num7 = 0x63;
            base.Items.Add(new ListItem("历史订单", num7.ToString(CultureInfo.InvariantCulture)));
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public OrderStatus SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return OrderStatus.All;
                }
                return (OrderStatus) int.Parse(base.SelectedValue);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

