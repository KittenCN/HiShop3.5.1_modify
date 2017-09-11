namespace Hidistro.Messages
{
    using System;
    using System.Runtime.CompilerServices;

    public class WxJsonResult
    {
        public object AppendData { get; set; }

        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}

