namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class BrandCategoryInfo
    {
        private IList<int> productTypes;

        public int BrandId { get; set; }

        [StringLengthValidator(1, 30, Ruleset="ValBrandCategory", MessageTemplate="品牌名称不能为空，长度限制在30个字符以内")]
        public string BrandName { get; set; }

        [RegexValidator("^(http)://.*", Ruleset="ValBrandCategory"), NotNullValidator(Negated=true, Ruleset="ValBrandCategory"), ValidatorComposition(CompositionType.Or, Ruleset="ValBrandCategory", MessageTemplate="品牌官方网站的网址必须以http://开头，长度限制在100个字符以内")]
        public string CompanyUrl { get; set; }

        [StringLengthValidator(0, 300, Ruleset="ValBrandCategory", MessageTemplate="品牌介绍不能为空,品牌介绍的长度限制在300个字符以内")]
        public string Description { get; set; }

        public int DisplaySequence { get; set; }

        public string Logo { get; set; }

        [StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内"), HtmlCoding]
        public string MetaDescription { get; set; }

        [StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内"), HtmlCoding]
        public string MetaKeywords { get; set; }

        public IList<int> ProductTypes
        {
            get
            {
                if (this.productTypes == null)
                {
                    this.productTypes = new List<int>();
                }
                return this.productTypes;
            }
            set
            {
                this.productTypes = value;
            }
        }

        public string RewriteName { get; set; }

        public string Theme { get; set; }
    }
}

