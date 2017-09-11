namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Globalization;

    public abstract class Validator<T> : Validator
    {
        protected Validator(string messageTemplate, string tag) : base(messageTemplate, tag)
        {
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if ((objectToValidate != null) && !(objectToValidate is T))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidTargetType, new object[] { typeof(T).FullName, objectToValidate.GetType().FullName });
                base.LogValidationResult(validationResults, message, currentTarget, key);
            }
            else
            {
                this.DoValidate((T) objectToValidate, currentTarget, key, validationResults);
            }
        }

        protected abstract void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults);
        public ValidationResults Validate(T target)
        {
            ValidationResults validationResults = new ValidationResults();
            this.Validate(target, validationResults);
            return validationResults;
        }

        public void Validate(T target, ValidationResults validationResults)
        {
            if (validationResults == null)
            {
                throw new ArgumentNullException("validationResults");
            }
            this.DoValidate(target, target, null, validationResults);
        }
    }
}

