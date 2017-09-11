namespace Hidistro.Entities.Store
{
    using System;

    public enum SystemAuthorizationState
    {
        未经官方授权 = 0,
        已过授权有效期 = -1,
        正常权限 = 1
    }
}

