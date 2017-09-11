namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VMemberAmountApply : VMemberTemplatedWebControl
    {
        protected decimal Surpluscommission = 0.00M;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("余额提现");
            if (!SettingsManager.GetMasterSettings(false).EnabelBalanceWithdrawal)
            {
                base.GotoResourceNotFound(ErrorType.前台404, "商家已关闭余额提现功能");
            }
            Literal literal = (Literal) this.FindControl("litApplyType");
            MemberInfo currentMemberInfo = base.CurrentMemberInfo;
            if (currentMemberInfo == null)
            {
                this.Page.Response.Redirect("/logout.aspx");
            }
            else
            {
                HtmlInputHidden hidden = (HtmlInputHidden) this.FindControl("MaxAmount");
                HtmlInputHidden hidden2 = (HtmlInputHidden) this.FindControl("MinAmount");
                HtmlInputHidden hidden3 = (HtmlInputHidden) this.FindControl("RealName");
                this.Surpluscommission = Math.Round(currentMemberInfo.AvailableAmount, 2);
                hidden.Value = Math.Round(this.Surpluscommission, 2).ToString();
                hidden3.Value = currentMemberInfo.RealName;
                decimal result = 0M;
                if (decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out result) && (result > 0M))
                {
                    hidden2.Value = result.ToString();
                }
                StringBuilder builder = new StringBuilder();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                if (masterSettings.DrawPayType.Contains("0"))
                {
                    builder.AppendFormat("<div class=\"payway\" name=\"{0}\">{1}</div>", 0, "微信钱包").AppendLine();
                }
                if (masterSettings.DrawPayType.Contains("1"))
                {
                    builder.AppendFormat("<div class=\"payway\" name=\"{0}\">{1}</div>", 1, "支付宝").AppendLine();
                }
                if (masterSettings.DrawPayType.Contains("2"))
                {
                    builder.AppendFormat("<div class=\"payway\" name=\"{0}\">{1}</div>", 2, "线下转帐").AppendLine();
                }
                if (masterSettings.DrawPayType.Contains("3"))
                {
                    builder.AppendFormat("<div class=\"payway\" name=\"{0}\">{1}</div>", 3, "微信红包").AppendLine();
                }
                literal.Text = builder.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberAmountApply.html";
            }
            base.OnInit(e);
        }
    }
}

