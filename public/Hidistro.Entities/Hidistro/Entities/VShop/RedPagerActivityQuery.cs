namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class RedPagerActivityQuery : Pagination
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public int RedPagerActivityId { get; set; }
    }
}

