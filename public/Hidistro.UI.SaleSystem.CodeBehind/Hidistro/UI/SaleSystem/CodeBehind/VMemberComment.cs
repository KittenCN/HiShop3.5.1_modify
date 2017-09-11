namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Orders;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class VMemberComment : VMemberTemplatedWebControl
    {
        private VshopTemplatedRepeater rptOrderItemList;
        private HtmlInputHidden txtShowTabNum;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            this.txtShowTabNum = (HtmlInputHidden) this.FindControl("txtShowTabNum");
            PageTitle.AddSiteNameTitle("评价列表");
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
                SortBy = "Id",
                SortOrder = SortAction.Desc
            };
            int currentMemberUserId = Globals.GetCurrentMemberUserId(false);
            int orderItemsStatus = 5;
            this.rptOrderItemList = (VshopTemplatedRepeater) this.FindControl("rptOrderItemList");
            DbQueryResult result = ProductBrowser.GetOrderMemberComment(query, currentMemberUserId, orderItemsStatus);
            this.rptOrderItemList.DataSource = result.Data;
            this.rptOrderItemList.DataBind();
            this.txtTotal.SetWhenIsNotNull(result.TotalRecords.ToString());
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMemberComment.html";
            }
            base.OnInit(e);
        }
    }
}

