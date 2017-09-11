namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
    public sealed class NotNullValidatorAttribute : ValueValidatorAttribute
    {
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new NotNullValidator(base.Negated);
        }
    }
}

