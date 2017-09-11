namespace Hishop.Components.Validation
{
    using System;
    using System.Collections.Generic;

    public static class Validation
    {
        public static ValidationResults Validate<T>(T target)
        {
            return ValidationFactory.CreateValidator<T>().Validate(target);
        }

        public static ValidationResults Validate<T>(T target, params string[] rulesets)
        {
            if (rulesets == null)
            {
                throw new ArgumentNullException("rulesets");
            }
            ValidationResults results = new ValidationResults();
            foreach (string str in rulesets)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) ValidationFactory.CreateValidator<T>(str).Validate(target))
                {
                    results.AddResult(result);
                }
            }
            return results;
        }

        public static ValidationResults ValidateFromAttributes<T>(T target)
        {
            return ValidationFactory.CreateValidatorFromAttributes<T>().Validate(target);
        }

        public static ValidationResults ValidateFromAttributes<T>(T target, params string[] rulesets)
        {
            if (rulesets == null)
            {
                throw new ArgumentNullException("rulesets");
            }
            ValidationResults results = new ValidationResults();
            foreach (string str in rulesets)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) ValidationFactory.CreateValidatorFromAttributes<T>(str).Validate(target))
                {
                    results.AddResult(result);
                }
            }
            return results;
        }
    }
}

