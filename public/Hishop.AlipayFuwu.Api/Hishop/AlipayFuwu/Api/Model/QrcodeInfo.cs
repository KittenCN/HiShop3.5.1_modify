namespace Hishop.AlipayFuwu.Api.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class QrcodeInfo
    {
        public Hishop.AlipayFuwu.Api.Model.codeInfo codeInfo { get; set; }

        public string codeType { get; set; }

        public int expireSecond { get; set; }

        public string showLogo { get; set; }
    }
}

