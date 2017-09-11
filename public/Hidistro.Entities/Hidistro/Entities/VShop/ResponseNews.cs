namespace Hidistro.Entities.VShop
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ResponseNews : AbstractResponseMessage
    {
        public ResponseNews()
        {
            base.MsgType = "news";
        }

        public int ArticleCount
        {
            get
            {
                if (this.MessageInfo != null)
                {
                    return this.MessageInfo.Count;
                }
                return 0;
            }
        }

        public IList<Hidistro.Entities.VShop.MessageInfo> MessageInfo { get; set; }
    }
}

