namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ShippingRegionInfo
    {
        public int GroupId { get; set; }

        public int RegionId { get; set; }

        public int TemplateId { get; set; }
    }
}

