namespace HiTemplate
{
    using System;
    using System.Web;

    public static class Urls
    {
        public static string ApplicationPath
        {
            get
            {
                string str = "http://";
                str = str + HttpContext.Current.Request.Url.Host;
                if (HttpContext.Current.Request.Url.Port != 80)
                {
                    str = str + ":" + HttpContext.Current.Request.Url.Port;
                }
                return str;
            }
        }
    }
}

