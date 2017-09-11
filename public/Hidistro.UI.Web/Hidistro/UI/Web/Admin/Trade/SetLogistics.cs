namespace Hidistro.UI.Web.Admin.Trade
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditOrders)]
    public class SetLogistics : AdminPage
    {
        protected Literal litOrdersCount;
        protected string orderIds;
        protected string Reurl;

        protected SetLogistics() : base("m03", "ddp03")
        {
            this.orderIds = string.Empty;
            this.Reurl = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = Globals.RequestFormStr("posttype");
            this.Reurl = Globals.RequestQueryStr("reurl");
            if (string.IsNullOrEmpty(this.Reurl))
            {
                this.Reurl = "BuyerAlreadyPaid.aspx";
            }
            if (str == "savelogistics")
            {
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
                string expressCompanyAbb = Globals.RequestFormStr("selid");
                string expressCompanyName = Globals.RequestFormStr("selname");
                string[] strArray = Globals.RequestFormStr("orderlist").Split(new char[] { ',' });
                int num = 0;
                foreach (string str6 in strArray)
                {
                    if (Globals.IsOrdersID(str6) && OrderHelper.SetPrintOrderExpress(str6, expressCompanyName, expressCompanyAbb, ""))
                    {
                        num++;
                    }
                }
                if (num > 0)
                {
                    s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
                }
                base.Response.Write(s);
                base.Response.End();
            }
            else
            {
                this.orderIds = Globals.RequestQueryStr("OrderId").Trim(new char[] { ',' });
                this.litOrdersCount.Text = this.orderIds.Split(new char[] { ',' }).Length.ToString();
            }
        }
    }
}

