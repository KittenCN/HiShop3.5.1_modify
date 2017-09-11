namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VRequestReturn : VshopTemplatedWebControl
    {
        private HtmlInputHidden hidorderid;
        private HtmlInputHidden hidorderitemid;
        private HtmlInputHidden hidOrderStatus;
        private HtmlInputHidden hidproductid;
        private HtmlInputHidden hidskuid;
        private Literal litimage;
        private Literal litItemAdjustedPrice;
        private Literal litname;
        private Literal litQuantity;
        private string orderId;
        private string ProductId;
        private VshopTemplatedRepeater rptOrderProducts;
        private string SKuId;

        protected override void AttachChildControls()
        {
            this.hidOrderStatus = (HtmlInputHidden) this.FindControl("OrderStatus");
            this.hidskuid = (HtmlInputHidden) this.FindControl("skuid");
            this.hidorderid = (HtmlInputHidden) this.FindControl("orderid");
            this.hidorderitemid = (HtmlInputHidden) this.FindControl("orderitemid");
            this.hidproductid = (HtmlInputHidden) this.FindControl("productid");
            this.orderId = this.Page.Request.QueryString["orderId"].Trim();
            this.SKuId = Globals.RequestQueryStr("skuid").Trim();
            if (string.IsNullOrEmpty(this.SKuId))
            {
                int id = Globals.RequestQueryNum("ID");
                LineItemInfo lineItemInfo = OrderSplitHelper.GetLineItemInfo(id, this.orderId);
                if (lineItemInfo != null)
                {
                    this.SKuId = lineItemInfo.SkuId;
                    this.hidorderitemid.Value = id.ToString();
                }
            }
            this.hidorderid.Value = this.orderId;
            this.hidskuid.Value = this.SKuId;
            this.litimage = (Literal) this.FindControl("litimage");
            this.litname = (Literal) this.FindControl("litname");
            this.litItemAdjustedPrice = (Literal) this.FindControl("litItemAdjustedPrice");
            this.litQuantity = (Literal) this.FindControl("litQuantity");
            this.rptOrderProducts = (VshopTemplatedRepeater) this.FindControl("rptOrderProducts");
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            this.hidOrderStatus.Value = ((int) orderInfo.OrderStatus).ToString();
            if (orderInfo == null)
            {
                base.GotoResourceNotFound("此订单已不存在");
            }
            bool flag = false;
            string str = "0";
            foreach (LineItemInfo info3 in orderInfo.LineItems.Values)
            {
                if (info3.SkuId.ToString() == this.SKuId)
                {
                    this.litimage.Text = "<image src=\"" + info3.ThumbnailsUrl + "\"></image>";
                    this.litname.Text = info3.ItemDescription;
                    this.litItemAdjustedPrice.Text = info3.ItemAdjustedPrice.ToString("0.00");
                    this.litQuantity.Text = info3.Quantity.ToString();
                    if (info3.ItemAdjustedPrice == info3.BalancePayMoney)
                    {
                        str = "1";
                    }
                    else if (info3.BalancePayMoney == 0M)
                    {
                        str = "2";
                    }
                    this.hidproductid.Value = info3.ProductId.ToString();
                    flag = true;
                    break;
                }
            }
            HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("payways");
            hidden.Value = str;
            if (!flag)
            {
                base.GotoResourceNotFound("此订单商品不存在");
            }
            PageTitle.AddSiteNameTitle("申请退货");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VRequestReturn.html";
            }
            base.OnInit(e);
        }
    }
}

