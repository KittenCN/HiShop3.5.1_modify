namespace Hidistro.Core.Urls
{
    using System;
    using System.Text.RegularExpressions;

    public class ApplicationKeyMapping
    {
        private string _locationName;
        private Regex regex;

        public ApplicationKeyMapping(string locationName, string pattern)
        {
            this._locationName = locationName;
            this.regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool IsMatch(string url)
        {
            return this.regex.IsMatch(url);
        }

        public string LocationName
        {
            get
            {
                return this._locationName;
            }
        }
    }
}

