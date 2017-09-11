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

    [PrivilegeCheck(Privilege.AddProductCategory)]
    public class CategoryEdit : AdminPage
    {
        protected HtmlForm aspnetForm;
        protected Button btnSaveAddCategory;
        protected Button btnSaveCategory;
        protected int categoryid;
        protected ProductCategoriesDropDownList dropCategories;
        protected ProductTypeDownList dropProductTypes;
        protected HtmlGenericControl liURL;
        protected string operatorName;
        protected string reurl;
        protected TextBox txtCategoryName;
        protected TextBox txtfirst;
        protected TextBox txtPageDesc;
        protected TextBox txtPageKeyTitle;
        protected TextBox txtPageKeyWords;
        protected TextBox txtRewriteName;
        protected TextBox txtsecond;
        protected TextBox txtSKUPrefix;
        protected TextBox txtthird;
        protected UpImg uploader1;

        protected CategoryEdit() : base("m02", "spp06")
        {
            this.reurl = Globals.RequestQueryStr("reurl");
            this.categoryid = Globals.RequestQueryNum("categoryId");
            this.operatorName = "添加";
        }

        private void BindCategoryInfo(CategoryInfo categoryInfo)
        {
            if (categoryInfo != null)
            {
                this.txtCategoryName.Text = categoryInfo.Name;
                this.dropProductTypes.SelectedValue = categoryInfo.AssociatedProductType;
                this.txtSKUPrefix.Text = categoryInfo.SKUPrefix;
                this.txtRewriteName.Text = categoryInfo.RewriteName;
                this.txtPageKeyTitle.Text = categoryInfo.MetaTitle;
                this.txtPageKeyWords.Text = categoryInfo.MetaKeywords;
                this.txtPageDesc.Text = categoryInfo.MetaDescription;
                this.txtfirst.Text = string.IsNullOrEmpty(categoryInfo.FirstCommission) ? "0" : categoryInfo.FirstCommission;
                this.txtsecond.Text = string.IsNullOrEmpty(categoryInfo.SecondCommission) ? "0" : categoryInfo.SecondCommission;
                this.txtthird.Text = string.IsNullOrEmpty(categoryInfo.ThirdCommission) ? "0" : categoryInfo.ThirdCommission;
                bool flag1 = categoryInfo.ParentCategoryId > 0;
                this.uploader1.UploadedImageUrl = categoryInfo.IconUrl;
            }
        }

        private void btnSaveAddCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = this.GetCategory();
            if (category != null)
            {
                if (CatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    this.ShowMsgAndReUrl(this.operatorName + "成功", true, "categoryedit.aspx");
                }
                else
                {
                    this.ShowMsg(this.operatorName + "商品分类失败,未知错误", false);
                }
            }
        }

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (this.categoryid > 0)
            {
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryid);
                if (category == null)
                {
                    this.ShowMsg("编缉商品分类错误,未知", false);
                }
                else
                {
                    category.IconUrl = this.uploader1.UploadedImageUrl;
                    category.Name = this.txtCategoryName.Text;
                    category.SKUPrefix = this.txtSKUPrefix.Text;
                    category.RewriteName = this.txtRewriteName.Text;
                    category.MetaTitle = this.txtPageKeyTitle.Text;
                    category.MetaKeywords = this.txtPageKeyWords.Text;
                    category.MetaDescription = this.txtPageDesc.Text;
                    category.AssociatedProductType = this.dropProductTypes.SelectedValue;
                    category.Notes1 = "";
                    category.Notes2 = "";
                    category.Notes3 = "";
                    if (category.Depth > 1)
                    {
                        CategoryInfo info2 = CatalogHelper.GetCategory(category.ParentCategoryId.Value);
                        if (string.IsNullOrEmpty(category.Notes1))
                        {
                            category.Notes1 = info2.Notes1;
                        }
                        if (string.IsNullOrEmpty(category.Notes2))
                        {
                            category.Notes2 = info2.Notes2;
                        }
                        if (string.IsNullOrEmpty(category.Notes3))
                        {
                            category.Notes3 = info2.Notes3;
                        }
                    }
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<CategoryInfo>(category, new string[] { "ValCategory" });
                    string msg = string.Empty;
                    if (!results.IsValid)
                    {
                        foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                        {
                            msg = msg + Formatter.FormatErrorMessage(result.Message);
                        }
                        this.ShowMsg(msg, false);
                    }
                    else
                    {
                        switch (CatalogHelper.UpdateCategory(category))
                        {
                            case CategoryActionStatus.Success:
                                this.ShowMsgAndReUrl(this.operatorName + "成功", true, this.reurl);
                                return;

                            case CategoryActionStatus.UpdateParentError:
                                this.ShowMsg("不能自己成为自己的上级分类", false);
                                return;
                        }
                        this.ShowMsg(this.operatorName + "商品分类错误", false);
                    }
                }
            }
            else
            {
                CategoryInfo info3 = this.GetCategory();
                if (info3 != null)
                {
                    if (CatalogHelper.AddCategory(info3) == CategoryActionStatus.Success)
                    {
                        this.ShowMsgAndReUrl("成功" + this.operatorName + "了商品分类", true, this.reurl);
                    }
                    else
                    {
                        this.ShowMsg(this.operatorName + "失败", false);
                    }
                }
            }
        }

        private CategoryInfo GetCategory()
        {
            CategoryInfo target = new CategoryInfo {
                IconUrl = this.uploader1.UploadedImageUrl,
                Name = this.txtCategoryName.Text.Trim(),
                ParentCategoryId = this.dropCategories.SelectedValue,
                SKUPrefix = this.txtSKUPrefix.Text.Trim(),
                AssociatedProductType = this.dropProductTypes.SelectedValue
            };
            if (!string.IsNullOrEmpty(this.txtRewriteName.Text.Trim()))
            {
                target.RewriteName = this.txtRewriteName.Text.Trim();
            }
            else
            {
                target.RewriteName = null;
            }
            target.MetaTitle = this.txtPageKeyTitle.Text.Trim();
            target.MetaKeywords = this.txtPageKeyWords.Text.Trim();
            target.MetaDescription = this.txtPageDesc.Text.Trim();
            target.Notes1 = "";
            target.Notes2 = "";
            target.Notes3 = "";
            target.DisplaySequence = 0;
            if (target.ParentCategoryId.HasValue)
            {
                CategoryInfo category = CatalogHelper.GetCategory(target.ParentCategoryId.Value);
                if ((category == null) || (category.Depth >= 5))
                {
                    this.ShowMsg(string.Format("您选择的上级分类有误，商品分类最多只支持{0}级分类", 5), false);
                    return null;
                }
                if (string.IsNullOrEmpty(target.Notes1))
                {
                    target.Notes1 = category.Notes1;
                }
                if (string.IsNullOrEmpty(target.Notes2))
                {
                    target.Notes2 = category.Notes2;
                }
                if (string.IsNullOrEmpty(target.Notes3))
                {
                    target.Notes3 = category.Notes3;
                }
                if (string.IsNullOrEmpty(target.RewriteName))
                {
                    target.RewriteName = category.RewriteName;
                }
            }
            if (string.IsNullOrEmpty(this.txtCategoryName.Text.Trim()))
            {
                this.ShowMsg("分类名称不能为空！", false);
                return null;
            }
            string str = Globals.RequestFormStr(this.txtfirst.ClientID.Replace("_", "$"));
            string str2 = Globals.RequestFormStr(this.txtsecond.ClientID.Replace("_", "$"));
            string str3 = Globals.RequestFormStr(this.txtthird.ClientID.Replace("_", "$"));
            if (string.IsNullOrEmpty(str))
            {
                str = "0";
            }
            if (string.IsNullOrEmpty(str2))
            {
                str2 = "0";
            }
            if (string.IsNullOrEmpty(str3))
            {
                str3 = "0";
            }
            target.FirstCommission = str;
            target.SecondCommission = str2;
            target.ThirdCommission = str3;
            bool flag = false;
            if ((Convert.ToDecimal(target.FirstCommission) < 0M) || (Convert.ToDecimal(target.FirstCommission) > 100M))
            {
                this.ShowMsg("输入的佣金格式不正确！", false);
                flag = true;
            }
            if ((Convert.ToDecimal(target.SecondCommission) < 0M) || (Convert.ToDecimal(target.SecondCommission) > 100M))
            {
                this.ShowMsg("输入的佣金格式不正确！", false);
                flag = true;
            }
            if ((Convert.ToDecimal(target.ThirdCommission) < 0M) || (Convert.ToDecimal(target.ThirdCommission) > 100M))
            {
                this.ShowMsg("输入的佣金格式不正确！", false);
                flag = true;
            }
            if (!flag)
            {
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<CategoryInfo>(target, new string[] { "ValCategory" });
                string msg = string.Empty;
                if (results.IsValid)
                {
                    return target;
                }
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.reurl))
            {
                this.reurl = "managecategories.aspx";
            }
            this.btnSaveCategory.Click += new EventHandler(this.btnSaveCategory_Click);
            this.btnSaveAddCategory.Click += new EventHandler(this.btnSaveAddCategory_Click);
            if (this.categoryid > 0)
            {
                this.operatorName = "编辑";
                this.btnSaveAddCategory.Visible = false;
            }
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                int result = 0;
                int.TryParse(base.Request["parentCategoryId"], out result);
                CategoryInfo category = CatalogHelper.GetCategory(result);
                if (category != null)
                {
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    base.Response.Write("{ ");
                    base.Response.Write(string.Format("\"SKUPrefix\":\"{0}\",\"f\":\"{1}\",\"s\":\"{2}\",\"t\":\"{3}\"", new object[] { category.SKUPrefix, category.FirstCommission, category.SecondCommission, category.ThirdCommission }));
                    base.Response.Write("}");
                    base.Response.End();
                }
            }
            if (!this.Page.IsPostBack)
            {
                if (this.categoryid > 0)
                {
                    CategoryInfo entity = CatalogHelper.GetCategory(this.categoryid);
                    this.dropProductTypes.DataBind();
                    this.dropProductTypes.SelectedValue = entity.AssociatedProductType;
                    if (entity == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(entity, false);
                        this.BindCategoryInfo(entity);
                        if (entity.Depth > 1)
                        {
                            this.liURL.Style.Add("display", "none");
                        }
                    }
                }
                else
                {
                    this.dropProductTypes.DataBind();
                    this.dropCategories.DataBind();
                }
            }
        }
    }
}

