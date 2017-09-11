namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Globalization;

    public class RelativeDateTimeValidator : ValueValidator<DateTime>
    {
        private RelativeDateTimeGenerator generator;
        private int lowerBound;
        private DateTimeUnit lowerUnit;
        private RangeChecker<DateTime> rangeChecker;
        private int upperBound;
        private DateTimeUnit upperUnit;

        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, false)
        {
        }

        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, false)
        {
        }

        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, bool negated) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, negated)
        {
        }

        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, string messageTemplate) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, messageTemplate)
        {
        }

        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, bool negated) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, negated)
        {
        }

        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, messageTemplate)
        {
        }

        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit) : this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive, false)
        {
        }

        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, string messageTemplate, bool negated) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, RangeBoundaryType.Inclusive, messageTemplate, negated)
        {
        }

        public RelativeDateTimeValidator(int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : this(0, DateTimeUnit.None, RangeBoundaryType.Ignore, upperBound, upperUnit, upperBoundType, messageTemplate, negated)
        {
        }

        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, int upperBound, DateTimeUnit upperUnit, bool negated) : this(lowerBound, lowerUnit, RangeBoundaryType.Inclusive, upperBound, upperUnit, RangeBoundaryType.Inclusive, negated)
        {
        }

        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType) : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, false)
        {
        }

        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, bool negated) : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, null, negated)
        {
        }

        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate) : this(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType, messageTemplate, false)
        {
        }

        public RelativeDateTimeValidator(int lowerBound, DateTimeUnit lowerUnit, RangeBoundaryType lowerBoundType, int upperBound, DateTimeUnit upperUnit, RangeBoundaryType upperBoundType, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
        {
            ValidatorArgumentsValidatorHelper.ValidateRelativeDatimeValidator(lowerBound, lowerUnit, lowerBoundType, upperBound, upperUnit, upperBoundType);
            this.lowerBound = lowerBound;
            this.lowerUnit = lowerUnit;
            this.upperBound = upperBound;
            this.upperUnit = upperUnit;
            this.generator = new RelativeDateTimeGenerator();
            DateTime now = DateTime.Now;
            DateTime time2 = this.generator.GenerateBoundDateTime(lowerBound, lowerUnit, now);
            DateTime time3 = this.generator.GenerateBoundDateTime(upperBound, upperUnit, now);
            this.rangeChecker = new RangeChecker<DateTime>(time2, lowerBoundType, time3, upperBoundType);
        }

        protected override void DoValidate(DateTime objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (this.rangeChecker.IsInRange(objectToValidate) == base.Negated)
            {
                base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
            }
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate == null)
            {
                base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
            }
            else
            {
                base.DoValidate(objectToValidate, currentTarget, key, validationResults);
            }
        }

        protected override string GetMessage(object objectToValidate, string key)
        {
            return string.Format(CultureInfo.CurrentCulture, base.MessageTemplate, new object[] { objectToValidate, key, base.Tag, this.lowerBound, this.LowerUnit, this.upperBound, this.UpperUnit });
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get
            {
                return Resources.RelativeDateTimeNegatedDefaultMessageTemplate;
            }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get
            {
                return Resources.RelativeDateTimeNonNegatedDefaultMessageTemplate;
            }
        }

        internal RelativeDateTimeGenerator Generator
        {
            get
            {
                return this.generator;
            }
        }

        internal int LowerBound
        {
            get
            {
                return this.lowerBound;
            }
        }

        internal RangeBoundaryType LowerBoundType
        {
            get
            {
                return this.rangeChecker.LowerBoundType;
            }
        }

        internal DateTimeUnit LowerUnit
        {
            get
            {
                return this.lowerUnit;
            }
        }

        internal int UpperBound
        {
            get
            {
                return this.upperBound;
            }
        }

        internal RangeBoundaryType UpperBoundType
        {
            get
            {
                return this.rangeChecker.UpperBoundType;
            }
        }

        internal DateTimeUnit UpperUnit
        {
            get
            {
                return this.upperUnit;
            }
        }
    }
}

