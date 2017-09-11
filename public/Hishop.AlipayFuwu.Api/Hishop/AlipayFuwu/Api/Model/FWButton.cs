namespace Hishop.AlipayFuwu.Api.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class FWButton
    {
        public string actionParam { get; set; }

        public string actionType { get; set; }

        public string name { get; set; }

        public IEnumerable<FWButton> subButton { get; set; }
    }
}

