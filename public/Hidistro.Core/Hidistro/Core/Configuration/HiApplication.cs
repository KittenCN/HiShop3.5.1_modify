namespace Hidistro.Core.Configuration
{
    using Hidistro.Core.Enums;
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal class HiApplication
    {
        private Hidistro.Core.Enums.ApplicationType _appType = Hidistro.Core.Enums.ApplicationType.Common;
        private string _name;
        private Regex _regex;

        internal HiApplication(string pattern, string name, Hidistro.Core.Enums.ApplicationType appType)
        {
            this._name = name.ToLower(CultureInfo.InvariantCulture);
            this._appType = appType;
            this._regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool IsMatch(string url)
        {
            return this._regex.IsMatch(url);
        }

        public Hidistro.Core.Enums.ApplicationType ApplicationType
        {
            get
            {
                return this._appType;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

