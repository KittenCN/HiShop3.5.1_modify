namespace Hidistro.Entities.WeiXin
{
    using Hidistro.Entities.VShop;
    using System;
    using System.Runtime.CompilerServices;

    public class SendAllInfo
    {
        public SendAllInfo()
        {
            this.ArticleID = 0;
            this.SendCount = 0;
            this.SendTime = DateTime.Now;
        }

        public int ArticleID { get; set; }

        public string Content { get; set; }

        public int Id { get; set; }

        public Hidistro.Entities.VShop.MessageType MessageType { get; set; }

        public string MsgID { get; set; }

        public int SendCount { get; set; }

        public int SendState { get; set; }

        public DateTime SendTime { get; set; }

        public string Title { get; set; }
    }
}

