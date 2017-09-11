namespace Hidistro.Core.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public class ListToDataTable
    {
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable table = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                table.Columns.Add(info.Name);
            }
            foreach (T local in items)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(local, null);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}

