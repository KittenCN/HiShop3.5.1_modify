namespace Hishop.Components.Validation
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class ValueAccess
    {
        protected ValueAccess()
        {
        }

        public abstract bool GetValue(object source, out object value, out string valueAccessFailureMessage);

        public abstract string Key { get; }
    }
}

