namespace Hidistro.Core.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public class DataTableToList
    {
        public static List<T> ToList<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            List<PropertyInfo> list2 = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (DataRow row in dt.Rows)
            {
                T local = Activator.CreateInstance<T>();
                Predicate<PropertyInfo> match = null;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (match == null)
                    {
                        match = p => p.Name == dt.Columns[i].ColumnName;
                    }
                    PropertyInfo info = list2.Find(match);
                    if ((info != null) && !Convert.IsDBNull(row[i]))
                    {
                        info.SetValue(local, row[i], null);
                    }
                }
                list.Add(local);
            }
            return list;
        }
    }
}

