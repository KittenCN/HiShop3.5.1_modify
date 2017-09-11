namespace Hishop.TransferManager
{
    using System;

    public abstract class ExportAdapter
    {
        protected ExportAdapter()
        {
        }

        public abstract void DoExport();

        public abstract Target ExportTo { get; }

        public abstract Target Source { get; }
    }
}

