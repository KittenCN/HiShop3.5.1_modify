namespace Hidistro.SaleSystem.Vshop
{
    using Hidistro.Core.Exceptions;
    using System;

    public class OrderException : HiException
    {
        public OrderException()
        {
        }

        public OrderException(string message) : base(message)
        {
        }
    }
}

