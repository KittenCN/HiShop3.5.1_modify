namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Collections;

    public class ObjectCollectionValidator : Validator
    {
        private string targetRuleset;
        private Type targetType;
        private Validator targetTypeValidator;

        public ObjectCollectionValidator(Type targetType) : this(targetType, string.Empty)
        {
        }

        public ObjectCollectionValidator(Type targetType, string targetRuleset) : base(null, null)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (targetRuleset == null)
            {
                throw new ArgumentNullException("targetRuleset");
            }
            this.targetType = targetType;
            this.targetRuleset = targetRuleset;
            this.targetTypeValidator = ValidationFactory.CreateValidator(this.targetType, this.targetRuleset);
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate != null)
            {
                IEnumerable enumerable = objectToValidate as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object obj2 in enumerable)
                    {
                        if (obj2 != null)
                        {
                            if (this.targetType.IsAssignableFrom(obj2.GetType()))
                            {
                                this.targetTypeValidator.DoValidate(obj2, obj2, null, validationResults);
                            }
                            else
                            {
                                base.LogValidationResult(validationResults, Resources.ObjectCollectionValidatorIncompatibleElementInTargetCollection, obj2, null);
                            }
                        }
                    }
                }
                else
                {
                    base.LogValidationResult(validationResults, Resources.ObjectCollectionValidatorTargetNotCollection, currentTarget, key);
                }
            }
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

