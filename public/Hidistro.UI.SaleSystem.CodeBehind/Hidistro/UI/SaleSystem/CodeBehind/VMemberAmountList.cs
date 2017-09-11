namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;

    public class VMemberAmountList : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("余额明细");
            int num = Globals.RequestQueryNum("type");
            HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("hidType");
            hidden.Value = num.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberAmountList.html";
            }
            base.OnInit(e);
        }
    }
}

