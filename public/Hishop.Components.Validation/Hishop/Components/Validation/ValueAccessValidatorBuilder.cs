namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Validators;
    using System;

    internal class ValueAccessValidatorBuilder : CompositeValidatorBuilder
    {
        private ValueAccess valueAccess;

        public ValueAccessValidatorBuilder(ValueAccess valueAccess, IValidatedElement validatedElement) : base(validatedElement)
        {
            this.valueAccess = valueAccess;
        }

        protected override Validator DoGetValidator()
        {
            return new ValueAccessValidator(this.valueAccess, base.DoGetValidator());
        }
    }
}

