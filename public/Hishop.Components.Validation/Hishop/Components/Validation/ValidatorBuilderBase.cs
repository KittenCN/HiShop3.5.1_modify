namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal class ValidatorBuilderBase
    {
        private MemberAccessValidatorBuilderFactory memberAccessValidatorFactory;

        public ValidatorBuilderBase() : this(new MemberAccessValidatorBuilderFactory())
        {
        }

        public ValidatorBuilderBase(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory)
        {
            this.memberAccessValidatorFactory = memberAccessValidatorFactory;
        }

        private void CollectValidatorsForFields(IEnumerable<IValidatedElement> validatedElements, List<Validator> validators, Type ownerType)
        {
            foreach (IValidatedElement element in validatedElements)
            {
                Validator item = this.CreateValidatorForValidatedElement(element, new CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForField));
                if (item != null)
                {
                    validators.Add(item);
                }
            }
        }

        private void CollectValidatorsForMethods(IEnumerable<IValidatedElement> validatedElements, List<Validator> validators, Type ownerType)
        {
            foreach (IValidatedElement element in validatedElements)
            {
                Validator item = this.CreateValidatorForValidatedElement(element, new CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForMethod));
                if (item != null)
                {
                    validators.Add(item);
                }
            }
        }

        private void CollectValidatorsForProperties(IEnumerable<IValidatedElement> validatedElements, List<Validator> validators, Type ownerType)
        {
            foreach (IValidatedElement element in validatedElements)
            {
                Validator item = this.CreateValidatorForValidatedElement(element, new CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForProperty));
                if (item != null)
                {
                    validators.Add(item);
                }
            }
        }

        private void CollectValidatorsForSelfValidationMethods(IEnumerable<MethodInfo> selfValidationMethods, List<Validator> validators)
        {
            foreach (MethodInfo info in selfValidationMethods)
            {
                validators.Add(new SelfValidationValidator(info));
            }
        }

        private void CollectValidatorsForType(IValidatedType validatedType, List<Validator> validators)
        {
            Validator item = this.CreateValidatorForValidatedElement(validatedType, new CompositeValidatorBuilderCreator(this.GetCompositeValidatorBuilderForType));
            if (item != null)
            {
                validators.Add(item);
            }
        }

        public Validator CreateValidator(IValidatedType validatedType)
        {
            List<Validator> validators = new List<Validator>();
            this.CollectValidatorsForType(validatedType, validators);
            this.CollectValidatorsForProperties(validatedType.GetValidatedProperties(), validators, validatedType.TargetType);
            this.CollectValidatorsForFields(validatedType.GetValidatedFields(), validators, validatedType.TargetType);
            this.CollectValidatorsForMethods(validatedType.GetValidatedMethods(), validators, validatedType.TargetType);
            this.CollectValidatorsForSelfValidationMethods(validatedType.GetSelfValidationMethods(), validators);
            if (validators.Count == 1)
            {
                return validators[0];
            }
            return new AndCompositeValidator(validators.ToArray());
        }

        protected Validator CreateValidatorForValidatedElement(IValidatedElement validatedElement, CompositeValidatorBuilderCreator validatorBuilderCreator)
        {
            IEnumerator<IValidatorDescriptor> enumerator = validatedElement.GetValidatorDescriptors().GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return null;
            }
            CompositeValidatorBuilder builder = validatorBuilderCreator(validatedElement);
            do
            {
                Validator valueValidator = enumerator.Current.CreateValidator(validatedElement.TargetType, validatedElement.MemberInfo.ReflectedType, this.memberAccessValidatorFactory.MemberValueAccessBuilder);
                builder.AddValueValidator(valueValidator);
            }
            while (enumerator.MoveNext());
            return builder.GetValidator();
        }

        protected CompositeValidatorBuilder GetCompositeValidatorBuilderForField(IValidatedElement validatedElement)
        {
            return this.memberAccessValidatorFactory.GetFieldValueAccessValidatorBuilder(validatedElement.MemberInfo as FieldInfo, validatedElement);
        }

        protected CompositeValidatorBuilder GetCompositeValidatorBuilderForMethod(IValidatedElement validatedElement)
        {
            return this.memberAccessValidatorFactory.GetMethodValueAccessValidatorBuilder(validatedElement.MemberInfo as MethodInfo, validatedElement);
        }

        protected CompositeValidatorBuilder GetCompositeValidatorBuilderForProperty(IValidatedElement validatedElement)
        {
            return this.memberAccessValidatorFactory.GetPropertyValueAccessValidatorBuilder(validatedElement.MemberInfo as PropertyInfo, validatedElement);
        }

        protected CompositeValidatorBuilder GetCompositeValidatorBuilderForType(IValidatedElement validatedElement)
        {
            return this.memberAccessValidatorFactory.GetTypeValidatorBuilder(validatedElement.MemberInfo as Type, validatedElement);
        }

        protected CompositeValidatorBuilder GetValueCompositeValidatorBuilderForProperty(IValidatedElement validatedElement)
        {
            return new CompositeValidatorBuilder(validatedElement);
        }

        protected delegate CompositeValidatorBuilder CompositeValidatorBuilderCreator(IValidatedElement validatedElement);
    }
}

