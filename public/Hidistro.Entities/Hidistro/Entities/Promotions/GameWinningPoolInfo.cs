namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameWinningPoolInfo
    {
        public int GameId { get; set; }

        public int GamePrizeId { get; set; }

        public int IsReceive { get; set; }

        public int Number { get; set; }

        public int WinningPoolId { get; set; }
    }
}

