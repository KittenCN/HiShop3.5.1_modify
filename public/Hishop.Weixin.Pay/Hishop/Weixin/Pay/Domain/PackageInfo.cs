namespace Hishop.Weixin.Pay.Domain
{
    using System;
    using System.Runtime.CompilerServices;

    public class PackageInfo
    {
        public PackageInfo()
        {
            this.BankType = "WX";
            this.FeeType = "1";
            this.InputCharset = "UTF-8";
            this.SpbillCreateIp = "127.0.0.1";
        }

        public string Attach { get; set; }

        public string BankType { get; private set; }

        public string Body { get; set; }

        public string FeeType { get; private set; }

        public string GoodsTag { get; set; }

        public string InputCharset { get; private set; }

        public string NotifyUrl { get; set; }

        public string OpenId { get; set; }

        public string OutTradeNo { get; set; }

        public string Partner { get; private set; }

        public decimal? ProductFee { get; set; }

        public string SpbillCreateIp { get; set; }

        public DateTime? TimeExpire { get; set; }

        public DateTime? TimeStart { get; set; }

        public decimal TotalFee { get; set; }

        public decimal? TransportFee { get; set; }
    }
}

