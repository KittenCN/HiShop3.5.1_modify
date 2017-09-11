﻿namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    public class MethodReturnValueValidator<T> : MemberAccessValidator<T>
    {
        public MethodReturnValueValidator(string methodName, Validator methodValueValidator) : base(MethodReturnValueValidator<T>.GetMethodValueAccess(methodName), methodValueValidator)
        {
        }

        private static ValueAccess GetMethodValueAccess(string methodName)
        {
            return new MethodValueAccess(ValidationReflectionHelper.GetMethod(typeof(T), methodName, true));
        }
    }
}

