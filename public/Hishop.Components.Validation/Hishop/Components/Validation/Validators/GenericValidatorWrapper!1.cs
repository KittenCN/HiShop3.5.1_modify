namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    internal sealed class GenericValidatorWrapper<T> : Validator<T>
    {
        private Validator wrappedValidator;

        public GenericValidatorWrapper(Validator wrappedValidator) : base(null, null)
        {
            this.wrappedValidator = wrappedValidator;
        }

        protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            this.wrappedValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);
        }

        protected override string DefaultMessageTemplate
        {
            get
            {
                return null;
            }
        }
    }
}

