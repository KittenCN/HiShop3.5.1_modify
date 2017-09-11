namespace Hidistro.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;

    public class ReaderConvert
    {
        private static object CheckType(object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }
            return Convert.ChangeType(value, conversionType);
        }

        public static T DataRowToModel<T>(DataRow objReader) where T: new()
        {
            if (objReader == null)
            {
                return default(T);
            }
            Type type = typeof(T);
            int count = objReader.Table.Columns.Count;
            T local = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
            for (int i = 0; i < count; i++)
            {
                if (!IsNullOrDBNull(objReader[i]))
                {
                    try
                    {
                        PropertyInfo property = type.GetProperty(objReader.Table.Columns[i].ColumnName.Replace("_", ""), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (property != null)
                        {
                            Type propertyType = property.PropertyType;
                            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                NullableConverter converter = new NullableConverter(propertyType);
                                propertyType = converter.UnderlyingType;
                            }
                            if (propertyType.IsEnum)
                            {
                                object obj2 = Enum.ToObject(propertyType, objReader[i]);
                                property.SetValue(local, obj2, null);
                            }
                            else
                            {
                                property.SetValue(local, CheckType(objReader[i], propertyType), null);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return local;
        }

        private static bool IsNullOrDBNull(object obj)
        {
            if ((obj != null) && !(obj is DBNull))
            {
                return false;
            }
            return true;
        }

        public static IList<T> ReaderToList<T>(IDataReader objReader) where T: new()
        {
            if (objReader == null)
            {
                return null;
            }
            List<T> list = new List<T>();
            Type type = typeof(T);
            while (objReader.Read())
            {
                T local2 = default(T);
                T local = (local2 == null) ? Activator.CreateInstance<T>() : default(T);
                for (int i = 0; i < objReader.FieldCount; i++)
                {
                    if (!IsNullOrDBNull(objReader[i]))
                    {
                        PropertyInfo property = type.GetProperty(objReader.GetName(i).Replace("_", ""), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (property != null)
                        {
                            Type propertyType = property.PropertyType;
                            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                NullableConverter converter = new NullableConverter(propertyType);
                                propertyType = converter.UnderlyingType;
                            }
                            if (property.PropertyType.IsEnum)
                            {
                                object obj2 = Enum.ToObject(propertyType, objReader[i]);
                                property.SetValue(local, obj2, null);
                            }
                            else
                            {
                                property.SetValue(local, CheckType(objReader[i], propertyType), null);
                            }
                        }
                    }
                }
                list.Add(local);
            }
            return list;
        }

        public static T ReaderToModel<T>(IDataReader objReader) where T: new()
        {
            if ((objReader == null) || !objReader.Read())
            {
                return default(T);
            }
            Type type = typeof(T);
            int fieldCount = objReader.FieldCount;
            T local = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
            for (int i = 0; i < fieldCount; i++)
            {
                if (!IsNullOrDBNull(objReader[i]))
                {
                    PropertyInfo property = type.GetProperty(objReader.GetName(i).Replace("_", ""), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (property != null)
                    {
                        Type propertyType = property.PropertyType;
                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        {
                            NullableConverter converter = new NullableConverter(propertyType);
                            propertyType = converter.UnderlyingType;
                        }
                        if (propertyType.IsEnum)
                        {
                            object obj2 = Enum.ToObject(propertyType, objReader[i]);
                            property.SetValue(local, obj2, null);
                        }
                        else
                        {
                            property.SetValue(local, CheckType(objReader[i], propertyType), null);
                        }
                    }
                }
            }
            return local;
        }
    }
}

