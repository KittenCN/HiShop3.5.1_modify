﻿namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Collections.Generic;

    public class DomainValidator<T> : ValueValidator<T>
    {
        private IEnumerable<T> domain;

        public DomainValidator(IEnumerable<T> domain) : this(domain, false)
        {
        }

        public DomainValidator(bool negated, params T[] domain) : this(new List<T>(domain), null, negated)
        {
        }

        public DomainValidator(IEnumerable<T> domain, bool negated) : this(domain, null, negated)
        {
        }

        public DomainValidator(string messageTemplate, params T[] domain) : this(new List<T>(domain), messageTemplate, false)
        {
        }

        public DomainValidator(IEnumerable<T> domain, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
        {
            ValidatorArgumentsValidatorHelper.ValidateDomainValidator(domain);
            this.domain = domain;
        }

        protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            bool flag = false;
            bool flag2 = objectToValidate == null;
            if (!flag2)
            {
                flag = true;
                foreach (T local in this.domain)
                {
                    if (local.Equals(objectToValidate))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag2 || (flag != base.Negated))
            {
                base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
            }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get
            {
                return Resources.DomainNegatedDefaultMessageTemplate;
            }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get
            {
                return Resources.DomainNonNegatedDefaultMessageTemplate;
            }
        }
    }
}

