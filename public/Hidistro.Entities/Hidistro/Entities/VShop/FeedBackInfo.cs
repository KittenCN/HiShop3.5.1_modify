namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class FeedBackInfo
    {
        public FeedBackInfo()
        {
            this.TimeStamp = DateTime.Now;
        }

        public string AppId { get; set; }

        public string ExtInfo { get; set; }

        public string FeedBackId { get; set; }

        public int FeedBackNotifyID { get; set; }

        public string MsgType { get; set; }

        public string OpenId { get; set; }

        public string Reason { get; set; }

        public string Solution { get; set; }

        public DateTime TimeStamp { get; set; }

        public string TransId { get; set; }
    }
}

