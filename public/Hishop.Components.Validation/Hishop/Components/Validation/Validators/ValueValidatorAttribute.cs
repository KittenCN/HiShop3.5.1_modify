namespace Hishop.Components.Validation.Validators
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public abstract class ValueValidatorAttribute : ValidatorAttribute
    {
        private bool negated;

        protected ValueValidatorAttribute()
        {
        }

        public bool Negated
        {
            get
            {
                return this.negated;
            }
            set
            {
                this.negated = value;
            }
        }
    }
}

