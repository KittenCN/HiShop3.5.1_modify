namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class SendRedpackRecordQuery : Pagination
    {
        public int BalanceDrawRequestID { get; set; }

        public int ID { get; set; }

        public bool IsSend { get; set; }

        public int UserID { get; set; }
    }
}

