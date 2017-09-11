namespace Hishop.Components.Validation
{
    using System;
    using System.Reflection;

    internal class MetadataValidatorBuilder : ValidatorBuilderBase
    {
        public MetadataValidatorBuilder()
        {
        }

        public MetadataValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory) : base(memberAccessValidatorFactory)
        {
        }

        public Validator CreateValidator(Type type, string ruleset)
        {
            return base.CreateValidator(new MetadataValidatedType(type, ruleset));
        }

        internal Validator CreateValidatorForField(FieldInfo fieldInfo, string ruleset)
        {
            return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(fieldInfo, ruleset), new ValidatorBuilderBase.CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForField));
        }

        internal Validator CreateValidatorForMethod(MethodInfo methodInfo, string ruleset)
        {
            return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(methodInfo, ruleset), new ValidatorBuilderBase.CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForMethod));
        }

        internal Validator CreateValidatorForProperty(PropertyInfo propertyInfo, string ruleset)
        {
            return base.CreateValidatorForValidatedElement(new MetadataValidatedElement(propertyInfo, ruleset), new ValidatorBuilderBase.CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForProperty));
        }

        internal Validator CreateValidatorForType(Type type, string ruleset)
        {
            return base.CreateValidatorForValidatedElement(new MetadataValidatedType(type, ruleset), new ValidatorBuilderBase.CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForType));
        }
    }
}

