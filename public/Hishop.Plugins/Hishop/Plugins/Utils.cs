namespace Hishop.Plugins
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Web;

    public static class Utils
    {
        public static string GetResourceContent(string sFileName)
        {
            string str2;
            try
            {
                string str = "";
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sFileName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        str = reader.ReadToEnd();
                    }
                }
                str2 = str;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Concat(new object[] { "Could not read resource \"", sFileName, "\": ", exception }));
            }
            return str2;
        }

        public static string ApplicationPath
        {
            get
            {
                string applicationPath = "/";
                if (HttpContext.Current != null)
                {
                    applicationPath = HttpContext.Current.Request.ApplicationPath;
                }
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                return applicationPath;
            }
        }
    }
}

