namespace Hidistro.Entities.Weibo
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ReplyKeyInfo : Pagination
    {
        public int Id { get; set; }

        public string Keys { get; set; }

        public string Matching { get; set; }

        public int Type { get; set; }
    }
}

