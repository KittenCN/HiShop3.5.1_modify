namespace Hidistro.Entities.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ExpressQueryResult
    {
        public string logisticsCode { get; set; }

        public string reason { get; set; }

        public string shipperCode { get; set; }

        public string state { get; set; }

        public bool success { get; set; }

        public List<Trace> traces { get; set; }
    }
}

