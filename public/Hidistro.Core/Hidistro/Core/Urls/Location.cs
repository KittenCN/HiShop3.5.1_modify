namespace Hidistro.Core.Urls
{
    using System;
    using System.Collections;
    using System.Text.RegularExpressions;

    public class Location : IEnumerable
    {
        private bool _exclude;
        private string _path;
        private string _physicalPath;
        private Regex _regex;
        private ArrayList _urls = new ArrayList();

        public Location(string path, string physicalPath, bool exclude)
        {
            this._path = path;
            if (physicalPath == null)
            {
                this._physicalPath = path;
            }
            else
            {
                this._physicalPath = physicalPath;
            }
            this._exclude = exclude;
            if (!string.IsNullOrEmpty(path))
            {
                this._regex = new Regex(path, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
        }

        public void Add(ReWrittenUrl url)
        {
            this._urls.Add(url);
        }

        public IEnumerator GetEnumerator()
        {
            return this._urls.GetEnumerator();
        }

        public bool IsMatch(string url)
        {
            if (this._regex == null)
            {
                return false;
            }
            return this._regex.IsMatch(url);
        }

        public virtual string ReWriteUrl(string path, string queryString)
        {
            if (this.Count > 0)
            {
                foreach (ReWrittenUrl url in this)
                {
                    if (url.IsMatch(path))
                    {
                        return url.Convert(path, queryString);
                    }
                }
            }
            return null;
        }

        public int Count
        {
            get
            {
                return this._urls.Count;
            }
        }

        public bool Exclude
        {
            get
            {
                return this._exclude;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
        }

        public string PhysicalPath
        {
            get
            {
                return this._physicalPath;
            }
        }
    }
}

