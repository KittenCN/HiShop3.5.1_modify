namespace Hidistro.Entities.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ManagerQuery : Pagination
    {
        public int RoleId { get; set; }

        public string Username { get; set; }
    }
}

