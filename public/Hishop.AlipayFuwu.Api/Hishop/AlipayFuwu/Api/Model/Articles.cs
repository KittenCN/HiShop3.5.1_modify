namespace Hishop.AlipayFuwu.Api.Model
{
    using Hishop.AlipayFuwu.Api.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Articles
    {
        public List<article> articles { get; set; }

        public string createTime
        {
            get
            {
                return AliOHHelper.TransferToMilStartWith1970(DateTime.Now).ToString("F0");
            }
        }

        public string msgType { get; set; }

        public MessageText text { get; set; }

        public string toUserId { get; set; }
    }
}

