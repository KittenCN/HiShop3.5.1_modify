namespace Hidistro.Entities.Store
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class FriendExtensionQuery : Pagination
    {
        [HtmlCoding]
        public string ExensionImg { get; set; }

        public string ExensiontRemark { get; set; }

        public int ExtensionId { get; set; }
    }
}

