namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VShoppingCartEmpty : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("购物车");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VShoppingCartEmpty.html";
            }
            base.OnInit(e);
        }
    }
}

