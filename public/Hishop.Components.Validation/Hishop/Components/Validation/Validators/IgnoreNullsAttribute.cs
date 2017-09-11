namespace Hishop.Components.Validation.Validators
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public sealed class IgnoreNullsAttribute : BaseValidationAttribute
    {
    }
}

