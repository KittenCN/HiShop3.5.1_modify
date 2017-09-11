namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class VNineImages : VshopTemplatedWebControl
    {
        private Pager pager;
        private VshopTemplatedRepeater refriendscircle;

        protected override void AttachChildControls()
        {
            this.refriendscircle = (VshopTemplatedRepeater) this.FindControl("refriendscircle");
            this.pager = (Pager) this.FindControl("pager");
            PageTitle.AddSiteNameTitle("九图一文素材");
            this.BindData();
        }

        private void BindData()
        {
            NineImgsesQuery query = new NineImgsesQuery {
                key = "",
                SortBy = "id",
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc
            };
            DbQueryResult nineImgsesList = ShareMaterialBrowser.GetNineImgsesList(query);
            this.refriendscircle.DataSource = nineImgsesList.Data;
            this.refriendscircle.DataBind();
            this.pager.TotalRecords = nineImgsesList.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VNineImages.html";
            }
            base.OnInit(e);
        }
    }
}

