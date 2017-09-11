namespace Hidistro.UI.Web.Pay
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;

    public class IframeAlipay : Page
    {
        protected string IframeUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = Globals.RequestQueryStr("OrderId");
            if (string.IsNullOrEmpty(str))
            {
                this.Page.Response.Redirect("/");
            }
            else
            {
                this.IframeUrl = "/Vshop/FinishOrder.aspx?PaymentType=1&IsAlipay=1&OrderId=" + str;
            }
        }
    }
}

