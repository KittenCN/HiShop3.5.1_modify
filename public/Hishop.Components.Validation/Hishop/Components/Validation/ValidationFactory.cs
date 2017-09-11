namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class ValidationFactory
    {
        private static IDictionary<ValidatorCacheKey, Validator> attributeAndDefaultConfigurationValidatorsCache = new Dictionary<ValidatorCacheKey, Validator>();
        private static object attributeAndDefaultConfigurationValidatorsCacheLock = new object();
        private static IDictionary<ValidatorCacheKey, Validator> attributeOnlyValidatorsCache = new Dictionary<ValidatorCacheKey, Validator>();
        private static object attributeOnlyValidatorsCacheLock = new object();
        private static IDictionary<ValidatorCacheKey, Validator> defaultConfigurationOnlyValidatorsCache = new Dictionary<ValidatorCacheKey, Validator>();
        private static object defaultConfigurationOnlyValidatorsCacheLock = new object();

        public static Validator<T> CreateValidator<T>()
        {
            return CreateValidator<T>(string.Empty, true);
        }

        public static Validator<T> CreateValidator<T>(string ruleset)
        {
            return CreateValidator<T>(ruleset, true);
        }

        public static Validator CreateValidator(Type targetType)
        {
            return CreateValidator(targetType, string.Empty);
        }

        private static Validator<T> CreateValidator<T>(string ruleset, bool cacheValidator)
        {
            Validator<T> validator = null;
            if (cacheValidator)
            {
                lock (attributeAndDefaultConfigurationValidatorsCacheLock)
                {
                    Validator validator2;
                    ValidatorCacheKey key = new ValidatorCacheKey(typeof(T), ruleset, true);
                    if (attributeAndDefaultConfigurationValidatorsCache.TryGetValue(key, out validator2))
                    {
                        return (Validator<T>) validator2;
                    }
                    validator = WrapAndInstrumentValidator<T>(InnerCreateValidatorFromAttributes(typeof(T), ruleset));
                    attributeAndDefaultConfigurationValidatorsCache[key] = validator;
                    return validator;
                }
            }
            return WrapAndInstrumentValidator<T>(InnerCreateValidatorFromAttributes(typeof(T), ruleset));
        }

        public static Validator CreateValidator(Type targetType, string ruleset)
        {
            return CreateValidator(targetType, ruleset, true);
        }

        private static Validator CreateValidator(Type targetType, string ruleset, bool cacheValidator)
        {
            Validator validator = null;
            if (cacheValidator)
            {
                lock (attributeAndDefaultConfigurationValidatorsCacheLock)
                {
                    Validator validator2;
                    ValidatorCacheKey key = new ValidatorCacheKey(targetType, ruleset, false);
                    if (attributeAndDefaultConfigurationValidatorsCache.TryGetValue(key, out validator2))
                    {
                        return validator2;
                    }
                    validator = WrapAndInstrumentValidator(InnerCreateValidatorFromAttributes(targetType, ruleset));
                    attributeAndDefaultConfigurationValidatorsCache[key] = validator;
                    return validator;
                }
            }
            return WrapAndInstrumentValidator(InnerCreateValidatorFromAttributes(targetType, ruleset));
        }

        public static Validator<T> CreateValidatorFromAttributes<T>()
        {
            return CreateValidatorFromAttributes<T>(string.Empty);
        }

        public static Validator<T> CreateValidatorFromAttributes<T>(string ruleset)
        {
            if (ruleset == null)
            {
                throw new ArgumentNullException("ruleset");
            }
            Validator<T> validator = null;
            lock (attributeOnlyValidatorsCacheLock)
            {
                Validator validator2;
                ValidatorCacheKey key = new ValidatorCacheKey(typeof(T), ruleset, true);
                if (attributeOnlyValidatorsCache.TryGetValue(key, out validator2))
                {
                    return (Validator<T>) validator2;
                }
                validator = WrapAndInstrumentValidator<T>(InnerCreateValidatorFromAttributes(typeof(T), ruleset));
                attributeOnlyValidatorsCache[key] = validator;
            }
            return validator;
        }

        public static Validator CreateValidatorFromAttributes(Type targetType, string ruleset)
        {
            if (ruleset == null)
            {
                throw new ArgumentNullException("ruleset");
            }
            Validator validator = null;
            lock (attributeOnlyValidatorsCacheLock)
            {
                Validator validator2;
                ValidatorCacheKey key = new ValidatorCacheKey(targetType, ruleset, false);
                if (attributeOnlyValidatorsCache.TryGetValue(key, out validator2))
                {
                    return validator2;
                }
                validator = WrapAndInstrumentValidator(InnerCreateValidatorFromAttributes(targetType, ruleset));
                attributeOnlyValidatorsCache[key] = validator;
            }
            return validator;
        }

        private static Validator InnerCreateValidatorFromAttributes(Type targetType, string ruleset)
        {
            MetadataValidatorBuilder builder = new MetadataValidatorBuilder();
            return builder.CreateValidator(targetType, ruleset);
        }

        public static void ResetCaches()
        {
            lock (attributeOnlyValidatorsCacheLock)
            {
                attributeOnlyValidatorsCache.Clear();
            }
            lock (attributeAndDefaultConfigurationValidatorsCacheLock)
            {
                attributeAndDefaultConfigurationValidatorsCache.Clear();
            }
            lock (defaultConfigurationOnlyValidatorsCacheLock)
            {
                defaultConfigurationOnlyValidatorsCache.Clear();
            }
        }

        private static Validator WrapAndInstrumentValidator(Validator validator)
        {
            return new ValidatorWrapper(validator);
        }

        private static Validator<T> WrapAndInstrumentValidator<T>(Validator validator)
        {
            return new GenericValidatorWrapper<T>(validator);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ValidatorCacheKey : IEquatable<ValidationFactory.ValidatorCacheKey>
        {
            private Type sourceType;
            private string ruleset;
            private bool generic;
            public ValidatorCacheKey(Type sourceType, string ruleset, bool generic)
            {
                this.sourceType = sourceType;
                this.ruleset = ruleset;
                this.generic = generic;
            }

            public override int GetHashCode()
            {
                return (this.sourceType.GetHashCode() ^ ((this.ruleset != null) ? this.ruleset.GetHashCode() : 0));
            }

            bool IEquatable<ValidationFactory.ValidatorCacheKey>.Equals(ValidationFactory.ValidatorCacheKey other)
            {
                return (((this.sourceType == other.sourceType) && ((this.ruleset == null) ? (other.ruleset == null) : this.ruleset.Equals(other.ruleset))) && (this.generic == other.generic));
            }
        }
    }
}

