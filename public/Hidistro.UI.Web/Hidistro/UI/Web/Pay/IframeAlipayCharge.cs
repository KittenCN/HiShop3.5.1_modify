namespace Hidistro.UI.Web.Pay
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;

    public class IframeAlipayCharge : Page
    {
        protected string IframeUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = Globals.RequestQueryStr("PayId");
            if (string.IsNullOrEmpty(str))
            {
                this.Page.Response.Redirect("/");
            }
            else
            {
                this.IframeUrl = "/Vshop/FinishRecharge.aspx?PaymentType=1&IsAlipay=1&PayId=" + str;
            }
        }
    }
}

