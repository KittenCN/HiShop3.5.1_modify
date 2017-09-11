namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public sealed class RegexValidatorAttribute : ValueValidatorAttribute
    {
        private RegexOptions options;
        private string pattern;
        private string patternResourceName;
        private Type patternResourceType;

        public RegexValidatorAttribute(string pattern) : this(pattern, RegexOptions.None)
        {
        }

        public RegexValidatorAttribute(string pattern, RegexOptions options) : this(pattern, null, null, options)
        {
        }

        public RegexValidatorAttribute(string patternResourceName, Type patternResourceType) : this(patternResourceName, patternResourceType, RegexOptions.None)
        {
        }

        public RegexValidatorAttribute(string patternResourceName, Type patternResourceType, RegexOptions options) : this(null, patternResourceName, patternResourceType, options)
        {
        }

        internal RegexValidatorAttribute(string pattern, string patternResourceName, Type patternResourceType, RegexOptions options)
        {
            ValidatorArgumentsValidatorHelper.ValidateRegexValidator(pattern, patternResourceName, patternResourceType);
            this.pattern = pattern;
            this.options = options;
            this.patternResourceName = patternResourceName;
            this.patternResourceType = patternResourceType;
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new RegexValidator(this.pattern, this.patternResourceName, this.patternResourceType, this.options, base.MessageTemplate, base.Negated);
        }
    }
}

