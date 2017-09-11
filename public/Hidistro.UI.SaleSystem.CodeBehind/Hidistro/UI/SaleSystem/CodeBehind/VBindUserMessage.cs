namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VBindUserMessage : VshopTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            if (!string.IsNullOrEmpty(MemberProcessor.GetCurrentMember().UserBindName))
            {
                this.Page.Response.Redirect("/vshop/MemberCenter.aspx", true);
            }
            PageTitle.AddSiteNameTitle("用户绑定帐号");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VBindUserMessage.html";
            }
            base.OnInit(e);
        }
    }
}

