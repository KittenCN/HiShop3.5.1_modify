namespace Hidistro.Entities.Settings
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SpecifyRegionGroup
    {
        private IList<SpecifyRegion> specifyRegions;

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "1000", RangeBoundaryType.Inclusive, Ruleset="ValRegionGroup", MessageTemplate="计量单位（件）请填写数值，最多保留两位小数！")]
        public decimal AddNumber { get; set; }

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValRegionGroup", MessageTemplate="增量价格请填写数值，最多保留两位小数！")]
        public decimal AddPrice { get; set; }

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "1000", RangeBoundaryType.Inclusive, Ruleset="ValRegionGroup", MessageTemplate="计量单位（件）请填写数值，最多保留两位小数！")]
        public decimal FristNumber { get; set; }

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValRegionGroup", MessageTemplate="首重价格请填写数值，最多保留两位小数！")]
        public decimal FristPrice { get; set; }

        public int GroupId { get; set; }

        public bool IsDefault { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive, Ruleset="ValRegionGroup", MessageTemplate="运输方式参数不正确！")]
        public int ModeId { get; set; }

        public string RegionIds { get; set; }

        public IList<SpecifyRegion> SpecifyRegions
        {
            get
            {
                return this.specifyRegions;
            }
            set
            {
                this.specifyRegions = value;
            }
        }

        public int TemplateId { get; set; }
    }
}

