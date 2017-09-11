namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberDetailOrderQuery : Pagination
    {
        public DateTime? EndDate { get; set; }

        public DateTime? EndFinishDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? StartFinishDate { get; set; }

        public OrderStatus[] Status { get; set; }

        public int? UserId { get; set; }
    }
}

