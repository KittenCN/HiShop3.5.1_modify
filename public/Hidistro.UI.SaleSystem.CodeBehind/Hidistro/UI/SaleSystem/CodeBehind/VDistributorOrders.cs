namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VDistributorOrders : VMemberTemplatedWebControl
    {
        private Literal litallnum;
        private Literal litfinishnum;
        private HtmlInputHidden txtShowTabNum;
        private HtmlInputHidden txtTotal;
        private VshopTemplatedRepeater vshoporders;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            this.txtShowTabNum = (HtmlInputHidden) this.FindControl("txtShowTabNum");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            OrderQuery query = new OrderQuery {
                PageIndex = num,
                PageSize = num2,
                SortBy = "OrderDate",
                SortOrder = SortAction.Desc
            };
            DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false));
            if (userIdDistributors == null)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else if (userIdDistributors.ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                this.vshoporders = (VshopTemplatedRepeater) this.FindControl("vshoporders");
                this.litfinishnum = (Literal) this.FindControl("litfinishnum");
                this.litallnum = (Literal) this.FindControl("litallnum");
                PageTitle.AddSiteNameTitle("店铺订单");
                int result = 0;
                int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out result);
                query.UserId = new int?(Globals.GetCurrentMemberUserId(false));
                if (result != 5)
                {
                    query.Status = OrderStatus.Finished;
                    this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                    query.Status = OrderStatus.All;
                    this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                }
                else
                {
                    this.litallnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                    query.Status = OrderStatus.Finished;
                    this.litfinishnum.Text = DistributorsBrower.GetDistributorOrderCount(query).ToString();
                }
                this.vshoporders.ItemDataBound += new RepeaterItemEventHandler(this.vshoporders_ItemDataBound);
                DataSet distributorOrder = DistributorsBrower.GetDistributorOrder(query);
                this.vshoporders.DataSource = distributorOrder;
                this.vshoporders.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorOrders.html";
            }
            base.OnInit(e);
        }

        private void vshoporders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable table = ((DataView) DataBinder.Eval(e.Item.DataItem, "OrderItems")).ToTable();
            object obj2 = table.Compute("sum(ItemAdjustedCommssion)", "OrderItemsStatus<>9 and OrderItemsStatus<>10 and IsAdminModify=0");
            decimal num = (obj2 != DBNull.Value) ? ((decimal) obj2) : 0M;
            obj2 = table.Compute("sum(itemsCommission)", "OrderItemsStatus<>9 and OrderItemsStatus<>10");
            decimal num2 = (obj2 != DBNull.Value) ? ((decimal) obj2) : 0M;
            Literal literal = (Literal) e.Item.Controls[0].FindControl("litCommission");
            literal.Text = (num2 - num).ToString("F2");
        }
    }
}

