namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.Vshop;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyLogistics : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            string str = this.Page.Request.QueryString["orderId"];
            if (string.IsNullOrEmpty(str))
            {
                base.GotoResourceNotFound("");
            }
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(str);
            ExpressSet expressSet = ExpressHelper.GetExpressSet();
            Literal literal = this.FindControl("litHasNewKey") as Literal;
            Literal literal2 = this.FindControl("litExpressUrl") as Literal;
            Literal control = this.FindControl("litCompanyCode") as Literal;
            literal.Text = "0";
            literal2.Text = "";
            if (expressSet != null)
            {
                if (!string.IsNullOrEmpty(expressSet.NewKey))
                {
                    literal.Text = "1";
                }
                if (!string.IsNullOrEmpty(expressSet.Url.Trim()))
                {
                    literal2.Text = expressSet.Url.Trim();
                }
            }
            Literal literal4 = this.FindControl("litOrderID") as Literal;
            Literal literal5 = this.FindControl("litNumberID") as Literal;
            Literal literal6 = this.FindControl("litCompanyName") as Literal;
            Literal literal7 = this.FindControl("litLogisticsNumber") as Literal;
            literal4.Text = str;
            literal5.Text = orderInfo.ShipOrderNumber;
            literal6.SetWhenIsNotNull(orderInfo.ExpressCompanyName);
            literal7.SetWhenIsNotNull(orderInfo.ShipOrderNumber);
            control.SetWhenIsNotNull(orderInfo.ExpressCompanyAbb);
            PageTitle.AddSiteNameTitle("我的物流");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyLogistics.html";
            }
            base.OnInit(e);
        }
    }
}

