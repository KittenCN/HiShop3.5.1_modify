namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OneyuanTaoQuery : Pagination
    {
        public int ReachType { get; set; }

        public int state { get; set; }

        public string title { get; set; }
    }
}

