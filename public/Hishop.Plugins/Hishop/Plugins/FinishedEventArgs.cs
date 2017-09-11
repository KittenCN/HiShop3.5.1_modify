namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    public class FinishedEventArgs : EventArgs
    {
        public FinishedEventArgs(bool isMedTrade)
        {
            this.IsMedTrade = isMedTrade;
        }

        public bool IsMedTrade { get; private set; }
    }
}

