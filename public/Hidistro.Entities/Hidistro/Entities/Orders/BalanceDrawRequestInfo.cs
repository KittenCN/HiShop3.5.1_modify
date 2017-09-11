namespace Hidistro.Entities.Orders
{
    using System;
    using System.Runtime.CompilerServices;

    public class BalanceDrawRequestInfo
    {
        public string AccountName { get; set; }

        public decimal Amount { get; set; }

        public string BankName { get; set; }

        public string CellPhone { get; set; }

        public DateTime CheckTime { get; set; }

        public string IsCheck { get; set; }

        public string MerchantCode { get; set; }

        public string RedpackId { get; set; }

        public string Remark { get; set; }

        public DateTime RequestTime { get; set; }

        public int RequestType { get; set; }

        public int SerialId { get; set; }

        public string StoreName { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserOpenId { get; set; }
    }
}

