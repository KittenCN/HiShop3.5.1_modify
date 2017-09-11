namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class VMemberAmountRequestDetail : VMemberTemplatedWebControl
    {
        private Literal litAccountCode;
        private Literal litAmount;
        private Literal litRemark;
        private Literal litRequestTime;
        private Literal litRequestType;
        private Literal litState;
        private Literal litTradeType;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("提现详情");
            int serialid = Globals.RequestQueryNum("id");
            this.litState = (Literal) this.FindControl("litState");
            this.litTradeType = (Literal) this.FindControl("litTradeType");
            this.litAccountCode = (Literal) this.FindControl("litAccountCode");
            this.litRequestType = (Literal) this.FindControl("litRequestType");
            this.litRequestTime = (Literal) this.FindControl("litRequestTime");
            this.litAmount = (Literal) this.FindControl("litAmount");
            this.litRemark = (Literal) this.FindControl("litRemark");
            MemberAmountRequestInfo amountRequestDetail = MemberAmountProcessor.GetAmountRequestDetail(serialid);
            if (amountRequestDetail != null)
            {
                string str = string.Empty;
                switch (amountRequestDetail.State)
                {
                    case RequesState.驳回:
                        str = "<span class='colorr'>已驳回</span>";
                        break;

                    case RequesState.未审核:
                        str = "<span class='colory'>待审核</span>";
                        break;

                    case RequesState.已审核:
                        str = "<span class='colorh'>已审核</span>";
                        break;

                    case RequesState.已发放:
                        str = "<span class='colorg'>已发放</span>";
                        break;

                    case RequesState.发放异常:
                        str = "<span class='colorr'>发放异常</span>";
                        break;
                }
                this.litState.Text = str;
                this.litRequestType.Text = VShopHelper.GetCommissionPayType(((int) amountRequestDetail.RequestType).ToString());
                this.litAccountCode.Text = amountRequestDetail.AccountCode;
                this.litRequestTime.Text = amountRequestDetail.RequestTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.litAmount.Text = amountRequestDetail.Amount.ToString();
                this.litRemark.Text = amountRequestDetail.Remark;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberAmountRequestDetail.html";
            }
            base.OnInit(e);
        }
    }
}

