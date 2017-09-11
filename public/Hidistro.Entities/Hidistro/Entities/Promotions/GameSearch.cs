namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class GameSearch : Pagination
    {
        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? GameType { get; set; }

        public string Status { get; set; }
    }
}

