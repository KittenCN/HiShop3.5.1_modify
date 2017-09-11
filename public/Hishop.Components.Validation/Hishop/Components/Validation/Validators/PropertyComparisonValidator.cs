namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    public class PropertyComparisonValidator : ValueAccessComparisonValidator
    {
        public PropertyComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator) : base(valueAccess, comparisonOperator)
        {
        }

        public PropertyComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, bool negated) : base(valueAccess, comparisonOperator, null, negated)
        {
        }
    }
}

