namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ActivitiesQuery : Pagination
    {
        public string ActivitiesName { get; set; }

        public string State { get; set; }
    }
}

