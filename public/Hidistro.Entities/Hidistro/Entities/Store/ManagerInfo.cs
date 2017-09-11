namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class ManagerInfo
    {
        public ManagerInfo()
        {
            this.CreateDate = DateTime.Now;
        }

        public virtual DateTime CreateDate { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual int RoleId { get; set; }

        public virtual int UserId { get; protected set; }

        public virtual string UserName { get; set; }
    }
}

