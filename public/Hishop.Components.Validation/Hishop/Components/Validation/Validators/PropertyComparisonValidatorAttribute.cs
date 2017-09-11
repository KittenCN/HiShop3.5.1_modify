namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Globalization;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public sealed class PropertyComparisonValidatorAttribute : ValueValidatorAttribute
    {
        private ComparisonOperator comparisonOperator;
        private string propertyToCompare;

        public PropertyComparisonValidatorAttribute(string propertyToCompare, ComparisonOperator comparisonOperator)
        {
            if (propertyToCompare == null)
            {
                throw new ArgumentNullException("propertyToCompare");
            }
            this.propertyToCompare = propertyToCompare;
            this.comparisonOperator = comparisonOperator;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            throw new NotImplementedException(Resources.ExceptionShouldNotCall);
        }

        protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            PropertyInfo propertyInfo = ValidationReflectionHelper.GetProperty(ownerType, this.propertyToCompare, false);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionPropertyToCompareNotFound, new object[] { this.propertyToCompare, ownerType.FullName }));
            }
            return new PropertyComparisonValidator(memberValueAccessBuilder.GetPropertyValueAccess(propertyInfo), this.comparisonOperator, base.Negated);
        }
    }
}

