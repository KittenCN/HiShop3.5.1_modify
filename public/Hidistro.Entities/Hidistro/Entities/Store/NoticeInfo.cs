namespace Hidistro.Entities.Store
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class NoticeInfo
    {
        public DateTime AddTime { get; set; }

        public string Author { get; set; }

        public int Id { get; set; }

        public int IsPub { get; set; }

        public string Memo { get; set; }

        public IList<Hidistro.Entities.Store.NoticeUserInfo> NoticeUserInfo { get; set; }

        public DateTime? PubTime { get; set; }

        public int SendTo { get; set; }

        public int SendType { get; set; }

        public string Title { get; set; }
    }
}

