namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProductType)]
    public class EditProductType : AdminPage
    {
        protected Button btnEditProductType;
        protected BrandCategoriesCheckBoxList chlistBrand;
        protected TextBox txtRemark;
        protected TextBox txtTypeName;
        private int typeId;

        protected EditProductType() : base("m02", "spp07")
        {
        }

        private void btnEditProductType_Click(object sender, EventArgs e)
        {
            ProductTypeInfo productType = new ProductTypeInfo {
                TypeId = this.typeId,
                TypeName = this.txtTypeName.Text,
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
            if (this.ValidationProductType(productType) && ProductTypeHelper.UpdateProductType(productType))
            {
                this.ShowMsg("修改成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
            {
                int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
            }
            this.btnEditProductType.Click += new EventHandler(this.btnEditProductType_Click);
            if (!this.Page.IsPostBack)
            {
                this.chlistBrand.DataBind();
                ProductTypeInfo productType = ProductTypeHelper.GetProductType(this.typeId);
                if (productType == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.txtTypeName.Text = productType.TypeName;
                    this.txtRemark.Text = productType.Remark;
                    foreach (ListItem item in this.chlistBrand.Items)
                    {
                        if (productType.Brands.Contains(int.Parse(item.Value)))
                        {
                            item.Selected = true;
                        }
                    }
                }
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

