namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Order_ItemsList : UserControl
    {
        protected DataList dlstOrderItems;
        protected HyperLink hlkReducedPromotion;
        protected Literal lblAmoutPrice;
        protected Literal lblBundlingPrice;
        protected FormatedMoneyLabel lblTotalPrice;
        protected Literal litWeight;
        private OrderInfo order;

        protected override void OnLoad(EventArgs e)
        {
            this.dlstOrderItems.DataSource = this.order.LineItems.Values;
            this.dlstOrderItems.DataBind();
            this.litWeight.Text = this.order.Weight.ToString();
            if (this.order.IsReduced)
            {
                this.lblAmoutPrice.Text = string.Format("商品金额：{0}", Globals.FormatMoney(this.order.GetAmount()));
                this.hlkReducedPromotion.Text = this.order.ReducedPromotionName + string.Format(" 优惠：{0}", Globals.FormatMoney(this.order.ReducedPromotionAmount));
                this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { this.order.ReducedPromotionId });
            }
            if (this.order.BundlingID > 0)
            {
                this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(this.order.BundlingPrice));
            }
            this.lblTotalPrice.Money = this.order.GetAmount() - this.order.ReducedPromotionAmount;
        }

        public OrderInfo Order
        {
            get
            {
                return this.order;
            }
            set
            {
                this.order = value;
            }
        }
    }
}

