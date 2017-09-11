namespace Hidistro.SqlDal
{
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class BackupRestoreDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public string BackupData(string path)
        {
            string database;
            using (DbConnection connection = this.database.CreateConnection())
            {
                database = connection.Database;
            }
            string str2 = database + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("backup database [{0}] to disk='{1}'", database, path + str2));
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return str2;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void Restor()
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
            catch
            {
            }
        }

        public bool RestoreData(string bakFullName)
        {
            string dataSource;
            string database;
            bool flag;
            using (DbConnection connection = this.database.CreateConnection())
            {
                database = connection.Database;
                dataSource = connection.DataSource;
            }
            SqlConnection connection2 = new SqlConnection(string.Format("Data Source={0};Initial Catalog=master;Integrated Security=SSPI", dataSource));
            try
            {
                connection2.Open();
                SqlCommand command = new SqlCommand(string.Format("SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{0}'", database), connection2);
                ArrayList list = new ArrayList();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetInt16(0));
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    new SqlCommand(string.Format("KILL {0}", list[i].ToString()), connection2).ExecuteNonQuery();
                }
                new SqlCommand(string.Format("RESTORE DATABASE [{0}]  FROM DISK = '{1}' WITH REPLACE", database, bakFullName), connection2).ExecuteNonQuery();
                flag = true;
            }
            catch
            {
                flag = false;
            }
            finally
            {
                connection2.Close();
            }
            return flag;
        }

        private string StringCut(string str, string bg, string ed)
        {
            string str2 = str.Substring(str.IndexOf(bg) + bg.Length);
            return str2.Substring(0, str2.IndexOf(ed));
        }
    }
}

