namespace Hishop.Weixin.MP.Domain
{
    using System;
    using System.Runtime.CompilerServices;

    public class Video : IMedia, IThumbMedia
    {
        public string MediaId { get; set; }

        public string ThumbMediaId { get; set; }
    }
}

