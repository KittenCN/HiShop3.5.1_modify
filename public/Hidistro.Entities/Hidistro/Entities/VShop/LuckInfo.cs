namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class LuckInfo
    {
        public string ActivityId { get; set; }

        public int BuyNum { get; set; }

        public DateTime BuyTime { get; set; }

        public int Id { get; set; }

        public bool IsWin { get; set; }

        public string Pid { get; set; }

        public string PrizeNum { get; set; }

        public string PrizeNums { get; set; }

        public string UserHead { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}

