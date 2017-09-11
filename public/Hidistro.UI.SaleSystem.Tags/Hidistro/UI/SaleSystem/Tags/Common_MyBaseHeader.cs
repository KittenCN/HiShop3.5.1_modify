namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;

    public class Common_MyBaseHeader : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "tags/Skin-Common_MyBaseHeader.html";
            }
            base.OnInit(e);
        }
    }
}

