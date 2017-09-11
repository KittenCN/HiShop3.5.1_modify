namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Validators;
    using System.Reflection;

    internal class ReflectionMemberValueAccessBuilder : MemberValueAccessBuilder
    {
        protected override ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo)
        {
            return new FieldValueAccess(fieldInfo);
        }

        protected override ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo)
        {
            return new MethodValueAccess(methodInfo);
        }

        protected override ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo)
        {
            return new PropertyValueAccess(propertyInfo);
        }
    }
}

