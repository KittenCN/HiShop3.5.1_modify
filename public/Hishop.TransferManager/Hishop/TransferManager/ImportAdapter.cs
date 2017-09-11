namespace Hishop.TransferManager
{
    using System;

    public abstract class ImportAdapter
    {
        protected ImportAdapter()
        {
        }

        public abstract object[] CreateMapping(params object[] initParams);
        public abstract object[] ParseIndexes(params object[] importParams);
        public abstract object[] ParseProductData(params object[] importParams);
        public abstract string PrepareDataFiles(params object[] initParams);

        public abstract Target ImportTo { get; }

        public abstract Target Source { get; }
    }
}

