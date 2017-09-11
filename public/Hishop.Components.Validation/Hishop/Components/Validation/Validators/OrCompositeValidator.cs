namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Collections.Generic;

    public class OrCompositeValidator : Validator
    {
        private IEnumerable<Validator> validators;

        public OrCompositeValidator(params Validator[] validators) : this(null, validators)
        {
        }

        public OrCompositeValidator(string messageTemplate, params Validator[] validators) : base(messageTemplate, null)
        {
            this.validators = validators;
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            List<ValidationResult> nestedValidationResults = new List<ValidationResult>();
            foreach (Validator validator in this.validators)
            {
                ValidationResults results = new ValidationResults();
                validator.DoValidate(objectToValidate, currentTarget, key, results);
                if (results.IsValid)
                {
                    return;
                }
                nestedValidationResults.AddRange(results);
            }
            base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key, nestedValidationResults);
        }

        protected override string DefaultMessageTemplate
        {
            get
            {
                return Resources.OrCompositeValidatorDefaultMessageTemplate;
            }
        }
    }
}

