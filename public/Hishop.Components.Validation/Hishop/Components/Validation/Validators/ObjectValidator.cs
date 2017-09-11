﻿namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;

    public class ObjectValidator : Validator
    {
        private string targetRuleset;
        private Type targetType;
        private Validator targetTypeValidator;

        public ObjectValidator(Type targetType) : this(targetType, string.Empty)
        {
        }

        public ObjectValidator(Type targetType, string targetRuleset) : base(null, null)
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
                if (this.targetType.IsAssignableFrom(objectToValidate.GetType()))
                {
                    this.targetTypeValidator.DoValidate(objectToValidate, objectToValidate, null, validationResults);
                }
                else
                {
                    base.LogValidationResult(validationResults, Resources.ObjectValidatorInvalidTargetType, currentTarget, key);
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

