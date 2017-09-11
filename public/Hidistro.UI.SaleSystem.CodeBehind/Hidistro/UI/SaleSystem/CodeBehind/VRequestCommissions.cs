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
    public class VRequestCommissions : VMemberTemplatedWebControl
    {
        private HtmlSelect accoutType;
        private HtmlInputHidden hidmoney;
        private Literal litmaxmoney;
        private HtmlAnchor requestcommission;
        private HtmlAnchor requestcommission1;
        private HtmlInputText txtaccount;
        private HtmlInputText txtAccountName;
        private HtmlInputText txtmoney;
        private HtmlInputText txtmoneyweixin;

        protected override void AttachChildControls()
        {
            DistributorsInfo nowCurrentDistributors = DistributorsBrower.GetNowCurrentDistributors(Globals.GetCurrentMemberUserId(false));
            MemberInfo currentMember = MemberProcessor.GetCurrentMember();
            if (nowCurrentDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                PageTitle.AddSiteNameTitle("申请提现");
                this.accoutType = (HtmlSelect) this.FindControl("accoutType");
                this.litmaxmoney = (Literal) this.FindControl("litmaxmoney");
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                if (masterSettings.DrawPayType.Contains("0"))
                {
                    this.accoutType.Items.Add(new ListItem("微信钱包", "0"));
                }
                if (masterSettings.DrawPayType.Contains("1"))
                {
                    this.accoutType.Items.Add(new ListItem("支付宝", "1"));
                }
                if (masterSettings.DrawPayType.Contains("2"))
                {
                    this.accoutType.Items.Add(new ListItem("线下转帐", "2"));
                }
                Literal literal = (Literal) this.FindControl("litAlipayBtn");
                literal.Text = "display:none";
                if (masterSettings.DrawPayType.Contains("3"))
                {
                    literal.Text = "";
                }
                this.txtAccountName = (HtmlInputText) this.FindControl("txtAccountName");
                this.txtaccount = (HtmlInputText) this.FindControl("txtaccount");
                this.txtmoney = (HtmlInputText) this.FindControl("txtmoney");
                this.txtmoneyweixin = (HtmlInputText) this.FindControl("txtmoneyweixin");
                this.hidmoney = (HtmlInputHidden) this.FindControl("hidmoney");
                this.requestcommission = (HtmlAnchor) this.FindControl("requestcommission");
                this.requestcommission1 = (HtmlAnchor) this.FindControl("requestcommission1");
                this.txtaccount.Value = nowCurrentDistributors.RequestAccount;
                this.txtAccountName.Value = currentMember.RealName;
                this.litmaxmoney.Text = nowCurrentDistributors.ReferralBlance.ToString("F2");
                decimal result = 0M;
                if (decimal.TryParse(SettingsManager.GetMasterSettings(false).MentionNowMoney, out result) && (result > 0M))
                {
                    this.txtmoney.Attributes["placeholder"] = "请输入大于等于" + result + "元的金额";
                    this.txtmoneyweixin.Attributes["placeholder"] = "最低提现金额" + ((result > 1.00M) ? result : 1.00M) + "元的金额";
                    this.hidmoney.Value = result.ToString();
                }
                if (DistributorsBrower.IsExitsCommionsRequest())
                {
                    this.requestcommission.Disabled = true;
                    this.requestcommission.InnerText = "您的申请正在审核当中";
                    this.requestcommission1.Disabled = true;
                    this.requestcommission1.InnerText = "您的申请正在审核当中";
                    literal.Text = "display:none";
                    Literal literal2 = (Literal) this.FindControl("litWechatBtn");
                    literal2.Text = "display:none";
                }
                else
                {
                    this.requestcommission.Attributes.Add("onclick", "return RequestCommissions(3)");
                    this.requestcommission1.Attributes.Add("onclick", "return RequestCommissions(1)");
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-RequestCommissions.html";
            }
            base.OnInit(e);
        }
    }
}

