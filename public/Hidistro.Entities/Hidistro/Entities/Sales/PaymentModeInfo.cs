namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PaymentModeInfo
    {
        public decimal CalcPayCharge(decimal cartMoney)
        {
            if (!this.IsPercent)
            {
                return this.Charge;
            }
            return (cartMoney * (this.Charge / 100M));
        }

        public PayApplicationType ApplicationType { get; set; }

        public decimal Charge { get; set; }

        public string Description { get; set; }

        public int DisplaySequence { get; set; }

        public string Gateway { get; set; }

        public bool IsPercent { get; set; }

        public bool IsUseInDistributor { get; set; }

        public bool IsUseInpour { get; set; }

        public int ModeId { get; set; }

        [HtmlCoding]
        public string Name { get; set; }

        public string Settings { get; set; }
    }
}

