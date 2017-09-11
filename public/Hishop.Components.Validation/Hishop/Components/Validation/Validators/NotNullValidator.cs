namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;

    public class NotNullValidator : ValueValidator
    {
        public NotNullValidator() : this(false)
        {
        }

        public NotNullValidator(bool negated) : this(negated, null)
        {
        }

        public NotNullValidator(string messageTemplate) : this(false, messageTemplate)
        {
        }

        public NotNullValidator(bool negated, string messageTemplate) : base(messageTemplate, null, negated)
        {
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if ((null == objectToValidate) == !base.Negated)
            {
                base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
            }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get
            {
                return Resources.NonNullNegatedValidatorDefaultMessageTemplate;
            }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get
            {
                return Resources.NonNullNonNegatedValidatorDefaultMessageTemplate;
            }
        }
    }
}

