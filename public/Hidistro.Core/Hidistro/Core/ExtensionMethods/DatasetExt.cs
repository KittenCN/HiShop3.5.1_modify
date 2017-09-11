namespace Hidistro.Core.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class DatasetExt
    {
        public static T GetObject<T>(this DataSet ds)
        {
            return ds.GetObjectList<T>(true).FirstOrDefault<T>();
        }

        public static List<T> GetObjectList<T>(this DataSet ds)
        {
            return ds.GetObjectList<T>(false);
        }

        public static List<T> GetObjectList<T>(this DataSet ds, bool bFirst)
        {
            if (bFirst)
            {
                return ds.GetObjectList<T>(1, 1);
            }
            return ds.GetObjectList<T>(0, 0);
        }

        public static List<T> GetObjectList<T>(this DataSet ds, int page, int rows)
        {
            List<T> list = new List<T>();
            int num = (page - 1) * rows;
            int num2 = 0;
            int count = ds.Tables[0].Rows.Count;
            while ((num < count) && ((num2 < rows) || (rows == 0)))
            {
                DataRow row = ds.Tables[0].Rows[num];
                object obj2 = Activator.CreateInstance(typeof(T));
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    try
                    {
                        obj2.UpdateObject(ds.Tables[0].Columns[i].ColumnName, row[i]);
                    }
                    catch
                    {
                    }
                }
                list.Add((T) obj2);
                num++;
                num2++;
            }
            return list;
        }
    }
}

