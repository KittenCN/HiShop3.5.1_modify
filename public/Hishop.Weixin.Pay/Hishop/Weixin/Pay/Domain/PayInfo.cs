﻿namespace Hishop.Weixin.Pay.Domain
{
    using System;
    using System.Runtime.CompilerServices;

    public class PayInfo
    {
        public PayInfo()
        {
            this.SignType = "MD5";
            this.ServiceVersion = "1.0";
            this.InputCharSet = "GBK";
            this.SignKeyIndex = 1;
            this.TimeEnd = DateTime.Now;
        }

        public string Attach { get; set; }

        public string BankBillNo { get; set; }

        public string BankType { get; set; }

        public string BuyerAlias { get; set; }

        public decimal Discount { get; set; }

        public int FeeType { get; set; }

        public string Info { get; set; }

        public string InputCharSet { get; set; }

        public string NotifyId { get; set; }

        public string OutTradeNo { get; set; }

        public string Partner { get; set; }

        public decimal ProductFee { get; set; }

        public string ServiceVersion { get; set; }

        public string Sign { get; set; }

        public int SignKeyIndex { get; set; }

        public string SignType { get; set; }

        public DateTime TimeEnd { get; set; }

        public decimal TotalFee { get; set; }

        public int TradeMode { get; set; }

        public int TradeState { get; set; }

        public string TransactionId { get; set; }

        public decimal TransportFee { get; set; }
    }
}

