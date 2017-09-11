namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberQuery : Pagination
    {
        public string CellPhone { get; set; }

        public string CharSymbol { get; set; }

        public string ClientType { get; set; }

        public DateTime? EndTime { get; set; }

        public int? GradeId { get; set; }

        public string GradeIds { get; set; }

        public int? GroupId { get; set; }

        public string GroupIds { get; set; }

        public bool? HasVipCard { get; set; }

        public bool? IsApproved { get; set; }

        public decimal? OrderMoney { get; set; }

        public int? OrderNumber { get; set; }

        public string Realname { get; set; }

        public DateTime? RegisterEndTime { get; set; }

        public DateTime? RegisterStartTime { get; set; }

        public DateTime? StartTime { get; set; }

        public string StoreName { get; set; }

        public UserStatus? Stutas { get; set; }

        public decimal? TradeMoneyEnd { get; set; }

        public decimal? TradeMoneyStart { get; set; }

        public int? TradeNumEnd { get; set; }

        public int? TradeNumStart { get; set; }

        public string UserBindName { get; set; }

        public string Username { get; set; }
    }
}

