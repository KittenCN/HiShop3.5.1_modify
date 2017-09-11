namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class TopicQuery : Pagination
    {
        public bool? IsincludeHomeProduct { get; set; }

        public bool? IsRelease { get; set; }
    }
}

