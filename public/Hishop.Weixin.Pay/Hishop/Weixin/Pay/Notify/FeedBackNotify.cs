namespace Hishop.Weixin.Pay.Notify
{
    using System;
    using System.Runtime.CompilerServices;

    public class FeedBackNotify : NotifyObject
    {
        public string AppId { get; set; }

        public string AppSignature { get; set; }

        public string ExtInfo { get; set; }

        public string FeedBackId { get; set; }

        public string MsgType { get; set; }

        public string OpenId { get; set; }

        public string Reason { get; set; }

        public string Solution { get; set; }

        public long TimeStamp { get; set; }

        public string TransId { get; set; }
    }
}

