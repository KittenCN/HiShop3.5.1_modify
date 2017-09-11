namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal sealed class FieldValueAccess : ValueAccess
    {
        private FieldInfo fieldInfo;

        public FieldValueAccess(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
        {
            value = null;
            valueAccessFailureMessage = null;
            if (source == null)
            {
                valueAccessFailureMessage = string.Format(CultureInfo.CurrentCulture, Resources.ErrorValueAccessNull, new object[] { this.Key });
                return false;
            }
            if (!this.fieldInfo.DeclaringType.IsAssignableFrom(source.GetType()))
            {
                valueAccessFailureMessage = string.Format(CultureInfo.CurrentCulture, Resources.ErrorValueAccessInvalidType, new object[] { this.Key, source.GetType().FullName });
                return false;
            }
            value = this.fieldInfo.GetValue(source);
            return true;
        }

        public override string Key
        {
            get
            {
                return this.fieldInfo.Name;
            }
        }
    }
}

