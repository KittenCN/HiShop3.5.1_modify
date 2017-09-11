namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VMemberAccountBalance : VMemberTemplatedWebControl
    {
        private Literal litAmount;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("账户余额");
            MemberInfo currentMemberInfo = base.CurrentMemberInfo;
            if (currentMemberInfo == null)
            {
                this.Page.Response.Redirect("/logout.aspx");
            }
            else
            {
                this.litAmount = (Literal) this.FindControl("litAmount");
                this.litAmount.Text = Math.Round(currentMemberInfo.AvailableAmount, 2).ToString();
                HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("EnabelBalanceWithdrawal");
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                hidden.Value = masterSettings.EnabelBalanceWithdrawal.ToString().ToLower();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberAccountBalance.html";
            }
            base.OnInit(e);
        }
    }
}

