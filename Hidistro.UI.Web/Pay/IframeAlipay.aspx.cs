using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
    public partial class IframeAlipay : System.Web.UI.Page
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