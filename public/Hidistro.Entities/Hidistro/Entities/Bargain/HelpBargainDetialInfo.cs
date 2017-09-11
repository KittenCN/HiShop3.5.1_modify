namespace Hidistro.Entities.Bargain
{
    using System;
    using System.Runtime.CompilerServices;

    public class HelpBargainDetialInfo
    {
        public int BargainDetialId { get; set; }

        public int BargainId { get; set; }

        public decimal BargainPrice { get; set; }

        public DateTime CreateDate { get; set; }

        public int Id { get; set; }

        public int UserId { get; set; }
    }
}

