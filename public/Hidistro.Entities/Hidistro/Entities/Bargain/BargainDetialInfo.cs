namespace Hidistro.Entities.Bargain
{
    using System;
    using System.Runtime.CompilerServices;

    public class BargainDetialInfo
    {
        public int BargainId { get; set; }

        public DateTime CreateDate { get; set; }

        public int Id { get; set; }

        public int IsDelete { get; set; }

        public int Number { get; set; }

        public int NumberOfParticipants { get; set; }

        public decimal Price { get; set; }

        public string Sku { get; set; }

        public int UserId { get; set; }
    }
}

