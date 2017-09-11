namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;

    public class ValueAccessValidator : Validator
    {
        private ValueAccess valueAccess;
        private Validator valueValidator;

        public ValueAccessValidator(ValueAccess valueAccess, Validator valueValidator) : base(null, null)
        {
            if (valueAccess == null)
            {
                throw new ArgumentNullException("valueAccess");
            }
            if (valueValidator == null)
            {
                throw new ArgumentNullException("valueValidator");
            }
            this.valueAccess = valueAccess;
            this.valueValidator = valueValidator;
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate != null)
            {
                object obj2;
                string str;
                if (this.valueAccess.GetValue(objectToValidate, out obj2, out str))
                {
                    this.valueValidator.DoValidate(obj2, objectToValidate, this.valueAccess.Key, validationResults);
                }
                else
                {
                    base.LogValidationResult(validationResults, str, currentTarget, this.valueAccess.Key);
                }
            }
            else
            {
                string messageTemplate = base.MessageTemplate;
                base.LogValidationResult(validationResults, messageTemplate, currentTarget, key);
            }
        }

        protected override string DefaultMessageTemplate
        {
            get
            {
                return Resources.ValueValidatorDefaultMessageTemplate;
            }
        }
    }
}

