namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Globalization;
    using System.Reflection;
    using Validators;
    internal static class ValidationReflectionHelper
    {
        public static T ExtractValidationAttribute<T>(ICustomAttributeProvider attributeProvider, string ruleset) where T: BaseValidationAttribute
        {
            if (attributeProvider != null)
            {
                foreach (T local in attributeProvider.GetCustomAttributes(typeof(T), false))
                {
                    if (ruleset.Equals(local.Ruleset))
                    {
                        return local;
                    }
                }
            }
            return default(T);
        }

        public static FieldInfo GetField(Type type, string fieldName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }
            FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (IsValidField(field))
            {
                return field;
            }
            if (throwIfInvalid)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidField, new object[] { fieldName, type.FullName }));
            }
            return null;
        }

        public static MethodInfo GetMethod(Type type, string methodName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            if (IsValidMethod(methodInfo))
            {
                return methodInfo;
            }
            if (throwIfInvalid)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidMethod, new object[] { methodName, type.FullName }));
            }
            return null;
        }

        public static PropertyInfo GetProperty(Type type, string propertyName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (IsValidProperty(property))
            {
                return property;
            }
            if (throwIfInvalid)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionInvalidProperty, new object[] { propertyName, type.FullName }));
            }
            return null;
        }

        public static bool IsValidField(FieldInfo fieldInfo)
        {
            return (null != fieldInfo);
        }

        public static bool IsValidMethod(MethodInfo methodInfo)
        {
            return (((methodInfo != null) && (typeof(void) != methodInfo.ReturnType)) && (methodInfo.GetParameters().Length == 0));
        }

        public static bool IsValidProperty(PropertyInfo propertyInfo)
        {
            return (((propertyInfo != null) && propertyInfo.CanRead) && (propertyInfo.GetIndexParameters().Length == 0));
        }
    }
}

