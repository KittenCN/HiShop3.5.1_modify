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
    public class EditBrandCategory : AdminPage
    {
        private int brandId;
        protected Button btnUpdateBrandCategory;
        protected ProductTypesCheckBoxList chlistProductTypes;
        protected ucUeditor fckDescription;
        protected TextBox txtBrandName;
        protected TextBox txtCompanyUrl;
        protected TextBox txtkeyword;
        protected TextBox txtMetaDescription;
        protected TextBox txtReUrl;
        protected UpImg uploader1;

        protected EditBrandCategory() : base("m02", "spp08")
        {
        }

        protected void btnUpdateBrandCategory_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (string.IsNullOrEmpty(brandCategoryInfo.Logo))
            {
                this.ShowMsg("请上传一张品牌LOGO图片", false);
            }
            else if (string.IsNullOrEmpty(brandCategoryInfo.BrandName))
            {
                this.ShowMsg("请输入品牌名称！", false);
            }
            else if (string.IsNullOrEmpty(brandCategoryInfo.Description))
            {
                this.ShowMsg("请输入品牌介绍！", false);
            }
            else if (CatalogHelper.UpdateBrandCategory(brandCategoryInfo))
            {
                base.Response.Redirect(Globals.GetAdminAbsolutePath("/Goods/BrandCategories.aspx"), true);
            }
            else
            {
                this.ShowMsg("编辑品牌分类失败", true);
            }
        }

        private BrandCategoryInfo GetBrandCategoryInfo()
        {
            BrandCategoryInfo info = new BrandCategoryInfo {
                BrandId = this.brandId,
                Logo = this.uploader1.UploadedImageUrl,
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
            info.Description = (!string.IsNullOrEmpty(this.fckDescription.Text) && (this.fckDescription.Text.Length > 0)) ? this.fckDescription.Text : null;
            IList<int> list = new List<int>();
            foreach (ListItem item in this.chlistProductTypes.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            info.ProductTypes = list;
            return info;
        }

        private void loadData()
        {
            BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(this.brandId);
            if (brandCategory == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.ViewState["Logo"] = brandCategory.Logo;
                foreach (ListItem item in this.chlistProductTypes.Items)
                {
                    if (brandCategory.ProductTypes.Contains(int.Parse(item.Value)))
                    {
                        item.Selected = true;
                    }
                }
                this.txtBrandName.Text = Globals.HtmlDecode(brandCategory.BrandName);
                this.txtCompanyUrl.Text = brandCategory.CompanyUrl;
                this.txtReUrl.Text = Globals.HtmlDecode(brandCategory.RewriteName);
                this.txtkeyword.Text = Globals.HtmlDecode(brandCategory.MetaKeywords);
                this.txtMetaDescription.Text = Globals.HtmlDecode(brandCategory.MetaDescription);
                this.fckDescription.Text = brandCategory.Description;
                if (!string.IsNullOrEmpty(brandCategory.Logo))
                {
                    this.uploader1.UploadedImageUrl = brandCategory.Logo;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["brandId"], out this.brandId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnUpdateBrandCategory.Click += new EventHandler(this.btnUpdateBrandCategory_Click);
                if (!this.Page.IsPostBack)
                {
                    this.chlistProductTypes.DataBind();
                    this.loadData();
                }
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

