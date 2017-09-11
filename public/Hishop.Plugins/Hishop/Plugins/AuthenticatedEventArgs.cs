namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    public class AuthenticatedEventArgs : EventArgs
    {
        public AuthenticatedEventArgs(string openId)
        {
            this.OpenId = openId;
        }

        public string OpenId { get; private set; }
    }
}

