namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public sealed class RelativeDateTimeValidatorAttribute : ValueValidatorAttribute
    {
        private int lowerBound;
        private RangeBoundaryType lowerBoundType;
        private DateTimeUnit lowerUnit;
        private int upperBound;
        private RangeBoundaryType upperBoundType;
        private DateTimeUnit upperUnit;

        public RelativeDateTimeValidatorAttribute(int upperBound, DateTimeUnit upperUnit) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive)
        {
        }

        public RelativeDateTimeValidatorAttribute(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType)
        {
        }

        public RelativeDateTimeValidatorAttribute(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit) : this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive)
        {
        }

        public RelativeDateTimeValidatorAttribute(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType)
        {
            ValidatorArgumentsValidatorHelper.ValidateRelativeDatimeValidator(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType);
            this.lowerBound = lowerBound;
            this.lowerUnit = lowerUnit;
            this.lowerBoundType = lowerBoundType;
            this.upperBound = upperBound;
            this.upperUnit = upperUnit;
            this.upperBoundType = upperBoundType;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new RelativeDateTimeValidator(this.lowerBound, this.lowerUnit, this.lowerBoundType, this.upperBound, this.upperUnit, this.upperBoundType, base.Negated);
        }
    }
}

