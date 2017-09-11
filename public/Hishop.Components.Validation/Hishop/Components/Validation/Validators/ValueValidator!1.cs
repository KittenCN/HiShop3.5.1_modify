namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using System;

    public abstract class ValueValidator<T> : Validator<T>
    {
        private bool negated;

        protected ValueValidator(string messageTemplate, string tag, bool negated) : base(messageTemplate, tag)
        {
            this.negated = negated;
        }

        protected sealed override string DefaultMessageTemplate
        {
            get
            {
                if (this.negated)
                {
                    return this.DefaultNegatedMessageTemplate;
                }
                return this.DefaultNonNegatedMessageTemplate;
            }
        }

        protected abstract string DefaultNegatedMessageTemplate { get; }

        protected abstract string DefaultNonNegatedMessageTemplate { get; }

        public bool Negated
        {
            get
            {
                return this.negated;
            }
        }
    }
}

