namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class LotteryTicketInfo : LotteryActivityInfo
    {
        public string GradeIds { get; set; }

        public string InvitationCode { get; set; }

        public bool IsOpened { get; set; }

        public int MinValue { get; set; }

        public DateTime OpenTime { get; set; }
    }
}

