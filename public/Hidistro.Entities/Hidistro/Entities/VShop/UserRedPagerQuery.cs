namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class UserRedPagerQuery : Pagination
    {
        public bool IsUsed { get; set; }

        public int RedPagerID { get; set; }

        public UserRedPagerType Type { get; set; }

        public int UserID { get; set; }
    }
}

