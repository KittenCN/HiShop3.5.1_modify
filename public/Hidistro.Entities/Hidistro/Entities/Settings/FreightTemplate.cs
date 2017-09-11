namespace Hidistro.Entities.Settings
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class FreightTemplate
    {
        private IList<FreeShipping> freeShippings;
        private IList<SpecifyRegionGroup> specifyRegionGroups;

        public bool FreeShip { get; set; }

        public IList<FreeShipping> FreeShippings
        {
            get
            {
                return this.freeShippings;
            }
            set
            {
                this.freeShippings = value;
            }
        }

        public bool HasFree { get; set; }

        public int MUnit { get; set; }

        [StringLengthValidator(1, 20, Ruleset="ValFreight", MessageTemplate="请填写模板名称，2-20个字符！")]
        public string Name { get; set; }

        public IList<SpecifyRegionGroup> SpecifyRegionGroups
        {
            get
            {
                return this.specifyRegionGroups;
            }
            set
            {
                this.specifyRegionGroups = value;
            }
        }

        public int TemplateId { get; set; }
    }
}

