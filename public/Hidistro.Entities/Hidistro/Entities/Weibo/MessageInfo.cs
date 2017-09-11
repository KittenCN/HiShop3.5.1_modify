namespace Hidistro.Entities.Weibo
{
    using System;
    using System.Runtime.CompilerServices;

    public class MessageInfo
    {
        public string Access_Token { get; set; }

        public int ArticleId { get; set; }

        public string Created_at { get; set; }

        public string DisplayName { get; set; }

        public string Image { get; set; }

        public int MessageId { get; set; }

        public string Receiver_id { get; set; }

        public string Sender_id { get; set; }

        public DateTime SenderDate { get; set; }

        public string SenderMessage { get; set; }

        public int Status { get; set; }

        public string Summary { get; set; }

        public string Text { get; set; }

        public string Tovfid { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public string Vfid { get; set; }
    }
}

