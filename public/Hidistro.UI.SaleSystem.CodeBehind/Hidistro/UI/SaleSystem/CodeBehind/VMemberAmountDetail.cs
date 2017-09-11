namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class VMemberAmountDetail : VMemberTemplatedWebControl
    {
        private Literal litAmount;
        private Literal litPayId;
        private Literal litRemark;
        private Literal litTrade;
        private Literal litTradeAmount;
        private Literal litTradeTime;
        private Literal litTradeType;
        private Literal litTradeWays;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("收支详情");
            int id = Globals.RequestQueryNum("id");
            this.litPayId = (Literal) this.FindControl("litPayId");
            this.litTradeType = (Literal) this.FindControl("litTradeType");
            this.litTrade = (Literal) this.FindControl("litTrade");
            this.litTradeAmount = (Literal) this.FindControl("litTradeAmount");
            this.litTradeWays = (Literal) this.FindControl("litTradeWays");
            this.litTradeTime = (Literal) this.FindControl("litTradeTime");
            this.litAmount = (Literal) this.FindControl("litAmount");
            this.litRemark = (Literal) this.FindControl("litRemark");
            MemberAmountDetailedInfo amountDetail = MemberAmountProcessor.GetAmountDetail(id);
            if (amountDetail != null)
            {
                this.litPayId.Text = amountDetail.PayId;
                this.litTradeType.Text = MemberHelper.GetEnumDescription(amountDetail.TradeType);
                this.litTrade.Text = (amountDetail.TradeAmount > 0M) ? "收入" : "支出";
                this.litTradeAmount.Text = amountDetail.TradeAmount.ToString();
                this.litTradeWays.Text = MemberHelper.GetEnumDescription(amountDetail.TradeWays);
                this.litTradeTime.Text = amountDetail.TradeTime.ToString();
                this.litAmount.Text = amountDetail.AvailableAmount.ToString();
                this.litRemark.Text = amountDetail.Remark;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberAmountDetail.html";
            }
            base.OnInit(e);
        }
    }
}

