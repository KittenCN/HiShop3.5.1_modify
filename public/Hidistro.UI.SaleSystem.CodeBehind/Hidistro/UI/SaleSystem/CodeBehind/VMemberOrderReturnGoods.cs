namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberOrderReturnGoods : VMemberTemplatedWebControl
    {
        private Literal litOrderDate;
        private Literal litOrderId;
        private string orderId;
        private VshopTemplatedRepeater rptOrderProducts;

        protected override void AttachChildControls()
        {
            this.orderId = this.Page.Request.QueryString["orderId"];
            this.litOrderId = (Literal) this.FindControl("litOrderId");
            this.litOrderDate = (Literal) this.FindControl("litOrderDate");
            this.rptOrderProducts = (VshopTemplatedRepeater) this.FindControl("rptOrderProducts");
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            if (orderInfo == null)
            {
                base.GotoResourceNotFound("此订单已不存在");
            }
            this.litOrderId.Text = this.orderId;
            this.litOrderDate.Text = orderInfo.OrderDate.ToString();
            this.rptOrderProducts.DataSource = orderInfo.LineItems.Values;
            this.rptOrderProducts.DataBind();
            PageTitle.AddSiteNameTitle("退换货商品");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberOrderReturnGoods.html";
            }
            base.OnInit(e);
        }
    }
}

