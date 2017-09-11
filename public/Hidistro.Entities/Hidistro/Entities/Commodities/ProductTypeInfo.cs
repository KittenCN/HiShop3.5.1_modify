namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ProductTypeInfo
    {
        private IList<int> brands;

        public IList<int> Brands
        {
            get
            {
                if (this.brands == null)
                {
                    this.brands = new List<int>();
                }
                return this.brands;
            }
            set
            {
                this.brands = value;
            }
        }

        [StringLengthValidator(0, 100, Ruleset="ValProductType", MessageTemplate="备注的长度限制在0-100个字符之间"), HtmlCoding]
        public string Remark { get; set; }

        public int TypeId { get; set; }

        [StringLengthValidator(1, 30, Ruleset="ValProductType", MessageTemplate="商品类型名称不能为空，长度限制在1-30个字符之间")]
        public string TypeName { get; set; }
    }
}

