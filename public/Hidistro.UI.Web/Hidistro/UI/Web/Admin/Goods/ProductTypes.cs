namespace Hidistro.UI.Web.Admin.Goods
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductTypes)]
    public class ProductTypes : AdminPage
    {
        protected Button btnSearchButton;
        protected Grid grdProductTypes;
        protected Pager pager;
        private string searchkey;
        protected TextBox txtSearchText;

        protected ProductTypes() : base("m02", "spp07")
        {
        }

        private void BindTypes()
        {
            ProductTypeQuery query = new ProductTypeQuery {
                TypeName = this.searchkey,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize
            };
            DbQueryResult productTypes = ProductTypeHelper.GetProductTypes(query);
            this.grdProductTypes.DataSource = productTypes.Data;
            this.grdProductTypes.DataBind();
            this.pager.TotalRecords = productTypes.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void grdProductTypes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label label = (Label) e.Row.FindControl("lbbrand");
                label.Text = ProductTypeHelper.GetBrandName(int.Parse(DataBinder.Eval(e.Row.DataItem, "TypeId").ToString()));
            }
        }

        private void grdProductTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int typeId = (int) this.grdProductTypes.DataKeys[e.RowIndex].Value;
            IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(typeId, AttributeUseageMode.Choose);
            if (ProductTypeHelper.DeleteProductType(typeId))
            {
                foreach (AttributeInfo info in attributes)
                {
                    foreach (AttributeValueInfo info2 in info.AttributeValues)
                    {
                        StoreHelper.DeleteImage(info2.ImageUrl);
                    }
                }
                this.BindTypes();
                this.ShowMsg("成功删除了一个商品类型", true);
            }
            else
            {
                this.ShowMsg("删除商品类型失败, 可能有商品正在使用该类型", false);
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                this.txtSearchText.Text = this.searchkey;
            }
            else
            {
                this.searchkey = this.txtSearchText.Text.Trim();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdProductTypes.RowDataBound += new GridViewRowEventHandler(this.grdProductTypes_RowDataBound);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.grdProductTypes.RowDeleting += new GridViewDeleteEventHandler(this.grdProductTypes_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindTypes();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", this.txtSearchText.Text);
            queryStrings.Add("pageSize", "10");
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

