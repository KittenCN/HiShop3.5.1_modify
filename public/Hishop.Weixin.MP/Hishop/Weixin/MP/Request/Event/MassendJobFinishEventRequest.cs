namespace Hishop.Weixin.MP.Request.Event
{
    using Hishop.Weixin.MP;
    using Hishop.Weixin.MP.Request;
    using System;
    using System.Runtime.CompilerServices;

    public class MassendJobFinishEventRequest : EventRequest
    {
        public string ErrorCount { get; set; }

        public override RequestEventType Event
        {
            get
            {
                return RequestEventType.MASSSENDJOBFINISH;
            }
            set
            {
            }
        }

        public string FilterCount { get; set; }

        public string SentCount { get; set; }

        public string Status { get; set; }

        public string TotalCount { get; set; }
    }
}

