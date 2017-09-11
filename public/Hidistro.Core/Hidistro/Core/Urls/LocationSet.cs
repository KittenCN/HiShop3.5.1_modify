namespace Hidistro.Core.Urls
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Reflection;
    using System.Text;

    public class LocationSet : IEnumerable
    {
        private ListDictionary locations = new ListDictionary();

        public void Add(string name, Location location)
        {
            if (name != null)
            {
                this.locations[name.ToLower(CultureInfo.InvariantCulture)] = location;
            }
        }

        public Location FindLocationByName(string name)
        {
            if (name != null)
            {
                name = name.ToLower(CultureInfo.InvariantCulture);
                foreach (string str in this.locations.Keys)
                {
                    if (str == name)
                    {
                        return (this.locations[name] as Location);
                    }
                }
            }
            return null;
        }

        public Location FindLocationByPath(string path)
        {
            foreach (Location location in this.locations.Values)
            {
                if (location.IsMatch(path))
                {
                    return location;
                }
            }
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return this.locations.GetEnumerator();
        }

        public string Filter
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (Location location in this.locations.Values)
                {
                    if ((location.Exclude && (location.Path != null)) && (location.Path.Length > 1))
                    {
                        builder.Append("|" + location.Path);
                    }
                }
                string str = builder.ToString();
                if ((str != null) && (str.Length > 0))
                {
                    str = str.Substring(1);
                }
                return str;
            }
        }

        public string this[string name]
        {
            get
            {
                if (name == null)
                {
                    return string.Empty;
                }
                return ((Location) this.locations[name.ToLower(CultureInfo.InvariantCulture)]).Path;
            }
        }
    }
}

