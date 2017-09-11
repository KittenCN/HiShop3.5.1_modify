namespace Hidistro.Entities.Commodities
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AttributeInfo
    {
        private IList<AttributeValueInfo> attributeValues;

        public int AttributeId { get; set; }

        [StringLengthValidator(1, 30, Ruleset="ValAttribute", MessageTemplate="扩展属性的名称，长度在1至30个字符之间")]
        public string AttributeName { get; set; }

        public IList<AttributeValueInfo> AttributeValues
        {
            get
            {
                if (this.attributeValues == null)
                {
                    this.attributeValues = new List<AttributeValueInfo>();
                }
                return this.attributeValues;
            }
            set
            {
                this.attributeValues = value;
            }
        }

        public int DisplaySequence { get; set; }

        public bool IsMultiView
        {
            get
            {
                return (this.UsageMode == AttributeUseageMode.MultiView);
            }
        }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public AttributeUseageMode UsageMode { get; set; }

        public bool UseAttributeImage { get; set; }

        public string ValuesString
        {
            get
            {
                string str = string.Empty;
                foreach (AttributeValueInfo info in this.AttributeValues)
                {
                    str = str + info.ValueStr + ",";
                }
                if (str.Length > 0)
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }
    }
}

