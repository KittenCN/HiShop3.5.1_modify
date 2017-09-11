namespace Hishop.Weixin.Pay.Domain
{
    using System;
    using System.Runtime.CompilerServices;

    public class DeliverInfo
    {
        public DeliverInfo()
        {
            this.TimeStamp = DateTime.Now;
            this.Status = true;
            this.Message = "ok";
            this.SignMethod = "sha1";
        }

        public DeliverInfo(string openId, string transId, string outTradeNo) : this()
        {
            this.OpenId = openId;
            this.TransId = transId;
            this.OutTradeNo = outTradeNo;
        }

        public string AppId { get; set; }

        public string AppSignature { get; set; }

        public string Message { get; set; }

        public string OpenId { get; set; }

        public string OutTradeNo { get; set; }

        public string SignMethod { get; private set; }

        public bool Status { get; set; }

        public DateTime TimeStamp { get; set; }

        public string TransId { get; set; }
    }
}

