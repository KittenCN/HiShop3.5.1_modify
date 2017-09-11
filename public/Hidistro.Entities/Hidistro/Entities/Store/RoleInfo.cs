namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class RoleInfo
    {
        public virtual string Description { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual int RoleId { get; set; }

        public virtual string RoleName { get; set; }
    }
}

