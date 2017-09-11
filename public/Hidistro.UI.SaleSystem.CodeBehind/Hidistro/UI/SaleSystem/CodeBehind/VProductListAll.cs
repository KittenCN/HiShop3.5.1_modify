namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class VProductListAll : VMemberTemplatedWebControl
    {
        private int categoryId;
        private string keyWord = string.Empty;
        private Literal litProtuctNoSelNum;
        private Literal litProtuctSelNum;
        private VshopTemplatedRepeater rpCategorys;
        private VshopTemplatedRepeater rpChooseProducts;
        private HtmlInputText txtkeywords;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("未上架商品");
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            this.keyWord = this.Page.Request.QueryString["keyWord"];
            if (!string.IsNullOrWhiteSpace(this.keyWord))
            {
                this.keyWord = this.keyWord.Trim();
            }
            this.txtkeywords = (HtmlInputText) this.FindControl("keywords");
            this.rpChooseProducts = (VshopTemplatedRepeater) this.FindControl("rpChooseProducts");
            this.rpCategorys = (VshopTemplatedRepeater) this.FindControl("rpCategorys");
            this.litProtuctSelNum = (Literal) this.FindControl("litProtuctSelNum");
            this.litProtuctNoSelNum = (Literal) this.FindControl("litProtuctNoSelNum");
            this.DataBindSoruce();
        }

        private void DataBindSoruce()
        {
            int num;
            this.txtkeywords.Value = this.keyWord;
            this.rpCategorys.DataSource = CategoryBrowser.GetCategories();
            this.rpCategorys.DataBind();
            this.rpChooseProducts.DataSource = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, 1, 20, out num, "DisplaySequence", "desc", false);
            this.rpChooseProducts.DataBind();
            this.litProtuctSelNum.Text = "（" + ProductBrowser.GetProductsNumber(true).ToString() + "）";
            this.litProtuctNoSelNum.Text = "（" + ProductBrowser.GetProductsNumber(false).ToString() + "）";
        }

        protected override void OnInit(EventArgs e)
        {
            string str = HttpContext.Current.Request["task"];
            if (str == "next")
            {
                int num;
                int.TryParse(HttpContext.Current.Request["categoryId"], out this.categoryId);
                this.keyWord = HttpContext.Current.Request["keyWord"];
                string str2 = HttpContext.Current.Request["pgSize"];
                string str3 = HttpContext.Current.Request["pgIndex"];
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = "20";
                }
                if (string.IsNullOrEmpty(str3))
                {
                    str3 = "2";
                }
                DataTable table = ProductBrowser.GetProducts(MemberProcessor.GetCurrentMember(), null, new int?(this.categoryId), this.keyWord, int.Parse(str3), int.Parse(str2), out num, "DisplaySequence", "desc", false);
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(table));
                HttpContext.Current.Response.End();
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ProductListAll.html";
            }
            base.OnInit(e);
        }
    }
}

