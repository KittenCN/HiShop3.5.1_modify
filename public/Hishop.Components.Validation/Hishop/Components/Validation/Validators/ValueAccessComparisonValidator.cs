namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Globalization;

    public class ValueAccessComparisonValidator : ValueValidator
    {
        private ComparisonOperator comparisonOperator;
        private ValueAccess valueAccess;

        public ValueAccessComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator) : this(valueAccess, comparisonOperator, null, (string) null)
        {
        }

        public ValueAccessComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, string messageTemplate, bool negated) : this(valueAccess, comparisonOperator, messageTemplate, null, negated)
        {
            this.valueAccess = valueAccess;
            this.comparisonOperator = comparisonOperator;
        }

        public ValueAccessComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, string messageTemplate, string tag) : this(valueAccess, comparisonOperator, messageTemplate, tag, false)
        {
            this.valueAccess = valueAccess;
            this.comparisonOperator = comparisonOperator;
        }

        public ValueAccessComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, string messageTemplate, string tag, bool negated) : base(messageTemplate, tag, negated)
        {
            if (valueAccess == null)
            {
                throw new ArgumentNullException("valueAccess");
            }
            this.valueAccess = valueAccess;
            this.comparisonOperator = comparisonOperator;
        }

        protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            object obj2;
            string str;
            if (!this.valueAccess.GetValue(currentTarget, out obj2, out str))
            {
                base.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentCulture, Resources.ValueAccessComparisonValidatorFailureToRetrieveComparand, new object[] { this.valueAccess.Key, str }), currentTarget, key);
            }
            else
            {
                bool flag2 = false;
                if ((this.comparisonOperator == ComparisonOperator.Equal) || (this.comparisonOperator == ComparisonOperator.NotEqual))
                {
                    flag2 = (((objectToValidate != null) ? objectToValidate.Equals(obj2) : (obj2 == null)) ^ (this.comparisonOperator == ComparisonOperator.NotEqual)) ^ base.Negated;
                }
                else
                {
                    IComparable comparable = objectToValidate as IComparable;
                    if (((comparable == null) || (obj2 == null)) || (comparable.GetType() != obj2.GetType()))
                    {
                        flag2 = false;
                    }
                    else
                    {
                        int num = comparable.CompareTo(obj2);
                        switch (this.comparisonOperator)
                        {
                            case ComparisonOperator.GreaterThan:
                                flag2 = num > 0;
                                break;

                            case ComparisonOperator.GreaterThanEqual:
                                flag2 = num >= 0;
                                break;

                            case ComparisonOperator.LessThan:
                                flag2 = num < 0;
                                break;

                            case ComparisonOperator.LessThanEqual:
                                flag2 = num <= 0;
                                break;
                        }
                        flag2 ^= base.Negated;
                    }
                }
                if (!flag2)
                {
                    base.LogValidationResult(validationResults, string.Format(CultureInfo.CurrentCulture, base.MessageTemplate, new object[] { objectToValidate, key, base.Tag, obj2, this.valueAccess.Key, this.comparisonOperator }), currentTarget, key);
                }
            }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get
            {
                return Resources.ValueAccessComparisonValidatorNegatedDefaultMessageTemplate;
            }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get
            {
                return Resources.ValueAccessComparisonValidatorNonNegatedDefaultMessageTemplate;
            }
        }
    }
}

