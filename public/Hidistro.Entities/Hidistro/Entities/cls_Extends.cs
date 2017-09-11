namespace Hidistro.Entities
{
    using System;
    using System.Runtime.CompilerServices;

    public static class cls_Extends
    {
        public static bool bBool(this string val, ref bool i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return bool.TryParse(val, out i);
        }

        public static bool bDate(this string val, ref DateTime i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return DateTime.TryParse(val, out i);
        }

        public static bool bDecimal(this string val, ref decimal i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            return decimal.TryParse(val, out i);
        }

        public static bool bInt(this string val, ref int i)
        {
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            if (val.Contains(".") || val.Contains("-"))
            {
                return false;
            }
            return int.TryParse(val, out i);
        }

        public static string ReplaceSingleQuoteMark(this string target)
        {
            return target.Replace("'", "''");
        }
    }
}

