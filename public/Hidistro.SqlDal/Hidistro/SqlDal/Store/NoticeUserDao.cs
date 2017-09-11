namespace Hidistro.SqlDal.Store
{
    using Entities.Store;
    using Hidistro.Entities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class NoticeUserDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void AddUser(int userid, string adminname)
        {
            string query = string.Concat(new object[] { "delete from Hishop_NoticeTempUser where LoginName=@LoginName and UserID=", userid, ";insert into Hishop_NoticeTempUser(UserID,LoginName)values(", userid, ",@LoginName)" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "LoginName", DbType.String, adminname);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void DelAllUser(int noticeId)
        {
            string query = "delete from Hishop_NoticeUser where NoticeId=" + noticeId;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public void DelUser(int userid, string adminname)
        {
            string query = "delete from Hishop_NoticeTempUser where LoginName=@LoginName and UserID=" + userid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "LoginName", DbType.String, adminname);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public IList<NoticeUserInfo> GetNoticeUserInfo(int noticeId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select NoticeId,UserId from Hishop_NoticeUser ");
            builder.Append(" where NoticeId=@NoticeId order by UserId asc ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "NoticeId", DbType.Int32, noticeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<NoticeUserInfo>(reader);
            }
        }

        public DataSet GetTempSelectedUser(string adminName)
        {
            string query = "select UserID from Hishop_NoticeTempUser where LoginName=@LoginName order by userid ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "LoginName", DbType.String, adminName);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }
    }
}

