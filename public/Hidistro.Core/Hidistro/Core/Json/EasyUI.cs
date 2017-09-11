namespace Hidistro.Core.Json
{
    using System;

    public class EasyUI
    {
        public static string Data(object data)
        {
            string str = "[]";
            if (data != null)
            {
                str = DefaultResolver.Convert(data);
            }
            return str;
        }

        public static string GridData(object data, long Total)
        {
            return GridData(data, Total, false);
        }

        public static string GridData(object data, long Total, bool ShowFooter)
        {
            string str = "{\"total\":0,\"rows\":[]}";
            if (data != null)
            {
                str = "{\"total\":" + Total.ToString() + ",\"rows\":" + DefaultResolver.Convert(data) + "}";
            }
            return str;
        }
    }
}

