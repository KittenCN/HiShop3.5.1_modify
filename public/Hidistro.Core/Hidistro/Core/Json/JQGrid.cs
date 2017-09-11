namespace Hidistro.Core.Json
{
    using Hidistro.Core.ExtensionMethods;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    public class JQGrid
    {
        public static string Json(DataSet ds, int total, int page, int rows)
        {
            return Json(ds, total, page, rows, null);
        }

        public static string Json(DataSet ds, int total, int page, int rows, List<string> Cols)
        {
            return Json(ds, total, page, rows, Cols, null);
        }

        public static string Json(DataSet ds, int total, int page, int rows, List<string> Cols, List<string> SumCols)
        {
            int num = 1;
            int num2 = 0;
            int count = ds.Tables[0].Rows.Count;
            int num4 = 0;
            int num5 = ds.Tables[0].Columns.Count;
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            string str = "";
            StringBuilder builder = new StringBuilder();
            if (Cols == null)
            {
                Cols = new List<string>();
            }
            if (SumCols == null)
            {
                SumCols = new List<string>();
            }
            if (Cols.Count == 0)
            {
                for (int i = 0; i < num5; i++)
                {
                    Cols.Add(ds.Tables[0].Columns[i].ColumnName);
                }
            }
            foreach (string str2 in SumCols)
            {
                dictionary.Add(str2, 0.0);
            }
            builder.Append("{");
            builder.Append("\"total\":");
            builder.Append("\"" + total.ToString() + "\"");
            builder.Append(",\"page\":");
            builder.Append("\"" + page.ToString() + "\"");
            builder.Append(",\"records\":");
            builder.Append("\"" + ds.Tables[0].Rows.Count.ToString() + "\"");
            builder.Append(",\"rows\":[");
            for (num2 = (page - 1) * rows; (num2 < count) && (num4 < rows); num2++)
            {
                DataRow row = ds.Tables[0].Rows[num2];
                if (num4 > 0)
                {
                    builder.Append(",");
                }
                builder.Append("{");
                try
                {
                    str = DataFormatter.Format(row["ID"]);
                }
                catch (Exception)
                {
                    str = num.ToString();
                    num++;
                }
                builder.Append("\"id\":\"" + str + "\",");
                builder.Append("\"cell\":[");
                bool flag = true;
                foreach (string str3 in Cols)
                {
                    if (!flag)
                    {
                        builder.Append(",");
                    }
                    builder.Append("\"" + DataFormatter.Format(row[str3]) + "\"");
                    flag = false;
                    try
                    {
                        Dictionary<string, double> dictionary2;
                        string str5;
                        (dictionary2 = dictionary)[str5 = str3] = dictionary2[str5] + row[str3].TryToDouble();
                    }
                    catch (Exception)
                    {
                        if (dictionary.ContainsKey(str3))
                        {
                            dictionary.Remove(str3);
                        }
                    }
                }
                builder.Append("]");
                builder.Append("}");
                num4++;
            }
            builder.Append("]");
            builder.Append(",\"userdata\":{");
            bool flag2 = true;
            foreach (string str4 in dictionary.Keys)
            {
                if (!flag2)
                {
                    builder.Append(",");
                }
                builder.Append("\"" + str4 + "\":");
                builder.Append("\"" + DataFormatter.Format(dictionary[str4]) + "\"");
                flag2 = false;
            }
            builder.Append("}");
            builder.Append("}");
            return builder.ToString();
        }
    }
}

