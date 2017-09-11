namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;

    public class VMemberAmountRecord : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("提现记录");
            if (!SettingsManager.GetMasterSettings(false).EnabelBalanceWithdrawal)
            {
                base.GotoResourceNotFound(ErrorType.前台404, "商家已关闭提现记录查看功能");
            }
            int num = Globals.RequestQueryNum("type");
            HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("hidType");
            hidden.Value = num.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberAmountRecord.html";
            }
            base.OnInit(e);
        }
    }
}

