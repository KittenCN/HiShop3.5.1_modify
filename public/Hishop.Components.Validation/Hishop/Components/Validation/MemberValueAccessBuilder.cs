namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Reflection;

    public abstract class MemberValueAccessBuilder
    {
        protected MemberValueAccessBuilder()
        {
        }

        protected abstract ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo);
        protected abstract ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo);
        protected abstract ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo);
        public ValueAccess GetFieldValueAccess(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException("fieldInfo");
            }
            return this.DoGetFieldValueAccess(fieldInfo);
        }

        public ValueAccess GetMethodValueAccess(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            if (typeof(void) == methodInfo.ReturnType)
            {
                throw new ArgumentException(Resources.ExceptionMethodHasNoReturnValue, "methodInfo");
            }
            if (0 < methodInfo.GetParameters().Length)
            {
                throw new ArgumentException(Resources.ExceptionMethodHasParameters, "methodInfo");
            }
            return this.DoGetMethodValueAccess(methodInfo);
        }

        public ValueAccess GetPropertyValueAccess(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }
            return this.DoGetPropertyValueAccess(propertyInfo);
        }
    }
}

