namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class AmountReturn : RePaymentTemplatedWebControl
    {
        private Literal litMessage;

        public AmountReturn() : base(false)
        {
        }

        protected override void AttachChildControls()
        {
            this.litMessage = (Literal) this.FindControl("litMessage");
        }

        protected override void DisplayMessage(string status)
        {
            switch (status)
            {
                case "ordernotfound":
                    this.litMessage.Text = string.Format("没有找到对应的充值信息，充值号：{0}", base.PayId);
                    return;

                case "gatewaynotfound":
                    this.litMessage.Text = "没有找到与此充值方式对应的支付方式，系统无法自动完成操作，请联系管理员";
                    return;

                case "verifyfaild":
                    this.litMessage.Text = "支付返回验证失败，操作已停止";
                    return;

                case "success":
                    this.litMessage.Text = string.Format("恭喜您，充值已成功完成支付：{0}</br>支付金额：{1}", base.PayId, this.Amount.ToString("F"));
                    return;

                case "fail":
                    this.litMessage.Text = string.Format("充值支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}", this.Amount.ToString("F"));
                    return;
            }
            this.litMessage.Text = "未知错误，操作已停止";
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-RePaymentReturn.html";
            }
            base.OnInit(e);
        }
    }
}

