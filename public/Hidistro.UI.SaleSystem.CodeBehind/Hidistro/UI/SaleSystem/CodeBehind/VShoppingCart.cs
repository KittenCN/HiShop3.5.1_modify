namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VShoppingCart : VMemberTemplatedWebControl
    {
        private HtmlAnchor aLink;
        private List<ShoppingCartInfo> cart;
        private List<ShoppingCartInfo> cartPoint;
        private HtmlGenericControl divShowTotal;
        private Literal litTotal;
        private decimal ReductionMoneyALL;
        private VshopTemplatedRepeater rptCartPointProducts;
        private VshopTemplatedRepeater rptCartProducts;

        protected override void AttachChildControls()
        {
            this.rptCartProducts = (VshopTemplatedRepeater) this.FindControl("rptCartProducts");
            this.rptCartProducts.ItemDataBound += new RepeaterItemEventHandler(this.rptCartProducts_ItemDataBound);
            this.rptCartPointProducts = (VshopTemplatedRepeater) this.FindControl("rptCartPointProducts");
            this.litTotal = (Literal) this.FindControl("litTotal");
            this.divShowTotal = (HtmlGenericControl) this.FindControl("divShowTotal");
            this.aLink = (HtmlAnchor) this.FindControl("aLink");
            this.Page.Session["stylestatus"] = "0";
            this.cart = ShoppingCartProcessor.GetShoppingCartAviti(0);
            this.cartPoint = ShoppingCartProcessor.GetShoppingCartAviti(1);
            if (this.cart != null)
            {
                this.rptCartProducts.DataSource = this.cart;
                this.rptCartProducts.DataBind();
            }
            else
            {
                Panel panel = (Panel) this.FindControl("products");
                panel.Visible = false;
            }
            if (this.cartPoint != null)
            {
                this.rptCartPointProducts.DataSource = this.cartPoint;
                this.rptCartPointProducts.DataBind();
            }
            else
            {
                Panel panel2 = (Panel) this.FindControl("pointproducts");
                panel2.Visible = false;
            }
            if ((this.cart != null) || (this.cartPoint != null))
            {
                this.aLink.HRef = "/Vshop/SubmmitOrder.aspx";
            }
            else
            {
                Panel panel3 = (Panel) this.FindControl("divEmpty");
                panel3.Visible = true;
                Panel panel4 = (Panel) this.FindControl("pannelGo");
                panel4.Visible = false;
                HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("hdIsShow");
                hidden.Value = "1";
            }
            decimal num = 0M;
            if (this.cart != null)
            {
                foreach (ShoppingCartInfo info in this.cart)
                {
                    num += info.GetAmount();
                }
            }
            int num2 = 0;
            if (this.cartPoint != null)
            {
                foreach (ShoppingCartInfo info2 in this.cartPoint)
                {
                    num2 += info2.GetTotalPoint();
                }
            }
            PageTitle.AddSiteNameTitle("购物车");
            string str = string.Empty;
            decimal num3 = num - this.ReductionMoneyALL;
            if (num3 > 0M)
            {
                str = "￥" + num3.ToString("F2");
            }
            if (num2 > 0)
            {
                if (num3 > 0M)
                {
                    str = str + "+";
                }
                str = str + num2.ToString() + "积分";
            }
            this.litTotal.Text = str;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VShoppingCart.html";
            }
            base.OnInit(e);
        }

        private void rptCartProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item)
            {
                ListItemType itemType = e.Item.ItemType;
            }
        }
    }
}

