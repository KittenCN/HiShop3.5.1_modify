namespace Hidistro.UI.Web.Admin
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

    public class ProductReviews : AdminPage
    {
        protected Button btnSearch;
        private int? categoryId;
        protected Grid dlstPtReviews;
        protected ProductCategoriesDropDownList dropCategories;
        protected PageSize hrefPageSize;
        private string keywords;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        protected ProductReviews() : base("m02", "spp13")
        {
            this.keywords = string.Empty;
        }

        private void BindPtReview()
        {
            ProductReviewQuery entity = new ProductReviewQuery {
                Keywords = this.keywords,
                CategoryId = this.categoryId,
                ProductCode = this.productCode,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "ReviewDate"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult productReviews = ProductCommentHelper.GetProductReviews(entity);
            this.dlstPtReviews.DataSource = productReviews.Data;
            this.dlstPtReviews.DataBind();
            this.pager.TotalRecords = productReviews.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductReviews(true);
        }

        private void dlstPtReviews_ReBindData(object sender)
        {
            this.BindPtReview();
        }

        private void dlstPtReviews_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ProductCommentHelper.DeleteProductReview((long) int.Parse(this.dlstPtReviews.DataKeys[e.RowIndex].Value.ToString())) > 0)
            {
                this.ShowMsg("成功删除了选择的客户评论", true);
                this.BindPtReview();
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.dlstPtReviews.RowDeleting += new GridViewDeleteEventHandler(this.dlstPtReviews_RowDeleting);
            this.dlstPtReviews.ReBindData += new Grid.ReBindDataEventHandler(this.dlstPtReviews_ReBindData);
            this.SetSearchControl();
        }

        private void ReloadProductReviews(bool isSearch)
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
                this.BindPtReview();
            }
            else
            {
                this.keywords = this.txtSearchText.Text.Trim();
                this.productCode = this.txtSKU.Text.Trim();
                this.categoryId = this.dropCategories.SelectedValue;
            }
        }
    }
}

