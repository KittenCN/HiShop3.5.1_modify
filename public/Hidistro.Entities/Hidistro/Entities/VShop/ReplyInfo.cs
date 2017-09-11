namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class ReplyInfo
    {
        public ReplyInfo()
        {
            this.LastEditDate = DateTime.Now;
            this.MatchType = Hidistro.Entities.VShop.MatchType.Like;
            this.MessageType = Hidistro.Entities.VShop.MessageType.Text;
        }

        public int ActivityId { get; set; }

        public int ArticleID { get; set; }

        public int Id { get; set; }

        public bool IsDisable { get; set; }

        public string Keys { get; set; }

        public DateTime LastEditDate { get; set; }

        public string LastEditor { get; set; }

        public Hidistro.Entities.VShop.MatchType MatchType { get; set; }

        public Hidistro.Entities.VShop.MessageType MessageType { get; set; }

        public string MessageTypeName
        {
            get
            {
                return this.MessageType.ToShowText();
            }
        }

        public Hidistro.Entities.VShop.ReplyType ReplyType { get; set; }
    }
}

