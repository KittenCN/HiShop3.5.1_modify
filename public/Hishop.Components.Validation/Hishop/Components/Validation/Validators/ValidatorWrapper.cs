namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    public sealed class ValidatorWrapper : Validator
    {
        private Validator wrappedValidator;

        public ValidatorWrapper(Validator wrappedValidator) : base(null, null)
        {
            this.wrappedValidator = wrappedValidator;
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
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

