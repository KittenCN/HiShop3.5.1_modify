namespace Hidistro.Entities.VShop
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NewsReplyInfo : ReplyInfo
    {
        public NewsReplyInfo()
        {
            base.MessageType = MessageType.List;
        }

        public IList<NewsMsgInfo> NewsMsg { get; set; }
    }
}

