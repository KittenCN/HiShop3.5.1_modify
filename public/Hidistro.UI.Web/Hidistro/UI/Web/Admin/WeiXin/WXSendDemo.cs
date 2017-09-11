namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class WXSendDemo : AdminPage
    {
        protected Button btnSend_NewOrder;
        protected Button btnSend_NewProduct;
        protected HtmlForm form1;
        protected Label lbMsg;
        private SiteSettings siteSettings;
        protected TextBox txtOrderId;

        protected WXSendDemo() : base("m06", "wxp01")
        {
            this.siteSettings = SettingsManager.GetMasterSettings(false);
        }

        protected void btnSend_NewOrder_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.txtOrderId.Text.Trim());
            if (orderInfo == null)
            {
                this.lbMsg.Text = "订单不存在！" + DateTime.Now.ToString();
            }
            else
            {
                this.lbMsg.Text = Messenger.SendWeiXinMsg_OrderCreate(orderInfo);
            }
        }

        protected void btnSend_NewProduct_Click(object sender, EventArgs e)
        {
            ProductInfo product = new ProductInfo {
                ProductName = "三星手机G9260"
            };
            ProductHelper.SendWXMessage_AddNewProduct(product);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isPostBack = this.Page.IsPostBack;
        }
    }
}

