namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class UserStatisticsInfo
    {
        public decimal AllUserCounts { get; set; }

        public decimal Lenth
        {
            get
            {
                return (this.Percentage * 4M);
            }
        }

        public decimal Percentage
        {
            get
            {
                if (this.AllUserCounts != 0M)
                {
                    return ((this.Usercounts / this.AllUserCounts) * 100M);
                }
                return 0M;
            }
        }

        public long RegionId { get; set; }

        public string RegionName { get; set; }

        public int Usercounts { get; set; }
    }
}

