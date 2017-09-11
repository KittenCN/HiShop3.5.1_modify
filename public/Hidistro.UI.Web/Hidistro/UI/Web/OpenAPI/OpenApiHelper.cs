namespace Hidistro.UI.Web.OpenAPI
{
    using global::Hishop.Open.Api;
    using Hidistro.Core;
    using Hishop.Open.Api;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Web;

    public class OpenApiHelper
    {
        public static bool CheckSystemParameters(SortedDictionary<string, string> parameters, string app_key, out string result)
        {
            result = string.Empty;
            if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["app_key"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_App_Key, "app_key");
                return false;
            }
            if (app_key != parameters["app_key"])
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_App_Key, "app_key");
                return false;
            }
            if (!parameters.Keys.Contains<string>("timestamp") || string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["timestamp"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Timestamp, "timestamp");
                return false;
            }
            if (!IsDate(parameters["timestamp"]) || !OpenApiSign.CheckTimeStamp(parameters["timestamp"]))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Invalid_Timestamp, "timestamp");
                return false;
            }
            if (string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["sign"])))
            {
                result = OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Signature, "sign");
                return false;
            }
            return true;
        }

        public static SortedDictionary<string, string> GetSortedParams(HttpContext context)
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            NameValueCollection values2 = new NameValueCollection();
            values2.Add(context.Request.Form);
            values2.Add(context.Request.QueryString);
            NameValueCollection values = values2;
            string[] allKeys = values.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                dictionary.Add(allKeys[i], values[allKeys[i]]);
            }
            dictionary.Remove("HIGW");
            return dictionary;
        }

        public static bool IsDate(string s)
        {
            DateTime time;
            return DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out time);
        }
    }
}

