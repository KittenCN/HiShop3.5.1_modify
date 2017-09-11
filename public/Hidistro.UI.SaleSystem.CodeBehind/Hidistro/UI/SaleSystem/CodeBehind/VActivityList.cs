namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VActivityList : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("活动专题");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VActivityList.html";
            }
            base.OnInit(e);
        }
    }
}

