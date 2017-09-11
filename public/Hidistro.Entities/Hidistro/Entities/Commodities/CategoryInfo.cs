﻿namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    [HasSelfValidation]
    public class CategoryInfo
    {
        [SelfValidation(Ruleset="ValCategory")]
        public void CheckCategory(ValidationResults results)
        {
            if (!string.IsNullOrEmpty(this.SKUPrefix) && ((this.SKUPrefix.Length > 5) || !Regex.IsMatch(this.SKUPrefix, "(?!_)(?!-)[a-zA-Z0-9_-]+")))
            {
                results.AddResult(new ValidationResult("商品编码前缀长度限制在5个字符以内,只能以字母或数字开头", this, "", "", null));
            }
            if (!string.IsNullOrEmpty(this.RewriteName) && ((this.RewriteName.Length > 60) || !Regex.IsMatch(this.RewriteName, "(^[-_a-zA-Z0-9]+$)")))
            {
                results.AddResult(new ValidationResult("使用URL重写长度限制在60个字符以内，必须为字母数字-和_", this, "", "", null));
            }
        }

        public int? AssociatedProductType { get; set; }

        public int CategoryId { get; set; }

        public int Depth { get; set; }

        public int DisplaySequence { get; set; }

        public string FirstCommission { get; set; }

        public bool HasChildren { get; set; }

        public string IconUrl { get; set; }

        [StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="告诉搜索引擎此分类浏览页面的主要内容，长度限制在100个字符以内"), HtmlCoding]
        public string MetaDescription { get; set; }

        [StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内"), HtmlCoding]
        public string MetaKeywords { get; set; }

        [StringLengthValidator(0, 50, Ruleset="ValCategory", MessageTemplate="告诉搜索引擎此分类浏览页面的标题，长度限制在50个字符以内"), HtmlCoding]
        public string MetaTitle { get; set; }

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValCategory", MessageTemplate="分类名称不能为空，长度限制在60个字符以内")]
        public string Name { get; set; }

        public string Notes1 { get; set; }

        public string Notes2 { get; set; }

        public string Notes3 { get; set; }

        public string Notes4 { get; set; }

        public string Notes5 { get; set; }

        public int? ParentCategoryId { get; set; }

        public string Path { get; set; }

        public string RewriteName { get; set; }

        public string SecondCommission { get; set; }

        [HtmlCoding]
        public string SKUPrefix { get; set; }

        public string Theme { get; set; }

        public string ThirdCommission { get; set; }

        public int TopCategoryId
        {
            get
            {
                if (this.Depth == 1)
                {
                    return this.CategoryId;
                }
                return int.Parse(this.Path.Substring(0, this.Path.IndexOf("|")));
            }
        }
    }
}

