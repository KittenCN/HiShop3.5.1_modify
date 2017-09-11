namespace Hidistro.Entities.Bargain
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class BargainQuery : Pagination
    {
        public string ProductName { get; set; }

        public int Status { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public int UserId { get; set; }
    }
}

