namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class ForceFollowInfo
    {
        public string ConcernMsg { get; set; }

        public bool EnableGuidePageSet { get; set; }

        public int FollowInfo { get; set; }

        public int GuideConcernType { get; set; }

        public string GuidePageSet { get; set; }

        public bool IsAutoGuide { get; set; }

        public bool IsMustConcern { get; set; }
    }
}

