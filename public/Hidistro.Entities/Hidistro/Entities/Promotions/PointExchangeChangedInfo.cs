namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class PointExchangeChangedInfo
    {
        public DateTime Date { get; set; }

        public int exChangeId { get; set; }

        public string exChangeName { get; set; }

        public int MemberGrades { get; set; }

        public int MemberID { get; set; }

        public int PointNumber { get; set; }

        public int ProductId { get; set; }
    }
}

