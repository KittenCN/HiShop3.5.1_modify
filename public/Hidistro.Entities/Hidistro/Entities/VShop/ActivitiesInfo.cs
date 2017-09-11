namespace Hidistro.Entities.VShop
{
    using Sales;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ActivitiesInfo
    {
        public string ActivitiesDescription { get; set; }

        public int ActivitiesId { get; set; }

        public string ActivitiesName { get; set; }

        public int ActivitiesType { get; set; }

        public string CloseRemark { get; set; }

        public DateTime EndTIme { get; set; }

        public List<ShoppingCartItemInfo> LineItems { get; set; }

        public decimal MeetMoney { get; set; }

        public decimal ReductionMoney { get; set; }

        public DateTime StartTime { get; set; }

        public int Type { get; set; }
    }
}

