namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class SystemAuthorizationInfo
    {
        public int DistributorCount { get; set; }

        public bool IsShowJixuZhiChi { get; set; }

        public SystemAuthorizationState state { get; set; }

        public string type { get; set; }
    }
}

