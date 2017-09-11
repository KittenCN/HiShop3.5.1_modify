namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    public class FailedEventArgs : EventArgs
    {
        public FailedEventArgs(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }
}

