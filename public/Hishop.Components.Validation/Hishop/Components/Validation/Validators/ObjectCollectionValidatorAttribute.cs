namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
    public sealed class ObjectCollectionValidatorAttribute : ValidatorAttribute
    {
        private string targetRuleset;
        private Type targetType;

        public ObjectCollectionValidatorAttribute(Type targetType) : this(targetType, string.Empty)
        {
        }

        public ObjectCollectionValidatorAttribute(Type targetType, string targetRuleset)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (targetRuleset == null)
            {
                throw new ArgumentNullException("targetRuleset");
            }
            this.targetType = targetType;
            this.targetRuleset = targetRuleset;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new ObjectCollectionValidator(this.targetType, this.targetRuleset);
        }
    }
}

