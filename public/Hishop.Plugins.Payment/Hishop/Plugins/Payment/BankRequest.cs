﻿namespace Hishop.Plugins.Payment
{
    using Hishop.Plugins;
    using System;

    [Plugin("线下支付")]
    public class BankRequest : PaymentRequest
    {
        private readonly string url;

        public BankRequest()
        {
        }

        public BankRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.url = showUrl;
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            this.RedirectToGateway(this.url);
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool IsMedTrade
        {
            get
            {
                return false;
            }
        }

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        protected override bool NeedProtect
        {
            get
            {
                return false;
            }
        }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

