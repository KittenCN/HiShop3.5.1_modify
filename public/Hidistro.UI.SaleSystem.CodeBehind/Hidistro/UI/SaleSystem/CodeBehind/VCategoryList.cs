namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VCategoryList : VshopTemplatedWebControl
    {
        private VshopTemplatedRepeater rptCategories;

        protected override void AttachChildControls()
        {
            this.rptCategories = (VshopTemplatedRepeater) this.FindControl("rptCategories");
            DataSet categoryList = CategoryBrowser.GetCategoryList();
            this.rptCategories.ItemDataBound += new RepeaterItemEventHandler(this.rptCategories_ItemDataBound);
            this.rptCategories.DataSource = categoryList;
            this.rptCategories.DataBind();
            PageTitle.AddSiteNameTitle("商品分类");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VCategoryList.html";
            }
            base.OnInit(e);
        }

        private void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataView view = (DataView) DataBinder.Eval(e.Item.DataItem, "SubCategories");
            DataRowView dataItem = (DataRowView) e.Item.DataItem;
            Convert.ToInt32(dataItem["CategoryId"]);
            Literal literal = (Literal) e.Item.Controls[0].FindControl("litPlus");
            if ((view == null) || (view.ToTable().Rows.Count == 0))
            {
                literal.Visible = false;
            }
            else
            {
                literal.Visible = true;
            }
        }
    }
}

