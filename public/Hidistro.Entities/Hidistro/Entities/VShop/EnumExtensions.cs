namespace Hidistro.Entities.VShop
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI.WebControls;

    public static class EnumExtensions
    {
        private static Dictionary<string, string> _cacheEnumShowTextdDic = new Dictionary<string, string>();

        public static void BindEnum<T>(this ListControl listControl, string Unbindkey = "") where T: struct
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("类型必须是枚举:" + enumType.FullName);
            }
            Array values = Enum.GetValues(enumType);
            if (!listControl.AppendDataBoundItems)
            {
                listControl.Items.Clear();
            }
            foreach (Enum enum2 in values)
            {
                if (enum2.ToString() != Unbindkey)
                {
                    listControl.Items.Add(new ListItem(enum2.ToShowText(), enum2.ToString()));
                }
            }
        }

        private static string GetEnumFullName(Enum en)
        {
            return (en.GetType().FullName + "." + en.ToString());
        }

        public static string ToShowText(this Enum en)
        {
            return en.ToShowText(false, ",");
        }

        public static string ToShowText(this Enum en, bool exceptionIfFail, string flagsSeparator)
        {
            string str;
            string enumFullName = GetEnumFullName(en);
            if (_cacheEnumShowTextdDic.TryGetValue(enumFullName, out str))
            {
                return str;
            }
            Type enumType = en.GetType();
            object[] customAttributes = enumType.GetCustomAttributes(typeof(FlagsAttribute), false);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                long num = Convert.ToInt64(en);
                StringBuilder builder = new StringBuilder();
                string[] names = Enum.GetNames(enumType);
                string showText = "";
                foreach (string str4 in names)
                {
                    long num2 = Convert.ToInt64(Enum.Parse(enumType, str4));
                    if (num2 == 0L)
                    {
                        object[] objArray2 = enumType.GetField(str4).GetCustomAttributes(typeof(EnumShowTextAttribute), false);
                        if (objArray2.Length > 0)
                        {
                            showText = ((EnumShowTextAttribute) objArray2[0]).ShowText;
                        }
                    }
                    else if ((num2 & num) == num2)
                    {
                        if (builder.Length != 0)
                        {
                            builder.Append(flagsSeparator);
                        }
                        object[] objArray3 = enumType.GetField(str4).GetCustomAttributes(typeof(EnumShowTextAttribute), false);
                        if (objArray3.Length > 0)
                        {
                            builder.Append(((EnumShowTextAttribute) objArray3[0]).ShowText);
                        }
                        else
                        {
                            if (exceptionIfFail)
                            {
                                throw new InvalidOperationException(string.Format("此枚举{0}未定义EnumShowTextAttribute", enumFullName));
                            }
                            builder.Append(str4);
                        }
                    }
                }
                if (builder.Length > 0)
                {
                    return builder.ToString();
                }
                return showText;
            }
            FieldInfo field = enumType.GetField(en.ToString());
            if (field == null)
            {
                throw new InvalidOperationException(string.Format("非完整枚举{0}", enumFullName));
            }
            object[] objArray4 = field.GetCustomAttributes(typeof(EnumShowTextAttribute), false);
            if (objArray4.Length > 0)
            {
                str = ((EnumShowTextAttribute) objArray4[0]).ShowText;
                lock (_cacheEnumShowTextdDic)
                {
                    _cacheEnumShowTextdDic[enumFullName] = str;
                    return str;
                }
            }
            if (exceptionIfFail)
            {
                throw new InvalidOperationException(string.Format("此枚举{0}未定义EnumShowTextAttribute", enumFullName));
            }
            return en.ToString();
        }
    }
}

