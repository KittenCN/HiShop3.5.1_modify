namespace Hidistro.Entities.VShop
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LotteryActivityInfo
    {
        public string ActivityDesc { get; set; }

        public int ActivityId { get; set; }

        public string ActivityKey { get; set; }

        public string ActivityName { get; set; }

        public string ActivityPic { get; set; }

        public int ActivityType { get; set; }

        public DateTime EndTime { get; set; }

        public int MaxNum { get; set; }

        public string PrizeSetting { get; set; }

        public List<Hidistro.Entities.VShop.PrizeSetting> PrizeSettingList { get; set; }

        public DateTime StartTime { get; set; }
    }
}

