namespace Hidistro.Core.Urls
{
    using System;

    public abstract class UrlReWriteProvider
    {
        private static readonly UrlReWriteProvider _instance = new HiUrlReWriter();

        protected UrlReWriteProvider()
        {
        }

        public static UrlReWriteProvider Instance()
        {
            if (_instance == null)
            {
                throw new Exception("UrlReWriteProvider could not be loaded");
            }
            return _instance;
        }

        public abstract string RewriteUrl(string path, string queryString);
    }
}

