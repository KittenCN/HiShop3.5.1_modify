namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class ManageNineImages : AdminPage
    {
        protected Literal NineTotal;
        protected Pager pager;
        private string ShareDesc;
        protected Repeater ShareRep;

        protected ManageNineImages() : base("m01", "dpp10")
        {
            this.ShareDesc = "";
        }

        private void BindData()
        {
            NineImgsesQuery query = new NineImgsesQuery {
                key = this.ShareDesc,
                SortBy = "id",
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc
            };
            DbQueryResult nineImgsesList = ShareMaterialBrowser.GetNineImgsesList(query);
            this.ShareRep.DataSource = nineImgsesList.Data;
            this.ShareRep.DataBind();
            this.pager.TotalRecords = nineImgsesList.TotalRecords;
            this.NineTotal.Text = this.pager.TotalRecords.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.ShowMsg("添加成功了", false);
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["key"]))
            {
                this.ShareDesc = base.Server.UrlDecode(this.Page.Request.QueryString["key"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

