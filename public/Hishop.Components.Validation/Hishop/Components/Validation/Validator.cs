namespace Hishop.Components.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public abstract class Validator
    {
        private string messageTemplate;
        private string tag;

        protected Validator(string messageTemplate, string tag)
        {
            this.messageTemplate = messageTemplate;
            this.tag = tag;
        }

        protected internal abstract void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults);
        protected virtual string GetMessage(object objectToValidate, string key)
        {
            return string.Format(CultureInfo.CurrentCulture, this.MessageTemplate, new object[] { objectToValidate, key, this.Tag });
        }

        protected void LogValidationResult(ValidationResults validationResults, string message, object target, string key)
        {
            validationResults.AddResult(new ValidationResult(message, target, key, this.Tag, this));
        }

        protected void LogValidationResult(ValidationResults validationResults, string message, object target, string key, IEnumerable<ValidationResult> nestedValidationResults)
        {
            validationResults.AddResult(new ValidationResult(message, target, key, this.Tag, this, nestedValidationResults));
        }

        public ValidationResults Validate(object target)
        {
            ValidationResults validationResults = new ValidationResults();
            this.DoValidate(target, target, null, validationResults);
            return validationResults;
        }

        public void Validate(object target, ValidationResults validationResults)
        {
            if (validationResults == null)
            {
                throw new ArgumentNullException("validationResults");
            }
            this.DoValidate(target, target, null, validationResults);
        }

        protected abstract string DefaultMessageTemplate { get; }

        public string MessageTemplate
        {
            get
            {
                if (this.messageTemplate == null)
                {
                    return this.DefaultMessageTemplate;
                }
                return this.messageTemplate;
            }
            set
            {
                this.messageTemplate = value;
            }
        }

        public string Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }
    }
}

