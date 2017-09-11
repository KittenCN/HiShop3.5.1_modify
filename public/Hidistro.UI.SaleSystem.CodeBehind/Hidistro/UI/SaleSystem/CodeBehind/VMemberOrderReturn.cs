namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMemberOrderReturn : VMemberTemplatedWebControl
    {
        private DataView ItemsDt = new DataView();
        private VshopTemplatedRepeater rptOrders;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("退换货");
            OrderQuery query = new OrderQuery();
            this.rptOrders = (VshopTemplatedRepeater) this.FindControl("rptOrders");
            this.rptOrders.ItemDataBound += new RepeaterItemEventHandler(this.rptOrders_ItemDataBound);
            DataSet userOrderReturn = MemberProcessor.GetUserOrderReturn(Globals.GetCurrentMemberUserId(false), query);
            this.ItemsDt = userOrderReturn.Tables[1].DefaultView;
            this.rptOrders.DataSource = userOrderReturn;
            this.rptOrders.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberOrderReturn.html";
            }
            base.OnInit(e);
        }

        private void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = (Literal) e.Item.Controls[0].FindControl("litStyle");
                string str = (string) DataBinder.Eval(e.Item.DataItem, "OrderId");
                this.ItemsDt.RowFilter = " OrderId='" + str + "'";
                if (this.ItemsDt.Count == 0)
                {
                    literal.Text = "style=\"display:none;\"";
                }
            }
        }
    }
}

