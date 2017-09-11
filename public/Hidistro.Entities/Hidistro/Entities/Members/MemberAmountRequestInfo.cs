namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class MemberAmountRequestInfo
    {
        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public decimal Amount { get; set; }

        public string BankName { get; set; }

        public string CellPhone { get; set; }

        public DateTime? CheckTime { get; set; }

        public int Id { get; set; }

        public string Operator { get; set; }

        public string RedpackId { get; set; }

        public string Remark { get; set; }

        public DateTime RequestTime { get; set; }

        public RequesType RequestType { get; set; }

        public RequesState State { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}

