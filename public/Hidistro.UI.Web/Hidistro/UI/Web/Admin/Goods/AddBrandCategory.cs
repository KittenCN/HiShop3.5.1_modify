namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.BrandCategories)]
    public class AddBrandCategory : AdminPage
    {
        protected Button btnAddBrandCategory;
        protected Button btnSave;
        protected ProductTypesCheckBoxList chlistProductTypes;
        protected ucUeditor fckDescription;
        protected TextBox txtBrandName;
        protected TextBox txtCompanyUrl;
        protected TextBox txtkeyword;
        protected TextBox txtMetaDescription;
        protected TextBox txtReUrl;
        protected UpImg uploader1;

        protected AddBrandCategory() : base("m02", "spp08")
        {
        }

        protected void btnAddBrandCategory_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (!string.IsNullOrEmpty(this.uploader1.UploadedImageUrl.ToString()))
            {
                brandCategoryInfo.Logo = this.uploader1.UploadedImageUrl;
            }
            else
            {
                this.ShowMsg("请上传图片！", false);
                return;
            }
            if (string.IsNullOrEmpty(brandCategoryInfo.BrandName))
            {
                this.ShowMsg("请输入品牌名称！", false);
            }
            else if (string.IsNullOrEmpty(brandCategoryInfo.Description))
            {
                this.ShowMsg("请输入品牌介绍！", false);
            }
            else if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
            {
                this.txtBrandName.Text = "";
                this.txtCompanyUrl.Text = "";
                this.txtkeyword.Text = "";
                this.txtMetaDescription.Text = "";
                this.txtReUrl.Text = "";
                this.fckDescription.Text = "";
                this.ShowMsg("成功添加品牌分类", true);
            }
            else
            {
                this.ShowMsg("添加品牌分类失败", true);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (!string.IsNullOrEmpty(this.uploader1.UploadedImageUrl.ToString()))
            {
                brandCategoryInfo.Logo = this.uploader1.UploadedImageUrl;
            }
            else
            {
                this.ShowMsg("请选择图片上传！", false);
                return;
            }
            if (string.IsNullOrEmpty(brandCategoryInfo.BrandName))
            {
                this.ShowMsg("请输入品牌名称！", false);
            }
            else if (string.IsNullOrEmpty(brandCategoryInfo.Description))
            {
                this.ShowMsg("请输入品牌介绍！", false);
            }
            else if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
            {
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/Goods/BrandCategories.aspx"), true);
            }
            else
            {
                this.ShowMsg("添加品牌分类失败", true);
            }
        }

        private BrandCategoryInfo GetBrandCategoryInfo()
        {
            BrandCategoryInfo info = new BrandCategoryInfo {
                BrandName = Globals.HtmlEncode(this.txtBrandName.Text.Trim())
            };
            if (!string.IsNullOrEmpty(this.txtCompanyUrl.Text))
            {
                info.CompanyUrl = this.txtCompanyUrl.Text.Trim();
            }
            else
            {
                info.CompanyUrl = null;
            }
            info.RewriteName = Globals.HtmlEncode(this.txtReUrl.Text.Trim());
            info.MetaKeywords = Globals.HtmlEncode(this.txtkeyword.Text.Trim());
            info.MetaDescription = Globals.HtmlEncode(this.txtMetaDescription.Text.Trim());
            IList<int> list = new List<int>();
            foreach (ListItem item in this.chlistProductTypes.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            info.ProductTypes = list;
            info.Description = (!string.IsNullOrEmpty(this.fckDescription.Text) && (this.fckDescription.Text.Length > 0)) ? this.fckDescription.Text : null;
            return info;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.btnAddBrandCategory.Click += new EventHandler(this.btnAddBrandCategory_Click);
            if (!base.IsPostBack)
            {
                this.chlistProductTypes.DataBind();
            }
        }

        private bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<BrandCategoryInfo>(brandCategory, new string[] { "ValBrandCategory" });
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

