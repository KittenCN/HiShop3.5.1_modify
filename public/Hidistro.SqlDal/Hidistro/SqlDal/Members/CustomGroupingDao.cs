namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;

    public class CustomGroupingDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public string AddCustomGrouping(CustomGroupingInfo customGroupingInfo)
        {
            if (this.GetGroupIdByGroupName(customGroupingInfo.GroupName, 0) > 0)
            {
                return "分组名称已存在";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_CustomGrouping(GroupName,Memo)VALUES(@GroupName,@Memo);select @@identity;");
            this.database.AddInParameter(sqlStringCommand, "GroupName", DbType.String, customGroupingInfo.GroupName);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, customGroupingInfo.Memo);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)).ToString();
        }

        public bool DelGroup(int groupid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[] { "Delete Vshop_CustomGroupingUser where GroupId=", groupid, ";Delete Vshop_CustomGrouping where Id=", groupid }));
            return (Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)) > 0);
        }

        public IList<CustomGroupingInfo> GetCustomGroupingList()
        {
            string query = "SELECT * FROM Vshop_CustomGrouping Order By UpdateTime desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<CustomGroupingInfo>(reader);
            }
        }

        public IList<CustomGroupingInfo> GetCustomGroupingList(string customGroupIds = "")
        {
            string query = "SELECT * FROM Vshop_CustomGrouping ";
            if (!string.IsNullOrEmpty(customGroupIds))
            {
                query = query + " where id in(" + customGroupIds + ");";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<CustomGroupingInfo>(reader);
            }
        }

        public DataTable GetCustomGroupingTable()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Vshop_CustomGrouping Order By UpdateTime desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetCustomGroupingUser(int groupId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT a.UserId,b.LastOrderDate,b.PayOrderDate,b.FinishOrderDate FROM Vshop_CustomGroupingUser a inner join aspnet_Members b on a.userid=b.userid where a.GroupId=" + groupId + " and b.Status=1 Order By UpdateTime desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public int GetGroupIdByGroupName(string groupName, int groupId)
        {
            int num = 0;
            string str = string.Empty;
            if (groupId > 0)
            {
                str = " and ID<>" + groupId;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 Id from Vshop_CustomGrouping where GroupName=@GroupName " + str);
            this.database.AddInParameter(sqlStringCommand, "GroupName", DbType.String, groupName);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                num = Globals.ToNum(table.Rows[0]["ID"]);
            }
            return num;
        }

        public CustomGroupingInfo GetGroupInfoById(int groupId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Vshop_CustomGrouping where id=@Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.String, groupId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<CustomGroupingInfo>(reader);
            }
        }

        public string UpdateCustomGrouping(CustomGroupingInfo customGroupingInfo)
        {
            if (this.GetGroupIdByGroupName(customGroupingInfo.GroupName, customGroupingInfo.Id) > 0)
            {
                return "分组名称已存在";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_CustomGrouping set GroupName=@GroupName where Id=" + customGroupingInfo.Id);
            this.database.AddInParameter(sqlStringCommand, "GroupName", DbType.String, customGroupingInfo.GroupName);
            this.database.AddInParameter(sqlStringCommand, "Memo", DbType.String, customGroupingInfo.Memo);
            return Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)).ToString();
        }
    }
}

