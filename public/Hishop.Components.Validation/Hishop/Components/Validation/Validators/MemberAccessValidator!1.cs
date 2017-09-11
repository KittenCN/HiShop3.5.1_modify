﻿namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    public abstract class MemberAccessValidator<T> : Validator<T>
    {
        private ValueAccessValidator valueAccessValidator;

        protected MemberAccessValidator(ValueAccess valueAccess, Validator valueValidator) : base(null, null)
        {
            this.valueAccessValidator = new ValueAccessValidator(valueAccess, valueValidator);
        }

        protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            this.valueAccessValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);
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

