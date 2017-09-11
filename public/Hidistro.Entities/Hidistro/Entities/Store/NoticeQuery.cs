namespace Hidistro.Entities.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class NoticeQuery : Pagination
    {
        public string Author { get; set; }

        public DateTime? EndTime { get; set; }

        public int? IsDel { get; set; }

        public bool? IsDistributor { get; set; }

        public int? IsNotShowRead { get; set; }

        public int? IsPub { get; set; }

        public int SendType { get; set; }

        public DateTime? StartTime { get; set; }

        public string Title { get; set; }

        public int? UserId { get; set; }
    }
}

