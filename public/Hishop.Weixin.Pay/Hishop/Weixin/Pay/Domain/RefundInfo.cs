namespace Hishop.Weixin.Pay.Domain
{
    using System;
    using System.Runtime.CompilerServices;

    public class RefundInfo
    {
        public RefundInfo()
        {
            this.BankType = "WX";
            this.FeeType = "1";
            this.InputCharset = "UTF-8";
        }

        public string BankType { get; private set; }

        public string FeeType { get; private set; }

        public string InputCharset { get; private set; }

        public string nonce_str { get; set; }

        public string NotifyUrl { get; set; }

        public string op_user_id { get; private set; }

        public string out_refund_no { get; set; }

        public string out_trade_no { get; set; }

        public decimal? RefundFee { get; set; }

        public string RefundID { get; set; }

        public decimal? TotalFee { get; set; }

        public string transaction_id { get; set; }
    }
}

