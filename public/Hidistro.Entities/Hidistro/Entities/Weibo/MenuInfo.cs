namespace Hidistro.Entities.Weibo
{
    using System;
    using System.Runtime.CompilerServices;

    public class MenuInfo
    {
        public string Content { get; set; }

        public int DisplaySequence { get; set; }

        public int MenuId { get; set; }

        public string Name { get; set; }

        public int ParentMenuId { get; set; }

        public string Type { get; set; }
    }
}

