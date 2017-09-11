namespace Hishop.Components.Validation.Validators
{
    using System;

    public class RangeValidator : RangeValidator<IComparable>
    {
        public RangeValidator(IComparable upperBound) : this(null, RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
        {
        }

        public RangeValidator(IComparable lowerBound, IComparable upperBound) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
        {
        }

        public RangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, (string) null)
        {
        }

        public RangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType, bool negated) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, null, negated)
        {
        }

        public RangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType, string messageTemplate) : this(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, false)
        {
        }

        public RangeValidator(IComparable lowerBound, RangeBoundaryType lowerBoundType, IComparable upperBound, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : base(lowerBound, lowerBoundType, upperBound, upperBoundType, messageTemplate, negated)
        {
        }
    }
}

