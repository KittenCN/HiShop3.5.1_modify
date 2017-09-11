namespace Hidistro.Entities.Settings
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class FreeShipping
    {
        private IList<FreeShippingRegion> freeShippingRegions;

        [RegexValidator(@"(^\d+(\$\d+)?$)", Ruleset="ValFree", MessageTemplate="包邮条件未设置！")]
        public string ConditionNumber { get; set; }

        public int ConditionType { get; set; }

        public int FreeId { get; set; }

        public IList<FreeShippingRegion> FreeShippingRegions
        {
            get
            {
                return this.freeShippingRegions;
            }
            set
            {
                this.freeShippingRegions = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive, Ruleset="ValFree", MessageTemplate="运输方式参数不正确！")]
        public int ModeId { get; set; }

        public string RegionIds { get; set; }

        public int TemplateId { get; set; }
    }
}

