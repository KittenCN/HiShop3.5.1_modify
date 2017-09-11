namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Web.Admin;
    using Hidistro.UI.Web.hieditor.ueditor.controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;

    [PrivilegeCheck(Privilege.AddProductCategory)]
    public class ProductEdit : ProductBasePage
    {
        protected Button btnNext;
        protected Button btnSave;
        protected int categoryid = Globals.RequestQueryNum("categoryId");
        protected CheckBox cbIsSetCommission;
        protected RadioButton ChkisfreeShipping;
        protected CheckBox chkSkuEnabled;
        protected CheckBox ckbIsDownPic;
        protected BrandCategoriesDropDownList dropBrandCategories;
        protected ProductTypeDownList dropProductTypes;
        protected ucUeditor fckDescription;
        protected FreightTemplateDownList FreightTemplateDownList1;
        protected HiddenField hdfSKUPrefix;
        protected int isnext = Globals.RequestQueryNum("isnext");
        protected HtmlGenericControl l_tags;
        protected Literal litCategoryName;
        protected ProductTagsLiteral litralProductTag;
        protected HyperLink lnkEditCategory;
        protected string operatorName = "编辑";
        protected int productId;
        protected RadioButton radInStock;
        protected RadioButton radOnSales;
        protected RadioButton radUnSales;
        protected RadioButton rbtIsSetTemplate;
        protected string reurl = Globals.RequestQueryStr("reurl");
        protected Script Script1;
        protected Script Script2;
        protected Script Script3;
        protected Script Script4;
        protected HtmlGenericControl spanJs;
        private string thisUrl = string.Empty;
        protected TrimTextBox txtAttributes;
        protected TrimTextBox txtCostPrice;
        protected TrimTextBox txtCubicMeter;
        protected TrimTextBox txtDisplaySequence;
        protected TrimTextBox txtFirstCommission;
        protected TrimTextBox txtFreightWeight;
        protected TrimTextBox txtMarketPrice;
        protected TrimTextBox txtMemberPrices;
        protected TrimTextBox txtProductCode;
        protected TrimTextBox txtProductName;
        protected TrimTextBox txtProductShortName;
        protected TrimTextBox txtProductTag;
        protected TrimTextBox txtSalePrice;
        protected TrimTextBox txtSecondCommission;
        protected TrimTextBox txtShortDescription;
        protected TrimTextBox txtShowSaleCounts;
        protected TrimTextBox txtSku;
        protected TrimTextBox txtSkus;
        protected TrimTextBox txtStock;
        protected TrimTextBox txtThirdCommission;
        protected TrimTextBox txtUnit;
        protected TrimTextBox txtWeight;
        protected ProductFlashUpload ucFlashUpload1;

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.SubmitProduct("next");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.SubmitProduct("update");
        }

        private void LoadProduct(ProductInfo product, Dictionary<int, IList<int>> attrs)
        {
            this.dropProductTypes.SelectedValue = product.TypeId;
            this.dropBrandCategories.SelectedValue = product.BrandId;
            if (product.IsSetCommission)
            {
                this.cbIsSetCommission.Checked = false;
                this.txtFirstCommission.Enabled = true;
                this.txtSecondCommission.Enabled = true;
                this.txtThirdCommission.Enabled = true;
                this.txtFirstCommission.Text = product.FirstCommission.ToString("F2");
                this.txtSecondCommission.Text = product.SecondCommission.ToString("F2");
                this.txtThirdCommission.Text = product.ThirdCommission.ToString("F2");
            }
            else
            {
                CategoryInfo category = CatalogHelper.GetCategory(this.categoryid);
                if (category != null)
                {
                    this.txtFirstCommission.Text = category.FirstCommission;
                    this.txtSecondCommission.Text = category.SecondCommission;
                    this.txtThirdCommission.Text = category.ThirdCommission;
                }
            }
            this.txtDisplaySequence.Text = product.DisplaySequence.ToString();
            this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
            this.txtProductShortName.Text = Globals.HtmlDecode(product.ProductShortName);
            this.txtProductCode.Text = product.ProductCode;
            this.txtUnit.Text = product.Unit;
            if (product.MarketPrice.HasValue)
            {
                this.txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            this.txtShortDescription.Text = product.ShortDescription;
            this.fckDescription.Text = product.Description;
            if (product.SaleStatus == ProductSaleStatus.OnSale)
            {
                this.radOnSales.Checked = true;
            }
            else if (product.SaleStatus == ProductSaleStatus.UnSale)
            {
                this.radUnSales.Checked = true;
            }
            else
            {
                this.radInStock.Checked = true;
            }
            this.ChkisfreeShipping.Checked = product.IsfreeShipping;
            string str = product.ImageUrl1 + "," + product.ImageUrl2 + "," + product.ImageUrl3 + "," + product.ImageUrl4 + "," + product.ImageUrl5;
            this.ucFlashUpload1.Value = str.Replace(",,", ",").Replace(",,", ",").Trim(new char[] { ',' });
            if ((attrs != null) && (attrs.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<xml><attributes>");
                foreach (int num in attrs.Keys)
                {
                    builder.Append("<item attributeId=\"").Append(num.ToString(CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int) ProductTypeHelper.GetAttribute(num).UsageMode).ToString()).Append("\" >");
                    foreach (int num2 in attrs[num])
                    {
                        builder.Append("<attValue valueId=\"").Append(num2.ToString(CultureInfo.InvariantCulture)).Append("\" />");
                    }
                    builder.Append("</item>");
                }
                builder.Append("</attributes></xml>");
                this.txtAttributes.Text = builder.ToString();
            }
            if (product.HasSKU && (product.Skus.Keys.Count > 0))
            {
                StringBuilder builder2 = new StringBuilder();
                builder2.Append("<xml><productSkus>");
                foreach (string str2 in product.Skus.Keys)
                {
                    SKUItem item = product.Skus[str2];
                    string str3 = ("<item skuCode=\"" + item.SKU + "\" salePrice=\"" + item.SalePrice.ToString("F2") + "\" costPrice=\"" + ((item.CostPrice > 0M) ? item.CostPrice.ToString("F2") : "") + "\" qty=\"" + item.Stock.ToString(CultureInfo.InvariantCulture) + "\" weight=\"" + ((item.Weight > 0M) ? item.Weight.ToString("F2") : "") + "\">") + "<skuFields>";
                    foreach (int num3 in item.SkuItems.Keys)
                    {
                        string[] strArray3 = new string[] { "<sku attributeId=\"", num3.ToString(CultureInfo.InvariantCulture), "\" valueId=\"", item.SkuItems[num3].ToString(CultureInfo.InvariantCulture), "\" />" };
                        string str4 = string.Concat(strArray3);
                        str3 = str3 + str4;
                    }
                    str3 = str3 + "</skuFields>";
                    if (item.MemberPrices.Count > 0)
                    {
                        str3 = str3 + "<memberPrices>";
                        foreach (int num4 in item.MemberPrices.Keys)
                        {
                            decimal num17 = item.MemberPrices[num4];
                            str3 = str3 + string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", num4.ToString(CultureInfo.InvariantCulture), num17.ToString("F2"));
                        }
                        str3 = str3 + "</memberPrices>";
                    }
                    str3 = str3 + "</item>";
                    builder2.Append(str3);
                }
                builder2.Append("</productSkus></xml>");
                this.txtSkus.Text = builder2.ToString();
            }
            else
            {
                product.HasSKU = false;
            }
            SKUItem defaultSku = product.DefaultSku;
            this.txtSku.Text = product.SKU;
            this.txtSalePrice.Text = defaultSku.SalePrice.ToString("F2");
            this.txtCostPrice.Text = (defaultSku.CostPrice > 0M) ? defaultSku.CostPrice.ToString("F2") : "";
            this.txtStock.Text = ProductHelper.GetProductSumStock(product.ProductId).ToString();
            this.txtWeight.Text = (defaultSku.Weight > 0M) ? defaultSku.Weight.ToString("F2") : "";
            if (defaultSku.MemberPrices.Count > 0)
            {
                this.txtMemberPrices.Text = "<xml><gradePrices>";
                foreach (int num5 in defaultSku.MemberPrices.Keys)
                {
                    decimal num22 = defaultSku.MemberPrices[num5];
                    this.txtMemberPrices.Text = this.txtMemberPrices.Text + string.Format("<grande id=\"{0}\" price=\"{1}\" />", num5.ToString(CultureInfo.InvariantCulture), num22.ToString("F2"));
                }
                this.txtMemberPrices.Text = this.txtMemberPrices.Text + "</gradePrices></xml>";
            }
            this.chkSkuEnabled.Checked = product.HasSKU;
            this.rbtIsSetTemplate.Checked = product.FreightTemplateId > 0;
            this.txtShowSaleCounts.Text = product.ShowSaleCounts.ToString();
            this.txtCubicMeter.Text = product.CubicMeter.ToString("F2");
            this.txtFreightWeight.Text = product.FreightWeight.ToString("F2");
            this.FreightTemplateDownList1.SelectedValue = product.FreightTemplateId;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Globals.RequestFormStr("gettype") == "getcategorycommission")
            {
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"0\",\"tips\":\"获取失败！\"}";
                this.categoryid = Globals.RequestFormNum("categoryId");
                if (this.categoryid > 0)
                {
                    CategoryInfo category = CatalogHelper.GetCategory(this.categoryid);
                    if (category != null)
                    {
                        s = "{\"type\":\"1\",\"f\":\"" + category.FirstCommission + "\",\"s\":\"" + category.SecondCommission + "\",\"t\":\"" + category.ThirdCommission + "\"}";
                    }
                }
                base.Response.Write(s);
                base.Response.End();
            }
            else
            {
                if (string.IsNullOrEmpty(this.reurl))
                {
                    this.reurl = "productonsales.aspx";
                }
                this.thisUrl = base.Request.Url.ToString().ToLower();
                if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && (base.Request.QueryString["isCallback"] == "true"))
                {
                    base.DoCallback();
                }
                else
                {
                    this.productId = Globals.RequestQueryNum("productId");
                    if ((this.productId == 0) && (this.isnext == 1))
                    {
                        this.thisUrl = this.thisUrl.Replace("isnext=1", "").Replace("&&", "&");
                        base.Response.Redirect(this.thisUrl);
                        base.Response.End();
                    }
                    if ((this.productId > 0) && (Globals.RequestFormStr("posttype") == "updatecontent"))
                    {
                        base.Response.ContentType = "application/json";
                        string str4 = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                        string content = Globals.RequestFormStr("memo");
                        if (Globals.ToNum(ProductHelper.UpdateProductContent(this.productId, content)) > 0)
                        {
                            str4 = "{\"type\":\"1\",\"tips\":\"商品信息保存成功！\"}";
                        }
                        base.Response.Write(str4);
                        base.Response.End();
                    }
                    if (!this.Page.IsPostBack)
                    {
                        this.FreightTemplateDownList1.DataBind();
                        IList<int> tagsId = null;
                        if (this.productId > 0)
                        {
                            Dictionary<int, IList<int>> dictionary;
                            ProductInfo product = ProductHelper.GetProductDetails(this.productId, out dictionary, out tagsId);
                            if (product == null)
                            {
                                base.GotoResourceNotFound();
                            }
                            else
                            {
                                if (this.categoryid > 0)
                                {
                                    this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryid);
                                    this.ViewState["ProductCategoryId"] = this.categoryid;
                                    this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryid.ToString(CultureInfo.InvariantCulture) + "&reurl=" + base.Server.UrlEncode(this.reurl);
                                }
                                else
                                {
                                    this.litCategoryName.Text = CatalogHelper.GetFullCategory(product.CategoryId);
                                    this.categoryid = product.CategoryId;
                                    this.ViewState["ProductCategoryId"] = product.CategoryId;
                                    this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + product.CategoryId.ToString(CultureInfo.InvariantCulture);
                                }
                                string navigateUrl = this.lnkEditCategory.NavigateUrl;
                                this.lnkEditCategory.NavigateUrl = navigateUrl + "&productId=" + product.ProductId.ToString(CultureInfo.InvariantCulture) + "&reurl=" + base.Server.UrlEncode(this.reurl);
                                this.litralProductTag.SelectedValue = tagsId;
                                if (tagsId.Count > 0)
                                {
                                    foreach (int num in tagsId)
                                    {
                                        this.txtProductTag.Text = this.txtProductTag.Text + num.ToString() + ",";
                                    }
                                    this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
                                }
                                this.dropProductTypes.DataBind();
                                this.dropBrandCategories.DataBind();
                                this.LoadProduct(product, dictionary);
                            }
                        }
                        else if (this.categoryid > 0)
                        {
                            this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryid);
                            this.ViewState["ProductCategoryId"] = this.categoryid;
                            this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryid.ToString(CultureInfo.InvariantCulture);
                            this.dropProductTypes.DataBind();
                            this.dropBrandCategories.DataBind();
                            CategoryInfo info3 = CatalogHelper.GetCategory(this.categoryid);
                            if (info3 != null)
                            {
                                this.txtFirstCommission.Text = info3.FirstCommission;
                                this.txtSecondCommission.Text = info3.SecondCommission;
                                this.txtThirdCommission.Text = info3.ThirdCommission;
                                this.txtSku.Text = info3.SKUPrefix;
                                this.txtProductCode.Text = info3.SKUPrefix;
                            }
                        }
                        else
                        {
                            base.Response.Redirect("selectcategory.aspx");
                            base.Response.End();
                        }
                    }
                }
            }
        }

        private void SubmitProduct(string opername)
        {
            int num2;
            int num3;
            decimal num4;
            decimal? nullable;
            decimal? nullable2;
            decimal num6;
            decimal num7;
            decimal num8;
            decimal num9;
            decimal num10;
            decimal? nullable3;
            string str = this.ucFlashUpload1.Value.Trim();
            this.ucFlashUpload1.Value = str;
            string[] strArray = str.Split(new char[] { ',' });
            string[] strArray2 = new string[] { "", "", "", "", "" };
            for (int i = 0; (i < strArray.Length) && (i < 5); i++)
            {
                strArray2[i] = strArray[i];
            }
            if (this.categoryid == 0)
            {
                this.categoryid = (int) this.ViewState["ProductCategoryId"];
            }
            bool flag = !this.cbIsSetCommission.Checked;
            int showSaleCounts = 0;
            if (this.ValidateConverts(this.txtProductName.Text.Trim(), this.chkSkuEnabled.Checked, out num2, out num4, out nullable, out nullable2, out num3, out nullable3, out showSaleCounts, out num6, out num7, out num8, out num9, out num10))
            {
                Globals.Debuglog("商品规格：" + this.chkSkuEnabled.Checked.ToString(), "_Debuglog.txt");
                if (!this.chkSkuEnabled.Checked && (num4 <= 0M))
                {
                    this.ShowMsg("商品现价必须大于0", false);
                }
                else
                {
                    string text = this.fckDescription.Text;
                    if (this.ckbIsDownPic.Checked)
                    {
                        text = base.DownRemotePic(text);
                    }
                    ProductInfo target = new ProductInfo {
                        ProductId = this.productId,
                        CategoryId = this.categoryid,
                        TypeId = this.dropProductTypes.SelectedValue,
                        ProductName = this.txtProductName.Text.Trim().Replace(@"\", ""),
                        ProductShortName = this.txtProductShortName.Text.Trim(),
                        ProductCode = this.txtProductCode.Text.Trim(),
                        DisplaySequence = num2,
                        MarketPrice = nullable2,
                        Unit = this.txtUnit.Text.Trim(),
                        ImageUrl1 = strArray2[0],
                        ImageUrl2 = strArray2[1],
                        ImageUrl3 = strArray2[2],
                        ImageUrl4 = strArray2[3],
                        ImageUrl5 = strArray2[4],
                        ThumbnailUrl40 = strArray2[0].Replace("/images/", "/thumbs40/40_"),
                        ThumbnailUrl60 = strArray2[0].Replace("/images/", "/thumbs60/60_"),
                        ThumbnailUrl100 = strArray2[0].Replace("/images/", "/thumbs100/100_"),
                        ThumbnailUrl160 = strArray2[0].Replace("/images/", "/thumbs160/160_"),
                        ThumbnailUrl180 = strArray2[0].Replace("/images/", "/thumbs180/180_"),
                        ThumbnailUrl220 = strArray2[0].Replace("/images/", "/thumbs220/220_"),
                        ThumbnailUrl310 = strArray2[0].Replace("/images/", "/thumbs310/310_"),
                        ThumbnailUrl410 = strArray2[0].Replace("/images/", "/thumbs410/410_"),
                        ShortDescription = this.txtShortDescription.Text,
                        IsfreeShipping = this.ChkisfreeShipping.Checked,
                        Description = (!string.IsNullOrEmpty(text) && (text.Length > 0)) ? text : null,
                        AddedDate = DateTime.Now,
                        BrandId = this.dropBrandCategories.SelectedValue,
                        FirstCommission = num6,
                        SecondCommission = num7,
                        ThirdCommission = num8,
                        FreightTemplateId = this.ChkisfreeShipping.Checked ? 0 : this.FreightTemplateDownList1.SelectedValue,
                        IsSetCommission = flag,
                        CubicMeter = num9,
                        FreightWeight = num10
                    };
                    ProductSaleStatus onSale = ProductSaleStatus.OnSale;
                    if (this.radInStock.Checked)
                    {
                        onSale = ProductSaleStatus.OnStock;
                    }
                    if (this.radUnSales.Checked)
                    {
                        onSale = ProductSaleStatus.UnSale;
                    }
                    if (this.radOnSales.Checked)
                    {
                        onSale = ProductSaleStatus.OnSale;
                    }
                    target.SaleStatus = onSale;
                    CategoryInfo category = CatalogHelper.GetCategory(this.categoryid);
                    if (category != null)
                    {
                        target.MainCategoryPath = category.Path + "|";
                    }
                    Dictionary<string, SKUItem> skus = null;
                    Dictionary<int, IList<int>> attrs = null;
                    if (this.chkSkuEnabled.Checked)
                    {
                        decimal num11 = 0M;
                        decimal num12 = new decimal(0);
                        target.HasSKU = true;
                        skus = base.GetSkus(this.txtSkus.Text);
                        if (skus == null)
                        {
                            this.ShowMsg("商品规格填写不完整！", false);
                            return;
                        }
                        decimal[] minSalePrice = new decimal[] { 79228162514264337593543950335M };
                        foreach (SKUItem item in from sku in skus.Values
                            where sku.SalePrice < minSalePrice[0]
                            select sku)
                        {
                            minSalePrice[0] = item.SalePrice;
                        }
                        num11 = minSalePrice[0];
                        decimal[] maxSalePrice = new decimal[] { -79228162514264337593543950335M };
                        foreach (SKUItem item2 in from sku in skus.Values
                            where sku.SalePrice > maxSalePrice[0]
                            select sku)
                        {
                            maxSalePrice[0] = item2.SalePrice;
                        }
                        num12 = maxSalePrice[0];
                        target.MinShowPrice = num11;
                        target.MaxShowPrice = num12;
                    }
                    else
                    {
                        Dictionary<string, SKUItem> dictionary3 = new Dictionary<string, SKUItem>();
                        SKUItem item3 = new SKUItem {
                            SkuId = "0",
                            SKU = this.txtSku.Text,
                            SalePrice = num4,
                            CostPrice = nullable.HasValue ? nullable.Value : 0M,
                            Stock = num3,
                            Weight = nullable3.HasValue ? nullable3.Value : 0M
                        };
                        dictionary3.Add("0", item3);
                        skus = dictionary3;
                        if (this.txtMemberPrices.Text.Length > 0)
                        {
                            base.GetMemberPrices(skus["0"], this.txtMemberPrices.Text);
                        }
                        target.MinShowPrice = num4;
                        target.MaxShowPrice = num4;
                    }
                    if (!string.IsNullOrEmpty(this.txtAttributes.Text) && (this.txtAttributes.Text.Length > 0))
                    {
                        attrs = base.GetAttributes(this.txtAttributes.Text);
                    }
                    ValidationResults validateResults = Hishop.Components.Validation.Validation.Validate<ProductInfo>(target);
                    if (!validateResults.IsValid)
                    {
                        this.ShowMsg(validateResults);
                    }
                    else
                    {
                        IList<int> tagIds = new List<int>();
                        if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
                        {
                            string str3 = this.txtProductTag.Text.Trim();
                            string[] strArray3 = null;
                            if (str3.Contains(","))
                            {
                                strArray3 = str3.Split(new char[] { ',' });
                            }
                            else
                            {
                                strArray3 = new string[] { str3 };
                            }
                            foreach (string str4 in strArray3)
                            {
                                tagIds.Add(Convert.ToInt32(str4));
                            }
                        }
                        if (this.productId > 0)
                        {
                            ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(this.productId);
                            target.SaleCounts = productBaseInfo.SaleCounts;
                            target.ShowSaleCounts = productBaseInfo.ShowSaleCounts;
                        }
                        else
                        {
                            target.SaleCounts = 0;
                            target.ShowSaleCounts = showSaleCounts;
                        }
                        if (this.productId > 0)
                        {
                            switch (ProductHelper.UpdateProduct(target, skus, attrs, tagIds))
                            {
                                case ProductActionStatus.AttributeError:
                                    this.ShowMsg("保存商品失败，保存商品属性时出错", false);
                                    return;

                                case ProductActionStatus.DuplicateName:
                                    this.ShowMsg("保存商品失败，商品名称不能重复", false);
                                    return;

                                case ProductActionStatus.DuplicateSKU:
                                    this.ShowMsg("保存商品失败，商品编码不能重复", false);
                                    return;

                                case ProductActionStatus.SKUError:
                                    this.ShowMsg("保存商品失败，商品编码不能重复", false);
                                    return;

                                case ProductActionStatus.OffShelfError:
                                    this.ShowMsg("保存商品失败，子站没在零售价范围内的商品无法下架", false);
                                    return;

                                case ProductActionStatus.ProductTagEroor:
                                    this.ShowMsg("保存商品失败，保存商品标签时出错", false);
                                    return;

                                case ProductActionStatus.Success:
                                    ProductHelper.UpdateShoppingCartsTemplateId(this.productId, target.FreightTemplateId);
                                    this.litralProductTag.SelectedValue = tagIds;
                                    if (opername == "next")
                                    {
                                        string thisUrl = this.thisUrl;
                                        if (this.isnext != 1)
                                        {
                                            thisUrl = string.Concat(new object[] { "/Admin/goods/ProductEdit.aspx?productId=", this.productId, "&isnext=1&reurl=", base.Server.UrlEncode(this.reurl) });
                                        }
                                        base.Response.Redirect(thisUrl);
                                        base.Response.End();
                                        return;
                                    }
                                    this.spanJs.InnerHtml = "<script>$('#ctl00_ContentPlaceHolder1_btnSave,#preview,#prevBtn').attr('disabled', 'true');setTimeout(function () { $('#ctl00_ContentPlaceHolder1_btnSave,#preview,#prevBtn').removeAttr('disabled'); }, 5000);</script>";
                                    this.ShowMsgAndReUrl("保存商品信息成功!", true, this.reurl);
                                    return;
                            }
                            this.ShowMsg("保存商品失败，未知错误", false);
                        }
                        else
                        {
                            string s = ProductHelper.AddProductNew(target, skus, attrs, tagIds);
                            int num13 = Globals.ToNum(s);
                            if (num13 > 0)
                            {
                                base.Response.Redirect("productedit.aspx?productid=" + num13 + "&isnext=1");
                                base.Response.End();
                            }
                            else
                            {
                                this.ShowMsg("保存商品失败，" + s, false);
                            }
                        }
                    }
                }
            }
        }

        private bool ValidateConverts(string productname, bool skuEnabled, out int displaySequence, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out decimal? weight, out int showSaleCounts, out decimal firstCommission, out decimal secondCommission, out decimal thirdCommission, out decimal cubicMeter, out decimal freightWeight)
        {
            decimal num;
            string str = string.Empty;
            if (string.IsNullOrEmpty(productname))
            {
                str = str + Formatter.FormatErrorMessage("请输入商品名称");
            }
            if (!int.TryParse(this.txtShowSaleCounts.Text.Trim(), out showSaleCounts))
            {
                showSaleCounts = 0;
            }
            if (showSaleCounts < 0)
            {
                showSaleCounts = 0;
            }
            costPrice = 0;
            marketPrice = 0;
            weight = 0;
            displaySequence = stock = 0;
            salePrice = 0M;
            if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
            {
                str = str + Formatter.FormatErrorMessage("请正确填写商品排序");
            }
            if (decimal.TryParse(this.txtMarketPrice.Text, out num))
            {
                marketPrice = new decimal?(num);
            }
            else
            {
                str = str + Formatter.FormatErrorMessage("请正确填写商品原价");
            }
            if (!skuEnabled)
            {
                if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品现价");
                }
                if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
                {
                    decimal num2;
                    if (decimal.TryParse(this.txtCostPrice.Text, out num2))
                    {
                        costPrice = new decimal?(num2);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的成本价");
                    }
                }
                if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品库存");
                }
                if (!string.IsNullOrEmpty(this.txtWeight.Text))
                {
                    decimal num3;
                    if (decimal.TryParse(this.txtWeight.Text, out num3))
                    {
                        weight = new decimal?(num3);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的重量");
                    }
                }
            }
            else if (!string.IsNullOrEmpty(this.txtSalePrice.Text))
            {
                if (decimal.TryParse(this.txtMarketPrice.Text, out num))
                {
                    salePrice = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品现价");
                }
            }
            if (this.txtThirdCommission.Text.Trim() == "")
            {
                thirdCommission = 0M;
            }
            else if (decimal.TryParse(this.txtThirdCommission.Text, out num))
            {
                thirdCommission = num;
            }
            else
            {
                thirdCommission = 0M;
                str = str + Formatter.FormatErrorMessage("请正确填写上二级佣金比例");
            }
            if (this.txtSecondCommission.Text.Trim() == "")
            {
                secondCommission = 0M;
            }
            else if (decimal.TryParse(this.txtSecondCommission.Text, out num))
            {
                secondCommission = num;
            }
            else
            {
                secondCommission = 0M;
                str = str + Formatter.FormatErrorMessage("请正确填写上一级佣金比例");
            }
            if (this.txtFirstCommission.Text.Trim() == "")
            {
                firstCommission = 0M;
            }
            else if (decimal.TryParse(this.txtFirstCommission.Text, out num))
            {
                firstCommission = num;
            }
            else
            {
                firstCommission = 0M;
                str = str + Formatter.FormatErrorMessage("请正确填写成交店铺佣金比例");
            }
            if (decimal.TryParse(this.txtCubicMeter.Text, out num))
            {
                cubicMeter = num;
            }
            else
            {
                cubicMeter = 0M;
            }
            if (decimal.TryParse(this.txtFreightWeight.Text, out num))
            {
                freightWeight = num;
            }
            else
            {
                freightWeight = 0M;
            }
            if (this.rbtIsSetTemplate.Checked && (this.FreightTemplateDownList1.SelectedValue < 1))
            {
                str = str + Formatter.FormatErrorMessage("请选择运费模版");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

