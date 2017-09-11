namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ExChangeSearch : Pagination
    {
        public bool? bFinished { get; set; }

        public string ProductName { get; set; }

        public ExchangeStatus status { get; set; }
    }
}

