namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorDescription : VMemberTemplatedWebControl
    {
        protected string IsEnable = "0";
        private Literal litApplicationDescription;
        private HtmlInputHidden litIsEnable;
        private HtmlInputHidden litRMsg;
        protected string RMsg = "您未达到开店条件，最低消费金额需要达到{0}元才能申请,你当前累计消费金额为{1}元";

        protected override void AttachChildControls()
        {
            this.litIsEnable = (HtmlInputHidden) this.FindControl("litIsEnable");
            this.litRMsg = (HtmlInputHidden) this.FindControl("litRMsg");
            this.litApplicationDescription = (Literal) this.FindControl("litApplicationDescription");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            this.litApplicationDescription.Text = HttpUtility.HtmlDecode(masterSettings.DistributorDescription);
            PageTitle.AddSiteNameTitle("分销说明信息");
            this.Page.Session["stylestatus"] = "2";
            if (DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false)) != null)
            {
                this.IsEnable = "1";
            }
            else
            {
                int finishedOrderMoney = masterSettings.FinishedOrderMoney;
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                this.RMsg = string.Format(this.RMsg, finishedOrderMoney, currentMember.Expenditure);
                if (currentMember.Expenditure >= finishedOrderMoney)
                {
                    this.IsEnable = "2";
                }
            }
            this.litIsEnable.Value = this.IsEnable;
            this.litRMsg.Value = this.RMsg;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VDistributorDesciption.html";
            }
            base.OnInit(e);
        }
    }
}

