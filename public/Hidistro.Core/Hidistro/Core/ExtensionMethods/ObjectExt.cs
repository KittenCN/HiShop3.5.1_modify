namespace Hidistro.Core.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class ObjectExt
    {
        public static object Copy(this object obj)
        {
            return ClassCopyer.Copy(obj);
        }

        public static T Copy<T>(this object obj)
        {
            return (T) obj.Copy();
        }

        public static decimal TryToDecimal(this object obj)
        {
            decimal num = 0M;
            try
            {
                num = Convert.ToDecimal(obj);
            }
            catch (Exception)
            {
            }
            return num;
        }

        public static double TryToDouble(this object obj)
        {
            double num = 0.0;
            try
            {
                num = Convert.ToDouble(obj);
            }
            catch (Exception)
            {
            }
            return num;
        }

        public static int TryToInt32(this object obj)
        {
            int num = 0;
            try
            {
                num = Convert.ToInt32(obj);
            }
            catch (Exception)
            {
            }
            return num;
        }

        public static long TryToInt64(this object obj)
        {
            long num = 0L;
            try
            {
                num = Convert.ToInt64(obj);
            }
            catch (Exception)
            {
            }
            return num;
        }

        public static string TryToString(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            return obj.ToString();
        }

        public static void UpdateObject(this object obj, NameValueCollection collection)
        {
            foreach (KeyValuePair<string, object> pair in collection)
            {
                obj.UpdateObject(pair.Key, pair.Value);
            }
        }

        public static void UpdateObject(this object obj, string Property, object Value)
        {
            PropertyInfo property = obj.GetType().GetProperty(Property);
            if (property == null)
            {
                throw new Exception("类属性[" + Property + "]不存在。");
            }
            property.SetValue(obj, Value, null);
        }
    }
}

