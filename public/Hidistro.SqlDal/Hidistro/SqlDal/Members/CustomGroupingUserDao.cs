namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class CustomGroupingUserDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddCustomGroupingUser(int UserId, int groupId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_CustomGroupingUser(GroupId,UserId)VALUES(@GroupId,@UserId)");
            this.database.AddInParameter(sqlStringCommand, "GroupId", DbType.Int32, groupId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            bool flag = Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)) > 0;
            this.UpdateGroupUserCount(groupId);
            return flag;
        }

        public bool DelGroupUser(string UserId, int groupId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[] { "Delete Vshop_CustomGroupingUser where GroupId=", groupId, " and UserId in (", UserId, ")" }));
            bool flag = Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)) > 0;
            this.UpdateGroupUserCount(groupId);
            return flag;
        }

        public int GetGroupIdByUserId(int UserId, int groupId)
        {
            int num = 0;
            string str = string.Empty;
            if (groupId > 0)
            {
                str = " and GroupId=" + groupId;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 GroupId from Vshop_CustomGroupingUser where UserId=@UserId " + str);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, UserId);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                num = Globals.ToNum(table.Rows[0]["GroupId"]);
            }
            return num;
        }

        public IList<int> GetMemberGroupList(int userId)
        {
            IList<int> list = new List<int>();
            string query = "select GroupId from dbo.Vshop_CustomGroupingUser where UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table == null) || (table.Rows.Count <= 0))
                {
                    return list;
                }
                foreach (DataRow row in table.Rows)
                {
                    int item = Convert.ToInt32(row["GroupId"]);
                    list.Add(item);
                }
            }
            return list;
        }

        public bool UpdateGroupUserCount(int groupId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_CustomGrouping set UserCount=(select count(0) from Vshop_CustomGroupingUser where GroupId=Vshop_CustomGrouping.id) where Id=" + groupId);
            return (Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)) > 0);
        }
    }
}

