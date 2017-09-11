namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameResultMemberLogInfo
    {
        public int GameId { get; set; }

        public int LogId { get; set; }

        public DateTime PlayTime { get; set; }

        public int PrizeId { get; set; }

        public int UserId { get; set; }
    }
}

