namespace Hishop.Weixin.Pay.Notify
{
    using System;
    using System.Runtime.CompilerServices;

    public class AlarmNotify : NotifyObject
    {
        public string AlarmContent { get; set; }

        public string AppId { get; set; }

        public string AppSignature { get; set; }

        public string Description { get; set; }

        public int ErrorType { get; set; }

        public string SignMethod { get; set; }

        public long TimeStamp { get; set; }
    }
}

