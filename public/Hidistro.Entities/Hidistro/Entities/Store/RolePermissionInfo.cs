namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class RolePermissionInfo
    {
        public static string GetPermissionId(string itemId, string pageLinkId)
        {
            return string.Format("{0}_{1}", itemId, pageLinkId);
        }

        public string PermissionId { get; set; }

        public int RoleId { get; set; }
    }
}

