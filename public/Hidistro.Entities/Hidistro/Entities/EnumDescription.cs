namespace Hidistro.Entities
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public class EnumDescription
    {
        public static string GetEnumDescription(Enum enumSubitem)
        {
            string name = enumSubitem.ToString();
            object[] customAttributes = enumSubitem.GetType().GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((customAttributes == null) || (customAttributes.Length == 0))
            {
                return name;
            }
            DescriptionAttribute attribute = (DescriptionAttribute) customAttributes[0];
            return attribute.Description;
        }

        public static string GetEnumDescription(Enum enumSubitem, int index)
        {
            string name = enumSubitem.ToString();
            object[] customAttributes = enumSubitem.GetType().GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((customAttributes == null) || (customAttributes.Length == 0))
            {
                return name;
            }
            DescriptionAttribute attribute = (DescriptionAttribute) customAttributes[0];
            return attribute.Description.Split(new char[] { '|' })[index];
        }

        public static bool GetEnumValue<TEnum>(string enumDescription, ref TEnum currentfiled)
        {
            FieldInfo[] fields = typeof(TEnum).GetFields();
            for (int i = 1; i < (fields.Length - 1); i++)
            {
                DescriptionAttribute attribute = fields[i].GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute;
                if (attribute.Description.Contains(enumDescription))
                {
                    currentfiled = (TEnum) fields[i].GetValue(typeof(TEnum));
                    return true;
                }
            }
            return false;
        }
    }
}

