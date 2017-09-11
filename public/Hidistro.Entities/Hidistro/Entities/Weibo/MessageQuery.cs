namespace Hidistro.Entities.Weibo
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MessageQuery : Pagination
    {
        public string Access_Token { get; set; }

        public int Status { get; set; }
    }
}

