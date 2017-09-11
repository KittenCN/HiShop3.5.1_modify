namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class MessageInfo
    {
        public string Content { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int MsgId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int UrlType { get; set; }
    }
}

