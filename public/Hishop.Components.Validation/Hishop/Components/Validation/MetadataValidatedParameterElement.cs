namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class MetadataValidatedParameterElement : IValidatedElement
    {
        private IgnoreNullsAttribute ignoreNullsAttribute;
        private ParameterInfo parameterInfo;
        private ValidatorCompositionAttribute validatorCompositionAttribute;

        public IEnumerable<IValidatorDescriptor> GetValidatorDescriptors()
        {
            if (this.parameterInfo != null)
            {
                foreach (object iteratorVariable0 in this.parameterInfo.GetCustomAttributes(typeof(ValidatorAttribute), false))
                {
                    yield return (IValidatorDescriptor) iteratorVariable0;
                }
            }
        }

        public void UpdateFlyweight(ParameterInfo parameterInfo)
        {
            this.parameterInfo = parameterInfo;
            this.ignoreNullsAttribute = ValidationReflectionHelper.ExtractValidationAttribute<IgnoreNullsAttribute>(parameterInfo, string.Empty);
            this.validatorCompositionAttribute = ValidationReflectionHelper.ExtractValidationAttribute<ValidatorCompositionAttribute>(parameterInfo, string.Empty);
        }

        public string CompositionMessageTemplate
        {
            get
            {
                if (this.validatorCompositionAttribute == null)
                {
                    return null;
                }
                return this.validatorCompositionAttribute.GetMessageTemplate();
            }
        }

        public string CompositionTag
        {
            get
            {
                if (this.validatorCompositionAttribute == null)
                {
                    return null;
                }
                return this.validatorCompositionAttribute.Tag;
            }
        }

        public Hishop.Components.Validation.CompositionType CompositionType
        {
            get
            {
                if (this.validatorCompositionAttribute == null)
                {
                    return Hishop.Components.Validation.CompositionType.And;
                }
                return this.validatorCompositionAttribute.CompositionType;
            }
        }

        public bool IgnoreNulls
        {
            get
            {
                return (this.ignoreNullsAttribute != null);
            }
        }

        public string IgnoreNullsMessageTemplate
        {
            get
            {
                if (this.ignoreNullsAttribute == null)
                {
                    return null;
                }
                return this.ignoreNullsAttribute.GetMessageTemplate();
            }
        }

        public string IgnoreNullsTag
        {
            get
            {
                if (this.ignoreNullsAttribute == null)
                {
                    return null;
                }
                return this.ignoreNullsAttribute.Tag;
            }
        }

        public System.Reflection.MemberInfo MemberInfo
        {
            get
            {
                return null;
            }
        }

        public Type TargetType
        {
            get
            {
                return null;
            }
        }

    }
}

