namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;

    public class VOneTaoPaySuccess : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("支付结果");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VOneTaoPaySuccess.html";
            }
            base.OnInit(e);
        }
    }
}

