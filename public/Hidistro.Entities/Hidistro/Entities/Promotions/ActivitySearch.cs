namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ActivitySearch : Pagination
    {
        public DateTime? begin { get; set; }

        public DateTime? end { get; set; }

        public string Name { get; set; }

        public ActivityStatus status { get; set; }
    }
}

