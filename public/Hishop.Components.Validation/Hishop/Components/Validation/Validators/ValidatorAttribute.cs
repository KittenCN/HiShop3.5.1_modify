namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
    public abstract class ValidatorAttribute : BaseValidationAttribute, IValidatorDescriptor
    {
        protected ValidatorAttribute()
        {
        }

        protected abstract Validator DoCreateValidator(Type targetType);
        protected virtual Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            return this.DoCreateValidator(targetType);
        }

        Validator IValidatorDescriptor.CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            Validator validator = this.DoCreateValidator(targetType, ownerType, memberValueAccessBuilder);
            validator.Tag = base.Tag;
            validator.MessageTemplate = base.GetMessageTemplate();
            return validator;
        }
    }
}

