namespace Hidistro.Core
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class DataHelper
    {
        public static string BuildNotinQuery(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string key, string filter, string selectFields)
        {
            string str = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
            string str2 = string.IsNullOrEmpty(filter) ? "" : ("AND " + filter);
            string str3 = string.IsNullOrEmpty(sortBy) ? "" : ("ORDER BY " + sortBy + " " + sortOrder.ToString());
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT TOP {0} {1} FROM {2} ", pageSize.ToString(CultureInfo.InvariantCulture), selectFields, table);
            if (pageIndex == 1)
            {
                builder.AppendFormat("{0} {1}", str, str3);
            }
            else
            {
                int num = (pageIndex - 1) * pageSize;
                builder.AppendFormat("WHERE {0} NOT IN (SELECT TOP {1} {0} FROM {2} {3} {4}) {5} {4}", new object[] { key, num, table, str, str3, str2 });
            }
            if (isCount)
            {
                builder.AppendFormat(";SELECT COUNT({0}) FROM {1} {2}", key, table, str);
            }
            return builder.ToString();
        }

        public static string BuildRownumberQuery(string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields, int partitionSize)
        {
            StringBuilder builder = new StringBuilder();
            string str = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
            if (partitionSize > 0)
            {
                builder.AppendFormat("SELECT TOP {0} {1}, ROW_NUMBER() OVER (ORDER BY ", partitionSize.ToString(CultureInfo.InvariantCulture), selectFields);
            }
            else
            {
                builder.AppendFormat("SELECT {0} , ROW_NUMBER() OVER (ORDER BY ", selectFields);
            }
            builder.AppendFormat("{0} {1}", string.IsNullOrEmpty(sortBy) ? pk : sortBy, sortOrder.ToString());
            builder.AppendFormat(") AS RowNumber FROM {0} {1}", table, str);
            builder.Insert(0, "SELECT * FROM (").Append(") T WHERE T.RowNumber BETWEEN @StartNumber AND @EndNumber");
            string str2 = "";
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.IndexOf(",") > 0)
                {
                    str2 = sortBy.Substring(0, sortBy.IndexOf(","));
                }
                else
                {
                    str2 = sortBy;
                }
            }
            if (isCount && (partitionSize == 0))
            {
                builder.AppendFormat(";SELECT COUNT(0) FROM {1} {2}", string.IsNullOrEmpty(str2) ? pk : str2, table, str);
            }
            return builder.ToString();
        }

        private static string BuildTopQuery(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
        {
            string str = string.IsNullOrEmpty(sortBy) ? pk : sortBy;
            string str2 = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
            string str3 = string.IsNullOrEmpty(filter) ? "" : ("AND " + filter);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT TOP {0} {1} FROM {2} ", pageSize.ToString(CultureInfo.InvariantCulture), selectFields, table);
            if (pageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY {1} {2}", str2, str, sortOrder.ToString());
            }
            else
            {
                int num = (pageIndex - 1) * pageSize;
                if (sortOrder == SortAction.Asc)
                {
                    builder.AppendFormat("WHERE {0} > (SELECT MAX({0}) FROM (SELECT TOP {1} {0} FROM {2} {3} ORDER BY {0} ASC) AS TMP) {4} ORDER BY {0} ASC", new object[] { str, num, table, str2, str3 });
                }
                else
                {
                    builder.AppendFormat("WHERE {0} < (SELECT MIN({0}) FROM (SELECT TOP {1} {0} FROM {2} {3} ORDER BY {0} DESC) AS TMP) {4} ORDER BY {0} DESC", new object[] { str, num, table, str2, str3 });
                }
            }
            if (isCount)
            {
                builder.AppendFormat(";SELECT COUNT({0}) FROM {1} {2}", str, table, str2);
            }
            return builder.ToString();
        }

        public static string CleanSearchString(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return null;
            }
            searchString = searchString.Replace("*", "%");
            searchString = Globals.StripHtmlXmlTags(searchString);
            searchString = Regex.Replace(searchString, "--|;|'|\"", " ", RegexOptions.Compiled | RegexOptions.Multiline);
            searchString = Regex.Replace(searchString, " {1,}", " ", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return searchString;
        }

        public static DataTable ConverDataReaderToDataTable(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            DataTable table = new DataTable {
                Locale = CultureInfo.InvariantCulture
            };
            int fieldCount = reader.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
            table.BeginLoadData();
            object[] values = new object[fieldCount];
            while (reader.Read())
            {
                reader.GetValues(values);
                table.LoadDataRow(values, true);
            }
            table.EndLoadData();
            return table;
        }

        public static string GetSafeDateTimeFormat(DateTime date)
        {
            return date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.SortableDateTimePattern, CultureInfo.InvariantCulture);
        }

        public static DbQueryResult PagingByRownumber(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
        {
            return PagingByRownumber(pageIndex, pageSize, sortBy, sortOrder, isCount, table, pk, filter, selectFields, 0);
        }

        public static DbQueryResult PagingByRownumber(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields, int partitionSize)
        {
            if (string.IsNullOrEmpty(table))
            {
                return null;
            }
            if (string.IsNullOrEmpty(sortBy) && string.IsNullOrEmpty(pk))
            {
                return null;
            }
            if (string.IsNullOrEmpty(selectFields))
            {
                selectFields = "*";
            }
            string query = BuildRownumberQuery(sortBy, sortOrder, isCount, table, pk, filter, selectFields, partitionSize);
            int num = ((pageIndex - 1) * pageSize) + 1;
            int num2 = (num + pageSize) - 1;
            DbQueryResult result = new DbQueryResult();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            database.AddInParameter(sqlStringCommand, "StartNumber", DbType.Int32, num);
            database.AddInParameter(sqlStringCommand, "EndNumber", DbType.Int32, num2);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                result.Data = ConverDataReaderToDataTable(reader);
                if ((isCount && (partitionSize == 0)) && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public static DbQueryResult PagingByTopnotin(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string key, string filter, string selectFields)
        {
            if (string.IsNullOrEmpty(table))
            {
                return null;
            }
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            if (string.IsNullOrEmpty(selectFields))
            {
                selectFields = "*";
            }
            string query = BuildNotinQuery(pageIndex, pageSize, sortBy, sortOrder, isCount, table, key, filter, selectFields);
            DbQueryResult result = new DbQueryResult();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                result.Data = ConverDataReaderToDataTable(reader);
                if (isCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public static DbQueryResult PagingByTopsort(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
        {
            if (string.IsNullOrEmpty(table))
            {
                return null;
            }
            if (string.IsNullOrEmpty(sortBy) && string.IsNullOrEmpty(pk))
            {
                return null;
            }
            if (string.IsNullOrEmpty(selectFields))
            {
                selectFields = "*";
            }
            string query = BuildTopQuery(pageIndex, pageSize, sortBy, sortOrder, isCount, table, pk, filter, selectFields);
            DbQueryResult result = new DbQueryResult();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                result.Data = ConverDataReaderToDataTable(reader);
                if (isCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public static bool SwapSequence(string table, string keyField, string sequenceField, int key, int replaceKey, int sequence, int replaceSequence)
        {
            string query = string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4}", new object[] { table, sequenceField, replaceSequence, keyField, key }) + string.Format(" UPDATE {0} SET {1} = {2} WHERE {3} = {4}", new object[] { table, sequenceField, sequence, keyField, replaceKey });
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

