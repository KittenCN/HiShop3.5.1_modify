namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VNotice : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            int num = Globals.RequestQueryNum("type");
            int num2 = Globals.RequestQueryNum("readtype");
            HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("hidType");
            HtmlInputHidden hidden2 = (HtmlInputHidden) this.FindControl("hidReadtype");
            string title = "公告";
            if (num == 1)
            {
                hidden.Value = "1";
                title = "消息";
            }
            if (num2 == 1)
            {
                hidden2.Value = "1";
            }
            PageTitle.AddSiteNameTitle(title);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-vNotice.html";
            }
            base.OnInit(e);
        }
    }
}

