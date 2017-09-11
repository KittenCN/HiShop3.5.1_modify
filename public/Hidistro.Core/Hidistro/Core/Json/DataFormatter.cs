namespace Hidistro.Core.Json
{
    using System;

    public class DataFormatter
    {
        public static string Format(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            if (obj is DateTime)
            {
                DateTime time = (DateTime) obj;
                return time.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return obj.ToString();
        }
    }
}

