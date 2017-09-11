namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
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
    public class VChirldrenDistributorDetials : VMemberTemplatedWebControl
    {
        private HtmlInputHidden txtShowTabNum1;
        private HtmlInputHidden txtTotal1;
        private VshopTemplatedRepeater vshoporders1;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            this.txtTotal1 = (HtmlInputHidden) this.FindControl("txtTotal");
            this.txtShowTabNum1 = (HtmlInputHidden) this.FindControl("txtShowTabNum");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            OrderQuery query = new OrderQuery();
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["distributorId"], out result))
            {
                query.ReferralUserId = new int?(result);
            }
            int num4 = 0;
            if (int.TryParse(this.Page.Request.QueryString["ReferralId"], out num4))
            {
                query.UserId = new int?(num4);
            }
            query.PageIndex = num;
            query.PageSize = num2;
            query.SortBy = "OrderDate";
            query.SortOrder = SortAction.Desc;
            if (DistributorsBrower.GetUserIdDistributors(Globals.GetCurrentMemberUserId(false)).ReferralStatus != 0)
            {
                HttpContext.Current.Response.Redirect("MemberCenter.aspx");
            }
            else
            {
                this.vshoporders1 = (VshopTemplatedRepeater) this.FindControl("vshoporders");
                PageTitle.AddSiteNameTitle("店铺订单");
                query.UserId = new int?(Globals.GetCurrentMemberUserId(false));
                query.Status = OrderStatus.Finished;
                this.vshoporders1.ItemDataBound += new RepeaterItemEventHandler(this.vshoporders1_ItemDataBound);
                DataSet distributorOrderByDetials = DistributorsBrower.GetDistributorOrderByDetials(query);
                this.vshoporders1.DataSource = distributorOrderByDetials;
                this.vshoporders1.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ChirldrenDistributorDetials.html";
            }
            base.OnInit(e);
        }

        private void vshoporders1_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

