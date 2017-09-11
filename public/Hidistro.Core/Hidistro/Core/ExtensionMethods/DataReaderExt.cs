namespace Hidistro.Core.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class DataReaderExt
    {
        public static object GetObject(this IDataReader rd, Type t)
        {
            return GetObjectList(rd, t, true).FirstOrDefault<object>();
        }

        public static List<object> GetObjectList(this IDataReader rd, Type t)
        {
            return GetObjectList(rd, t, false);
        }

        private static List<object> GetObjectList(IDataReader rd, Type t, bool bFirst)
        {
            List<object> list = new List<object>();
            while (rd.Read())
            {
                object obj2 = Activator.CreateInstance(t);
                for (int i = 0; i < rd.FieldCount; i++)
                {
                    try
                    {
                        obj2.UpdateObject(rd.GetName(i), rd.GetValue(i));
                    }
                    catch
                    {
                    }
                }
                list.Add(obj2);
                if (bFirst)
                {
                    return list;
                }
            }
            return list;
        }
    }
}

