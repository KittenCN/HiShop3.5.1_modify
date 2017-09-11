namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class NewsMsgInfo
    {
        public string Content { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        public string PicUrl { get; set; }

        public NewsReplyInfo Reply { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}

