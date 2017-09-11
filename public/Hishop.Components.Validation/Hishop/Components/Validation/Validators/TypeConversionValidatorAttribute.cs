namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public sealed class TypeConversionValidatorAttribute : ValueValidatorAttribute
    {
        private Type targetType;

        public TypeConversionValidatorAttribute(Type targetType)
        {
            ValidatorArgumentsValidatorHelper.ValidateTypeConversionValidator(targetType);
            this.targetType = targetType;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new TypeConversionValidator(this.targetType, base.Negated);
        }
    }
}

