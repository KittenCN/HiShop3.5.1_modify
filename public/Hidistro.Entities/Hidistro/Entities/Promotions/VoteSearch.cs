namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class VoteSearch : Pagination
    {
        public string Name { get; set; }

        public VoteStatus status { get; set; }
    }
}

