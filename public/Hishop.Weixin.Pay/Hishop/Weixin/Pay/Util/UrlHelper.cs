namespace Hishop.Weixin.Pay.Util
{
    using System;
    using System.Web;

    internal class UrlHelper
    {
        public static int GetIntUrlParam(string key)
        {
            return GetIntUrlParam(key, 0);
        }

        public static int GetIntUrlParam(string key, int defaultValue)
        {
            string str = HttpContext.Current.Request.QueryString[key];
            if (str == null)
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public static string GetStringUrlParam(string key)
        {
            return GetStringUrlParam(key, string.Empty);
        }

        public static string GetStringUrlParam(string key, string defaultValue)
        {
            return (HttpContext.Current.Request.QueryString[key] ?? defaultValue);
        }
    }
}

