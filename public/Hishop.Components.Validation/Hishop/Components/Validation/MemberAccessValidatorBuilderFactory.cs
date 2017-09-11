namespace Hishop.Components.Validation
{
    using System;
    using System.Reflection;

    internal class MemberAccessValidatorBuilderFactory
    {
        private Hishop.Components.Validation.MemberValueAccessBuilder valueAccessBuilder;

        public MemberAccessValidatorBuilderFactory() : this(new ReflectionMemberValueAccessBuilder())
        {
        }

        public MemberAccessValidatorBuilderFactory(Hishop.Components.Validation.MemberValueAccessBuilder valueAccessBuilder)
        {
            this.valueAccessBuilder = valueAccessBuilder;
        }

        public virtual ValueAccessValidatorBuilder GetFieldValueAccessValidatorBuilder(FieldInfo fieldInfo, IValidatedElement validatedElement)
        {
            return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetFieldValueAccess(fieldInfo), validatedElement);
        }

        public virtual ValueAccessValidatorBuilder GetMethodValueAccessValidatorBuilder(MethodInfo methodInfo, IValidatedElement validatedElement)
        {
            return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetMethodValueAccess(methodInfo), validatedElement);
        }

        public virtual ValueAccessValidatorBuilder GetPropertyValueAccessValidatorBuilder(PropertyInfo propertyInfo, IValidatedElement validatedElement)
        {
            return new ValueAccessValidatorBuilder(this.valueAccessBuilder.GetPropertyValueAccess(propertyInfo), validatedElement);
        }

        public virtual CompositeValidatorBuilder GetTypeValidatorBuilder(Type type, IValidatedElement validatedElement)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return new CompositeValidatorBuilder(validatedElement);
        }

        public Hishop.Components.Validation.MemberValueAccessBuilder MemberValueAccessBuilder
        {
            get
            {
                return this.valueAccessBuilder;
            }
        }
    }
}

