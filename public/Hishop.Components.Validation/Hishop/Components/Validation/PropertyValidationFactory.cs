namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal static class PropertyValidationFactory
    {
        private static IDictionary<PropertyValidatorCacheKey, Validator> attributeAndDefaultConfigurationPropertyValidatorsCache = new Dictionary<PropertyValidatorCacheKey, Validator>();
        private static object attributeAndDefaultConfigurationPropertyValidatorsCacheLock = new object();
        private static IDictionary<PropertyValidatorCacheKey, Validator> attributeOnlyPropertyValidatorsCache = new Dictionary<PropertyValidatorCacheKey, Validator>();
        private static object attributeOnlyPropertyValidatorsCacheLock = new object();
        private static IDictionary<PropertyValidatorCacheKey, Validator> defaultConfigurationOnlyPropertyValidatorsCache = new Dictionary<PropertyValidatorCacheKey, Validator>();
        private static object defaultConfigurationOnlyPropertyValidatorsCacheLock = new object();

        internal static Validator GetPropertyValidator(Type type, PropertyInfo propertyInfo, string ruleset, MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
        {
            Validator validator = null;
            lock (attributeAndDefaultConfigurationPropertyValidatorsCacheLock)
            {
                PropertyValidatorCacheKey key = new PropertyValidatorCacheKey(type, propertyInfo.Name, ruleset);
                if (!attributeAndDefaultConfigurationPropertyValidatorsCache.TryGetValue(key, out validator))
                {
                    validator = GetPropertyValidatorFromAttributes(type, propertyInfo, ruleset, memberAccessValidatorBuilderFactory);
                    attributeAndDefaultConfigurationPropertyValidatorsCache[key] = validator;
                }
            }
            return validator;
        }

        internal static Validator GetPropertyValidator(Type type, PropertyInfo propertyInfo, string ruleset, ValidationSpecificationSource validationSpecificationSource, MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
        {
            if (type == null)
            {
                throw new InvalidOperationException(Resources.ExceptionTypeNotFound);
            }
            if (propertyInfo == null)
            {
                throw new InvalidOperationException(Resources.ExceptionPropertyNotFound);
            }
            if (!propertyInfo.CanRead)
            {
                throw new InvalidOperationException(Resources.ExceptionPropertyNotReadable);
            }
            switch (validationSpecificationSource)
            {
                case ValidationSpecificationSource.Attributes:
                    return GetPropertyValidatorFromAttributes(type, propertyInfo, ruleset, memberAccessValidatorBuilderFactory);

                case ValidationSpecificationSource.Both:
                    return GetPropertyValidator(type, propertyInfo, ruleset, memberAccessValidatorBuilderFactory);
            }
            return null;
        }

        internal static Validator GetPropertyValidator(Type type, PropertyInfo propertyInfo, string ruleset, ValidationSpecificationSource validationSpecificationSource, MemberValueAccessBuilder memberValueAccessBuilder)
        {
            return GetPropertyValidator(type, propertyInfo, ruleset, validationSpecificationSource, new MemberAccessValidatorBuilderFactory(memberValueAccessBuilder));
        }

        internal static Validator GetPropertyValidatorFromAttributes(Type type, PropertyInfo propertyInfo, string ruleset, MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
        {
            Validator validator = null;
            lock (attributeOnlyPropertyValidatorsCacheLock)
            {
                PropertyValidatorCacheKey key = new PropertyValidatorCacheKey(type, propertyInfo.Name, ruleset);
                if (!attributeOnlyPropertyValidatorsCache.TryGetValue(key, out validator))
                {
                    validator = GetTypeValidatorBuilder(memberAccessValidatorBuilderFactory).CreateValidatorForProperty(propertyInfo, ruleset);
                    attributeOnlyPropertyValidatorsCache[key] = validator;
                }
            }
            return validator;
        }

        private static MetadataValidatorBuilder GetTypeValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
        {
            return new MetadataValidatorBuilder(memberAccessValidatorBuilderFactory);
        }

        internal static void ResetCaches()
        {
            lock (attributeOnlyPropertyValidatorsCacheLock)
            {
                attributeOnlyPropertyValidatorsCache.Clear();
            }
            lock (attributeAndDefaultConfigurationPropertyValidatorsCacheLock)
            {
                attributeAndDefaultConfigurationPropertyValidatorsCache.Clear();
            }
            lock (defaultConfigurationOnlyPropertyValidatorsCacheLock)
            {
                defaultConfigurationOnlyPropertyValidatorsCache.Clear();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PropertyValidatorCacheKey : IEquatable<PropertyValidationFactory.PropertyValidatorCacheKey>
        {
            private Type sourceType;
            private string propertyName;
            private string ruleset;
            public PropertyValidatorCacheKey(Type sourceType, string propertyName, string ruleset)
            {
                this.sourceType = sourceType;
                this.propertyName = propertyName;
                this.ruleset = ruleset;
            }

            public override int GetHashCode()
            {
                return ((this.sourceType.GetHashCode() ^ this.propertyName.GetHashCode()) ^ ((this.ruleset != null) ? this.ruleset.GetHashCode() : 0));
            }

            bool IEquatable<PropertyValidationFactory.PropertyValidatorCacheKey>.Equals(PropertyValidationFactory.PropertyValidatorCacheKey other)
            {
                if ((this.sourceType != other.sourceType) || !this.propertyName.Equals(other.propertyName))
                {
                    return false;
                }
                if (this.ruleset != null)
                {
                    return this.ruleset.Equals(other.ruleset);
                }
                return (other.ruleset == null);
            }
        }
    }
}

