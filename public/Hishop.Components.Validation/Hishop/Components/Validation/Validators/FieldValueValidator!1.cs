namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    public class FieldValueValidator<T> : MemberAccessValidator<T>
    {
        public FieldValueValidator(string fieldName, Validator fieldValueValidator) : base(FieldValueValidator<T>.GetFieldValueAccess(fieldName), fieldValueValidator)
        {
        }

        private static ValueAccess GetFieldValueAccess(string fieldName)
        {
            return new FieldValueAccess(ValidationReflectionHelper.GetField(typeof(T), fieldName, true));
        }
    }
}

