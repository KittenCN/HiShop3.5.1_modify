namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class SendRedpackRecordInfo
    {
        public string ActName { get; set; }

        public int Amount { get; set; }

        public int BalanceDrawRequestID { get; set; }

        public string ClientIP { get; set; }

        public int ID { get; set; }

        public bool IsSend { get; set; }

        public string OpenID { get; set; }

        public DateTime SendTime { get; set; }

        public int UserID { get; set; }

        public string Wishing { get; set; }
    }
}

