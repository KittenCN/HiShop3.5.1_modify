namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Properties;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;

    internal class CompositeValidatorBuilder
    {
        private Validator builtValidator;
        private IValidatedElement validatedElement;
        private List<Validator> valueValidators;

        public CompositeValidatorBuilder(IValidatedElement validatedElement)
        {
            this.validatedElement = validatedElement;
            this.valueValidators = new List<Validator>();
        }

        internal void AddValueValidator(Validator valueValidator)
        {
            this.valueValidators.Add(valueValidator);
        }

        protected virtual Validator DoGetValidator()
        {
            Validator validator;
            if (this.valueValidators.Count == 1)
            {
                validator = this.valueValidators[0];
            }
            else if (this.validatedElement.CompositionType == CompositionType.And)
            {
                validator = new AndCompositeValidator(this.valueValidators.ToArray());
            }
            else
            {
                validator = new OrCompositeValidator(this.valueValidators.ToArray()) {
                    MessageTemplate = this.validatedElement.CompositionMessageTemplate,
                    Tag = this.validatedElement.CompositionTag
                };
            }
            if (this.validatedElement.IgnoreNulls)
            {
                return new OrCompositeValidator(new Validator[] { new NotNullValidator(true), validator }) { MessageTemplate = (this.validatedElement.IgnoreNullsMessageTemplate != null) ? this.validatedElement.IgnoreNullsMessageTemplate : Resources.IgnoreNullsDefaultMessageTemplate, Tag = this.validatedElement.IgnoreNullsTag };
            }
            return validator;
        }

        public Validator GetValidator()
        {
            this.builtValidator = this.DoGetValidator();
            return this.builtValidator;
        }
    }
}

