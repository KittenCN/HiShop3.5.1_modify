namespace Hishop.Weixin.Pay.Lib
{
    using System;

    public class WxPayException : Exception
    {
        public WxPayException(string msg) : base(msg)
        {
        }
    }
}

