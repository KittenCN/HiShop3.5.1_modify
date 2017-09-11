namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
    public sealed class ValidatorCompositionAttribute : BaseValidationAttribute
    {
        private Hishop.Components.Validation.CompositionType compositionType;

        public ValidatorCompositionAttribute(Hishop.Components.Validation.CompositionType compositionType)
        {
            this.compositionType = compositionType;
        }

        internal Hishop.Components.Validation.CompositionType CompositionType
        {
            get
            {
                return this.compositionType;
            }
        }
    }
}

