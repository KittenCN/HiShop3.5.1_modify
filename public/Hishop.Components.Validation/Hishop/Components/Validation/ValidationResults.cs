namespace Hishop.Components.Validation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [Serializable]
    public class ValidationResults : IEnumerable<ValidationResult>, IEnumerable
    {
        private List<ValidationResult> validationResults = new List<ValidationResult>();

        public void AddAllResults(IEnumerable<ValidationResult> sourceValidationResults)
        {
            this.validationResults.AddRange(sourceValidationResults);
        }

        public void AddResult(ValidationResult validationResult)
        {
            this.validationResults.Add(validationResult);
        }

        public ValidationResults FindAll(TagFilter tagFilter, params string[] tags)
        {
            if (tags == null)
            {
                tags = new string[1];
            }
            ValidationResults results = new ValidationResults();
            foreach (ValidationResult result in (IEnumerable<ValidationResult>) this)
            {
                bool flag = false;
                foreach (string str in tags)
                {
                    if (((str == null) && (result.Tag == null)) || ((str != null) && str.Equals(result.Tag)))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag ^ (tagFilter == TagFilter.Ignore))
                {
                    results.AddResult(result);
                }
            }
            return results;
        }

        IEnumerator<ValidationResult> IEnumerable<ValidationResult>.GetEnumerator()
        {
            return this.validationResults.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.validationResults.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this.validationResults.Count;
            }
        }

        public bool IsValid
        {
            get
            {
                return (this.validationResults.Count == 0);
            }
        }
    }
}

