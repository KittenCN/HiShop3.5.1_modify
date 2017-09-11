namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class CustomPage
    {
        public DateTime CreateTime { get; set; }

        public string Details { get; set; }

        public string DraftDetails { get; set; }

        public bool DraftIsShowMenu { get; set; }

        public string DraftJson { get; set; }

        public string DraftName { get; set; }

        public string DraftPageUrl { get; set; }

        public string FormalJson { get; set; }

        public int Id { get; set; }

        public bool IsShowMenu { get; set; }

        public string Name { get; set; }

        public string PageUrl { get; set; }

        public int PV { get; set; }

        public int Status { get; set; }

        public string TempIndexName { get; set; }
    }
}

