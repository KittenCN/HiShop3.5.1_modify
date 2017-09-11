namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public sealed class DateTimeRangeValidatorAttribute : ValueValidatorAttribute
    {
        private DateTime lowerBound;
        private RangeBoundaryType lowerBoundType;
        private DateTime upperBound;
        private RangeBoundaryType upperBoundType;

        public DateTimeRangeValidatorAttribute(DateTime upperBound) : this(new DateTime(), RangeBoundaryType.Ignore, upperBound, RangeBoundaryType.Inclusive)
        {
        }

        public DateTimeRangeValidatorAttribute(string upperBound) : this(ConvertToISO8601Date(upperBound))
        {
        }

        public DateTimeRangeValidatorAttribute(DateTime lowerBound, DateTime upperBound) : this(lowerBound, RangeBoundaryType.Inclusive, upperBound, RangeBoundaryType.Inclusive)
        {
        }

        public DateTimeRangeValidatorAttribute(string lowerBound, string upperBound) : this(ConvertToISO8601Date(lowerBound), ConvertToISO8601Date(upperBound))
        {
        }

        public DateTimeRangeValidatorAttribute(DateTime lowerBound, RangeBoundaryType lowerBoundType, DateTime upperBound, RangeBoundaryType upperBoundType)
        {
            this.lowerBound = lowerBound;
            this.lowerBoundType = lowerBoundType;
            this.upperBound = upperBound;
            this.upperBoundType = upperBoundType;
        }

        public DateTimeRangeValidatorAttribute(string lowerBound, RangeBoundaryType lowerBoundType, string upperBound, RangeBoundaryType upperBoundType) : this(ConvertToISO8601Date(lowerBound), lowerBoundType, ConvertToISO8601Date(upperBound), upperBoundType)
        {
        }

        private static DateTime ConvertToISO8601Date(string iso8601DateString)
        {
            DateTime time;
            if (string.IsNullOrEmpty(iso8601DateString))
            {
                return new DateTime();
            }
            try
            {
                time = DateTime.ParseExact(iso8601DateString, "s", CultureInfo.InvariantCulture);
            }
            catch (FormatException exception)
            {
                throw new ArgumentException(Resources.ExceptionInvalidDate, "dateString", exception);
            }
            return time;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new DateTimeRangeValidator(this.lowerBound, this.lowerBoundType, this.upperBound, this.upperBoundType, base.Negated);
        }
    }
}

