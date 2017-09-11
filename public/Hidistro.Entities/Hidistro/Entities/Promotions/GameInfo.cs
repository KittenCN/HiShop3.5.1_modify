namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameInfo
    {
        public string ApplyMembers { get; set; }

        public DateTime BeginTime { get; set; }

        public string CustomGroup { get; set; }

        public string DefualtGroup { get; set; }

        public string Description { get; set; }

        public DateTime EndTime { get; set; }

        public int GameId { get; set; }

        public string GameQRCodeAddress { get; set; }

        public string GameTitle { get; set; }

        public Hidistro.Entities.Promotions.GameType GameType { get; set; }

        public string GameUrl { get; set; }

        public int GivePoint { get; set; }

        public string KeyWork { get; set; }

        public int LimitEveryDay { get; set; }

        public int MaximumDailyLimit { get; set; }

        public int MemberCheck { get; set; }

        public int NeedPoint { get; set; }

        public string NotPrzeDescription { get; set; }

        public bool OnlyGiveNotPrizeMember { get; set; }

        public Hidistro.Entities.Promotions.PlayType PlayType { get; set; }

        public float PrizeRate { get; set; }

        public GameStatus Status { get; set; }

        public string WinningPool { get; set; }
    }
}

