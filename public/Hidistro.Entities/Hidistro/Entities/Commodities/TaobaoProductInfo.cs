﻿namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Runtime.CompilerServices;

    public class TaobaoProductInfo
    {
        public long Cid { get; set; }

        public decimal EMSFee { get; set; }

        public decimal ExpressFee { get; set; }

        public string FreightPayer { get; set; }

        public bool HasDiscount { get; set; }

        public bool HasInvoice { get; set; }

        public bool HasWarranty { get; set; }

        public string InputPids { get; set; }

        public string InputStr { get; set; }

        public DateTime ListTime { get; set; }

        public string LocationCity { get; set; }

        public string LocationState { get; set; }

        public long Num { get; set; }

        public decimal PostFee { get; set; }

        public int ProductId { get; set; }

        public string PropertyAlias { get; set; }

        public string ProTitle { get; set; }

        public string SkuOuterIds { get; set; }

        public string SkuPrices { get; set; }

        public string SkuProperties { get; set; }

        public string SkuQuantities { get; set; }

        public string StuffStatus { get; set; }

        public long ValidThru { get; set; }
    }
}

