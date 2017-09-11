namespace Hidistro.UI.Web.Admin.Goods
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class ProductConsultationsReplyed : AdminPage
    {
        protected Button btnSearch;
        private int? categoryId;
        protected ProductCategoriesDropDownList dropCategories;
        protected Grid grdConsultation;
        private string keywords;
        protected Pager pager;
        private string productCode;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        protected ProductConsultationsReplyed() : base("m02", "spp09")
        {
            this.keywords = string.Empty;
        }

        private void BinddlstProductConsultation()
        {
            ProductConsultationAndReplyQuery entity = new ProductConsultationAndReplyQuery {
                Keywords = this.keywords,
                CategoryId = this.categoryId,
                ProductCode = this.productCode,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "ReplyDate",
                Type = ConsultationReplyType.Replyed
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult consultationProducts = ProductCommentHelper.GetConsultationProducts(entity);
            this.grdConsultation.DataSource = consultationProducts.Data;
            this.grdConsultation.DataBind();
            this.pager.TotalRecords = consultationProducts.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductConsultationsReplyed(true);
        }

        private void grdConsultation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int consultationId = (int) this.grdConsultation.DataKeys[e.RowIndex].Value;
            if (ProductCommentHelper.DeleteProductConsultation(consultationId) > 0)
            {
                this.ShowMsg("成功删除了选择的商品咨询", true);
                this.BinddlstProductConsultation();
            }
            else
            {
                this.ShowMsg("删除商品咨询失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdConsultation.RowDeleting += new GridViewDeleteEventHandler(this.grdConsultation_RowDeleting);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.SetSearchControl();
        }

        private void ReloadProductConsultationsReplyed(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", this.txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
            queryStrings.Add("productCode", this.txtSKU.Text.Trim());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
                {
                    this.keywords = base.Server.UrlDecode(this.Page.Request.QueryString["Keywords"]);
                }
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                {
                    this.categoryId = new int?(result);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
                {
                    this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
                }
                this.txtSearchText.Text = this.keywords;
                this.txtSKU.Text = this.productCode;
                this.dropCategories.DataBind();
                this.dropCategories.SelectedValue = this.categoryId;
                this.BinddlstProductConsultation();
            }
            else
            {
                this.keywords = this.txtSearchText.Text;
                this.productCode = this.txtSKU.Text;
                this.categoryId = this.dropCategories.SelectedValue;
            }
        }
    }
}

