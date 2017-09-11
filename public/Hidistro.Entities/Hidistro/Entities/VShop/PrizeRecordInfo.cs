namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class PrizeRecordInfo
    {
        public int ActivityID { get; set; }

        public string ActivityName { get; set; }

        public string CellPhone { get; set; }

        public bool IsPrize { get; set; }

        public string Prizelevel { get; set; }

        public string PrizeName { get; set; }

        public DateTime? PrizeTime { get; set; }

        public string RealName { get; set; }

        public int RecordId { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }
    }
}

