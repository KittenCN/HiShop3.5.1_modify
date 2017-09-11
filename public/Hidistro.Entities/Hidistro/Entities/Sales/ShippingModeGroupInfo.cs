namespace Hidistro.Entities.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ShippingModeGroupInfo
    {
        private IList<ShippingRegionInfo> modeRegions = new List<ShippingRegionInfo>();

        public decimal AddPrice { get; set; }

        public int GroupId { get; set; }

        public IList<ShippingRegionInfo> ModeRegions
        {
            get
            {
                return this.modeRegions;
            }
            set
            {
                this.modeRegions = value;
            }
        }

        public decimal Price { get; set; }

        public int TemplateId { get; set; }
    }
}

