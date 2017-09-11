namespace Hishop.MeiQia.Api.Util
{
    using Newtonsoft.Json.Linq;
    using System;

    public class Common
    {
        public static string GetJsonValue(string msg, string Field)
        {
            string str = "";
            JObject obj2 = JObject.Parse(msg);
            if (obj2[Field] != null)
            {
                str = obj2[Field].ToString();
            }
            return str;
        }
    }
}

