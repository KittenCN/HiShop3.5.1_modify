namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VMemberOrders : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptOrders;
        private HtmlInputHidden txtShowTabNum;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            this.txtShowTabNum = (HtmlInputHidden) this.FindControl("txtShowTabNum");
            PageTitle.AddSiteNameTitle("会员订单");
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 10;
            }
            int num3 = 0;
            int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out num3);
            OrderQuery query = new OrderQuery {
                PageIndex = num,
                PageSize = num2,
                SortBy = "OrderDate",
                SortOrder = SortAction.Desc
            };
            switch (num3)
            {
                case 1:
                    query.Status = OrderStatus.WaitBuyerPay;
                    break;

                case 2:
                    query.Status = OrderStatus.BuyerAlreadyPaid;
                    break;

                case 3:
                    query.Status = OrderStatus.SellerAlreadySent;
                    break;

                case 4:
                    query.Status = OrderStatus.BuyerAlreadyPaid;
                    break;
            }
            this.rptOrders = (VshopTemplatedRepeater) this.FindControl("rptOrders");
            DbQueryResult userOrderByPage = MemberProcessor.GetUserOrderByPage(Globals.GetCurrentMemberUserId(false), query);
            this.txtTotal.SetWhenIsNotNull(userOrderByPage.TotalRecords.ToString());
            this.rptOrders.DataSource = userOrderByPage.Data;
            this.rptOrders.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberOrders.html";
            }
            base.OnInit(e);
        }
    }
}

