namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OneyuanTaoPartInQuery : Pagination
    {
        public string ActivityId { get; set; }

        public string Atitle { get; set; }

        public string CellPhone { get; set; }

        public int IsPay { get; set; }

        public string PayWay { get; set; }

        public string Pid { get; set; }

        public int state { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}

