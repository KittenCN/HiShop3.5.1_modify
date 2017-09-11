namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CategoriesQuery : Pagination
    {
        public string Name { get; set; }
    }
}

