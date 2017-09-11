namespace Hishop.Components.Validation.Validators
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Properties;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class ContainsCharactersValidator : ValueValidator<string>
    {
        private string characterSet;
        private Hishop.Components.Validation.Validators.ContainsCharacters containsCharacters;

        public ContainsCharactersValidator(string characterSet) : this(characterSet, Hishop.Components.Validation.Validators.ContainsCharacters.Any)
        {
        }

        public ContainsCharactersValidator(string characterSet, Hishop.Components.Validation.Validators.ContainsCharacters containsCharacters) : this(characterSet, containsCharacters, false)
        {
        }

        public ContainsCharactersValidator(string characterSet, Hishop.Components.Validation.Validators.ContainsCharacters containsCharacters, bool negated) : this(characterSet, containsCharacters, null, negated)
        {
        }

        public ContainsCharactersValidator(string characterSet, Hishop.Components.Validation.Validators.ContainsCharacters containsCharacters, string messageTemplate) : this(characterSet, containsCharacters, messageTemplate, false)
        {
        }

        public ContainsCharactersValidator(string characterSet, Hishop.Components.Validation.Validators.ContainsCharacters containsCharacters, string messageTemplate, bool negated) : base(messageTemplate, null, negated)
        {
            ValidatorArgumentsValidatorHelper.ValidateContainsCharacterValidator(characterSet);
            this.characterSet = characterSet;
            this.containsCharacters = containsCharacters;
        }

        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            bool flag = false;
            bool flag2 = objectToValidate == null;
            if (!flag2)
            {
                if (this.ContainsCharacters != Hishop.Components.Validation.Validators.ContainsCharacters.Any)
                {
                    if (Hishop.Components.Validation.Validators.ContainsCharacters.All == this.ContainsCharacters)
                    {
                        List<char> list2 = new List<char>(objectToValidate);
                        bool flag4 = true;
                        foreach (char ch2 in this.CharacterSet)
                        {
                            if (!list2.Contains(ch2))
                            {
                                flag4 = false;
                                break;
                            }
                        }
                        flag = !flag4;
                    }
                }
                else
                {
                    List<char> list = new List<char>(this.characterSet);
                    bool flag3 = false;
                    foreach (char ch in objectToValidate)
                    {
                        if (list.Contains(ch))
                        {
                            flag3 = true;
                            break;
                        }
                    }
                    flag = !flag3;
                }
            }
            if (flag2 || (flag != base.Negated))
            {
                base.LogValidationResult(validationResults, this.GetMessage(objectToValidate, key), currentTarget, key);
            }
        }

        protected override string GetMessage(object objectToValidate, string key)
        {
            return string.Format(CultureInfo.CurrentCulture, base.MessageTemplate, new object[] { objectToValidate, key, base.Tag, this.CharacterSet, this.ContainsCharacters });
        }

        internal string CharacterSet
        {
            get
            {
                return this.characterSet;
            }
        }

        internal Hishop.Components.Validation.Validators.ContainsCharacters ContainsCharacters
        {
            get
            {
                return this.containsCharacters;
            }
        }

        protected override string DefaultNegatedMessageTemplate
        {
            get
            {
                return Resources.ContainsCharactersNegatedDefaultMessageTemplate;
            }
        }

        protected override string DefaultNonNegatedMessageTemplate
        {
            get
            {
                return Resources.ContainsCharactersNonNegatedDefaultMessageTemplate;
            }
        }
    }
}

