namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddProductType)]
    public class AddProductType : AdminPage
    {
        protected Button btnAddProductType;
        protected BrandCategoriesCheckBoxList chlistBrand;
        protected HtmlForm thisForm;
        protected TextBox txtRemark;
        protected TextBox txtTypeName;

        protected AddProductType() : base("m02", "spp07")
        {
        }

        private void btnAddProductType_Click(object sender, EventArgs e)
        {
            ProductTypeInfo productType = new ProductTypeInfo {
                TypeName = this.txtTypeName.Text.Trim(),
                Remark = this.txtRemark.Text
            };
            IList<int> list = new List<int>();
            foreach (ListItem item in this.chlistBrand.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            productType.Brands = list;
            if (this.ValidationProductType(productType))
            {
                int num = ProductTypeHelper.AddProductType(productType);
                if (num > 0)
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/goods/AddAttribute.aspx?typeId=" + num), true);
                }
                else
                {
                    this.ShowMsg("添加商品类型失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAddProductType.Click += new EventHandler(this.btnAddProductType_Click);
            if (!base.IsPostBack)
            {
                this.chlistBrand.DataBind();
            }
        }

        private bool ValidationProductType(ProductTypeInfo productType)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductTypeInfo>(productType, new string[] { "ValProductType" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

