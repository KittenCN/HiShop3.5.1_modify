namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VProgressCheck : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptOrderReturns;

        protected override void AttachChildControls()
        {
            string title = "历史记录";
            int type = Globals.RequestQueryNum("type");
            if (type != 1)
            {
                type = 0;
                title = "售后列表";
            }
            PageTitle.AddSiteNameTitle(title);
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            this.rptOrderReturns = (VshopTemplatedRepeater) this.FindControl("rptOrderReturns");
            this.rptOrderReturns.DataSource = ShoppingProcessor.GetOrderReturnTable(currentMember.UserId, "", type);
            this.rptOrderReturns.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VProgressCheck.html";
            }
            base.OnInit(e);
        }
    }
}

