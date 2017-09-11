namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public sealed class ObjectValidatorAttribute : ValidatorAttribute
    {
        private string targetRuleset;

        public ObjectValidatorAttribute() : this(string.Empty)
        {
        }

        public ObjectValidatorAttribute(string targetRuleset)
        {
            if (targetRuleset == null)
            {
                throw new ArgumentNullException("targetRuleset");
            }
            this.targetRuleset = targetRuleset;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new ObjectValidator(targetType, this.targetRuleset);
        }
    }
}

