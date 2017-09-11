namespace Hidistro.Core.ExtensionMethods
{
    using System;
    using System.Collections;
    using System.Data.Objects.DataClasses;
    using System.Reflection;

    public class ClassCopyer
    {
        public static object Copy(object obj)
        {
            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] properties = type.GetProperties();
            object obj2 = Activator.CreateInstance(type);
            foreach (FieldInfo info in fields)
            {
                object obj3 = info.GetValue(obj);
                if (!(obj3 is IRelatedEnd) && !(obj3 is EntityObject))
                {
                    Type type2 = info.FieldType.GetInterface("ICloneable", true);
                    if (info.FieldType.GetInterface("IEnumerable", true) == null)
                    {
                        if (type2 != null)
                        {
                            info.SetValue(obj2, ((ICloneable) info.GetValue(obj)).Clone());
                        }
                        else
                        {
                            info.SetValue(obj2, info.GetValue(obj));
                        }
                    }
                }
            }
            foreach (PropertyInfo info2 in properties)
            {
                object obj4 = info2.GetValue(obj, null);
                if (!(obj4 is IRelatedEnd) && !(obj4 is EntityObject))
                {
                    Type type4 = info2.PropertyType.GetInterface("ICloneable", true);
                    if (info2.CanWrite)
                    {
                        if (info2.PropertyType.GetInterface("IEnumerable", true) != null)
                        {
                            IEnumerable enumerable = (IEnumerable) obj4;
                            if (enumerable != null)
                            {
                                Type type6 = info2.PropertyType.GetInterface("IList", true);
                                Type type7 = info2.PropertyType.GetInterface("IDictionary", true);
                                if (type6 != null)
                                {
                                    object obj5 = Activator.CreateInstance(info2.PropertyType);
                                    foreach (object obj6 in enumerable)
                                    {
                                        if (obj6.GetType().GetInterface("ICloneable", true) != null)
                                        {
                                            ICloneable cloneable2 = (ICloneable) obj6;
                                            ((IList) obj5).Add(cloneable2.Clone());
                                        }
                                        else
                                        {
                                            ((IList) obj5).Add(obj6);
                                        }
                                    }
                                    info2.SetValue(obj2, obj5, null);
                                }
                                else if (type7 != null)
                                {
                                    IDictionary dictionary = Activator.CreateInstance(info2.PropertyType) as IDictionary;
                                    foreach (object obj7 in (enumerable as IDictionary).Keys)
                                    {
                                        if ((enumerable as IDictionary)[obj7].GetType().GetInterface("ICloneable", true) != null)
                                        {
                                            dictionary.Add(obj7, ((ICloneable) (enumerable as IDictionary)[obj7]).Clone());
                                        }
                                        else
                                        {
                                            dictionary.Add(obj7, (enumerable as IDictionary)[obj7]);
                                        }
                                    }
                                    info2.SetValue(obj2, dictionary, null);
                                }
                                else
                                {
                                    info2.SetValue(obj2, info2.GetValue(obj, null), null);
                                }
                            }
                        }
                        else if (type4 != null)
                        {
                            info2.SetValue(obj2, ((ICloneable) info2.GetValue(obj, null)).Clone(), null);
                        }
                        else
                        {
                            info2.SetValue(obj2, info2.GetValue(obj, null), null);
                        }
                    }
                }
            }
            return obj2;
        }

        public static T Copy<T>(object obj)
        {
            return (T) Copy(obj);
        }
    }
}

